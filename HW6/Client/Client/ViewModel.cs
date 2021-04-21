using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using HW6T1;

namespace Gui
{
    /// <summary>
    /// View Model for client gui.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private string ip;
        private string port;
        private Client client;
        public event PropertyChangedEventHandler PropertyChanged;
        private string serverPath = ".";
        private string downloadDestination = "../../../../reposit";

        /// <summary>
        /// Previous files and folders.
        /// </summary>
        private Stack<ObservableCollection<String>> history = new Stack<ObservableCollection<string>>();

        
        /// <summary>
        /// Contains info about names of downloaded files and their status.
        /// </summary>
        public ObservableCollection<string> Downloads { get; set; } = new ObservableCollection<string>();
        
        /// <summary>
        /// Contaion all files and folders.
        /// </summary>
        public ObservableCollection<string> AllData { get; set; } = new ObservableCollection<string>();

        private List<bool> isDir = new List<bool>();

        private Stack<List<bool>> isDirHistory = new Stack<List<bool>>();

        private string selectedData;

        private string isEnabledBackButton = "False";

        private string isEnabledDownloadAllButton = "False";
        
        private string isEnabledShowButton = "False";

        private string isEnabledConnectButton = "False";

        /// <summary>
        /// Is button that provides connection enable.
        /// </summary>
        public string IsEnabledConnectButton
        {
            get => this.isEnabledConnectButton;
            set
            {
                this.isEnabledConnectButton = value;
                OnPropertyChanged("IsEnabledConnectButton");
            }
        }

        /// <summary>
        /// Is button that gives an opportunity to download all enabled.
        /// </summary>
        public string IsEnabledDownloadAllButton
        {
            get => this.isEnabledDownloadAllButton;
            set
            {
                this.isEnabledDownloadAllButton = value;
                OnPropertyChanged("IsEnabledDownloadAllButton");
            }
        }

        /// <summary>
        /// Is button that gives an opportunity to go back enabled.
        /// </summary>
        public string IsEnabledBackButton
        {
            get => this.isEnabledBackButton;
            set
            {
                this.isEnabledBackButton = value;
                OnPropertyChanged("IsEnabledBackButton");
            }
        }

        /// <summary>
        /// Is button that shows content in current path enabled.
        /// </summary>
        public string IsEnabledShowButton
        {
            get => this.isEnabledShowButton;
            set
            {
                this.isEnabledShowButton = value;
                OnPropertyChanged("IsEnabledShowButton");
            }
        }


        /// <summary>
        /// Selected folder in the list box.
        /// </summary>
        public string SelectedData
        {
            get => this.selectedData;
            set
            {
                this.selectedData = value;
                OnPropertyChanged("SelectedData");
            }
        }

        /// <summary>
        /// Contains ip information.
        /// </summary>
        public string Ip
        {
            get => this.ip;
            set
            {
                if (this.client == null)
                {
                    ip = value;
                    OnPropertyChanged("Ip");
                }
                else
                {
                    MessageBox.Show("Impossible to change connection");
                }
            }
        }

        /// <summary>
        /// Contains information about port.
        /// </summary>
        public string Port
        {
            get => this.port;
            set
            {
                if (this.client == null)
                {
                    port = value;
                    OnPropertyChanged("Port");
                }
                else
                {
                    MessageBox.Show("Impossible to change connection");
                }
            }
        }

        private string selectedFile;

        public string SelectedFile
        {
            get => this.selectedFile;
            set
            {
                this.selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }

        public ViewModel()
        {

        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Establish connection.
        /// </summary>
        public void EstablishConnection()
        {
            try
            {
                this.client = new Client(ip, int.Parse(port));
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect input!");
                return;
            }

            UpdateList();
        }

        /// <summary>
        /// Update list of files and folders.
        /// </summary>
        public void UpdateList()
        {
            if (this.client == null)
            {
                MessageBox.Show("No connection");
                return;
            }
            
            try
            {
                var pathToServer = serverPath;
                Task t = Task.Factory.StartNew(() =>
                {
                    return this.client.List(pathToServer);
                }).ContinueWith((task) =>
                {
                    try
                    {
                        if (task.Result == null)
                        {
                            return;
                        }

                        this.AllData.Clear();
                        this.isDir.Clear();

                        foreach (var item in task.Result.Result)
                        {
                            this.AllData.Add(item.Item1);
                            this.isDir.Add(item.Item2);

                            this.IsEnabledDownloadAllButton = "True";
                            this.IsEnabledShowButton = "True";
                        }

                        this.history.Clear();
                        this.history.Push(this.AllData);
                        this.isDirHistory.Clear();
                        this.isDirHistory.Push(this.isDir);
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Impossible to data from this dir");
                        return;
                    }
                    catch (AggregateException)
                    {
                        MessageBox.Show("Impossible to connect. Try again.");
                        this.client = null;
                        this.IsEnabledDownloadAllButton = "False";
                        this.IsEnabledShowButton = "False";
                        return;
                    }

                }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()); 
            }
            catch (AggregateException)
            {
                MessageBox.Show("Impossible to connect.");
                return;
            }
            catch (SocketException)
            {
                MessageBox.Show("Impossible to connect.");
                return;
            }

        }

        /// <summary>
        /// Simple file system navigation. 
        /// </summary>
        public async void GetIntoFolder()
        {
            if (!isDir[this.AllData.IndexOf(this.selectedData)])
            {
                await this.DownloadFile();
                return;
            }
            try
            {
                var data = await this.client.List(this.selectedData);
                var archAllData = new ObservableCollection<string>();
                var archFiles = new ObservableCollection<string>();
                var archIsDir = new List<bool>();
                foreach (var item in this.AllData)
                {
                    archAllData.Add(item);
                }
                foreach (var item in this.isDir)
                {
                    archIsDir.Add(item);
                }

                this.history.Push(archAllData);
                this.isDirHistory.Push(archIsDir);
                this.AllData.Clear();
                
                this.isDir.Clear();
                foreach (var item in data)
                {
                    this.AllData.Add(item.Item1);
                    this.isDir.Add(item.Item2);
                }
                this.IsEnabledBackButton = "True";
            }
            catch (SocketException)
            {
                MessageBox.Show("Impossible to connect.");
            }
        }

        /// <summary>
        /// Get back.
        /// </summary>
        public void Back()
        {
            this.AllData.Clear();
            this.isDir.Clear();
            try
            {
                foreach (var item in this.history.Pop())
                {
                    this.AllData.Add(item);
                }

                foreach (var item in this.isDirHistory.Pop())
                {
                    this.isDir.Add(item);
                }

                if (this.history.Count == 1)
                {
                    this.IsEnabledBackButton = "False";
                    return;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Impossible operation");
            }
            
        }

        /// <summary>
        /// Download file from path to path.
        /// </summary>
        public async Task DownloadFile()
        {
            try
            {
                if (!Directory.Exists(downloadDestination))
                {
                    Directory.CreateDirectory(downloadDestination);
                }
                this.Downloads.Add(Path.GetFileName(this.selectedData) + " installing");
                
                await client.Get(this.SelectedData, downloadDestination);
                this.Downloads.Remove(Path.GetFileName(this.selectedData) + " installing");
                this.Downloads.Add(Path.GetFileName(this.selectedData));
                MessageBox.Show($"Your file was downloaded at: {this.downloadDestination}");
            }
            catch (SocketException)
            {
                MessageBox.Show("You are not connected to the server");
            }
        }

        private async Task DownloadFromTo(string downloadFrom, string downloadTo)
        {
            try
            {
                if (!Directory.Exists(downloadTo))
                {
                    Directory.CreateDirectory(downloadTo);
                }
                await client.Get(downloadFrom, downloadTo); 

            }
            catch (SocketException)
            {
                MessageBox.Show("You are not connected to the server");
            }
        }

        /// <summary>
        /// Download all from the current directory.
        /// </summary>
        public void DownloadAll()
        {
            var tasks = new List<Task>();
            for (int iter = 0; iter < this.AllData.Count; iter++)
            {
                var li = iter;
                if (!isDir[li])
                {
                    var downloadFrom = AllData[li];
                    if (!Directory.Exists(this.downloadDestination))
                    {
                        Directory.CreateDirectory(this.downloadDestination);
                    }
                    Parallel.Invoke(async () => await this.DownloadFromTo(downloadFrom, this.downloadDestination));
                    this.Downloads.Add(Path.GetFileName(AllData[li]));
                }
            }
        }
    }
}
