using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HeartOfGold.GUI
{
	public static class ArgumentHelper
	{
		public static List<ArgumentPair> GetArguments(string[] args)
		{
			if (args.Length > 0)
			{
				var returnValue = new List<ArgumentPair>();
				ArgumentPair current = null;
				var data = new StringBuilder();

				foreach (var str in args)
				{
					 if (str[0] == '/')
					 {
						 if (current != null && !string.IsNullOrEmpty(current.Flag))
						 {
							 current.Data = data.ToString();
							 returnValue.Add(current);
							 data.Clear();
						 }

						 current = new ArgumentPair { Flag = str.Substring(1) };
					 }
					 else
					 {
						 if (current != null)
							 data.Append(string.Format((data.Length > 0 ? " {0}" : "{0}"), str));
						 else
							 throw new SyntaxErrorException(string.Format("Token '{0}' found without flag.", str));
					 }
				}

				if (current != null)
				{
					current.Data = data.ToString();
					returnValue.Add(current);
				}

				return returnValue;
			}

			return null;
		}
	}

	public class ArgumentPair
	{
		public string Flag { get; set; }
		public string Data { get; set; }
	}
}
