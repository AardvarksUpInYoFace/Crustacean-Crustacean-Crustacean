using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine; 

namespace Crustacean.Dialogue {
	public class ConversationParser {
		Dictionary<string, UnresolvedDialogueElement> dialogueElements = new Dictionary<string, UnresolvedDialogueElement>();

		public Conversation Parse(string json) {
			JsonTextReader reader = new JsonTextReader(new StringReader(json));
			reader.Read();
			Spit(reader);
			if (reader.TokenType == JsonToken.StartArray) {
				reader.Read();
				//loop here
				ParseDialogueElement(reader);
			} else if (reader.TokenType == JsonToken.StartObject) {
				ParseDialogueElement(reader);
			} else {
				throw new JsonReaderException("Unexpected element: " + reader.TokenType);
			}
			//...

			ResolveDialogueElements();
			return new Conversation(dialogueElements.ToDictionary(
				k => k.Key,
				v => (Conversation.DialogueElement)v.Value
				)
				);
		}

		private string ParseDialogueElement(JsonTextReader reader) {
			string id = null;
			Conversation.DialoguePrecondition pre;
			ICollection<string> options = null, body = null;
			Conversation.DialoguePostcondition post;
			while(reader.Read()) {
				Spit(reader);
				if(reader.TokenType == JsonToken.PropertyName) {
					string propertyValue = reader.Value.ToString().ToLower();
					reader.Read();
					Spit(reader);

					switch(propertyValue) {
						case "id":
							if(reader.TokenType == JsonToken.String) {
								id = (string) reader.Value;
							} else { 
								throw new JsonReaderException("Unexpected element when parsing dialogue element id: " + reader.TokenType);
							}
							break;
						case "pre":
							if(reader.TokenType == JsonToken.StartObject) {
								pre = ParsePrecondition(reader);
							} else {
								throw new JsonReaderException("Unexpected element when parsing dialogue element precondition: " + reader.TokenType);
							}
							break;
						case "body":
							if(reader.TokenType == JsonToken.StartArray) {
								List<string> bodyElements = new List<string>();
								while(reader.TokenType != JsonToken.EndArray) {
									if(reader.TokenType == JsonToken.String) {
										bodyElements.Add((string)reader.Value);
									} else {
										throw new JsonReaderException("Unexpected element when parsing dialogue element body: " + reader.TokenType);
									}
									reader.Read();
									Spit(reader);
								}
								body = bodyElements.ToArray();
							} else if (reader.TokenType == JsonToken.String) {
								body = new string[] { (string) reader.Value };
							} else {
								throw new JsonReaderException("Unexpected element when parsing dialogue element body: " + reader.TokenType);
							}
							break;
						case "options":
							if(reader.TokenType == JsonToken.StartArray) {
								options = ParseOptions(reader);
							} else {
								throw new JsonReaderException("Unexpected element when parsing dialogue element options: " + reader.TokenType);
							}
							break;
						case "post":
							if(reader.TokenType == JsonToken.StartObject) {
								post = ParsePostcondition(reader);
							} else {
								throw new JsonReaderException("Unexpected element when parsing dialogue element postcondition: " + JsonToken.PropertyName);
							}
							break;
					}
				} else if(reader.TokenType == JsonToken.EndObject) {
					if(null == id) throw new JsonReaderException("No id found when parsing dialogue element");
					if(null == body) throw new JsonReaderException("No body elements found for dialogue element " + id);
					dialogueElements.Add(id, new UnresolvedDialogueElement(id, pre, body.ToArray(), options.ToArray(), post));
					return id;
				} else {
					throw new JsonReaderException("Unexpected element when parsing dialogue element: " + JsonToken.PropertyName);
				}
			}
			throw new JsonReaderException("Unexpected EOF when parsing dialogue element: ");
		}

		private Conversation.DialoguePrecondition ParsePrecondition(JsonTextReader reader) {

		}

		private string[] ParseOptions(JsonTextReader reader) {

		}

		private Conversation.DialoguePostcondition ParsePostcondition(JsonReader reader) {

		}


		private void ResolveDialogueElements() {
			Dictionary<string, Conversation.DialogueElement> resolved = new Dictionary<string, Conversation.DialogueElement>();
			foreach(KeyValuePair<string, UnresolvedDialogueElement> k in dialogueElements) {
				List<Conversation.DialogueElement> resolvedOptions = new List<Conversation.DialogueElement>();
				foreach(string id in k.Value.getUnresolvedOptions()) {
					resolvedOptions.Add(dialogueElements[id]);
				}
				k.Value.setOptions(resolvedOptions.ToArray());
			}
		}

		private void Spit(JsonTextReader reader) {
			if(reader.Value != null) {
				Debug.Log(string.Concat("Token: ", reader.TokenType, " Value: ", reader.Value));
			} else {
				Debug.Log(string.Concat("Token: ", reader.TokenType));
			}
		}

		private class UnresolvedDialogueElement : Conversation.DialogueElement {
			private string[] unresolvedOptions;

			public UnresolvedDialogueElement(string id, Conversation.DialoguePrecondition pre, string[] body, string[] unresolvedOptions, Conversation.DialoguePostcondition post) {
				this.id = id;
				this.pre = pre;
				this.body = body;
				this.unresolvedOptions = unresolvedOptions;
				this.post = post;
			}
			
			public void setOptions(Conversation.DialogueElement[] resolvedOptions) {
				options = resolvedOptions;
			}

			public string[] getUnresolvedOptions() { return unresolvedOptions; }
		}
	}
}
