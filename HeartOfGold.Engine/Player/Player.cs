using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Player
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
		[NBT.NBTProperty("Name", typeof(NBT.StringNode))]
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
		[NBT.NBTProperty("Inventory", typeof(NBT.ListNode), typeof(NBT.ObjectNode), "HeartOfGold.Engine.Items.{0},HeartOfGold.Engine", "Category")]
		public List<Items.ItemBase> Inventory { get; private set; }

		/// <summary>
		/// The Player's currently equipped Weapon, if any.
		/// </summary>
		[NBT.NBTProperty("Equipped Weapon", typeof(NBT.ObjectNode))]
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
		[NBT.NBTProperty("Location", typeof(NBT.ObjectNode))]
		public MapServices.Location Location { get; set; }

		/// <summary>
		/// The Player's Statistics.
		/// </summary>
		[NBT.NBTProperty("Stats", typeof(NBT.ObjectNode))]
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

		#endregion
	}
}
