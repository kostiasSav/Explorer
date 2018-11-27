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
using System.IO;
using Microsoft.Win32;
using System.Xml;

namespace Explorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach (string driveName in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = driveName;
                item.Tag = driveName;
                item.Items.Add(null);
                item.Expanded += new RoutedEventHandler(TreeViewExpanded);
                FoldersTree.Items.Add(item);
            }
        }
        void TreeViewExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == null)
            {
                item.Items.Clear();
                try
                {
                    foreach (string directoryName in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = directoryName.Substring(directoryName.LastIndexOf("\\") + 1);
                        subitem.Tag = directoryName;
                        subitem.Items.Add(null);
                        subitem.Expanded += new RoutedEventHandler(TreeViewExpanded);
                        item.Items.Add(subitem);
                    }
                    foreach (string fileName in Directory.GetFiles(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        subitem.Tag = fileName;
                        subitem.Expanded += new RoutedEventHandler(TreeViewExpanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception exception)
                {
                    MessageLabel.Content = exception.Message;
                }
            }
        }
        private void FoldersTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var path = FoldersTree.SelectedValue.ToString();
            MessageLabel.Content = (string)path;
            /*if(System.IO.File.Exists(FileInfo.Full))
            {
                FileInfo fileInfo = ((FileInfo)FoldersTree.SelectedItem);
                var text = File.ReadAllText(fileInfo.FullName);
            }*/
        }
    }
}
