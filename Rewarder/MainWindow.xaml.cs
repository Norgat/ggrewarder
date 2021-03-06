﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using GGConnector;
using GGConnector.GGObjects;

using Rewarder.Selectors;
using Rewarder.Collections;

using System.Collections.ObjectModel;
using System.Timers;
using System.ComponentModel;

namespace Rewarder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {

        private ConnectionManager _manager = null;
        private Timer _userListUpdateTimer = null;

        private XTimer _xTimer;

        private ObservableCollection<HistoryRecord> _history = new ObservableCollection<HistoryRecord>();

        public MainWindow() {
            InitializeComponent();

            cb_pem.Selector = new PremiumSelector();
            cb_nonpem.Selector = new NotPremiumSelector();
            cb_donat0.Selector = new Donat0Selector();
            cb_donat1.Selector = new Donat1Selector();
            cb_donat2.Selector = new Donat2Selector();
            cb_donat3.Selector = new Donat3Selector();
            cb_donat4.Selector = new Donat4Selector();
            cb_donat5.Selector = new Donat5Selector();
            cb_mod.Selector = new RightSelector();

            //cb_chat.SelectorType = false;

            var historyBind = new Binding();
            historyBind.Source = _history;
            History.SetBinding(ListView.ItemsSourceProperty, historyBind);

            _xTimer = new XTimer();

            //var timeBind = new Binding();
            //timeBind.Source = _xTimer.Time;
            //xTimerViewBlock.SetBinding(TextBlock.TextProperty, timeBind);

            xTimerViewBlock.DataContext = _xTimer;
        }

        private void Button_Connect(object sender, RoutedEventArgs e) {
            var streamer = StreamerNameInput.Text;

            if (streamer == "") {
                MessageBox.Show("Введите имя стримера");
                return;
            }

            try {
                if (_userListUpdateTimer != null) {
                    _userListUpdateTimer.Dispose();
                }

                if (_xTimer != null) {
                    _xTimer.Stop();
                }
                
                var id = GG.GetChannelId(streamer);

                if (_manager != null) { 
                    _manager.Dispose(); 
                }                

                _manager = new ConnectionManager(id);
                BindingOperations.ClearBinding(usersListView, ListView.ItemsSourceProperty);
                var usersBind = new Binding();
                usersBind.Source = _manager.users;
                usersListView.SetBinding(ListView.ItemsSourceProperty, usersBind);

                BindingOperations.ClearBinding(chatControl, ListView.ItemsSourceProperty);
                var chatBind = new Binding();
                chatBind.Source = _manager.messages;
                chatControl.SetBinding(ListView.ItemsSourceProperty, chatBind);

                BindingOperations.ClearBinding(whiteUsersListView, ListView.ItemsSourceProperty);
                var premiumBind = new Binding();
                premiumBind.Source = _manager.WhiteList;
                whiteUsersListView.SetBinding(ListView.ItemsSourceProperty, premiumBind);

                //BindingOperations.ClearBinding(blackUsersListView, ListView.ItemsSourceProperty);
                //var blackListBind = new Binding();
                //blackListBind.Source = _manager.BlackList;
                //blackUsersListView.SetBinding(ListView.ItemsSourceProperty, blackListBind);

                BindingOperations.ClearBinding(forRandowUsersListView, ListView.ItemsSourceProperty);
                var forRandomBind = new Binding();
                forRandomBind.Source = _manager.ForRandom;
                forRandowUsersListView.SetBinding(ListView.ItemsSourceProperty, forRandomBind);

                cb_pem.UserList = (FilteredList<User>) _manager.WhiteList;
                cb_nonpem.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_donat0.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_donat1.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_donat2.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_donat3.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_donat4.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_donat5.UserList = (FilteredList<User>)_manager.WhiteList;
                cb_mod.UserList = (FilteredList<User>)_manager.WhiteList;

                _userListUpdateTimer = new Timer();
                _userListUpdateTimer.Interval = 5000;
                _userListUpdateTimer.Elapsed += new ElapsedEventHandler(OnUpdateUserList);
                _userListUpdateTimer.Enabled = true;

                _xTimer.Start();
            } catch (Exception) {
                MessageBox.Show("Что-то пошло не так. Найдите последовательность действий, приводящих к этому сообщению и отправьте её на norg113@gmail.com");                
            }
        }

        private void OnUpdateUserList(object source, ElapsedEventArgs e) {
            if (_manager != null) {
                _manager.UpdateUserList();
            }
        }

        private void TextBlock_To_BlackList(object sender, MouseButtonEventArgs e) {
            var box = (TextBlock)sender;
            var user = (User)box.DataContext;

            DragDrop.DoDragDrop((TextBlock)e.Source, user, DragDropEffects.Move);
        }

        private void blackUsersListView_Drop(object sender, DragEventArgs e) {
            if (_manager != null && e.Data != null && e.Data.GetDataPresent("GGConnector.GGObjects.User")) {
                var user = e.Data.GetData("GGConnector.GGObjects.User") as GGConnector.GGObjects.User;

                if (!_manager.isInBlackList(user)) {
                    _manager.AddToBlackList(user);
                }
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e) {
            var panel = (StackPanel)sender;
            var message = (Message)panel.DataContext;
            var user = new User { id = message.user_id, name = message.user_name };

            DragDrop.DoDragDrop((StackPanel)sender, user, DragDropEffects.Move);
        }

        private void TextBlock_KeyDown(object sender, KeyEventArgs e) {
            if (e != null) {
                if (e.Key == Key.Delete) {
                    var list = (ListView)sender;
                    var user = (User)list.SelectedItem;

                    _manager.DeleteFromBlackList(user);
                }
            }
        }

        private void Button_Raward(object sender, RoutedEventArgs e) {
            if (_manager == null) {
                MessageBox.Show("Вы не в чате.");
                return;
            }

            int count = 0;
            if (!int.TryParse(UCount.Text, out count)) {
                MessageBox.Show("Укажите правильное количество пользователей!");
                return;
            }

            if (count <= 0) {
                MessageBox.Show("Укажите правильное количество пользователей!");
                return;
            }           

            var randomSeq = (IEnumerable<User>)_manager.ForRandom;
            var rList = randomSeq.ToList();

            if (rList.Count <= 0) {
                MessageBox.Show("Увы, никого нет.");
                return;
            }

            if (rList.Count <= count) {
                MessageBox.Show("Слишком мало пользователей. Все выйграли!");
                return;
            }

            var rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            var luckyList = new List<int>();

            while (luckyList.Count != count) {
                var lucky = rand.Next(rList.Count);
                if (!luckyList.Contains(lucky)) {
                    luckyList.Add(lucky);
                }
            }

            foreach (var lucky in luckyList) {
                var record = new HistoryRecord { Time = DateTime.Now, Name = rList[lucky].name };
                _history.Add(record);
                MessageBox.Show(rList[lucky].name);
            }            
        }

        private void Button_ClearRewardList(object sender, RoutedEventArgs e) {

        }

        private void Button_Click_NotPremium_White(object sender, RoutedEventArgs e) {
            if (_manager != null) {
                var sel = new Selectors.NotPremiumSelector();
                var fl = (Rewarder.Collections.FilteredList<User>)_manager.WhiteList;

                if (fl.isSelectorContained(sel)) {
                    fl.RemoveSelector(sel);
                } else {
                    fl.AddSelector(sel);
                }
            }
        }

        private void Button_Click_Premium_White(object sender, RoutedEventArgs e) {
            if (_manager != null) {
                var sel = new Selectors.PremiumSelector();
                var fl = (Rewarder.Collections.FilteredList<User>)_manager.WhiteList;

                if (fl.isSelectorContained(sel)) {
                    fl.RemoveSelector(sel);
                } else {
                    fl.AddSelector(sel);
                }
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e) {
            var mods = (Keyboard.Modifiers & ModifierKeys.Control);
            if ((e.Key == Key.C) 
                && (mods == ModifierKeys.Control)) {
                var lw = (ListView)sender;
                var record = (HistoryRecord)lw.SelectedItem;
                Clipboard.SetText(record.Name);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            var record = (HistoryRecord)this.History.SelectedItem;
            if (record != null) {
                Clipboard.SetText(record.Name);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            if (_manager != null) {
                var sel = new InChatDeselector(_manager.ChatActive);
                ((FilteredList<User>)_manager.ForRandom).AddDeselector(sel);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            if (_manager != null) {
                var sel = new InChatDeselector(_manager.ChatActive);
                ((FilteredList<User>)_manager.ForRandom).RemoveDeselector(sel);
            }
        }

        private void Button_DropChatActiveList(object sender, RoutedEventArgs e) {
            if (_manager != null && _xTimer != null) {
                _xTimer.Stop();
                _manager.DropChatActiveList();
                _xTimer.Restart();
            }
        }        
    }
}
