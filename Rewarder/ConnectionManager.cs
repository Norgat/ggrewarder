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
        private int _channelId;
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
        private HashSet<IElementSelector<User>> _blackListSelectors = new HashSet<IElementSelector<User>>();
        public INotifyCollectionChanged BlackList {
            get { return _blackList; }
        }

        public void AddToBlackList(User user) {
            _blackList.Add(user);

            // Где-то не происходит уведомление вовремя. Поэтому приходится обновлять ручками. 
            _whiteList.Updated();
            _ForRandom.Updated();
        }

        public void DeleteFromBlackList(User user) {
            int indx = -1;
            for (int i = 0; i < _blackList.Count; i++) {
                if (_blackList[i].id == user.id) {
                    indx = i;
                    break;
                }
            }

            if (indx != -1) {
                _blackList.RemoveAt(indx);
            }
        }

        public void DeleteFromBlackList(IElementSelector<User> sel) {
            if (_blackListSelectors.Contains(sel)) {
                _blackListSelectors.Remove(sel);

                var remItems = new List<User>();
                foreach (var item in _blackList) {
                    var flag = false;
                    foreach (var s in _blackListSelectors) {
                        if (s.isOk(item)) {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag) {
                        remItems.Add(item);
                    }
                }

                foreach (var rem in remItems) {
                    _blackList.Remove(rem);
                }
            }
        }

        public bool isInBlackList(User user) {
            return _blackList.Any(U => U.id == user.id);
        }
        #endregion        

        private ObservableCollection<User> _forRandom = new ObservableCollection<User>();
        private FilteredList<User> _ForRandom;
        public INotifyCollectionChanged ForRandom {
            get { return _ForRandom; }
        }


        private ObservableCollection<User> _chatActive = new ObservableCollection<User>();
        private FilteredList<User> _ChatActive;
        public INotifyCollectionChanged ChatActive {
            get { return _ChatActive; }
        }
        
        public ConnectionManager(int channel_id) {
            _channelId = channel_id;

            #region List initializations
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<Message>();

            // Чёрный список
            _blackList.CollectionChanged +=_blackList_CollectionChanged;

            // Группа пользователей для отбора для розыгрыша
            _whiteList = new FilteredList<User>(_users); 
            _whiteList.Observe(_blackList);
            //_whiteList.AddDeselector(new Selectors.BlackListSelector(_blackList));

            _ChatActive = new FilteredList<User>(_chatActive);

            // Составляем список людей для розыгрыша
            _ForRandom = new FilteredList<User>(_users);
            _ForRandom.Observe(_blackList);
            _ForRandom.Observe(_whiteList);
            _ForRandom.AddSelector(new Selectors.WhiteListSelector(_whiteList));
            _ForRandom.AddDeselector(new Selectors.InChatDeselector(_chatActive));
            #endregion


            _gg = new GG();
            #region GG API Callbacks
            _gg.OnGetWelcome += (sender, welcome) => {
                _gg.GetUsersList(channel_id);
                _gg.Join(channel_id);
            };

            _gg.OnGetUsersList += (sender, usersList) => {
                if (_users.Count == 0) {
                    foreach (var u in usersList.users) {
                        _users.Add(u);
                    }                    
                } else {
                    foreach (var u in usersList.users) {
                        if (!_users.Contains(u, new UserEqualityComparer())) {
                            _users.Add(u);
                        }
                    }                    
                }
                _whiteList.Updated();
            };

            _gg.OnMessageRecieved += (sender, message) => {
                _messages.Add(message);

                var user = new User { id = message.user_id, name = message.user_name };
                if (!_blackList.Any(U => U.id == user.id)) {
                    //if (!_forRandom.Any(U => U.id == user.id)) {
                    //    _forRandom.Add(user);
                    //}

                    if (!_chatActive.Any(U => U.id == user.id)) {
                        _chatActive.Add(user);
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

        public void UpdateUserList() {
            _gg.GetUsersList(_channelId);
        }
    }
}
