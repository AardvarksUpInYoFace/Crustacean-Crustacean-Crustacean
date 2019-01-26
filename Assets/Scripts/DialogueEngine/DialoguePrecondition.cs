using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.Dialogue {
	public class DialoguePrecondition {
		private Dictionary<PreconditionOperators, string[]> preconditions;

		public DialoguePrecondition(Dictionary<PreconditionOperators, string[]> preconditions) {
			this.preconditions = preconditions;
		}

		public Dictionary<PreconditionOperators, string[]> GetPreconditions() {
			return preconditions;
		}
	}
}
