using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace ME_SK_Libs
{
    public class SFTPNet
    {
        private string _host;
        private string _user;
        private string _pass;
        private int _port;

        private string _lastMessage;
        private string _lastResult;
        private string _lastDirectory;
        private string _lastFile;

        private SftpClient _client;

        public SFTPNet(string host = "127.0.0.1", string user = "root", string pass = "root", int port = 22)
        {
            this._host = host;
            this._user = user;
            this._pass = pass;
            this._port = port;
        }


        public void ChangeConnection(string host = "127.0.0.1", string user = "root", string pass = "root", int port = 22)
        {
            if (this._host != host)
                this._host = host;
            if (this._user != user)
                this._user = user;
            if (this._pass != pass)
                this._pass = pass;
            if (this._port != port)
                this._port = port;
        }
        
        public void PutFile(string path, string file, string localPath) {
            if (!this._client.IsConnected)
                this._makeClient();
            try {
                _client.Connect();
                _client.ChangeDirectory(path);
                using (var uploadFile = System.IO.File.OpenRead(localPath+"/"+file)) {
                    _client.UploadFile(uploadFile, file, true);
                }
                this._lastDirectory = localPath + "||" + path;
                this._lastResult = "Sucess";
                this._lastMessage = "none";
            }catch(Exception er) {
                this._lastResult = "Error";
                this._lastMessage = er.Message;
            }
            _closeClient();
        }

        public void GetFile(string serverPath, string file, string localPath) {
            if (!this._client.IsConnected)
                this._makeClient();
            try {
                using (System.IO.Stream fileStream = System.IO.File.Create(System.IO.Path.Combine(localPath, file))){
                    _client.DownloadFile(serverPath + "/" + file, fileStream);
                }
                this._lastResult = "Sucess";
                this._lastMessage = "none";
            }catch(Exception er) {
                this._lastResult = "Error";
                this._lastMessage = er.Message;
            }
            _closeClient();
        }

        public void GetDirectory(string serverPath, string localPath) {
            if (!this._client.IsConnected)
                this._makeClient();
            var files = _client.ListDirectory(serverPath);
            _closeClient();
            foreach (var file in files)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    GetFile(serverPath, file.Name, localPath);
                }
                else if (file.IsSymbolicLink)
                {
                }
                else if (file.Name != "." && file.Name != "..")
                {
                    var dir = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(localPath, file.Name));
                    GetDirectory(file.FullName, dir.FullName);
                }
            }
        }

        private void _makeClient() {
            this._client = new SftpClient(this._host, this._port, this._user, this._pass);
        }

        private void _closeClient() {
            this._client.Disconnect();
        }
    }
}
