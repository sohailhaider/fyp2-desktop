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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Instructor___Omlate
{
    /// <summary>
    /// Interaction logic for QuizStarted.xaml
    /// </summary>
    public partial class QuizStarted : Window
    {
        DispatcherTimer _timer;
        TimeSpan _time;
        int time;

        public QuizStarted()
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
            _time = TimeSpan.FromSeconds(10);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                label1.Content = _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    this.Close();
                    label.Content = "Now you can close this window";
                    this.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing_Allow);
                    label1.Content = "Ended";
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
        public QuizStarted(int t)
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
            time = t;

            _time = TimeSpan.FromMinutes(time);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                label1.Content = _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    this.Close();
                    label.Content = "Now you can close this window";
                    this.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing_Allow);
                    label1.Content = "Ended";
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
        void Window_Closing_Allow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
