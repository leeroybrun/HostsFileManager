using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using System.Text.RegularExpressions;
using System.ComponentModel;

namespace HostsFileManager
{
    public class HostsFileLine : INotifyPropertyChanged
    {
        private string _ip;
        private string _domain;
        private bool _enabled = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static HostsFileLine FromString(string line)
        {
            Match match = Regex.Match(line, @"\s*(#)?(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s+(\S+)", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return new HostsFileLine(match.Groups[2].Value, match.Groups[3].Value, match.Groups[1].Value != "#");
            }
            else
            {
                return null;
            }
        }

        public HostsFileLine(string ip = "", string domain = "", bool enabled = true)
        {
            this._ip = ip;
            this._domain = domain;
            this._enabled = enabled;
        }

        public string ip
        {
            get
            {
                return this._ip;
            }

            set
            {
                if (value != this._ip)
                {
                    this._ip = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string domain
        {
            get
            {
                return this._domain;
            }

            set
            {
                if (value != this._domain)
                {
                    this._domain = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool enabled
        {
            get
            {
                return this._enabled;
            }

            set
            {
                if (value != this._enabled)
                {
                    this._enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return ((this._enabled) ? "" : "#") + this._ip +"\t"+ this._domain;
        }
    }
}
