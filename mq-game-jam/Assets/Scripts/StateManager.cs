using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [Header("Components")]
    public PlayerController player;
    public NPCGenerator generator;
    public DialogueManager dialogueManager;

    public Camera cam;

    [Header("Prefabs")]
    public GameObject threadLR;

    private NPC findNPC = null;
    private NPC helpNPC = null;
    private Thread currentThread;

    public GameObject exclaimation;
    private NPC exclaimNPC;
    private int exclainNPCIndex = -1;
    private string currDialogue = "";

    bool searching = false;
    bool lastPerson = false;

    public static StateManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        NewExclaimation();
    }

    public void NewExclaimation()
    {
        exclainNPCIndex = Random.Range(0, generator.npcs.Count);
        exclaimNPC = generator.npcs[exclainNPCIndex];
        exclaimNPC.moveAway = false;
        exclaimNPC.exclaimation = true;
    }

    public void NewSearch(NPC fromNPC)
    {
        Debug.Log("Starting new search");

        if (generator.npcs.Count == 1)
        {
            dialogueManager.ShowDialogue("Hey! I need help finding someone. They look like...");
            dialogueManager.ShowDialogue("...you :)");
            player.canMove = false;
            generator.npcs[0].HeartFade(1);
            player.HeartFade(1);

            lastPerson = true;
        }
        else
        {
            if (!searching)
            {
                helpNPC = fromNPC;

                helpNPC.FadeRope(1);

                findNPC = generator.GetRandomNPC(exclainNPCIndex);
                findNPC.moveAway = false;

                GameObject newThread = Instantiate(threadLR, transform.position, Quaternion.identity);
                if (!newThread.GetComponent<Thread>()) { Debug.Log("Tf"); }
                currentThread = newThread.GetComponent<Thread>();
                currentThread.CreateThread(fromNPC.transform, player.transform, true);

                searching = true;
                string dialogue = generator.RandomReference();

                if (findNPC.clothingItem.clothingItem.prefixA)
                {
                    Debug.Log("Prefix A");
                    dialogue.Replace("<color> <item>", "a <color> <item>");
                    dialogue = Regex.Replace(dialogue, "(?<!(<color>)) (<item>)", "a <item>");
                }

                string formattedClothingColour = $"<color=#{rgbToHex(findNPC.clothingItem.clothingColour.colour)}>{findNPC.clothingItem.clothingColour.colourName}</color>";
                currDialogue = dialogue.Replace("<color>", formattedClothingColour).Replace("<item>", findNPC.clothingItem.clothingItem.clothingName);
            }
 
            dialogueManager.ShowDialogue(currDialogue);
        }
    }

    public void CheckNPC(NPC npc)
    {
        if (findNPC != null)
        {
            if (npc == findNPC)
            {
                currentThread.AddPos(npc.transform.position);
                currentThread.moving = false;

                findNPC.ConnectThread(currentThread.GetComponent<LineRenderer>(), helpNPC, true);
                helpNPC.ConnectThread(currentThread.GetComponent<LineRenderer>(), findNPC, false);

                findNPC.FadeRope(1);

                helpNPC.exclaimation = false;

                generator.RemoveNPC(findNPC);
                generator.RemoveNPC(helpNPC);

                findNPC = null;
                helpNPC = null;

                searching = false;

                NewExclaimation();
            }
        }
    }

    string rgbToHex(Color c)
    {
        return ColorUtility.ToHtmlStringRGB(c);
    }

    private void Update()
    {
        if (exclaimNPC != null)
        {
            exclaimation.transform.position = exclaimNPC.transform.position;
        }

        if (lastPerson)
        {
            if (cam.orthographicSize < 1)
            {
                cam.orthographicSize += Time.deltaTime * 2;
            }
        }
    }
}
