using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crustacean.FlagSystem;

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

		public static void Update(this PostconditionOperators op, string operand, Conversation conversation) {
			switch(op) {
				case PostconditionOperators.SET_GLOBAL_FLAG:
					GlobalFlags.instance.Set(operand);
					break;
				case PostconditionOperators.UNSET_GLOBAL_FLAG:
					GlobalFlags.instance.Unset(operand);
					break;
				case PostconditionOperators.SET_CONVERSATION_FLAG:
					conversation.Set(operand);
					break;
				case PostconditionOperators.UNSET_CONVERSATION_FLAG:
					conversation.Unset(operand);
					break;
				default:
					throw new Exception("Fairly certain it's impossible to be here...");
			}
		}
	}
}
