using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace ME_SK_Libs
{
    public class SSHNet
    {
        private string _host;
        private string _user;
        private string _pass;
        private int _port;

        private string _lastCommand;
        private string _lastResult;

        public SSHNet(string host = "127.0.0.1", string user = "root", string pass = "root", int port = 22)
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

        public string RunCommand(string command)
        {
            string result = "none";
            using (SshClient ssh = new SshClient(this._host, this._port, this._user, this._pass)) {
                try
                {
                    ssh.Connect();
                    var _command = ssh.CreateCommand(command);
                    result = _command.Execute();
                    ssh.Disconnect();
                }
                catch (Exception er)
                {
                    result = er.Message;
                }
            }
            this._lastCommand = command;
            this._lastResult = result;
            return result;
        }

        public string GetLastCommand() { return this._lastCommand; }
        public string GetLastResult() { return this._lastResult; }
    }
}
