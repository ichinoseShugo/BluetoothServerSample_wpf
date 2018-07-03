using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;


namespace BluetoothServerSample_wpf
{
    public partial class MainWindow : Window
    {
        List<BluetoothServer> bServerList = new List<BluetoothServer>();
        /// <summary>
        /// 接続されるデバイスの順番を表すID
        /// </summary>
        int deviceOerder = 0;
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BluetoothServer bluetoothServer = new BluetoothServer();
            bluetoothServer.Listen(deviceOerder);
            bServerList.Add(bluetoothServer);
            deviceOerder++;
            //各種ボタンを使用可能に
            ListenButton.IsEnabled = true;
            DisconnectButton.IsEnabled = true;
            ReadButton.IsEnabled = true;
            SendButton.IsEnabled = true;
            PingButton.IsEnabled = true;
        }
        private void SendButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bServerList[0].Send();
        }
        private void ReadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bServerList[0].Receive();
        }
        private void PingButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bServerList[0].Ping(0);
        }
        private void DisconnectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //bluetoothServer.Disconnect();
            for (int i = 0; i < 50; i++)
                MessageBox.Show("" + bServerList[0].delayTimeList[i]);
            bServerList[0].delayTimeList.Clear();
        }

        private void Player1_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("プレイヤー1接続");
        }
        private void Player2_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("プレイヤー2接続");
        }
        private void Player3_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("プレイヤー3接続");
        }

        public async void Player_Connect(int PlayerId)
        {
            await Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    if (PlayerId == 0)
                                    {
                                        Player1.IsChecked = true;
                                    }
                                    else if (PlayerId == 1)
                                        Player2.IsChecked = true;
                                    else if (PlayerId == 2)
                                        Player3.IsChecked = true;
                                })
                        );
        }
    }
}