using System;
using System.Collections.Generic;

namespace Crustacean.Dialogue {
	public class Conversation {

		private Dictionary<string, DialogueElement> dialogueElements;

		public Conversation(Dictionary<string, DialogueElement> dialogueElements) {
			this.dialogueElements = dialogueElements;
		}

		public abstract class DialogueElement {
			protected string id;
			protected DialoguePrecondition pre;
			protected string[] body;
			protected DialogueElement[] options;
			protected DialoguePostcondition post;

			public string getId() { return id; }
			public DialoguePrecondition getPrecondition() { return pre; }
			public string[] getBody() { return body; }
			public string getBody(int i) { return body[i]; }
			public int getBodyLength() { return body.Length; }
			private DialogueElement[] getOptions() { return options; }
			private DialoguePostcondition getPostcondition() { return post; }

		}

		public struct DialoguePrecondition {

		}

		public struct DialoguePostcondition {

		}
	}
}
