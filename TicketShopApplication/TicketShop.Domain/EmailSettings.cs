using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public int SMTPServerPort { get; set; }
        public bool EnableSSL { get; set; }
        public string EmailDisplayName { get; set; }
        public string SenderName { get; set; }

        public EmailSettings()
        {
            
        }

        public EmailSettings(string SMTPServer, string SMTPUserName, string SMTPPassword, int SMTPServerPort)
        {
            this.SMTPServer = SMTPServer;
            this.SMTPUserName = SMTPUserName;
            this.SMTPPassword = SMTPPassword;
            this.SMTPServerPort = SMTPServerPort;
        }
    }
}
