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
    public class ViewModel //: INotifyPropertyChanged
    {
        private static string defaultIP = "127.0.0.1";
        private static int defaultPort = 8888;
        private string ip = defaultIP;
        private int port = defaultPort;
        private Server server;
        private Client client;
        public event PropertyChangedEventHandler PropertyChanged;
        public string serverPath;
        public string currentServerPath;
        public string pathToSaveFiles;
        private Stack<string> openFolder = new Stack<string>();

        private Stack<ObservableCollection<String>> history = new Stack<ObservableCollection<string>>();

        /// <summary>
        /// Contains info about names of downloaded files and their status.
        /// </summary>
        public ObservableCollection<string> Downloads { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DirectoriesAndFiles { get; set; } = new ObservableCollection<string>();


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

        private static string defaultPathToDownLoad;
        private string pathToDownload = defaultPathToDownLoad;

        public string PathToDownload
        {
            get => this.pathToDownload;
            set
            {
                pathToDownload = value;
                OnPropertyChanged("PathToDownload");
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

        public void Connection(string ip, string port)
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

        public async void UpdateList(string path)
        {
            this.DirectoriesAndFiles.Clear();
            if (path == "")
            {
                MessageBox.Show("Input path!");
                return;
            }
            var updatedData = await this.client.List(path);
            foreach (var item in updatedData)
            {
                this.DirectoriesAndFiles.Add(item.Item1);
            }
            this.history.Clear();
            this.history.Push(this.DirectoriesAndFiles);
        }

        public async void GetIntoFolder(string path)
        {
            try
            {
                var data = await this.client.List(path);
                var arch = new ObservableCollection<string>();
                foreach (var item in this.DirectoriesAndFiles)
                {
                    arch.Add(item);
                }
                this.history.Push(arch);
                this.DirectoriesAndFiles.Clear();
                foreach (var item in data)
                {
                    this.DirectoriesAndFiles.Add(item.Item1);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible operation");
            }
        }

        public void Back()
        {
            this.DirectoriesAndFiles.Clear();
            try
            {
                foreach (var item in this.history.Pop())
                {
                    this.DirectoriesAndFiles.Add(item);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible operation");
            }
            
        }

        public async Task DownloadFile(string downloadFrom, string downloadTo)
        {
            try
            {
             /*   if (!Directory.Exists(pathToSaveFiles))
                {
                    Directory.CreateDirectory(pathToSaveFiles);
                }*/
                this.Downloads.Add(Path.GetFileName(downloadFrom) + "installing");
            //    DownloadingFiles.Add(path);
                
                await client.Get(downloadFrom, downloadTo);
                this.Downloads.Remove(Path.GetFileName(downloadFrom) + "installing");
                this.Downloads.Add(Path.GetFileName(downloadFrom));
            }
            catch (SocketException)
            {
                MessageBox.Show("You are not connected to the server");
            }
        }

        public Task DownloadAll()
        {
            var tasks = new List<Task>();
            for (int iter = 0; iter < this.DirectoriesAndFiles.Count; iter++)
            {
                var li = iter;
                if (File.Exists(DirectoriesAndFiles[li]))
                {
                    tasks.Add(new Task(async () =>
                    {
                        await this.DownloadFile(DirectoriesAndFiles[li], "../../../../destination");
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
