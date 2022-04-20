using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour {
    public GameObject npcPrefab;
    public int npcCount = 100;
    public float spawnRange = 25;
    
    private List<GameObject> npcs = new List<GameObject>();

    private ClothingItems[] clothingItems;
    private ClothingColour[] clothingColours = new ClothingColour[] {
        new ClothingColour(new Color(1,0,0), "red"),
        new ClothingColour(new Color(0,1,0), "blue"),
        new ClothingColour(new Color(0,0,1), "green")
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

            Vector3 npcPos = Random.insideUnitSphere * spawnRange;
            npcPos.y = 0;

            newNPC.transform.position = npcPos;

            NPC npc = newNPC.GetComponent<NPC>();

            ClothingItems cItem = clothingItems[Random.Range(0, clothingItems.Length)];
            ClothingColour cColour = clothingColours[Random.Range(0, clothingColours.Length)];
            ClothingItem newClothing = new ClothingItem(cItem, cColour);

            npc.CreateNPC(newClothing);
        }
    }
}

 public class ClothingColour {
    public Color colour;
    public string colourName;

    public ClothingColour(Color c, string cName) {
        this.colour = c;
        this.colourName = cName;
    }
}
