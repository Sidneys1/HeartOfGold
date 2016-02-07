using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using HeartOfGold.NBT;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using ProtoBuf;

namespace HeartOfGold.NbtEditorWPF {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged {
		string _saveLoc;
		string SaveLoc {
			get { return _saveLoc; }
			set {
				_saveLoc = value;
				PropChanged("FileOpen");
			}
		}

		public bool FileOpen => !string.IsNullOrWhiteSpace(SaveLoc);

		public MainWindow() {
			InitializeComponent();
			//this.DataContext = this;
			Root = new ObservableCollection<Node>();
			TreeView1.ItemsSource = Root;
			ResetNodes();
		}

		private void ResetNodes() {
			var rootNode = new ListNode { Name = "root" };
			//rootNode.Children.Add(new StringNode("test string") { Name = "Test" });
			//List<ListNode> nodes = new List<ListNode>();
			//nodes.Add(rootNode);

			Root.Clear();
			SaveLoc = string.Empty;
			Root.Add(rootNode);

			//Root = new ObservableCollection<ListNode>(nodes);

			//treeView1.ItemsSource = Root;
		}

		private ObservableCollection<Node> _root;
		public ObservableCollection<Node> Root {
			get { return _root; }
			set {
				_root = value;
				PropChanged("Root");
			}
		}

		private void PropChanged(string p) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
		}

		private void OpenMenuItem_Click(object sender, RoutedEventArgs e) {
			var dlg = new OpenFileDialog { Filter = "NBT XML Files|*.nbtx|NBT Binary Files|*.nbt", Multiselect = false };
			var result = dlg.ShowDialog(this);
			if (!(bool)result) return;


			var mboxres = MessageBox.Show(this, "Would you like to save your current NBT file?", "Opening File...", MessageBoxButton.YesNoCancel);

			switch (mboxres) {
				case MessageBoxResult.Yes:
					SaveMenuItem_Click(this, null);
					break;
				case MessageBoxResult.Cancel:
					return;
			}

			SaveLoc = dlg.FileName;
			LoadFile();
		}

		private void LoadFile() {
			ContainerNode rootNode = null;

			switch (Path.GetExtension(SaveLoc)) {
				case @".nbtx":
					{
						try {
							rootNode = ListNode.Deserialize(SaveLoc);
						} catch {
							rootNode = ObjectNode.Deserialize(SaveLoc);
						}
					}
					break;
				case @".nbt":
					{
						using (var file = File.OpenRead(SaveLoc)) {
							rootNode = Serializer.Deserialize<ContainerNode>(file);
						}
					}
					break;
				default:
					return;
			}

			if (rootNode == null) return;
			Root.Clear();
			Root.Add(rootNode);
		}

		private void SaveFile() {
			if (Root.Count < 1) return;

			switch (Path.GetExtension(SaveLoc)) {
				case @".nbtx":
					{
						var b = new XmlSerializer(Root[0].GetType(), @"HeartOfGold.NBT");
						var set = new XmlWriterSettings { Indent = true, IndentChars = "\t", NewLineHandling = NewLineHandling.Replace, NewLineChars = Environment.NewLine };
						using (var s = XmlWriter.Create(SaveLoc, set)) {
							b.Serialize(s, Root[0]);
							s.Flush();
							s.Close();
						}
					}
					break;
				case @".nbt":
					{
						using (var file = File.OpenWrite(SaveLoc)) {
							Serializer.Serialize(file, Root[0]);
						}
					}
					break;
				default:
					return;
			}


			//s.Dispose();
		}

		private void NewFileMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			var mboxres = MessageBox.Show(this, "Would you like to save your current NBT file?", "New File...", MessageBoxButton.YesNoCancel);

			switch (mboxres) {
				case MessageBoxResult.Cancel:
					return;
				case MessageBoxResult.Yes:
					SaveMenuItem_Click(this, null);
					break;
			}

			ResetNodes();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void RefreshMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			if (FileOpen)
				LoadFile();
		}

		private void SaveMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			if (FileOpen)
				SaveFile();
			else
				SaveAsMenuItem_Click(this, null);
		}

		TreeViewItem _lastSelectedTreeViewItem;
		private void EditTitleMenuButton_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem == null) return;
			var item = _lastSelectedTreeViewItem;
			var x = FindVisualChildByType<EditableTextBlock>(item);
			if (x.IsEditable)
				x.IsInEditMode = true;
		}

		public static T FindVisualChildByType<T>(DependencyObject parent) where T : DependencyObject {
			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
				var child = VisualTreeHelper.GetChild(parent, i);
				if (child is EditableTextBlock) {
					return child as T;
				}
				var result = FindVisualChildByType<T>(child);
				if (result != null)
					return result;
			}
			return null;
		}

		private void treeView1_Selected(object sender, RoutedEventArgs e) {
			_lastSelectedTreeViewItem = e.OriginalSource as TreeViewItem;
		}

		private void SaveAsMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			var dlg = new SaveFileDialog { Filter = "NBT XML Files|*.nbtx|NBT Binary Files|*.nbt" };
			if (FileOpen) {
				dlg.RestoreDirectory = true;
				dlg.InitialDirectory = SaveLoc.Substring(0, SaveLoc.LastIndexOf(Path.DirectorySeparatorChar));
			}

			var result = dlg.ShowDialog(this);

			if (!(bool)result) return;
			SaveLoc = dlg.FileName;
			SaveFile();
		}

		private void EditValueMenuButton_Click(object sender, RoutedEventArgs e) {
			//MessageBox.Show("Do Something");

			var v = new EditValue((Node)TreeView1.SelectedItem) { Owner = this };
			v.ShowDialog();
		}

		private void DeleteNodeMenuButton_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem != null && TreeView1.SelectedItem != Root[0]) {
				var parent = Root[0] as ContainerNode;

				parent = FindParentNode(parent, TreeView1.SelectedItem as Node);

				parent.Children.Remove(TreeView1.SelectedItem as Node);

				PropChanged("Root");

				if (parent != Root[0])
					SetSelectedItem(ref TreeView1, Root[0] as ContainerNode, parent);
			}
		}

		private ContainerNode FindParentNode(ContainerNode parent, Node node) {
			if (parent.Children.Contains(node))
				return parent;
			return (from ContainerNode child in parent.Children.Where(o => o is ContainerNode) select FindParentNode(child, node)).FirstOrDefault(ret => ret != null);
		}

		private bool GenNodePath(ContainerNode parent, Node node, List<int> ret) {
			if (parent.Children.Contains(node)) {
				ret.Add(parent.Children.IndexOf(node));
				return true;
			}
			foreach (var node1 in parent.Children.Where(o => o is ContainerNode)) {
				var child = (ContainerNode)node1;
				ret.Add(parent.Children.IndexOf(child));
				if (GenNodePath(child, node, ret))
					return true;
				ret.Remove(ret.Count - 1);
				return false;
			}

			return false;
		}

		private void MoveUpMenuButton_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem != null && TreeView1.SelectedItem != Root[0]) {
				var parent = Root[0] as ContainerNode;

				parent = FindParentNode(parent, TreeView1.SelectedItem as Node);
				var n = (TreeView1.SelectedItem as Node);

				var i = parent.Children.IndexOf(n);

				if (i > 0)
					i--;
				else return;

				parent.Children.Remove(n);
				parent.Children.Insert(i, n);

				PropChanged("Root");

				SetSelectedItem(ref TreeView1, Root[0] as ContainerNode, n);
			}
		}

		private void MoveDownMenuButton_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem != null && TreeView1.SelectedItem != Root[0]) {
				var parent = Root[0] as ContainerNode;

				parent = FindParentNode(parent, TreeView1.SelectedItem as Node);
				var n = (TreeView1.SelectedItem as Node);

				var i = parent.Children.IndexOf(n);

				if (i != parent.Children.Count - 1)
					i++;
				else return;

				parent.Children.Remove(n);
				parent.Children.Insert(i, n);

				PropChanged("Root");

				SetSelectedItem(ref TreeView1, Root[0] as ContainerNode, n);
			}
		}

		public void SetSelectedItem(ref TreeView control, ContainerNode root, Node item) {
			try {
				var path = new List<int>();
				GenNodePath(root, item, path);


				var dObject = control
					.ItemContainerGenerator
					.ContainerFromItem(Root[0]);

				dObject = path.Aggregate(dObject,
					(current, i) => ((ItemsControl)current).ItemContainerGenerator.ContainerFromIndex(i));


				//uncomment the following line if UI updates are unnecessary
				//((TreeViewItem)dObject).IsSelected = true;                

				var selectMethod =
					typeof(TreeViewItem).GetMethod("Select",
						BindingFlags.NonPublic | BindingFlags.Instance);

				selectMethod.Invoke(dObject, new object[] { true });
			} catch (Exception) {
				Trace.WriteLine("SetSelectedItem error");
			}
		}

		private void AddListBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new ListNode { Name = "New List" });
			}
		}

		private void AddStringBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new StringNode { Name = "New String" });
			}
		}

		private void AddByteBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new ByteNode { Name = "New Byte" });
			}
		}

		private void AddDoubleBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new DoubleNode { Name = "New Double" });
			}
		}

		private void AddFloatBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new FloatNode { Name = "New Float" });
			}
		}

		private void AddIntBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new IntNode { Name = "New Integer" });
			}
		}

		private void AddLongBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new LongNode { Name = "New Long" });
			}
		}

		private void SortMenuButton_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				var node = (TreeView1.SelectedItem as ContainerNode);
				var sort = (TreeView1.SelectedItem as ContainerNode).Children.OrderBy(o => o.Name).ToArray();

				node.Children.Clear();

				foreach (var item in sort) {
					node.Children.Add(item);
				}
			}
		}

		private void AddObjectBtn_Click(object sender, RoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				(TreeView1.SelectedItem as ContainerNode).Children.Insert(0, new ObjectNode { Name = "New Object" });
			}
		}

		private void CutMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			Clipboard.Clear();

			var format = DataFormats.GetDataFormat(typeof(Node).FullName);

			//now copy to clipboard
			IDataObject dataObj = new DataObject();
			dataObj.SetData(format.Name, TreeView1.SelectedItem, false);
			Clipboard.SetDataObject(dataObj, true);

			DeleteNodeMenuButton_Click(this, null);
		}

		private void CopyMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			Clipboard.Clear();

			var format = DataFormats.GetDataFormat(typeof(Node).FullName);

			//now copy to clipboard
			IDataObject dataObj = new DataObject();
			dataObj.SetData(format.Name, TreeView1.SelectedItem, false);
			Clipboard.SetDataObject(dataObj, true);
		}

		private void PasteMenuItem_Click(object sender, ExecutedRoutedEventArgs e) {
			if (TreeView1.SelectedItem is ContainerNode) {
				var n = TreeView1.SelectedItem as ContainerNode;
				Node doc = null;
				var dataObj = Clipboard.GetDataObject();
				var format = typeof(Node).FullName;

				if (dataObj != null && dataObj.GetDataPresent(format)) {
					doc = dataObj.GetData(format) as Node;
				}
				n.Children.Add(doc);
			}
		}
	}

	public class NodeImgConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is Node) {
				//var val = value.ToString();
				//MessageBox.Show(val);

				return string.Format("Resources/{0}.png", value);
			}
			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}

	public class NullToBoolConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}

	public class NodeToBoolConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null)
				return false;
			if (value is Node)
				return (value as Node).HasValue;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}

	public class ListToBoolConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null)
				return false;
			if (value is Node)
				return (value as Node).CanHaveChildren;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}

}
