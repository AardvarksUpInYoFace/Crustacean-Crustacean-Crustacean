using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.Dialogue {

	public abstract class DialogueElement {
		protected string id;
		protected string optionText;
		protected DialoguePrecondition pre;
		protected string[] body;
		protected DialogueElement[] options;
		protected DialoguePostcondition post;

		public string getId() { return id; }
		public string getOptionText() { return optionText; }
		public DialoguePrecondition getPrecondition() { return pre; }
		public string[] getBody() { return body; }
		public string getBody(int i) { return body[i]; }
		public int getBodyLength() { return body.Length; }
		public DialogueElement[] getOptions() { return options; }
		public DialoguePostcondition getPostcondition() { return post; }

	}
}
