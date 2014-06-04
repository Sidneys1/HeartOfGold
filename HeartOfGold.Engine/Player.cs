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

		/// <summary>
		/// Default constructor!
		/// </summary>
		public Player()
		{
			Inventory = new List<Items.ItemBase>();
			Stats = new Entities.Stats();
		}
	}
}
