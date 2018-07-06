
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

using System.Management;
using Microsoft.Win32;

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
            GetBluetoothID();
        }

        private void GetBluetoothID()
        {
            // COM番号を抜き出すための準備
            Regex regexPortName = new Regex(@"(COM\d+)");
            ManagementObjectSearcher searchSerial = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");  // デバイスマネージャーから情報を取得するためのオブジェクト

            // デバイスマネージャーの情報を列挙する
            foreach (ManagementObject obj in searchSerial.Get())
            {
                //10:f0:05:75:54:71-d0:7e:35:3a:24:28
                //Console.WriteLine();
                //Console.WriteLine(obj.ToString());
                string name = obj["Name"] as string; // デバイスマネージャーに表示されている機器名
                string classGuid = obj["ClassGuid"] as string; // GUID
                string devicePass = obj["DeviceID"] as string; // デバイスインスタンスパ
                if (devicePass.Contains("BLUETOOTH_"))
                {
                    Console.WriteLine(devicePass);
                    return;
                }
                else
                {
                    continue;
                }

                    /*
                    if (devicePass.Contains("BLUETOOTH_"))
                    {
                        Console.WriteLine(devicePass);
                        return;
                    }
                    else
                    {
                        return;
                    }
                    */

                    if (name != null)
                    if (name.Contains("Bluetooth"))
                    {
                        Console.WriteLine("devicePass" + devicePass);
                        //Console.WriteLine("name:" + name + " classGuid:" + classGuid + " devicePass" + devicePass);
                    }

                if (classGuid != null && devicePass != null)
                {
                    // デバイスインスタンスパスからBluetooth接続機器のみを抽出
                    // {4d36e978-e325-11ce-bfc1-08002be10318}はBluetooth接続機器を示す固定値
                    if (String.Equals(classGuid, "{4d36e978-e325-11ce-bfc1-08002be10318}",StringComparison.InvariantCulture))
                    {
                        Console.WriteLine(devicePass);
                        // デバイスインスタンスパスからデバイスIDを2段階で抜き出す
                        string[] tokens = devicePass.Split('&');
                        string[] addressToken = tokens[4].Split('_');

                        string bluetoothAddress = addressToken[0];
                        Match m = regexPortName.Match(name);
                        string comPortNumber = "";
                        if (m.Success)
                        {
                            // COM番号を抜き出す
                            comPortNumber = m.Groups[1].ToString();
                        }

                        if (Convert.ToUInt64(bluetoothAddress, 16) > 0)
                        {
                            string bluetoothName = GetBluetoothRegistryName(bluetoothAddress);
                            Console.WriteLine(bluetoothName);
                        }
                        // bluetoothNameが接続機器名
                        // comPortNumberが接続機器名のCOM番号
                    }
                }
            }
        }

        /// <summary>機器名称取得</summary> 
        /// <param name="address">[in] アドレス</param> 
        /// <returns>[out] 機器名称</returns> 
        private string GetBluetoothRegistryName(string address)
        {
            string deviceName = "";
            // 以下のレジストリパスはどのPCでも共通
            string registryPath = @"SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices";
            string devicePath = String.Format(@"{0}\{1}", registryPath, address);

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(devicePath))
            {
                if (key != null)
                {
                    Object o = key.GetValue("Name");

                    byte[] raw = o as byte[];

                    if (raw != null)
                    {
                        // ASCII変換
                        deviceName = Encoding.ASCII.GetString(raw);
                    }
                }
            }
            // NULL文字をトリミングしてリターン
            return deviceName.TrimEnd('\0');
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