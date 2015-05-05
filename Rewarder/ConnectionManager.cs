using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector;
using GGConnector.GGObjects;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Rewarder {
    class ConnectionManager: IDisposable {
        private GG _gg;

        private ObservableCollection<User> _users;
        private ObservableCollection<Message> _messages;

        public INotifyCollectionChanged users {
            get { return _users; }
        }

        public INotifyCollectionChanged messages {
            get { return _messages; } 
        }
        
        public ConnectionManager(int channel_id) {
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<Message>();

            _gg = new GG();
            _gg.OnGetWelcome += (sender, welcome) => {
                _gg.GetUsersList(channel_id);
                _gg.Join(channel_id);
            };

            _gg.OnGetUsersList += (sender, usersList) => {
                foreach (var u in usersList.users) {
                    _users.Add(u);
                }
            };

            _gg.OnMessageRecieved += (sender, message) => {
                _messages.Add(message);
            };

            _gg.Connect();
        }


        public void Dispose() {
            if (_gg != null) {
                _gg.Dispose();
            }
        }
    }
}
