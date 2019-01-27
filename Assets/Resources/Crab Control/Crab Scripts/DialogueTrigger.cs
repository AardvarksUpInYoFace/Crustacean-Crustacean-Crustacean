using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogueEngine
{
    [RequireComponent(typeof(Collider))]
    public class DialogueTrigger : MonoBehaviour
    {

        [SerializeField]
        TextAsset json;

        private Collider myCollider;

        private bool CanTalk;

        public string Name;

        private void Start()
        {
            myCollider = GetComponent<Collider>();
        }

        private void StartMyDialogue()
        {
            DialogueController.Instance.StartDialogue(json, gameObject, Name);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.name == "Player Crab Variant")
            {
                CanTalk = true;
                //show the can interact dialogue
                GameObject.Find("Can Interact Text").GetComponent<Text>().enabled = true;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.name == "Player Crab Variant")
            {
                CanTalk = false;
                //turn off the can interact dialogue
                GameObject.Find("Can Interact Text").GetComponent<Text>().enabled = false;
            }
        }

        private void Update()
        {
            if(CanTalk)
            {
                if(Input.GetKeyDown(KeyCode.E) && DialogueController.Instance.ConvoDone)
                {
                    StartMyDialogue();
                }
            }
        }
    }
}
