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

namespace Rewarder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {

        private ConnectionManager _manager = null;

        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Connect(object sender, RoutedEventArgs e) {
            var streamer = StreamerNameInput.Text;

            try {
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

                BindingOperations.ClearBinding(blackUsersListView, ListView.ItemsSourceProperty);
                var blackListBind = new Binding();
                blackListBind.Source = _manager.BlackList;
                blackUsersListView.SetBinding(ListView.ItemsSourceProperty, blackListBind);

                BindingOperations.ClearBinding(forRandowUsersListView, ListView.ItemsSourceProperty);
                var forRandomBind = new Binding();
                forRandomBind.Source = _manager.ForRandom;
                forRandowUsersListView.SetBinding(ListView.ItemsSourceProperty, forRandomBind);

            } catch (Exception ex) {
                MessageBox.Show("Не удалось получить Id стрима. Проверьте введённый ник стримера.");                
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            var box = (TextBlock)sender;
            var user = (User)box.DataContext;

            MessageBox.Show(string.Format("{0}: {1}", user.id, user.name));
        }
    }
}
