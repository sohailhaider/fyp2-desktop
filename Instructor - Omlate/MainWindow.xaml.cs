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
        public MainWindow()
        {
            InitializeComponent();
            pptloader = new PPTFileLoader();
            pptloader.ChangeToProjecterView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Toolbar toolbar = new Toolbar();
            //toolbar.Show();

            SlideShow slider = new SlideShow();
            slider.ShowDialog();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Toolbar toolbar = new Toolbar();
            //toolbar.Show();
            //this.Close();
        }
    }
}
