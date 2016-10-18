using Instructor___Omlate.ContentLoad;
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

namespace Instructor___Omlate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PPTFileLoader pptloader;
        SlideShow slider;
        public MainWindow()
        {
            InitializeComponent();
            pptloader = new PPTFileLoader();
            pptloader.ChangeToProjecterView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (slider == null)
                slider = new SlideShow();
            slider.ShowDialog();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var name = (String)username.Text;
            var passw = (String)password.Password;
            var client = new RestSharp.RestClient("http://localhost:3825/omlate");
            var request = new RestSharp.RestRequest("/Instructor/validateUsernamePassword", RestSharp.Method.POST);
            request.AddParameter("username", name);
            request.AddParameter("password", passw);
            RestSharp.IRestResponse response = client.Execute(request);
            var content = response.Content;
            bool cont = Convert.ToBoolean(content);
            if (cont)
            {
                Properties.Settings.Default["username"] = name;
                if (slider == null)
                    slider = new SlideShow();
                slider.ShowDialog();

                this.Close();
            } else
            {
                label.Content = "*Invalid Username or Password!";
                Properties.Settings.Default["username"]= "-1";
            }


            Properties.Settings.Default.Save();
            //Toolbar toolbar = new Toolbar();
            //toolbar.Show();
            //this.Close();
        }
    }
}
