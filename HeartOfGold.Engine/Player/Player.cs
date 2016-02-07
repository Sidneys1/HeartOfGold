using System.Collections.Generic;
using System.Linq;
using HeartOfGold.Engine.Entities;
using HeartOfGold.Engine.Items;
using HeartOfGold.Engine.MapServices;
using HeartOfGold.NBT;

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
		[NBTProperty("Name", typeof(StringNode))]
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
		[NBTProperty("Inventory", typeof(ListNode), typeof(ObjectNode), "HeartOfGold.Engine.Items.{0},HeartOfGold.Engine", "Category")]
		public List<ItemBase> Inventory { get; private set; }

		/// <summary>
		/// The Player's currently equipped Weapon, if any.
		/// </summary>
		[NBTProperty("Equipped Weapon", typeof(ObjectNode))]
		public Weapon EquippedWeapon { get; set; }

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
		[NBTProperty("Location", typeof(ObjectNode))]
		public Location Location { get; set; }

		/// <summary>
		/// The Player's Statistics.
		/// </summary>
		[NBTProperty("Stats", typeof(ObjectNode))]
		public Stats Stats { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor!
		/// </summary>
		public Player()
		{
			// Initialize lists and Stats struct.
			Inventory = new List<ItemBase>();
			Stats = new Stats();
		}

		#endregion
	}
}
