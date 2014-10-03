using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HostsFileManager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private HostsFilesManager manager;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Should load files Environment.SystemDirectory + "\\drivers\\etc\\hosts_" + name; to populate host files list
            // Load default/enabled hosts file
            // Populate hosts lines

            this.manager = new HostsFilesManager();

            this.manager.LoadHostsFilesList();
            this.manager.LoadHostsFile();

            DataContext = this;

            NotifyPropertyChanged("hostsEntries");
            NotifyPropertyChanged("hostsFiles");
            NotifyPropertyChanged("hostsFileLoaded");
        }

        public HostsFile hostsFileLoaded
        {
            get
            {
                return this.manager.hostsFileLoaded;
            }
        }

        public ObservableCollection<HostsFileLine> hostsEntries
        {
            get
            {
                return this.manager.hostsEntries;
            }
        }

        public ObservableCollection<string> hostsFiles
        {
            get
            {
                return this.manager.hostsFiles;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.manager.hostsFileLoaded.save();
        }

        private void makeActiveButton_Click(object sender, RoutedEventArgs e)
        {
            this.manager.makeCurrentActive();
        }

        private void addEntryButton_Click(object sender, RoutedEventArgs e)
        {
            this.manager.hostsFileLoaded.addEntry();
        }

        private void addFileButton_Click(object sender, RoutedEventArgs e)
        {
            Prompt dialog = new Prompt("Please enter the new file name :");
            if (dialog.ShowDialog() == true)
            {
                this.manager.addHostsFile(dialog.ResponseText);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: check if changes on current file has been saved

            this.manager.LoadHostsFile((string)hostsFilesListBox.SelectedValue);
            NotifyPropertyChanged("hostsEntries");
            NotifyPropertyChanged("hostsFileLoaded");
        }

        private void duplicateFileButton_Click(object sender, RoutedEventArgs e)
        {
            Prompt dialog = new Prompt("Please enter the new file name :");
            if (dialog.ShowDialog() == true)
            {
                this.manager.duplicate(dialog.ResponseText);
            }
        }
    }
}
