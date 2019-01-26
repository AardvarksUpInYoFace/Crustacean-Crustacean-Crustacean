using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crustacean.Dialogue;
using UnityEngine;

namespace Assets.Scripts.DialogueEngine {
	class DialogueTest : MonoBehaviour {
		[SerializeField]
		TextAsset json;

		private void Start() {
			new ConversationParser().Parse(json.text);
		}
	}
}
