using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.Dialogue {
	public enum PreconditionOperators {
		GLOBAL_FLAG_SET,
		GLOBAL_FLAG_NOT_SET,
		CONVERSATION_FLAG_SET,
		CONVERSATION_FLAG_NOT_SET
	}

	public static class PreconditionOperatorMethods {
		public static PreconditionOperators FromString(string str) {
			switch(str.ToLower()) {
				case "global_flag_set":
					return PreconditionOperators.GLOBAL_FLAG_SET;
				case "global_flag_not_set":
					return PreconditionOperators.GLOBAL_FLAG_NOT_SET;
				case "conversation_flag_set":
					return PreconditionOperators.CONVERSATION_FLAG_SET;
				case "conversation_flag_not_set":
					return PreconditionOperators.CONVERSATION_FLAG_NOT_SET;
				default:
					throw new ArgumentException(str + " is not a valid string definition for PreconditionOperators");
			}
		}
	}
}
