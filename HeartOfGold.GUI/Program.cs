using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HeartOfGold.GUI
{
	class Program
	{
		static void Main(string[] args)
		{
			// Random Data

			//try
			//{
			//	List<ArgumentPair> pairs = ArgumentHelper.GetArguments(args);

			//	if (pairs != null)
			//	{
			//		foreach (ArgumentPair pair in pairs)
			//		{
			//			switch (pair.Flag)
			//			{
			//				case "?":
			//					PrintHelp();
			//					return;

			//				case "debug":
			//					break;

			//				default:
			//					break;
			//			}
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	Console.WriteLine("Warning, Malformed arguments:");
			//	Console.WriteLine(string.Format("\t{0}", ex.Message));
			//	Console.WriteLine(string.Format("Enter '{0} /?' for correct syntax.", Constants.CommandLine));
			//	return;
			//}

			Console.WriteLine("Please, enter a path to a 'root' file:");
			string path = Console.ReadLine();

			HeartOfGold.Engine.Engine eng = new Engine.Engine();
			eng.LoadState(path);

			Console.ReadLine();
		}

		//static void PrintHelp()
		//{
		//	Console.Clear();
		//	Console.WriteLine("Runs the 'Heart of Gold' Adventure Game or associated utilities.");
		//	Console.WriteLine();
		//	Console.WriteLine("HeartOfGold [/?] [/debug] [/c infile outpath]");
		//	Console.WriteLine();
		//	Console.WriteLine("\t/?\tDisplays this help file.");
		//	Console.WriteLine("\t/debug\tRuns game in Debugging mode.");
		//	//Console.WriteLine("\t/c\tCompiles a *.nbtx XML File into a *.nbt Binary File.");
		//	//Console.WriteLine("\tinfile\tThe *.nbtx XML File to complile.");
		//	//Console.WriteLine("\toutpath\tThe directory to place the compiled *.nbt Binary File into.");
		//}
	}
}
