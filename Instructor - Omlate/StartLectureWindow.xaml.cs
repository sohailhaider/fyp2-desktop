using HandoutsGeneration;
using Newtonsoft.Json;
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

namespace Instructor___Omlate
{
    /// <summary>
    /// Interaction logic for StartLectureWindow.xaml
    /// </summary>
    public partial class StartLectureWindow : Window
    {
        SlideShow slideShow;
        public StartLectureWindow()
        {
            InitializeComponent();
            var client = new RestSharp.RestClient("http://" + Config.WebHostIP + ":" + Config.WebHostPort + "/omlate");
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["courseid"] = comboBox.SelectedValue.ToString();
            this.Hide();
            if (slideShow == null)
                slideShow = new SlideShow();
            slideShow.ShowDialog();
            this.Close();
        }
    }
}
