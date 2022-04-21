using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [Header("Components")]
    public PlayerController player;
    public NPCGenerator generator;
    public DialogueManager dialogueManager;

    [Header("Prefabs")]
    public GameObject threadLR;

    private NPC findNPC = null;
    private NPC helpNPC = null;
    private Thread currentThread;

    public GameObject exclaimation;
    private int exclaimNPC = 0;

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
        exclaimNPC = Random.Range(0, generator.npcs.Count);
        generator.npcs[exclaimNPC].moveAway = false;
        generator.npcs[exclaimNPC].exclaimation = true;
    }

    public void NewSearch(NPC fromNPC)
    {
        Debug.Log("Starting new search");

        if (generator.npcs.Count == 1)
        {
            dialogueManager.ShowDialogue("Hey! I need help finding someone. They look like...");
            dialogueManager.ShowDialogue("...you :)");
            player.canMove = false;
        }
        else
        {
            helpNPC = fromNPC;

            findNPC = generator.GetRandomNPC(exclaimNPC);
            findNPC.moveAway = false;
            string newDialogue = $"Please help me find my brother. They're wearing <color=#{rgbToHex(findNPC.clothingItem.clothingColour.colour)}>{findNPC.clothingItem.clothingColour.colourName}</color> {findNPC.clothingItem.clothingItem.clothingName}";

            GameObject newThread = Instantiate(threadLR, transform.position, Quaternion.identity);
            if (!newThread.GetComponent<Thread>()) { Debug.Log("Tf"); }
            currentThread = newThread.GetComponent<Thread>();
            Debug.Log(fromNPC);
            currentThread.CreateThread(fromNPC.transform, player.transform, true);

            dialogueManager.ShowDialogue(newDialogue);
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

                findNPC = null;
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
        exclaimation.transform.position = generator.npcs[exclaimNPC].transform.position;
    }
}
