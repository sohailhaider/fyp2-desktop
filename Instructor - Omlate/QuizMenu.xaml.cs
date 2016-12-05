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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Instructor___Omlate
{
    /// <summary>
    /// Interaction logic for QuizMenu.xaml
    /// </summary>
    public partial class QuizMenu : Window
    {
        List<int> durations;
        ChatBox.MainWindow chat;
        public QuizMenu(ChatBox.MainWindow mn)
        {
            InitializeComponent();
            chat = mn;
            durations = new List<int>();
            var client = new RestSharp.RestClient("http://"+Config.WebHostIP+":" + Config.WebHostPort + "/omlate");
            var request = new RestSharp.RestRequest("/Instructor/MyCourses", RestSharp.Method.GET);
            request.AddParameter("instructorID", Properties.Settings.Default.username);
            RestSharp.IRestResponse response = client.Execute(request);
            var content = response.Content;
            var data = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(content);
            List<ComboBoxPairs> source = new List<ComboBoxPairs>();
            foreach (var d in data)
            {
                source.Add(new ComboBoxPairs(d["CourseCode"] + " - " + d["CourseTitle"], d["OfferedCourseID"]));
            }
            this.comboBox.DisplayMemberPath = "_Key";
            this.comboBox.SelectedValuePath = "_Value";
            this.comboBox.ItemsSource = source;
            this.comboBox.SelectedIndex = 0;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex < 0 || comboBox1.SelectedIndex < 0)
            {
                label2.Content = "Please Select Any Quiz First!";
            } else
            {
                QuizStarted qs = new QuizStarted(durations.ElementAt(comboBox1.SelectedIndex));
                chat.sendQuizStartedMsg(comboBox1.SelectedValue.ToString());
                qs.Show();
                this.Close();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String s = this.comboBox.SelectedValue.ToString();
            var client = new RestSharp.RestClient("http://"+Config.WebHostIP+ ":" + Config.WebHostPort + "/omlate");
            var request = new RestSharp.RestRequest("/Instructor/getQuizsbyOfferedCourseID", RestSharp.Method.GET);
            request.AddParameter("OfferedCourseID", s);
            RestSharp.IRestResponse response = client.Execute(request);
            var content = response.Content;
            var data = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(content);
            List<ComboBoxPairs> source = new List<ComboBoxPairs>();
            durations = new List<int>();
            foreach (var d in data)
            {
                source.Add(new ComboBoxPairs(d["quizTitle"], d["quizId"]));
                durations.Add(int.Parse(d["quizDuration"]));
            }
            this.comboBox1.DisplayMemberPath = "_Key";
            this.comboBox1.SelectedValuePath = "_Value";
            this.comboBox1.ItemsSource = source;
            this.comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBox1.SelectedIndex > -1)
            {
                label4.Content = durations.ElementAt(comboBox1.SelectedIndex) + " mints";
            }
            else
            {
                label4.Content = "--";
            }
        }
    }
    public class ComboBoxPairs
    {
        public string _Key { get; set; }
        public string _Value { get; set; }

        public ComboBoxPairs(string _key, string _value)
        {
            _Key = _key;
            _Value = _value;
        }
    }
}
