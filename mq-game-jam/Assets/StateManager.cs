using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public PlayerController player;
    public NPCGenerator generator;
    public DialogueManager dialogueManager;

    private NPC findNPC = null;

    public void NewSearch()
    {
        NPC rndNPC = generator.GetRandomNPC();
        string newDialogue = $"Please help me find my brother. They're wearing a <color=#{rgbToHex(rndNPC.clothingItem.clothingColour.colour)}>{rndNPC.clothingItem.clothingColour.colourName}</color>{rndNPC.clothingItem.clothingItem.clothingName}";

        findNPC = rndNPC;
        dialogueManager.ShowDialogue(newDialogue);
    }

    string rgbToHex(Color c)
    {
        return (((int)c.r << 16) + ((int)c.g << 8) + c.b).ToString();
    }
}
