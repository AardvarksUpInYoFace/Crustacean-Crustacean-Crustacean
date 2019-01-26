using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.Dialogue {
	public enum PostconditionOperators {
		SET_GLOBAL_FLAG,
		UNSET_GLOBAL_FLAG,
		SET_CONVERSATION_FLAG,
		UNSET_CONVERSATION_FLAG
	}

	public static class PostconditionOperatorMethods {
		public static PostconditionOperators FromString(string str) {
			switch(str.ToLower()) {
				case "set_global_flags":
					return PostconditionOperators.SET_GLOBAL_FLAG;
				case "unset_global_flags":
					return PostconditionOperators.UNSET_GLOBAL_FLAG;
				case "set_conversation_flags":
					return PostconditionOperators.SET_CONVERSATION_FLAG;
				case "unset_conversation_flags":
					return PostconditionOperators.UNSET_CONVERSATION_FLAG;
				default:
					throw new ArgumentException(str + " is not a valid string definition for PostconditionOperators");
			}
		}
	}
}
