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
using Client;
using Generated;
using Grpc.Core;

namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , IObserver
    {
        const string Host = "localhost";
        const int Port = 16842;

        Channel channel = new Channel($"{Host}:{Port}", ChannelCredentials.Insecure);

        LogInRequest client = new LogInRequest();
        ChatRequest clientMessage = new ChatRequest();

        bool isConnected = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AppendLineToChatBox(string message)
        {
            chatbox.Dispatcher.BeginInvoke(new Action<string>((messageToAdd) =>
            {
                chatbox.AppendText(messageToAdd + "\n");
                chatbox.ScrollToEnd();
            }), new object[] { message });
        }

        private async Task InitializeChatAsync()
        {
            chatbox.Clear();

            var chat = new ChatService.ChatServiceClient(channel);
            var stream = chat.chatStream(clientMessage);

            string message;

            while (await stream.ResponseStream.MoveNext())
            {
                var serverMessage = stream.ResponseStream.Current;

                message = serverMessage.Name + " says: " + serverMessage.Textmessage;

                AppendLineToChatBox(message);
            }

        }

        ///------------------> Buttons <------------------//////

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {

            if (txt_name.Text.ToString() == "")
                MessageBox.Show("Ups! You forgot to set your name!", "Error!");
            else
            {
                if (isConnected == true)
                    MessageBox.Show("Ups! You are already connected!", "Error!");
                else
                {
                    client.Name = txt_name.Text.ToString();
                    var logIn = new ChatService.ChatServiceClient(channel);
                    logIn.logIn(client);

                    //ChatService chat = new ChatService();
                    //chat.Subscribe();

                    isConnected = true;
                }
            }
        }
        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected == false)
                MessageBox.Show("Ups! You forgot to connect!", "Error!");
            else
            {
                if (txt_message.Text.ToString() == "")
                    MessageBox.Show("Ups! You forgot to enter your message!", "Error!");
                else
                {
                    clientMessage.Name = client.Name;
                    clientMessage.Textmessage = txt_message.Text.ToString();

                    var chat = new ChatService.ChatServiceClient(channel);
                    chat.sendMessage(clientMessage);

                    InitializeChatAsync();
                }
            }

        }

        public void Update()
        {
            InitializeChatAsync();
        }
    }
}
