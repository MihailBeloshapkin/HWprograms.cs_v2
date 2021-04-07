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
        private string downloadTo;

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

        private string selectedFolder;

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
        public async Task EstablishConnection()
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
    
            await UpdateList();
        }


        /// <summary>
        /// Update list of files and folders.
        /// </summary>
        public async Task UpdateList()
        {
            this.AllData.Clear();
            if (this.client == null)
            {
                MessageBox.Show("No connection");
                return;
            }
            if (serverPath == "" || this.serverPath == null)
            {
                MessageBox.Show("Input path!");
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
                        foreach (var item in task.Result.Result)
                        {
                            this.AllData.Add(item.Item1);
                        }
                    }
                    catch (AggregateException)
                    {
                        MessageBox.Show("Impossible to connect.");
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            
            this.history.Clear();
            this.history.Push(this.AllData);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Simple file system navigation. 
        /// </summary>
        public async void GetIntoFolder()
        {
            try
            {
                var data = await this.client.List(this.selectedFolder);
                var arch = new ObservableCollection<string>();
                foreach (var item in this.AllData)
                {
                    arch.Add(item);
                }
                this.history.Push(arch);
                this.AllData.Clear();
                foreach (var item in data)
                {
                    this.AllData.Add(item.Item1);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible operation");
            }
        }

        /// <summary>
        /// Get back.
        /// </summary>
        public void Back()
        {
            this.AllData.Clear();
            try
            {
                foreach (var item in this.history.Pop())
                {
                    this.AllData.Add(item);
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
                this.Downloads.Add(Path.GetFileName(downloadFrom) + "installing");
                
                await client.Get(downloadFrom, downloadTo);
                this.Downloads.Remove(Path.GetFileName(downloadFrom) + "installing");
                this.Downloads.Add(Path.GetFileName(downloadFrom));
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
