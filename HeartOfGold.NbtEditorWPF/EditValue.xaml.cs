using HeartOfGold.NBT;
using System.Windows;

namespace HeartOfGold.NbtEditorWPF
{
	/// <summary>
	/// Interaction logic for EditValue.xaml
	/// </summary>
	public partial class EditValue
	{
		readonly object _originalValue;
		readonly Node _context;

		public EditValue(Node context)
		{
			InitializeComponent();

			_context = context;
			DataContext = context;
			_originalValue = context.GetType().GetProperty("Value").GetValue(context, null);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_context.GetType().GetProperty("Value").SetValue(_context, _originalValue, null);
			DialogResult = false;
			Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			
			DialogResult = true;
			Close();
		}
	}
}
