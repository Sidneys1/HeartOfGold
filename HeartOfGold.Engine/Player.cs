using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine
{
	/// <summary>
	/// The Player of the game, and any information associated with them.
	/// </summary>
	public class Player
	{
		#region Properties

		/// <summary>
		/// What the player is known as. Usually shortened to the first name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// A substring of Name representing the first Word in Name if it has more than one word.
		/// </summary>
		public string FirstName
		{
			get
			{
				return Name.Contains(' ') ? Name.Substring(0, Name.IndexOf(' ')) : Name;
			}
		}

		/// <summary>
		/// The Player's Inventory, or the items they are carrying.
		/// </summary>
		public List<Items.ItemBase> Inventory { get; private set; }

		/// <summary>
		/// The Player's currently equipped Weapon, if any.
		/// </summary>
		public Items.Weapon EquippedWeapon { get; set; }

		/// <summary>
		/// Meta: Whether the Player has a weapon equipped or not.
		/// </summary>
		public bool HasWeapon
		{
			get
			{
				// In the future, check for default weapon... "Fists"?
				return EquippedWeapon != null;
			}
		}

		/// <summary>
		/// Where the Player currently is.
		/// </summary>
		public MapServices.Location Location { get; set; }

		/// <summary>
		/// The Player's Statistics.
		/// </summary>
		public Entities.Stats Stats { get; private set; } 

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor!
		/// </summary>
		public Player()
		{
			// Initialize lists and Stats struct.
			Inventory = new List<Items.ItemBase>();
			Stats = new Entities.Stats();
		}

		/// <summary>
		/// Instantiates a Player object from NBT
		/// </summary>
		/// <param name="savedNode">The ObjectNode parameter to instantiate from.</param>
		public Player(NBT.ObjectNode savedNode)
		{
			// Initialize lists and Stats struct.
			Inventory = new List<Items.ItemBase>();
			Stats = new Entities.Stats();

			// Insure the passed ObjectNode is actually a Player
			if (savedNode.Name != "Player")
				throw new FormatException("ObjectNode was not of format 'Player'");

			#region Parse String Node "Name"

			NBT.StringNode NameNode = savedNode.FindChild<NBT.StringNode>("Name");

			if (NameNode == null)
				throw new FormatException("ObjectNode of type 'Player' did not contain expected StringNode 'Name'");
			else
				Name = NameNode.Value; 

			#endregion

			#region Parse Object Node "Equipped Weapon"

			NBT.ObjectNode WeaponNode = savedNode.FindChild<NBT.ObjectNode>("Equipped Weapon");

			if (WeaponNode == null)
				throw new FormatException("ObjectNode of type 'Player' did not contain expected ObjectNode 'Equipped Weapon'");
			else if (WeaponNode.Children.Count == 0)
				// Denotes no weapon, for now. Will be fists later.
				EquippedWeapon = null;
			else
				EquippedWeapon = new Items.Weapon(WeaponNode);

			#endregion

			#region Parse Object Node "Location"

			NBT.ObjectNode LocationNode = savedNode.FindChild<NBT.ObjectNode>("Location");

			if (LocationNode == null)
				throw new FormatException("ObjectNode of type 'Player' did not contain expected ObjectNode 'Location'");
			else
				Location = new MapServices.Location(LocationNode);

			#endregion

			#region Parse Object Node "Stats"

			NBT.ObjectNode StatsNode = savedNode.FindChild<NBT.ObjectNode>("Stats");

			if (StatsNode == null)
				throw new FormatException("ObjectNode of type 'Player' did not contain expected ObjectNode 'Stats'");
			else
				Stats = new Entities.Stats(StatsNode);

			#endregion

			#region Parse List Node "Inventory"

			NBT.ListNode InventoryNode = savedNode.FindChild<NBT.ListNode>("Inventory");

			if (InventoryNode == null)
				throw new FormatException("ObjectNode of type 'Player' did not contain expected ListNode 'Inventory'");
			else
			{
				foreach (NBT.ObjectNode ItemNode in InventoryNode.Children)
				{
					if (ItemNode.Name != "Item")
						throw new FormatException("'Player.Inventory' can only contain ObjectNodes of type 'Item'");
					else
					{
						NBT.StringNode CategoryNode = ItemNode.FindChild<NBT.StringNode>("Category");

						if (CategoryNode == null)
							throw new FormatException("ObjectNode of type 'Item' did not contain expected StringNode 'Category'");

						switch (CategoryNode.Value)
						{
							case "Weapon":
								Inventory.Add(new Items.Weapon(ItemNode));
								break;

							default:
								throw new FormatException("ObjectNode of type 'Item' contained StringNode 'Category' with unexpected Value");
						}
					}
				}
			}

			#endregion
		} 

		#endregion
	}
}
