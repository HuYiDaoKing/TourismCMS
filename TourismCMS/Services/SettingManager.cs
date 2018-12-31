using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Services
{
    public class SettingManager : Dictionary<string, string>
    {
        private static SettingManager _Settings = null;
        public static SettingManager Settings
        {
            get
            {
                if (_Settings == null)
                    _Settings = new SettingManager();
                return _Settings;
            }
        }

        private SettingManager()
        {
            //Init Data
            //DataSoure:truely data here...
            var strTempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/bin/Config");

            var oXlement = XElement.Load(strTempPath);
            var oQuery = from t in oXlement.Descendants("setting")
                         select new
                         {
                             Element = t.Name.LocalName,
                             Name = t.Attribute("name").Value,
                             Pwd = t.Attribute("pwd").Value
                         };

            if (oQuery != null)
            {
                if (!this.ContainsKey("username"))
                    this.Add("username", oQuery.Name);

                if (!this.ContainsKey("password"))
                    this.Add("password", oQuery.Pwd);
            }
        }

        public string this[string key]
        {
            get
            {
                if (!this.ContainsKey(key))
                    return String.Empty;
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }

        public static bool GetBoolValue(string key)
        {
            bool value = false;
            bool.TryParse(Settings[key], out value);
            return value;
        }

        public static int GetIntValue(string key)
        {
            int value = 0;
            int.TryParse(Settings[key], out value);
            return value;
        }
    }
}