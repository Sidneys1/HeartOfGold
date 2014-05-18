using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold
{
	public static class ArgumentHelper
	{
		public static List<ArgumentPair> GetArguments(string[] args)
		{
			if (args.Length > 0)
			{
				List<ArgumentPair> returnValue = new List<ArgumentPair>();
				ArgumentPair current = null;
				StringBuilder data = new StringBuilder();

				foreach (string str in args)
				{
					 if (str[0] == '/')
					 {
						 if (current != null && !string.IsNullOrEmpty(current.Flag))
						 {
							 current.Data = data.ToString();
							 returnValue.Add(current);
							 data.Clear();
							 current = null;
						 }

						 current = new ArgumentPair() { Flag = str.Substring(1) };
					 }
					 else
					 {
						 if (current != null)
							 data.Append(string.Format((data.Length > 0 ? " {0}" : "{0}"), str));
						 else
							 throw new System.Data.SyntaxErrorException(string.Format("Token '{0}' found without flag.", str));
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
