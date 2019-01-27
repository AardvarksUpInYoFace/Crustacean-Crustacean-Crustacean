using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crustacean.Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogueEngine
{
    class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance;

        public Text myText;

        public GameObject OptionButtonPrefab;

        private Conversation myConversation;
        private ConversationTransaction entryTrans;

        private bool InConvo;

        private string[] Dialogues;
        private int CurrentDialogue;

        private bool CanContinue = false;
        public bool ConvoDone = true;

        public GameObject ContinueButton, EndButton;

        private List<GameObject> OptionButtons = new List<GameObject>();

        private List<Image> PanelImages;
        private List<Text> PanelTexts;

        public GameObject CrabFriend;
        public Text CrabNameText;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            PanelImages = GetComponentsInChildren<Image>().ToList();
            PanelTexts = GetComponentsInChildren<Text>().ToList();
        }

        private void SwitchPanelStuff(bool On)
        {
            foreach (Image image in PanelImages)
            {
                image.enabled = On;
            }

            foreach (Text text in PanelTexts)
            {
                text.enabled = On;
            }
        }

        public void StartDialogue(TextAsset json, GameObject Triggerer, string name)
        {
            CrabFriend = Triggerer;
            CrabNameText.text = name;


            myConversation = new ConversationParser().Parse(json.text);

            entryTrans = myConversation.EnterConversation();
            StartConversation();

            SwitchPanelStuff(true);
        }

        public void EndDialogue()
        {
            ConvoDone = true;
            SwitchPanelStuff(false);
            EndButton.SetActive(false);
        }

        public void StartConversation()
        {
            foreach(GameObject option in OptionButtons)
            {
                Destroy(option);
            }

            OptionButtons.Clear();

            InConvo = true;
            ConvoDone = false;

            Dialogues = entryTrans.conversationPages;

            CurrentDialogue = 0;
            GoToNextDialogue();
        }

        public void NextConversation(string ID)
        {
            entryTrans = myConversation.GetNext(ID);
            Dialogues = entryTrans.conversationPages;

            StartConversation();
        }

        private void GoToNextDialogue()
        {
            ContinueButton.SetActive(false);

            if (CurrentDialogue < Dialogues.Length)
            {
                ScrollText.Instance.StartScroll(Dialogues[CurrentDialogue]);
                CanContinue = false;
                return;
            }
        }

        public void SetContinueTrue()
        {
            CanContinue = true;
            GoToNextDialogue();
        }

        public void Update()
        {
            if (InConvo)
            {
                myText.text = ScrollText.Instance.OutputText;

                if (ScrollText.Instance.isFinished())
                {
                    if (!CanContinue)
                    {
                        if (!ContinueButton.activeSelf)
                        {
                            CurrentDialogue++;
                            if(CurrentDialogue < Dialogues.Length) ContinueButton.SetActive(true);
                            else
                            {

                                InConvo = false;
                                //show options.

                                var ListOVals = entryTrans.responses.Values.ToList();
                                
                                //if no options, end of dialogue
                                if (ListOVals.Count < 1)
                                {
                                    //ConvoDone = true;
                                    EndButton.SetActive(true);
                                    //bring up the end button.
                                    return;
                                }

                                else
                                {
                                    // if there are options, spawn the buttons.
                                    int NumOptions = ListOVals.Count;
                                    int i = 0;

                                    foreach(String option in ListOVals)
                                    {
                                        //setup 
                                        var obj = Instantiate(OptionButtonPrefab, transform);
                                        obj.GetComponent<RectTransform>().localPosition = new Vector3(-227, 200, 0);
                                        obj.GetComponent<RectTransform>().localPosition += new Vector3(i * 310, 0, 0);
                                        obj.GetComponent<DialogueOptionButton>().SetConvoID(entryTrans.responses.FirstOrDefault(x => x.Value == option).Key, option);

                                        OptionButtons.Add(obj);

                                        i++;
                                    }
                           
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
