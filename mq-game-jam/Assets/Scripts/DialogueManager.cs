using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public CanvasGroup dialogueCanvas;
    public TextMeshProUGUI dialogueText;

    public float fadeSpeed = 3;
    public float charSpeed = 0.05f;


    private float fadeTo = 0;

    private float lastShownChar = 0;
    private bool allCharsShown = false;

    private float fadeTimer = 0;

    private void Start()
    {
        dialogueCanvas.alpha = 0;
    }

    private void Update()
    {
        dialogueCanvas.alpha = Mathf.MoveTowards(dialogueCanvas.alpha, fadeTo, Time.deltaTime * fadeSpeed);

        if (dialogueText.maxVisibleCharacters <= dialogueText.text.Length)
        {
            if (Time.time - lastShownChar > charSpeed)
            {
                lastShownChar = Time.time;
                dialogueText.maxVisibleCharacters++;
            }
        }
        else
        {
            if (!allCharsShown)
            {
                allCharsShown = true;
                fadeTimer = Time.time;
            } else
            {
                if(Time.time - fadeTimer > 3)
                {
                    fadeTo = 0;
                }
            }
        }
    }

    public void ShowDialogue(string dialogue)
    {
        Debug.Log("UI showing");
        fadeTo = 1;
        dialogueText.text = dialogue;

        lastShownChar = Time.time;
        allCharsShown = false;
        dialogueText.maxVisibleCharacters = 0;
    }
}
