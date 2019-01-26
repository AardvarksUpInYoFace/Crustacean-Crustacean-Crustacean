using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.Dialogue {
	public class DialoguePostcondition {
		private Dictionary<PostconditionOperators, string[]> postconditions;

		public DialoguePostcondition(Dictionary<PostconditionOperators, string[]> postconditions) {
			this.postconditions = postconditions;
		}

		public Dictionary<PostconditionOperators, string[]> GetPostconditions() {
			return postconditions;
		}
	}
}
