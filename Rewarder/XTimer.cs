using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Rewarder {
    public class XTimer: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;        
        protected void OnPropertyChanged(string name) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private Timer _timer = null;
        private TimeSpan _ts;


        public XTimer() { }

        private string _time = "00:00:00";

        public string Time {
            get {
                return _time;
            }

            set {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        public void Start() {
            if (_timer == null) {
                Restart();
            } else {
                _timer.Enabled = true;
            }
        }


        public void Stop() {
            if (_timer != null) {
                _timer.Enabled = false;
            }
        }

        public void Restart() {
            if (_timer != null) {
                _timer.Dispose();
            }

            _ts = new TimeSpan(0, 0, 0);
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new ElapsedEventHandler(OnTick);
            _timer.Enabled = true;
            Time = "00:00:00";
        }

        private void OnTick(object sendet, ElapsedEventArgs e) {
            _ts = _ts.Add(TimeSpan.FromSeconds(1));
            Time = string.Format("{0:00}:{1:00}:{2:00}", _ts.Hours, _ts.Minutes, _ts.Seconds);                        
        }
    }
}
