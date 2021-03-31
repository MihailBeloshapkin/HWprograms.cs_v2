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
    public class ViewModel : INotifyPropertyChanged
    {
        private string ip = "127.0.0.1";
        private int port = 8888;
        private Server server;
        private Client client;
        public event PropertyChangedEventHandler PropertyChanged;
        public string serverPath;

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


        public string Ip
        {
            get => ip;
            set
            {
                ip = value;
                OnPropertyChanged("Ip");
            }
        }

        public int Port
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

        public void EstablishConnection(string ip, string port)
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
            UpdateList(serverPath);
        }

        /// <summary>
        /// Update list of files and folders.
        /// </summary>
        public async void UpdateList(string path)
        {
            this.AllData.Clear();
            if (path == "")
            {
                MessageBox.Show("Input path!");
                return;
            }
            var updatedData = await this.client.List(path);
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
        public async Task DownloadFile(string downloadFrom, string downloadTo)
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

        /// <summary>
        /// Download all from the current directory.
        /// </summary>
        public Task DownloadAll()
        {
            var tasks = new List<Task>();
            for (int iter = 0; iter < this.AllData.Count; iter++)
            {
                var li = iter;
                if (File.Exists(AllData[li]))
                {
                    tasks.Add(new Task(async () =>
                    {
                        await this.DownloadFile(AllData[li], "../../../../destination");
                    }));
                }
            }

            foreach (var item in tasks)
            {
                item.RunSynchronously();
            }
            return Task.CompletedTask;
        }
    }
}
