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
			bool validPath = false;
			string path = string.Empty;
			Console.WriteLine("Please, enter a path to a 'root' file:");
			do
			{
				path = Console.ReadLine();
				path = path.Trim().Replace("\"","");
				try
				{
					FileInfo inf = new FileInfo(path);
					if (inf.Exists)
						validPath = true;
				}
				catch (ArgumentException) { }
				catch (System.IO.PathTooLongException) { }
				catch (NotSupportedException) { }
				if (!validPath)
					Console.WriteLine("Invalid path. Try again:");
			} while (!validPath);

			HeartOfGold.NBT.ObjectNode rootNode = NBT.ObjectNode.Deserialize(path);

			HeartOfGold.Engine.Engine eng = rootNode.Instantiate<Engine.Engine>();
			
			HeartOfGold.Engine.Player.Player p = eng.Player;

			Console.WriteLine(string.Format("Loaded save of {0}, who has {1} strapped to his back!", p.Name, p.HasWeapon ? p.EquippedWeapon.Name : "nothing"));
			Console.WriteLine(string.Format("\u00BB {0}'s Stats:", p.FirstName));
			Console.WriteLine(string.Format("  \u251C {0,10}: {1} ({2:0.#%})", "Health", p.Stats.Health, (double)p.Stats.Health / p.Stats.MaxHealth));
			Console.WriteLine(string.Format("  \u251C {0,10}: {1}", "Max Health", p.Stats.MaxHealth));
			Console.WriteLine(string.Format("  \u2514 {0,10}: {1}", "Strength", p.Stats.Strength));
			Console.WriteLine();
			Console.WriteLine(string.Format("\u00BB {0}'s Inventory{1}", p.FirstName, p.Inventory.Count > 0 ? ":" : " is empty."));


			for (int i = 0; i < p.Inventory.Count; i++)
			{
				Engine.Items.ItemBase item = p.Inventory[i];
				Console.WriteLine(string.Format("  {3} {0} ({1}, Worth ${2:0.00})", item.Name, item.Category, item.Worth, (i == p.Inventory.Count-1) ? "\u2514":"\u251c"));
				Console.WriteLine(string.Format("  {1} \u2514 \"{0}\"", item.Description, (i == p.Inventory.Count - 1) ? " " : "\u2502"));
			}

			Console.ReadLine();
		}
	}
}
