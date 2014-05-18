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
using HeartOfGold.NBT;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;

namespace HeartOfGold.NbtEditorWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		string _saveLoc;
		string SaveLoc
		{
			get { return _saveLoc; }
			set 
			{
				_saveLoc = value;
				PropChanged("FileOpen");
			}
		}

		public bool FileOpen 
		{
			get { return !string.IsNullOrWhiteSpace(SaveLoc); }
		}

		public MainWindow()
		{
			InitializeComponent();
			//this.DataContext = this;
			Root = new ObservableCollection<ListNode>();
			treeView1.ItemsSource = Root;
			ResetNodes();
		}

		private void ResetNodes()
		{
			ListNode rootNode = new ListNode() { Name = "root" };
			//rootNode.Children.Add(new StringNode("test string") { Name = "Test" });
			//List<ListNode> nodes = new List<ListNode>();
			//nodes.Add(rootNode);

			Root.Clear();
			SaveLoc = string.Empty;
			Root.Add(rootNode);

			//Root = new ObservableCollection<ListNode>(nodes);

			//treeView1.ItemsSource = Root;
		}

		private ObservableCollection<ListNode> _root;
		public ObservableCollection<HeartOfGold.NBT.ListNode> Root
		{
			get { return _root; }
			set
			{
				_root = value; 
				PropChanged("Root");
			}
		}

		private void PropChanged(string p)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(p));
		}

		private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "NBT XML Files|*.nbtx";
			dlg.Multiselect = false;
			bool? result = dlg.ShowDialog(this);
			if (!result.HasValue)
				return; 
			else if ((bool)result)
			{
				var mboxres = MessageBox.Show(this, "Would you like to save your current NBT file?", "Opening File...", MessageBoxButton.YesNoCancel);

				if (mboxres == MessageBoxResult.Yes)
				{
					SaveMenuItem_Click(this, null);
				}
				else if (mboxres == MessageBoxResult.Cancel)
					return;

				SaveLoc = dlg.FileName;
				LoadFile();
			}
		}

		private void LoadFile()
		{
			ListNode rootNode = new ListNode(SaveLoc);

			Root.Clear();
			Root.Add(rootNode);
		}

		private void SaveFile()
		{
			if (Root.Count >= 1)
			{
				XmlSerializer b = new XmlSerializer(typeof(NBT.ListNode), "HeartOfGold.NBT");
				XmlWriterSettings set = new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineHandling = NewLineHandling.Replace, NewLineChars = Environment.NewLine };
				XmlWriter s = XmlWriter.Create(SaveLoc, set);

				b.Serialize(s, Root[0]);
				s.Flush();
				s.Close();
				s.Dispose();
			}
		}

		private void NewFileMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			var mboxres = MessageBox.Show(this, "Would you like to save your current NBT file?", "New File...", MessageBoxButton.YesNoCancel);

			if (mboxres == MessageBoxResult.Cancel)
			{
				return;
			}
			else if (mboxres == MessageBoxResult.Yes)
			{
				SaveMenuItem_Click(this, null);
			}

			ResetNodes();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void RefreshMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			if (FileOpen)
				LoadFile();
		}

		private void SaveMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			if (FileOpen)
				SaveFile();
			else
				SaveAsMenuItem_Click(this, null);
		}

		TreeViewItem lastSelectedTreeViewItem;
		private void EditTitleMenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem != null)
			{
				TreeViewItem item = lastSelectedTreeViewItem;
				EditableTextBlock x = FindVisualChildByType<EditableTextBlock>(item);
				if (x.IsEditable)
					x.IsInEditMode = true;
			}
		}

		public static T FindVisualChildByType<T>(DependencyObject parent) where T : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				if (child is EditableTextBlock)
				{
					return child as T;
				}
				else
				{
					T result = FindVisualChildByType<T>(child);
					if (result != null)
						return result;
				}
			}
			return null;
		}

		private void treeView1_Selected(object sender, RoutedEventArgs e)
		{
			lastSelectedTreeViewItem = e.OriginalSource as TreeViewItem;
		}

		private void SaveAsMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.Filter = "NBT XML Files|*.nbtx";
			if (FileOpen)
			{
				dlg.RestoreDirectory = true;
				dlg.InitialDirectory = SaveLoc.Substring(0, SaveLoc.LastIndexOf(System.IO.Path.DirectorySeparatorChar));
			}

			bool? result = dlg.ShowDialog(this);

			if (result == null || !(bool)result)
				return;
			else
			{
				SaveLoc = dlg.FileName;
				SaveFile();
			}
		}

		private void EditValueMenuButton_Click(object sender, RoutedEventArgs e)
		{
			//MessageBox.Show("Do Something");

			EditValue v = new EditValue((Node)treeView1.SelectedItem);
			v.Owner = this;
			v.ShowDialog();
		}

		private void DeleteNodeMenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem != null && treeView1.SelectedItem != Root[0])
			{
				ContainerNode parent = Root[0];

				parent = FindParentNode(parent, treeView1.SelectedItem as Node);

				parent.Children.Remove(treeView1.SelectedItem as Node);

				PropChanged("Root");

				if (parent != Root[0])
					SetSelectedItem(ref treeView1, Root[0], parent);
			}
		}

		private ContainerNode FindParentNode(ContainerNode parent, Node node)
		{
			if (parent.Children.Contains(node))
				return parent;
			else
				foreach (ContainerNode child in parent.Children.Where(o=>o is ContainerNode))
				{
					var ret = FindParentNode(child, node);
					if (ret != null)
						return ret;
				}

			return null;
		}

		private bool GenNodePath(ContainerNode parent, Node node, List<int> ret)
		{
			if (parent.Children.Contains(node))
			{
				ret.Add(parent.Children.IndexOf(node));
				return true;
			}
			else
				foreach (ContainerNode child in parent.Children.Where(o => o is ContainerNode))
				{
					ret.Add(parent.Children.IndexOf(child));
					if (GenNodePath(child, node, ret))
						return true;
					else
					{
						ret.Remove(ret.Count - 1);
						return false;
					}
				}

			return false;
		}

		private void MoveUpMenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem != null && treeView1.SelectedItem != Root[0])
			{
				ContainerNode parent = Root[0];

				parent = FindParentNode(parent, treeView1.SelectedItem as Node);
				Node n = (treeView1.SelectedItem as Node);

				int i = parent.Children.IndexOf(n);

				if (i > 0)
					i--;
				else return;

				parent.Children.Remove(n);
				parent.Children.Insert(i, n);

				PropChanged("Root");

				SetSelectedItem(ref treeView1, Root[0], n);
			}
		}

		private void MoveDownMenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem != null && treeView1.SelectedItem != Root[0])
			{
				ContainerNode parent = Root[0];

				parent = FindParentNode(parent, treeView1.SelectedItem as Node);
				Node n = (treeView1.SelectedItem as Node);

				int i = parent.Children.IndexOf(n);

				if (i != parent.Children.Count - 1)
					i++;
				else return;

				parent.Children.Remove(n);
				parent.Children.Insert(i, n);

				PropChanged("Root");

				SetSelectedItem(ref treeView1, Root[0], n);
			}
		}

		public void SetSelectedItem(ref TreeView control, ContainerNode Root, Node item)
		{
			try
			{
				List<int> path = new List<int>();
				GenNodePath(Root, item, path);


				DependencyObject dObject = control
					.ItemContainerGenerator
					.ContainerFromItem(this.Root[0]);

				foreach (int i in path)
				{
					dObject = (DependencyObject)(dObject as ItemsControl).ItemContainerGenerator.ContainerFromIndex(i);
				}
				

				//uncomment the following line if UI updates are unnecessary
				//((TreeViewItem)dObject).IsSelected = true;                

				MethodInfo selectMethod =
				   typeof(TreeViewItem).GetMethod("Select",
				   BindingFlags.NonPublic | BindingFlags.Instance);

				selectMethod.Invoke(dObject, new object[] { true });
			}
			catch { }
		}

		private void AddListBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new ListNode() { Name = "New List" });
			}
		}

		private void AddStringBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new StringNode() { Name = "New String" });
			}
		}

		private void AddByteBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new ByteNode() { Name = "New Byte" });
			}
		}

		private void AddDoubleBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new DoubleNode() { Name = "New Double" });
			}
		}

		private void AddFloatBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new FloatNode() { Name = "New Float" });
			}
		}

		private void AddIntBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new IntNode() { Name = "New Integer" });
			}
		}

		private void AddLongBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new LongNode() { Name = "New Long" });
			}
		}

		private void SortMenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				ContainerNode node = (treeView1.SelectedItem as ContainerNode);
				var sort = (treeView1.SelectedItem as ContainerNode).Children.OrderBy(o => o.Name).ToArray();

				node.Children.Clear();

				foreach (Node item in sort)
				{
					node.Children.Add(item);
				}
			}
		}

		private void AddObjectBtn_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				(treeView1.SelectedItem as ContainerNode).Children.Insert(0, new ObjectNode() { Name = "New Object" });
			}
		}

		private void CutMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			Clipboard.Clear();

			DataFormat format = DataFormats.GetDataFormat(typeof(Node).FullName);

			//now copy to clipboard
			IDataObject dataObj = new DataObject();
			dataObj.SetData(format.Name, treeView1.SelectedItem, false);
			Clipboard.SetDataObject(dataObj, true);

			DeleteNodeMenuButton_Click(this, null);
		}

		private void CopyMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			Clipboard.Clear();

			DataFormat format = DataFormats.GetDataFormat(typeof(Node).FullName);

			//now copy to clipboard
			IDataObject dataObj = new DataObject();
			dataObj.SetData(format.Name, treeView1.SelectedItem, false);
			Clipboard.SetDataObject(dataObj, true);
		}

		private void PasteMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
		{
			if (treeView1.SelectedItem is ContainerNode)
			{
				ContainerNode n = treeView1.SelectedItem as ContainerNode;
				Node doc = null;
				IDataObject dataObj = Clipboard.GetDataObject();
				string format = typeof(Node).FullName;

				if (dataObj.GetDataPresent(format))
				{
					doc = dataObj.GetData(format) as Node;
				}
				n.Children.Add(doc);
			}
		}
	}

	public class NodeImgConverter: IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is Node)
			{
				string val = value.ToString();
				//MessageBox.Show(val);

				return string.Format("Resources/{0}.png", value);
			}
			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class NullToBoolConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class NodeToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return false;
			else if (value is Node)
				return (value as Node).HasValue;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class ListToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return false;
			else if (value is Node)
				return (value as Node).CanHaveChildren;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
