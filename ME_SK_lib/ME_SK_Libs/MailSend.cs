using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ME_SK_Libs
{
    public class MailSend
    {
        private string _host;
        private string _user;
        private string _pass;
        private int _port;

        private string _mesg;
        private string _send;
        private string _head;
        private string _empf = "gitlabkoehne83@googlemail.com";
        private List<Attachment> _att;

        private string _lastMsg;

        public MailSend(string host = "smtp.gmail.com", int port = 587, string user = "koehne83@googlemail.com", string pass = "MKFidb0290!") {
            this._host = host;
            this._port = port;
            this._user = user;
            this._pass = pass;
            this._att = new List<Attachment>();
        }

        public void AddAttachment(Attachment att) {this._att.Add(att);}
        public void DelAttachment(Attachment att) {this._att.Remove(att);}
        public bool IsAttachment(Attachment att) {return this._att.Contains(att);}
        public void SetSender(string send) {this._send = send;}
        public void SetMessage(string message) {this._mesg = message;}
        public void SetHeader(string header) {this._head = header;}
        public string GetLastMsg() {return this._lastMsg;}

        public void SendEmail() {
            try {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(this._host);
                mail.From = new MailAddress(this._user);
                mail.To.Add(this._empf);
                mail.Subject = this._head;
                mail.Body = this._mesg;
                foreach(Attachment at in this._att)
                    mail.Attachments.Add(at);
                SmtpServer.Port = this._port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(this._user, this._pass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception er)
            {
                this._lastMsg = er.Message;
            }
        }
    }
}
