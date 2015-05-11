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

namespace Rewarder.Controls {
    /// <summary>
    /// Interaction logic for ColorButton.xaml
    /// </summary>
    public partial class ColorButton: UserControl {

        private int _state = 0;

        public ColorButton() {
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
            _state = (_state + 1) % 3;

            switch (_state) {
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
