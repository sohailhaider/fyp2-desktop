using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ChatBox
{
    public partial class MainWindow : Window
    {
        public String UserName { get; set; }
        public IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://localhost:3825/omlate/signalr";
        public HubConnection Connection { get; set; }

        public MainWindow()
        {
            this.Title = "Instructor Chat Box";
            InitializeComponent();

            UserName = "Instructor";
            ConnectAsync();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            HubProxy.Invoke("Send", "class1",UserName, TextBoxMessage.Text);
            TextBoxMessage.Text = String.Empty;
            TextBoxMessage.Focus();
        }

        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("ChatHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            HubProxy.On<string, string>("addNewMessageToPage", (name, message) =>
                this.Dispatcher.Invoke(() =>
                    RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", name, message))
                )
            );
            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                RichTextBoxConsole.AppendText( "Unable to connect to server: Start server before connecting clients.");
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            //Show chat UI; hide login UI
            //ChatPanel.Visibility = Visibility.Visible;
            ButtonSend.IsEnabled = true;
            TextBoxMessage.Focus();
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");

            await HubProxy.Invoke("Join", "class1");
        }

        /// <summary>
        /// If the server is stopped, the connection will time out after 30 seconds (default), and the 
        /// Closed event will fire.
        /// </summary>
        void Connection_Closed()
        {
            //Hide chat UI; show login UI
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
            dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
        }
        

        private void WPFClient_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
            }
        }
    }
}
