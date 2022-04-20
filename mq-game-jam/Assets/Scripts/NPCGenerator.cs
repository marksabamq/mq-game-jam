using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour {
    public GameObject npcPrefab;
    public int npcCount = 100;
    
    private List<GameObject> npcs = new List<GameObject>();

    private ClothingItems[] clothingItems;
    private ClothingColour[] clothingColours = new ClothingColour[] {
        new ClothingColour(new Color(1,0,0), "red")
    };

    void Start() {
        clothingItems = Resources.LoadAll<ClothingItems>("ClothingItems/");
        GenerateNPCs();
    }

    void GenerateNPCs()  {
        for(int i =0; i < npcCount; i++) {
            GameObject newNPC = Instantiate(npcPrefab, transform.position, Quaternion.identity);
            if(!newNPC.GetComponent<NPC>()) {
                newNPC.AddComponent<NPC>();
            }

            NPC npc = newNPC.GetComponent<NPC>();

            ClothingItems cItem = clothingItems[Random.Range(0, clothingItems.Length)];
            ClothingColour cColour = clothingColours[Random.Range(0, clothingColours.Length)];
            string cDesc = cItem.description.Replace("<color>", cColour.colourName);
            ClothingItem newClothing = new ClothingItem(cItem, cColour.colour, cDesc);

            npc.CreateNPC(newClothing);
        }
    }

    class ClothingColour {
        public Color colour;
        public string colourName;

        public ClothingColour(Color c, string cName) {
            this.colour = c;
            this.colourName = cName;
        }
    }
}
