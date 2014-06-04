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
