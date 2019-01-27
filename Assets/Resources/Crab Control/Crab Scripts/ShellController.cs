using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crustacean.FlagSystem;
using UnityEngine.UI;
using Assets.Scripts.DialogueEngine;

 public class ShellController : MonoBehaviour
{
    public static ShellController Instance;

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

        //set  all the global shell flags in this start script.
        
        GlobalFlags.instance.Unset("HasBusinessShell");
        GlobalFlags.instance.Unset("HasCelebrityShell");
        GlobalFlags.instance.Unset("HasMoneyShell");
        GlobalFlags.instance.Unset("HasMarketShell");
        GlobalFlags.instance.Unset("HasRanchShell");
        GlobalFlags.instance.Unset("HasFamilyShell");
        GlobalFlags.instance.Unset("HasAristocrabShell");
        GlobalFlags.instance.Unset("HasChefShell");
        GlobalFlags.instance.Unset("HasArtistShell");
        GlobalFlags.instance.Unset("HasGardenShell");
        GlobalFlags.instance.Unset("HasBakerShell");
        GlobalFlags.instance.Unset("HasBackpackerShell");
        GlobalFlags.instance.Unset("HasAstroShell");

        GlobalFlags.instance.OnFlagChange = CheckSwap;
    }


    private void CheckSwap(string name, bool set)
    {
        
        if(name.Contains("Shell") && set)
        {
            SwapShells();
        }
    }

    private void SwapShells()
    {
        SpriteRenderer PlayerShell = GameObject.Find("Player Crab Variant").GetComponentInChildren<ShellHolder>().GetComponent<SpriteRenderer>();
        SpriteRenderer NPCShell = DialogueController.Instance.CrabFriend.transform.parent.GetComponentInChildren<ShellHolder>().GetComponent<SpriteRenderer>();

        Sprite PlayerSprite = PlayerShell.sprite;
        Sprite NPCSprite = NPCShell.sprite;

        PlayerShell.sprite = NPCSprite;
        NPCShell.sprite = PlayerSprite;        
    }
}
