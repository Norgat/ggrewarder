using System;
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

using GGConnector.GGObjects;
using Rewarder.Collections;
using Rewarder.Selectors;

namespace Rewarder.Controls {
    /// <summary>
    /// Interaction logic for ColorButton.xaml
    /// </summary>
    public partial class ColorButton: UserControl {

        public bool SelectorType { get; set; }

        private IElementSelector<User> _selector = null;
        public IElementSelector<User> Selector {
            get { return _selector; }
            set {
                RemoveListSelector(); // вызываем отдельно для того, чтобы почистить коллекцию от старого селектора
                _selector = value;
                UpdateState(_state);
            }
        }

        private FilteredList<User> _list = null;
        public FilteredList<User> UserList {
            get { return _list; }
            set {
                RemoveListSelector();
                _list = value;
                UpdateState(_state);
            }
        }

        private void UpdateState(int new_state) {
            // Проверка на равенство состояний не проводится с той целью, чтобы можно было обновить селектор/коллекцию и обновить стейт этой коллекции
            if (UserList != null) {
                RemoveListSelector();
                AddListSelector(new_state);
            }

            _state = new_state;
        }

        private void AddListSelector(int new_state) {
            switch (new_state) {
                case 1:
                    if (_selector != null && UserList != null) {
                        if (SelectorType) {
                            UserList.AddSelector(_selector);
                        } else {
                            UserList.AddDeselector(_selector);
                        }
                    }
                    break;

                case 2:
                    if (_selector != null && UserList != null) {
                        if (!SelectorType) {
                            UserList.AddSelector(_selector);
                        } else {
                            UserList.AddDeselector(_selector);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void RemoveListSelector() {
            switch (_state) {
                case 1:
                    if (_selector != null && UserList != null) {
                        if (SelectorType) {
                            UserList.RemoveSelector(_selector);
                        } else {
                            UserList.RemoveDeselector(_selector);
                        }
                    }
                    break;

                case 2:
                    if (_selector != null && UserList != null) {
                        if (!SelectorType) {
                            UserList.RemoveSelector(_selector);
                        } else {
                            UserList.RemoveDeselector(_selector);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        

        private int _state = 0;

        public ColorButton() {
            SelectorType = true;
            InitializeComponent();

            (this.Content as FrameworkElement).DataContext = this;
            BColor = Brushes.DarkGray;            
        }
        

        public static DependencyProperty BColorProperty =
            DependencyProperty.Register("BColor", typeof(Brush), typeof(ColorButton));

        public Brush BColor {
            get {
                return (Brush)GetValue(BColorProperty);
            }

            set {
                SetValue(BColorProperty, value);
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            var new_state = (_state + 1) % 3;

            switch (new_state) {
                case 0:
                    BColor = Brushes.DarkGray;
                    break;
                case 1:
                    BColor = Brushes.DarkGreen;
                    break;
                case 2:
                    BColor = Brushes.DarkOrange;
                    break;
                default:
                    break;
            }

            UpdateState(new_state);
        }

        public static DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ColorButton));

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set {
                SetValue(TextProperty, value);
            }
        }
    }
}
