using System;
using System.Collections.Generic;
using Crustacean.FlagSystem;

namespace Crustacean.Dialogue {
	public class Conversation : FlagHolder {

		private Dictionary<string, DialogueElement> dialogueElements;

		public Conversation(Dictionary<string, DialogueElement> dialogueElements) {
			this.dialogueElements = dialogueElements;
		}

		public ConversationTransaction EnterConversation() {
			List<DialogueElement> entrypoints = new List<DialogueElement>();
			foreach(DialogueElement d in dialogueElements.Values) {
				if(d.isEntrypoint() && AssessPreconditions(d)) {
					entrypoints.Add(d);
				}
			}
			if(entrypoints.Count == 0) throw new Exception("No valid entrypoints found!");
			if(entrypoints.Count > 1) throw new Exception("Multiple valid entrpoints found!");
			return GetNext(entrypoints[0].getId());
		}

		public ConversationTransaction GetNext(string id) {
			DialogueElement d = dialogueElements[id];
			Dictionary<string, string> idOptionLabelPairs = new Dictionary<string, string>();
			foreach(DialogueElement option in d.getOptions()) {
				if(AssessPreconditions(option)) {
					idOptionLabelPairs.Add(option.getId(), option.getOptionText());
				}
			}
			if(idOptionLabelPairs.Count == 0) idOptionLabelPairs = null;
			RunPostconditions(d);
			return new ConversationTransaction(d.getBody(), idOptionLabelPairs);
		}

		private bool AssessPreconditions(DialogueElement d) {
			bool pass = true;
			if(d.getPrecondition() == null) return true;
			foreach(KeyValuePair<PreconditionOperators, string[]> set in d.getPrecondition().GetPreconditions()) {
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

		private void RunPostconditions(DialogueElement d) {
			if(d.getPostcondition() == null) return;
			foreach(KeyValuePair<PostconditionOperators, string[]> set in d.getPostcondition().GetPostconditions()) {
				foreach(string operand in set.Value) {
					set.Key.Update(operand, this);
				}
			}
		}
	}
}
