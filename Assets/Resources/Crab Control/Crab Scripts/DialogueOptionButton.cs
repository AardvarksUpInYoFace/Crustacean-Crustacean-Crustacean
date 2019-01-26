using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogueEngine
{
    [RequireComponent(typeof(Button))]
    public class DialogueOptionButton : MonoBehaviour
    {
        public void SetConvoID(string ID, string choice)
        {
            GetComponent<Button>().onClick.AddListener(delegate { DialogueController.Instance.NextConversation(ID); });

            GetComponentInChildren<Text>().text = choice;
        }
    }
}
