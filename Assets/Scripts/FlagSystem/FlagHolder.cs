using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.FlagSystem {
	public abstract class FlagHolder {
		private Dictionary<string, bool> flags = new Dictionary<string, bool>();

		public bool IsSet(string flag) {
			return flags.ContainsKey(flag) && flags[flag];
		}

		public void Set(string flag) {
			if(flags.ContainsKey(flag)) {
				flags[flag] = true;
			} else {
				flags.Add(flag, true);
			}
		}

		public void Unset(string flag) {
			if(flags.ContainsKey(flag)) {
				flags[flag] = false;
			} else {
				flags.Add(flag, false);
			}
		}
	}
}
