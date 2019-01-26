using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.FlagSystem {
	class GlobalFlags : FlagHolder {
		public static GlobalFlags instance {
			get {
				if(inst == null) inst = new GlobalFlags();
				return inst;
			}
		}
		private static GlobalFlags inst;
	}
}
