﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector;
using GGConnector.GGObjects;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Rewarder.Collections;

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

        private FilteredList<User> _whiteList;
        public INotifyCollectionChanged WhiteList {
            get { return _whiteList; }
        }

        private ObservableCollection<User> _blackList = new ObservableCollection<User>();
        public ObservableCollection<User> BlackList {
            get { return _blackList; }
        }

        private ObservableCollection<User> _forRandom = new ObservableCollection<User>();
        public ObservableCollection<User> ForRandom {
            get {
                return _forRandom;
            }
        }
        
        public ConnectionManager(int channel_id) {
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<Message>();

            _whiteList = new FilteredList<User>(_users);
            _whiteList.AddOrSelector(new Selectors.PremiumSelector());

            _gg = new GG();
            _gg.OnGetWelcome += (sender, welcome) => {
                _gg.GetUsersList(channel_id);
                _gg.Join(channel_id);
            };

            _gg.OnGetUsersList += (sender, usersList) => {
                foreach (var u in usersList.users) {
                    _users.Add(u);
                }
                _whiteList.Updated();
            };

            _gg.OnMessageRecieved += (sender, message) => {
                _messages.Add(message);

                var user = new User { id = message.user_id, name = message.user_name };
                if (!_forRandom.Any(U => U.id == user.id)) {
                    _forRandom.Add(user);
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
