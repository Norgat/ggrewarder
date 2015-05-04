using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector;
using GGConnector.GGObjects;
using System.Collections.ObjectModel;

namespace Rewarder {
    class ConnectionManager: IDisposable {
        private GG _gg;

        //private enum State { Wait = 0, Connected, Error };
        //private State _connection;
        //private int _channelId;

        public ObservableCollection<User> users { get; set; }


        public ConnectionManager(int channel_id) {
            //_channelId = channel_id;
            //_connection = State.Wait;

            users = new ObservableCollection<User>();

            _gg = new GG();
            _gg.OnGetWelcome += (sender, welcome) => {
                //_connection = State.Connected;
                _gg.GetUsersList(channel_id);
            };

            _gg.OnGetUsersList += (sender, usersList) => {
                foreach (var u in usersList.users) {
                    users.Add(u);
                }
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
