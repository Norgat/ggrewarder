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

using GGConnector;

namespace Rewarder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Connect(object sender, RoutedEventArgs e) {
            var streamer = StreamerNameInput.Text;

            try {
                var id = GG.GetChannelId(streamer);
                MessageBox.Show(string.Format("Stream id: {0}", id));
            } catch (Exception) {
                MessageBox.Show("Не удалось получить Id стрима. Проверьте введённый ник стримера.");                
            }
        }
    }
}
