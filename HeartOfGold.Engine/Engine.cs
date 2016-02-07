using System;
using System.Linq;
using HeartOfGold.NBT;

namespace HeartOfGold.Engine
{
	/// <summary>
	/// The core of Heart of Gold
	/// </summary>
    public class Engine
    {
		/// <summary>
		/// Represents the current human player.
		/// </summary>
		[NBTProperty("Player", typeof(ObjectNode))]
		public Player.Player Player { get; private set; }

		///// <summary>
		///// Represents locations.
		///// </summary>
		//public MapServices.Map CurrentMap { get; private set; }

		/// <summary>
		/// Saves the current game state to file.
		/// </summary>
		/// <param name="location">The path to save the file to.</param>
		public void SaveState(string location) { }

		/// <summary>
		/// Loads a game state from file.
		/// </summary>
		/// <param name="location">The path to saved file.</param>
		public void LoadState(string location) 
		{
			var root = ListNode.Deserialize(location);

			// Validation
			if (root.Name != "root")
				throw new FormatException("ListNode was not of format 'root'");

			#region Load Object Node "Player"

			var playerNode = (ObjectNode)root.Children.FirstOrDefault(o => o is ObjectNode && o.Name == "Player");

			// Validation
			if (playerNode == null)
				throw new FormatException("ListNode of type 'root' did not contain expected ObjectNode 'Player'");

			// Let's create a player object directly from the ObjectNode!
			Player = playerNode.Instantiate<Player.Player>();

			#endregion
		}
    }
}



// I have no idea what the fuk I'm doing.
// So here's a centaur.

//						 _____
//			   (________/  ,--\
//			   `-,        (' a(
//			   _/         (\__/
//			    )_     .__ \/
//			    ,-'_ ,--/ )A
//				    ' |/ / _)
//					  | / /
//				   _,'//,,\______
//			    _,'__|/       __ )
//		    _,-'  //-/   __,-'  ) \
//		  ,'            _/__`\ _3_|
//	    _|      ,   _..'  _>_/ \_/
//	  _/ |       \-'      \_/
//  ___/  (`.       |
//  )    /' \`-.    |
// /_   (   _> _)  /
// ' ) ,-'  `, /, (
//   \(_,     )_\\ `.
//    )/        )_),_`.
//				 ' )_\