using HeartOfGold.NBT;
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
using System.Windows.Shapes;

namespace HeartOfGold.NbtEditorWPF
{
	/// <summary>
	/// Interaction logic for EditValue.xaml
	/// </summary>
	public partial class EditValue : Window
	{
		object originalValue = null;
		Node Context;

		public EditValue(Node context)
		{
			InitializeComponent();

			Context = context;
			this.DataContext = context;
			originalValue = context.GetType().GetProperty("Value").GetValue(context);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Context.GetType().GetProperty("Value").SetValue(Context, originalValue);
			this.DialogResult = false;
			this.Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			
			this.DialogResult = true;
			this.Close();
		}
	}
}
