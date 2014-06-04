using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine
{
    public class Engine
    {
		public Player Player { get; private set; }
		public MapServices.Map CurrentMap { get; private set; }

		public void SaveState(string location) { }
		public void LoadState(string location) { }
    }
}



// I have no idea what the fuk I'm doing.
// So here's a centaur.

//						_____
//			  (________/  ,--\
//			  `-,        (' a(
//			  _/         (\__/
//			   )_     .__ \/
//			   ,-'_ ,--/ )A
//				   ' |/ / _)
//					 | / /
//				  _,'//,,\______
//			   _,'__|/       __ )
//		   _,-'  //-/   __,-'  ) \
//		 ,'            _/__`\ _3_|
//	   _|      ,   _..'  _>_/ \_/
//	 _/ |       \-'      \_/
// ___/  (`.       |
// )    /' \`-.    |
///_   (   _> _)  /
//' ) ,-'  `, /, (
//  \(_,     )_\\ `.
//   )/        )_),_`.
//				' )_\