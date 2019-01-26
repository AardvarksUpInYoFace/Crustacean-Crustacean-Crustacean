using System;
using System.Collections.Generic;
using Crustacean.FlagSystem;

namespace Crustacean.Dialogue {
	public class Conversation : FlagHolder {

		private Dictionary<string, DialogueElement> dialogueElements;

		public Conversation(Dictionary<string, DialogueElement> dialogueElements) {
			this.dialogueElements = dialogueElements;
		}

		public void EnterConversation() {
			List<DialogueElement> entrypoints = new List<DialogueElement>();
			foreach(DialogueElement d in dialogueElements.Values) {
				if(d.isEntrypoint() && AssessPreconditions(d.getId())) {
					entrypoints.Add(d);
				}
			}
			if(entrypoints.Count == 0) throw new Exception("No valid entrypoints found!");
			if(entrypoints.Count > 1) throw new Exception("Multiple valid entrpoints found!");
		}

		private bool AssessPreconditions(string id) {
			bool pass = true;
			foreach(KeyValuePair<PreconditionOperators, string[]> set in dialogueElements[id].getPrecondition().GetPreconditions()) {
				foreach(string operand in set.Value) {
					if(!set.Key.Assess(operand, this)) {
						pass = false;
						break;
					}
				}
				if(!pass) break;
			}
			return pass;
		}

		private void RunPostconditions(string id) {
			foreach(KeyValuePair<PostconditionOperators, string[]> set in dialogueElements[id].getPostcondition().GetPostconditions()) {
				foreach(string operand in set.Value) {
					set.Key.Update(operand, this);
				}
			}
		}
	}
}
