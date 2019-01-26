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
				if(d.isEntrypoint() && assessPreconditions(d.getId())) {
					entrypoints.Add(d);
				}
			}
			if(entrypoints.Count == 0) throw new Exception("No valid entrypoints found!");
			if(entrypoints.Count > 1) throw new Exception("Multiple valid entrpoints found!");
		}

		private bool assessPreconditions(string id) {
			bool pass = true;
			foreach(KeyValuePair<PreconditionOperators, string[]> set in dialogueElements[id].getPrecondition().GetPreconditions()) {
				foreach(string flag in set.Value) {
					if(!set.Key.Assess(flag, this)) {
						pass = false;
						break;
					}
				}
				if(!pass) break;
			}
			return pass;
		}
	}
}
