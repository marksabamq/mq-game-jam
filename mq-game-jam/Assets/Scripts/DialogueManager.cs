using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {
    public CanvasGroup dialogueCanvas;
    public TextMeshProUGUI dialogueText;

    public float fadeSpeed = 3;
    private float fadeTo = 0;

    private void Start()
    {
        dialogueCanvas.alpha = 0;
    }

    private void Update()
    {
        dialogueCanvas.alpha = Mathf.MoveTowards(dialogueCanvas.alpha, fadeTo, Time.deltaTime * fadeSpeed);
    }

    public void ShowDialogue(string dialogue)
    {
        fadeTo = 1;
        dialogueText.text = dialogue;
    }
}
