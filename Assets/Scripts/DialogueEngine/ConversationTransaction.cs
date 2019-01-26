using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crustacean.Dialogue {
	public struct ConversationTransaction {
		public string[] conversationPages;
		public Dictionary<string, string> responses;

		public ConversationTransaction(string[] conversationPages, Dictionary<string, string> responses) {
			this.conversationPages = conversationPages;
			this.responses = responses;
		}
	}
}
