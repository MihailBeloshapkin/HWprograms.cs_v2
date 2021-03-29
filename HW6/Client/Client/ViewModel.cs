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

        public ObservableCollection<string> DownloadingFiles { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> DownloadedFiles { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> DirectoriesAndFiles { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<bool> IsDirectory { get; set; } = new ObservableCollection<bool>();

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
            }
            _ = server.Process();
            ShowCurrentFoldersAndFilesAsync(serverPath);
        }

        private async void ShowCurrentFoldersAndFilesAsync(string path)
        {
            ClearFileList();
            if (path == "..")
            {
                ShowCurrentFoldersAndFilesAsync(openFolder.Pop());
                return;
            }
            var foldersAndFiles = await client.List(path);
            if (path != serverPath)
            {
                DirectoriesAndFiles.Add("..");
                IsDirectory.Add(true);
                openFolder.Push(currentServerPath);
            }
            currentServerPath = path;
            foreach (var item in foldersAndFiles.Item2)
            {
                DirectoriesAndFiles.Add(item.Item1);
                IsDirectory.Add(item.Item2);
            }
        }

        private void ClearFileList()
        {
            IsDirectory.Clear();
            DirectoriesAndFiles.Clear();
        }

        public void EditListBox(string path)
        {
            ShowCurrentFoldersAndFilesAsync(path);
        }

        public async Task DownloadFile(string path)
        {
            try
            {
                if (!Directory.Exists(pathToSaveFiles))
                {
                    Directory.CreateDirectory(pathToSaveFiles);
                }
                DownloadingFiles.Add(path);
                var fileStream = new MemoryStream();
            //    await client.Get(path, pathToDownload, );
                using var contentStreamReader = new StreamReader(fileStream);
                var content = await contentStreamReader.ReadToEndAsync();
                var currentPath = new DirectoryInfo(path).Name;
                using var textFile = new StreamWriter(pathToSaveFiles + @"\" + currentPath);
                textFile.WriteLine(content);
                DownloadingFiles.Remove(path);
                DownloadedFiles.Add(path);
            }
            catch (SocketException)
            {
                MessageBox.Show("You are not connected to the server");
            }
        }
    }
}
