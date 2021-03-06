﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Crustacean.FlagSystem;

public class MenuController : MonoBehaviour
{

    private List<Image> MenuImages = new List<Image>();
    private List<Text> MenuTexts = new List<Text>();

	[SerializeField]
	private Image family, fame, fortune;

    public Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        MenuImages = GetComponentsInChildren<Image>().ToList();
        MenuTexts = GetComponentsInChildren<Text>().ToList();
		GlobalFlags.instance.OnFlagChange += CheckEnding;

		//THIS LINE DOESN'T BELONG HERE
		GlobalFlags.instance.Set("HasStarterShell");
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GoToGame()
    {
        StartButton.interactable = false;
        StartCoroutine(FadeWholeMenu(0, 1.3f, 1, 0));
    }

	void CheckEnding(string flag, bool value) {
		if(flag == "RollCredits") {
			if(GlobalFlags.instance.IsSet("HasFamilyShell")) {
				ShowCard(family);
			} else if(GlobalFlags.instance.IsSet("HasMoneyShell")) {
				ShowCard(fortune);
			} else if(GlobalFlags.instance.IsSet("HasPrinceShell")) {
				ShowCard(fame);
			}
		} else if(flag == "HasFamilyShell" && GlobalFlags.instance.IsSet("RollCredits")) {
			ShowCard(family);
		} else if(flag == "HasMoneyShell" && GlobalFlags.instance.IsSet("RollCredits")) {
			ShowCard(fortune);
		} else if(flag == "HasPrinceShell" && GlobalFlags.instance.IsSet("RollCredits")) {
			ShowCard(fame);
		}
	}

	private void ShowCard(Image img) {
		StartCoroutine(DoShowCard(img));
	}

	private IEnumerator DoShowCard(Image img) {
		yield return new WaitForSeconds(5.0f);
		img.gameObject.SetActive(true);
		img.color = Color.white;
	}

	private IEnumerator FadeWholeMenu(float iterator, float time, float start, float end)
    {
        yield return new WaitForSeconds(Time.deltaTime);

        iterator += Time.deltaTime;


        var val = iterator / time;
        if (val > 1) val = 1;

        var newVal = Mathf.Lerp(start, end, val);

        foreach(Image image in MenuImages)
        {
            Color col = image.color;
            image.color = new Color(col.r, col.g, col.b, newVal);
        }

        foreach (Text text in MenuTexts)
        {
            Color col = text.color;
            text.color = new Color(col.r, col.g, col.b, newVal);
        }

        if (val < 1)
        {
            StartCoroutine(FadeWholeMenu(iterator, time, start, end));
        }
    }


}
