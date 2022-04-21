using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public CanvasGroup dialogueCanvas;
    public TextMeshProUGUI dialogueText;

    public float fadeSpeed = 2;
    public float charSpeed = 0.05f;


    private float fadeTo = 0;

    private float lastShownChar = 0;
    private bool allCharsShown = false;

    private float fadeTimer = 0;

    private List<string> dialogueQueue = new List<string>();

    private void Start()
    {
        dialogueCanvas.alpha = 0;
    }

    private void Update()
    {
        dialogueCanvas.alpha = Mathf.MoveTowards(dialogueCanvas.alpha, fadeTo, Time.deltaTime * fadeSpeed);

        if (dialogueQueue.Count > 0)
        {
            if (dialogueText.maxVisibleCharacters <= dialogueQueue[0].Length)
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
                }
                else
                {
                    if (Time.time - fadeTimer > 3)
                    {
                        dialogueQueue.RemoveAt(0);
                        if (dialogueQueue.Count > 0)
                        {
                            NextText();
                        }
                        else
                        {
                            fadeTo = 0;
                        }
                    }
                }
            }
        }
    }

    public void ShowDialogue(string dialogue)
    {
        Debug.Log("UI showing");
        fadeTo = 1;

        if (dialogueQueue.Count == 0 || dialogueQueue[dialogueQueue.Count - 1] != dialogue)
        {
            dialogueQueue.Add(dialogue);
        }

        if (dialogueQueue.Count == 1)
        {
            NextText();
        }
    }

    void NextText()
    {
        dialogueText.text = dialogueQueue[0];
        lastShownChar = Time.time;
        allCharsShown = false;
        dialogueText.maxVisibleCharacters = 0;
    }
}
