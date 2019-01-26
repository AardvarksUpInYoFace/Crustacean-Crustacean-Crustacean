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
			if(reader.TokenType == JsonToken.StartArray) {
				while(reader.Read()) {
					if(reader.TokenType == JsonToken.StartObject) {
						ParseDialogueElement(reader);
					} else if (reader.TokenType == JsonToken.EndArray) {
						ResolveDialogueElements();
						return new Conversation(dialogueElements.ToDictionary(
							k => k.Key,
							v => (DialogueElement) v.Value
							)
							);
					} else {
						throw new JsonReaderException("Unexpected element: " + reader.TokenType);
					}
				}
				throw new JsonReaderException("Unexpected EOF when parsing file");
			} else if(reader.TokenType == JsonToken.StartObject) {
				ParseDialogueElement(reader);
				ResolveDialogueElements();
				return new Conversation(dialogueElements.ToDictionary(
					k => k.Key,
					v => (DialogueElement) v.Value
					)
					);
			} else {
				throw new JsonReaderException("Unexpected element: " + reader.TokenType);
			}
		}

		private string ParseDialogueElement(JsonTextReader reader) {
			string id = null, optionText = null;
			bool entrypoint = false;
			DialoguePrecondition pre = null;
			ICollection<string> options = null, body = null;
			DialoguePostcondition post = null;
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
						case "option_text":
							if(reader.TokenType == JsonToken.String) {
								optionText = (string) reader.Value;
							} else {
								throw new JsonReaderException("Unexpected element when parsing dialogue element option text: " + reader.TokenType);
							}
							break;
						case "entrypoint":
							if(reader.TokenType == JsonToken.Boolean) {
								entrypoint = (bool) reader.Value;
							} else {
								throw new JsonReaderException("Unexpected element when parsing dialogue element option text: " + reader.TokenType);
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
								reader.Read();
								Spit(reader);
								while(reader.TokenType != JsonToken.EndArray) {
									if(reader.TokenType == JsonToken.String) {
										bodyElements.Add((string) reader.Value);
									} else {
										throw new JsonReaderException("Unexpected element when parsing dialogue element body: " + reader.TokenType);
									}
									reader.Read();
									Spit(reader);
								}
								body = bodyElements.ToArray();
							} else if(reader.TokenType == JsonToken.String) {
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
						default:
							throw new JsonReaderException("Unexpected property name when parsing dialogue element: " + propertyValue);
					}
				} else if(reader.TokenType == JsonToken.EndObject) {
					if(null == id) throw new JsonReaderException("No id found when parsing dialogue element");
					if(null == body) throw new JsonReaderException("No body elements found for dialogue element " + id);
					dialogueElements.Add(
						id, 
						new UnresolvedDialogueElement(
							id, 
							entrypoint,
							optionText,
							pre, 
							body.ToArray(), 
							options == null ? null : options.ToArray(), 
							post));
					return id;
				} else {
					throw new JsonReaderException("Unexpected element when parsing dialogue element: " + reader.TokenType);
				}
			}
			throw new JsonReaderException("Unexpected EOF when parsing dialogue element");
		}

		private DialoguePrecondition ParsePrecondition(JsonTextReader reader) {
			Dictionary<PreconditionOperators, string[]> preconditions = new Dictionary<PreconditionOperators, string[]>();
			while(reader.Read()) {
				Spit(reader);
				if(reader.TokenType == JsonToken.PropertyName) {
					PreconditionOperators op = PreconditionOperatorMethods.FromString((string) reader.Value);
					reader.Read();
					Spit(reader);
					if(reader.TokenType != JsonToken.StartArray) throw new JsonReaderException("Unexpected element when parsing precondition:" + reader.TokenType);
					preconditions.Add(op, ParseConditionList(reader));
				} else if(reader.TokenType == JsonToken.EndObject) {
					return new DialoguePrecondition(preconditions);
				} else {
					throw new JsonReaderException("Unexpected element when parsing precondition: " + reader.TokenType);
				}
			}
			throw new JsonReaderException("Unexpected EOF when parsing preconditions");
		}

		private string[] ParseOptions(JsonTextReader reader) {
			List<string> options = new List<string>();
			while(reader.Read()) {
				Spit(reader);
				if(reader.TokenType == JsonToken.String) {
					options.Add((string) reader.Value);
				} else if(reader.TokenType == JsonToken.StartObject) {
					options.Add(ParseDialogueElement(reader));
				} else if(reader.TokenType == JsonToken.EndArray) {
					return options.ToArray();
				} else {
					throw new JsonReaderException("Unexpected element when parsing options: " + reader.TokenType);
				}
			}
			throw new JsonReaderException("Unexpected EOF when parsing options");
		}

		private DialoguePostcondition ParsePostcondition(JsonTextReader reader) {
			Dictionary<PostconditionOperators, string[]> postconditions = new Dictionary<PostconditionOperators, string[]>();
			while(reader.Read()) {
				Spit(reader);
				if(reader.TokenType == JsonToken.PropertyName) {
					PostconditionOperators op = PostconditionOperatorMethods.FromString((string) reader.Value);
					reader.Read();
					Spit(reader);
					if(reader.TokenType != JsonToken.StartArray) throw new JsonReaderException("Unexpected element when parsing postcondition:" + reader.TokenType);
					postconditions.Add(op, ParseConditionList(reader));
				} else if(reader.TokenType == JsonToken.EndObject) {
					return new DialoguePostcondition(postconditions);
				} else {
					throw new JsonReaderException("Unexpected element when parsing postcondition: " + reader.TokenType);
				}
			}
			throw new JsonReaderException("Unexpected EOF when parsing preconditions");
		}

		private string[] ParseConditionList(JsonTextReader reader) {
			List<string> values = new List<string>();
			while(reader.Read()) {
				Spit(reader);
				if(reader.TokenType == JsonToken.String) {
					values.Add((string) reader.Value);
				} else if(reader.TokenType == JsonToken.EndArray) {
					return values.ToArray();
				} else {
					throw new JsonReaderException("Unexpected element when parsing condition list: " + reader.TokenType);
				}
			}
			throw new JsonReaderException("Unexpected EOF when parsing condition list");
		}

		private void ResolveDialogueElements() {
			Dictionary<string, DialogueElement> resolved = new Dictionary<string, DialogueElement>();
			foreach(KeyValuePair<string, UnresolvedDialogueElement> k in dialogueElements) {
				List<DialogueElement> resolvedOptions = new List<DialogueElement>();
				if(k.Value.getUnresolvedOptions() == null) continue;
				foreach(string id in k.Value.getUnresolvedOptions()) {
					if(!dialogueElements.ContainsKey(id)) throw new JsonReaderException("Could not find dialogue element id " + id + " when resolving options");
					if(dialogueElements[id].getOptionText() == null) throw new JsonReaderException("Cannot make option for " + id + " as it has no option text.");
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

		private class UnresolvedDialogueElement : DialogueElement {
			private string[] unresolvedOptions;

			public UnresolvedDialogueElement(string id, bool entrypoint, string optionText, DialoguePrecondition pre, string[] body, string[] unresolvedOptions, DialoguePostcondition post) {
				this.id = id;
				this.entrypoint = entrypoint;
				this.optionText = optionText;
				this.pre = pre;
				this.body = body;
				this.unresolvedOptions = unresolvedOptions;
				this.post = post;
			}
			
			public void setOptions(DialogueElement[] resolvedOptions) {
				options = resolvedOptions;
			}

			public string[] getUnresolvedOptions() { return unresolvedOptions; }
		}
	}
}
