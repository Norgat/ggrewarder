using System;
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

        #region Black list
        private ObservableCollection<User> _blackList = new ObservableCollection<User>();
        private FilteredList<User> _BlackListFilter;
        public INotifyCollectionChanged BlackList {
            get { return _BlackListFilter; }
        }

        public void AddToBlackList(User user) {
            _blackList.Add(user);

            // Где-то не происходит уведомление вовремя. Поэтому приходится обновлять ручками. 
            _whiteList.Updated();
            _ForRandom.Updated();
        }

        public void DeleteFromBlackList(User user) {
            var deletedUser = _blackList.Single(U => U.id == user.id);
            _blackList.Remove(deletedUser);
        } 
        #endregion

        public bool isInBlackList(User user) {
            return _blackList.Any(U => U.id == user.id);
        }

        private ObservableCollection<User> _forRandom = new ObservableCollection<User>();
        private FilteredList<User> _ForRandom;
        public INotifyCollectionChanged ForRandom {
            get { return _ForRandom; }
        }
        
        public ConnectionManager(int channel_id) {
            #region List initializations
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<Message>();

            // Группа пользователей для отбора для розыгрыша
            _whiteList = new FilteredList<User>(_users);
            _whiteList.AddOrSelector(new Selectors.PremiumSelector());

            // Чёрный список
            _blackList.CollectionChanged +=_blackList_CollectionChanged;
            _BlackListFilter = new FilteredList<User>(_blackList);

            // Выкидываем людей состоящих в чёрном списке из белого спика
            _whiteList.Observe(_blackList);
            _whiteList.AddAndSelector(new Selectors.BlackListDeselector(_blackList));

            // Составляем список людей для розыгрыша
            _ForRandom = new FilteredList<User>(_forRandom);
            _ForRandom.Observe(_blackList);
            _ForRandom.Observe(_whiteList);
            _ForRandom.AddOrSelector(new Selectors.BlackListDeselector(_blackList)); 
            #endregion
            
            _gg = new GG();
            #region GG API Callbacks
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
                if (!_blackList.Any(U => U.id == user.id)) {
                    if (!_forRandom.Any(U => U.id == user.id)) {
                        _forRandom.Add(user);
                    }
                }
            };

            #endregion
            _gg.Connect();
        }

        void _blackList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            _whiteList.Updated();
        }


        public void Dispose() {
            if (_gg != null) {
                _gg.Dispose();
            }
        }
    }
}
