using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public Player Player { get; private set; }

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
			NBT.ListNode root = new NBT.ListNode(location);

			if (root.Name != "root")
				throw new FormatException("ListNode was not of format 'root'");


			#region Load Object Node "Player"

			NBT.ObjectNode PlayerNode = (NBT.ObjectNode)root.Children.FirstOrDefault(o => o is NBT.ObjectNode && o.Name == "Player");

			if (PlayerNode == null)
				throw new FormatException("ListNode of type 'root' did not contain expected ObjectNode 'Player'");

			Player = PlayerNode.Instantiate<Player>(); //new Player(PlayerNode);

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