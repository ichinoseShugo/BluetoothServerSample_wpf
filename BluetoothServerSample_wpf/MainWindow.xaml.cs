using System;
using System.IO;
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
using System.ComponentModel;

using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;


namespace BluetoothServerSample_wpf
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        List<BluetoothServer> bServerList = new List<BluetoothServer>();
        public int id = 0;
        /// 時間計測ストップウォッチ
        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();


        public MainWindow()
        {
            InitializeComponent();
            Closing += WindowClosing;
        }

        private void ListenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BluetoothServer bluetoothServer = new BluetoothServer();
            bluetoothServer.listen(id);
            bServerList.Add(bluetoothServer);
            id++;

            ListenButton.IsEnabled = true;
            DisconnectButton.IsEnabled = true;
            ReadButton.IsEnabled = true;
            SendButton.IsEnabled = true;
            PingButton.IsEnabled = true;

        }





        private void SendButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bServerList[0].send();
        }
        private void ReadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bServerList[0].receive();

        }

        private void PingButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bServerList[0].ping(0);

        }




        private void DisconnectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //bluetoothServer.Disconnect();
            for (int i = 0; i < 50; i++)
                MessageBox.Show("" + bServerList[0].DelayTimeList[i]);
            bServerList[0].DelayTimeList.Clear();
        }


        /// <summary>
        /// 初期化処理(Kinectセンサーやバッファ類の初期化)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Windowが閉じる時に呼び出されるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {

        }

        private void Player1_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("プレイヤー1接続");
        }

        private void Player2_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("プレイヤー2接続");
        }

        private void Player3_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("プレイヤー3接続");
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
    }//MainWindow
}//namespace