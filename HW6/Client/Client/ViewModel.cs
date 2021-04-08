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
        private string serverPath;
        private string downloadFrom;
        private string downloadTo = "../../../../reposit";

        /// <summary>
        /// Previous files and folders.
        /// </summary>
        private Stack<ObservableCollection<String>> history = new Stack<ObservableCollection<string>>();

        /// <summary>
        /// Previous files to download.
        /// </summary>
        private Stack<ObservableCollection<string>> filesArchive = new Stack<ObservableCollection<string>>();

        /// <summary>
        /// Contains info about names of downloaded files and their status.
        /// </summary>
        public ObservableCollection<string> Downloads { get; set; } = new ObservableCollection<string>();
        
        /// <summary>
        /// Contaion all files and folders.
        /// </summary>
        public ObservableCollection<string> AllData { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Contains files that we can download.
        /// </summary>
        public ObservableCollection<string> files { get; set; } = new ObservableCollection<string>();

        private string selectedFolder;

        private string isEnabledBackButton = "False";

        private string isEnabledDownloadAllButton = "False";

        public string isEnabledShowButton = "False";

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
        /// Is button that gives an opportunuti to go back enabled.
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
        public string SelectedFolder
        {
            get => this.selectedFolder;
            set
            {
                this.selectedFolder = value;
                OnPropertyChanged("SelectedFolder");
            }
        }

        /// <summary>
        /// In case if we want to download file we should specify the DownloadFrom path.
        /// </summary>
        public string DownloadFrom
        {
            get => this.downloadFrom;
            set
            {
                this.downloadFrom = value;
                OnPropertyChanged("DownloadFrom");
            }
        }

        /// <summary>
        /// In case if we want to download file we should specify the DownloadTo path.
        /// </summary>
        public string DownloadTo
        {
            get => this.downloadTo;
            set
            {
                this.downloadTo = value;
                OnPropertyChanged("DownloadTo");
            }
        }

        /// <summary>
        /// Servers shows files and folders by this path. 
        /// </summary>
        public string ServerPath
        {
            get => this.serverPath;
            set
            {
                this.serverPath = value;
                OnPropertyChanged("ServerPath");
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
            catch (Exception)
            {
                MessageBox.Show("Connection failed!");
                return;
            }

            UpdateList();
            this.IsEnabledDownloadAllButton = "True";
            this.IsEnabledShowButton = "True";
        }


        /// <summary>
        /// Update list of files and folders.
        /// </summary>
        public void UpdateList()
        {
            this.AllData.Clear();
            this.files.Clear();
            if (this.client == null)
            {
                MessageBox.Show("No connection");
                return;
            }
            
            try
            {
                if (serverPath == "" || this.serverPath == null)
                {
                    this.serverPath = ".";
                }
                var pathToServer = serverPath;
                Task t = Task.Factory.StartNew(() =>
                {
                    return this.client.List(pathToServer);
                }).ContinueWith((task) =>
                {
                    try
                    {
                        foreach (var item in task.Result.Result)
                        {
                            this.AllData.Add(item.Item1);
                            if (!item.Item2)
                            {
                                this.files.Add(item.Item1);
                            }
                        }
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
            
            this.history.Clear();
            this.history.Push(this.AllData);
        }

        /// <summary>
        /// Simple file system navigation. 
        /// </summary>
        public async void GetIntoFolder()
        {
            try
            {
                var data = await this.client.List(this.selectedFolder);
                var archAllData = new ObservableCollection<string>();
                var archFiles = new ObservableCollection<string>();
                foreach (var item in this.AllData)
                {
                    archAllData.Add(item);
                }
                foreach (var item in this.files)
                {
                    archFiles.Add(item);
                }

                this.history.Push(archAllData);
                this.filesArchive.Push(archFiles);
                this.AllData.Clear();
                this.files.Clear();
                foreach (var item in data)
                {
                    this.AllData.Add(item.Item1);
                    if (!item.Item2)
                    {
                        this.files.Add(item.Item1);
                    }
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
            this.files.Clear();
            try
            {
                foreach (var item in this.history.Pop())
                {
                    this.AllData.Add(item);
                }

                foreach (var item in this.filesArchive.Pop())
                {
                    this.files.Add(item);
                }

                if (this.history.Count == 1)
                {
                    this.IsEnabledBackButton = "False";
                    return;
                }
            }
            catch (Exception)
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
                if (!Directory.Exists(downloadTo))
                {
                    Directory.CreateDirectory(downloadTo);
                }
                this.Downloads.Add(Path.GetFileName(this.selectedFile) + " installing");
                
                await client.Get(this.SelectedFile, downloadTo);
                this.Downloads.Remove(Path.GetFileName(this.selectedFile) + " installing");
                this.Downloads.Add(Path.GetFileName(this.selectedFile));
                MessageBox.Show($"Your file was downloaded at: {this.downloadTo}");
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
        public async Task DownloadAll()
        {
            var tasks = new List<Task>();
            for (int iter = 0; iter < this.AllData.Count; iter++)
            {
                var li = iter;
                if (File.Exists(AllData[li]))
                {
                    this.downloadFrom = AllData[li];
                    this.downloadTo = "../../../../destination";
                    if (!Directory.Exists(downloadTo))
                    {
                        Directory.CreateDirectory(downloadTo);
                    }
                    await Task.Run(() => this.DownloadFromTo(this.downloadFrom, "../../../../destination"));
                    this.Downloads.Add(Path.GetFileName(AllData[li]));
                }
            }
        }
    }
}
