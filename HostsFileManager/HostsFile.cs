using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HostsFileManager
{
    public class HostsFile : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        //private bool _enabled;
        private ObservableCollection<HostsFileLine> _entries;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public HostsFile(string name)
        {
            this.load(name);
        }

        // Load the hosts file associated with the given name
        public void load(string name)
        {
            string line;
            HostsFileLine hostsLine;

            this._entries = new ObservableCollection<HostsFileLine>();

            // Compute file path
            this._path = Environment.SystemDirectory + "\\drivers\\etc\\" + name;

            this._name = name.Replace("hosts_", "");

            System.IO.StreamReader file = new System.IO.StreamReader(this._path);
            while ((line = file.ReadLine()) != null)
            {
                hostsLine = HostsFileLine.FromString(line);

                if(hostsLine != null)
                {
                    this._entries.Add(hostsLine);
                }
            }

            file.Close();

            NotifyPropertyChanged("entries");
        }

        // Save the hosts file
        public void save(string fileName = null)
        {
            if (fileName == null)
            {
                fileName = this.name;
            }

            if (fileName != "hosts")
            {
                fileName = "hosts_" + fileName;
            }

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.SystemDirectory + "\\drivers\\etc\\" + fileName))
                {
                    foreach (HostsFileLine line in this.entries)
                    {
                        file.WriteLine(line.ToString());
                    }
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("An error occurred while saving the file.\nPlease run as Administrator or check your antivirus software.");
            }
        }

        // Add empty entry
        public void addEntry()
        {
            this._entries.Add(new HostsFileLine());
        }

        // Get the list of entries
        public ObservableCollection<HostsFileLine> entries
        {
            get
            {
                return this._entries;
            }
        }

        public string name
        {
            get {
                return this._name;
            }

            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
