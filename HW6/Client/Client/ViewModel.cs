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
        private Server server;
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
                ip = value;
                OnPropertyChanged("Ip");
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
                port = value;
                OnPropertyChanged("Port");
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
                this.server = new Server(ip, int.Parse(port));
                this.client = new Client(ip, int.Parse(port));
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed");
                return;
            }
            _ = server.Process();
            await UpdateList();
        }

        /// <summary>
        /// Update list of files and folders.
        /// </summary>
        public async Task UpdateList()
        {
            this.AllData.Clear();
            if (serverPath == "")
            {
                MessageBox.Show("Input path!");
                return;
            }
            var updatedData = await this.client.List(serverPath);
            foreach (var item in updatedData)
            {
                this.AllData.Add(item.Item1);
            }
            this.history.Clear();
            this.history.Push(this.AllData);
        }

        /// <summary>
        /// Simple file system navigation. 
        /// </summary>
        public async void GetIntoFolder(string path)
        {
            try
            {
                var data = await this.client.List(path);
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
                }
            }
            await Task.CompletedTask;
        }
    }
}
