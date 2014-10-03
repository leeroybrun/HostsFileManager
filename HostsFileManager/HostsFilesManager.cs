using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HostsFileManager
{
    public class HostsFilesManager : INotifyPropertyChanged
    {
        private ObservableCollection<string> _hostsFiles;
        private HostsFile _hostsFileLoaded;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void LoadHostsFilesList()
        {
            string filesDir = Environment.SystemDirectory + "\\drivers\\etc\\";
            string filesPrefix = "hosts";

            this._hostsFiles = new ObservableCollection<string>();

            foreach (string name in Directory.GetFiles(filesDir, filesPrefix + "*"))
            {
                string hostsName;
                hostsName = name.Replace(filesDir, "");

                if (hostsName != "hosts")
                {
                    hostsName = hostsName.Replace(filesPrefix + "_", "");
                }

                this._hostsFiles.Add(hostsName);
            }

            NotifyPropertyChanged("hostsFiles");
        }

        public void LoadHostsFile(string name = "hosts")
        {
            if (name != "hosts")
            {
                name = "hosts_" + name;
            }

            this._hostsFileLoaded = new HostsFile(name);

            NotifyPropertyChanged("hostsFileLoaded");
            NotifyPropertyChanged("hostsEntries");
        }

        // Add a new hosts file
        public void addHostsFile(string name)
        {
            System.IO.File.WriteAllText(Environment.SystemDirectory + "\\drivers\\etc\\hosts_" + name, "");
            this._hostsFiles.Add(name);

            NotifyPropertyChanged("hostsFiles");

            this.LoadHostsFile(name);
        }

        // Duplicate current hosts file
        public void duplicate(string newName = null)
        {
            if (newName == null)
            {
                newName = this.hostsFileLoaded.name + "_copy";
            }

            this.hostsFileLoaded.save(newName);
            this._hostsFiles.Add(newName);

            NotifyPropertyChanged("hostsFiles");

            this.LoadHostsFile(newName);
        }

        // Get the list of host files
        public ObservableCollection<string> hostsFiles
        {
            get
            {
                return this._hostsFiles;
            }
        }

        // Get the host file loaded
        public HostsFile hostsFileLoaded
        {
            get
            {
                return this._hostsFileLoaded;
            }
        }

        // Get the list of host lines for file loaded
        public ObservableCollection<HostsFileLine> hostsEntries
        {
            get
            {
                return this._hostsFileLoaded.entries;
            }
        }
    }
}
