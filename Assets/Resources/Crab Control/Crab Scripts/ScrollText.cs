using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrollText : MonoBehaviour
{
    public static ScrollText Instance;

    public bool CanSkip;

    public string InputText;
    private string NonBlankOutputText;
    public string OutputText { get; private set; }

    private bool Finished;
    private int Counter;

    private float fullStopDelay = 15 / 60f,
        commaDelay = 8f / 60f,
        normalDelay = 1f / 60f;

    private IEnumerator coroutine;

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

    public void Reset()
    {
        Finished = false;
        Counter = -1;
        NonBlankOutputText = OutputText = "";
    }


    public void StartScroll(string text)
    {
        InputText = text;
        NonBlankOutputText = OutputText = "";

        /*
        foreach (char chr in text)
        {
            OutputText += " ";
        }
        */

        Counter = -1;

        Finished = false;

        coroutine = Scroll(normalDelay, 0);
        StartCoroutine(coroutine);
    }


    private IEnumerator Scroll(float delay, float incr)
    {

        //this happens before the return

        yield return new WaitForSeconds(Time.deltaTime);

        incr += Time.deltaTime;

        if (incr < delay)
        {
            coroutine = Scroll(delay, incr);
            StartCoroutine(coroutine);
            yield break;
        }

        incr = 0;


        //this happens after the return

        Counter++;

        if (OutputText == InputText)
        {
            Finished = true;
        }
        else if (Counter < InputText.Length)
        {
            char tempChar = InputText[Counter];

            NonBlankOutputText += tempChar;

            OutputText = NonBlankOutputText + "<color=#0000>" + InputText.Substring(Counter) + "</color>";

            switch (tempChar)
            {
                case ',':
                case ':':
                case ';':
                    coroutine = Scroll(commaDelay, 0);
                    StartCoroutine(coroutine);
                    break;
                case '.':
                case '?':
                case '!':
                    coroutine = Scroll(fullStopDelay, 0);
                    StartCoroutine(coroutine);
                    break;
                default:
                    coroutine = Scroll(normalDelay, 0);
                    StartCoroutine(coroutine);
                    break;
            }
        }
        else
        {
            Finished = true;

        }
    }

    public bool isFinished()
    {
        return Finished;
    }


    private void Update()
    {

        if (CanSkip && !Finished)
        {
            //this if statement delays finished from turning true for a frame (meaining the player has to click again to do other actions after skipping the text.
            if (OutputText == InputText)
            {
                Finished = true;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                StopAllCoroutines();

                OutputText = InputText;
            }
        }
    }
}