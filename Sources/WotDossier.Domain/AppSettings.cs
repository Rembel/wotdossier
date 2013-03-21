using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WotDossier.Domain
{
    public class AppSettings
    {
        private string _language = "ru-RU";
        public string PlayerId { get; set; }
        public string Server { get; set; }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
    }
}
