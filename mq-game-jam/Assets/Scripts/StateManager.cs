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
    private NPC exclaimNPC;

    bool searching = false;

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
        exclaimNPC = generator.GetRandomNPC();
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
        }
        else
        {
            helpNPC = fromNPC;

            findNPC = generator.GetRandomNPC(generator.npcs.IndexOf(exclaimNPC));
            findNPC.moveAway = false;

            if (!searching)
            {
                GameObject newThread = Instantiate(threadLR, transform.position, Quaternion.identity);
                if (!newThread.GetComponent<Thread>()) { Debug.Log("Tf"); }
                currentThread = newThread.GetComponent<Thread>();
                currentThread.CreateThread(fromNPC.transform, player.transform, true);

                searching = true;
            }

            string[] clothingReferences = findNPC.clothingItem.clothingItem.customReferences.Length > 0 && Random.Range(0, 5) > 3 ? findNPC.clothingItem.clothingItem.customReferences : generator.clothingReferences;
            string formattedClothingColour = $"<color=#{rgbToHex(findNPC.clothingItem.clothingColour.colour)}>{findNPC.clothingItem.clothingColour.colourName}</color>";
            string newDialogue = clothingReferences[Random.Range(0, clothingReferences.Length)].Replace("<color>", formattedClothingColour).Replace("<item>", findNPC.clothingItem.clothingItem.clothingName);
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
                helpNPC = null;

                generator.RemoveNPC(findNPC);
                generator.RemoveNPC(helpNPC);

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
    }
}
