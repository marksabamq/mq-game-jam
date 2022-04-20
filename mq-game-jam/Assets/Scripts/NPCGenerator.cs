using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour {
    public GameObject npcPrefab;
    public float spawnRange = 25;
    
    private List<GameObject> npcs = new List<GameObject>();

    private ClothingItems[] clothingItems;
    private ClothingColour[] clothingColours = new ClothingColour[] {
        new ClothingColour(new Color(1,0,0), "red"),
        new ClothingColour(new Color(0,1,0), "blue"),
        new ClothingColour(new Color(0,0,1), "green"),
        new ClothingColour(new Color(0,1,1), "yellow"),
        new ClothingColour(new Color(1,1,0), "purple"),
        new ClothingColour(new Color(0,0,0), "black")
    };

    private List<ClothingItem> permutatedClothing = new List<ClothingItem>();

    void Start() {
        clothingItems = Resources.LoadAll<ClothingItems>("ClothingItems/");
        GenerateClothes();
        GenerateNPCs();
    }

    void GenerateClothes() {
        for (int i = 0; i < clothingItems.Length; i++)
        {
            for (int j = 0; j < clothingColours.Length; j++)
            {
                permutatedClothing.Add(new ClothingItem(clothingItems[i], clothingColours[j]));
            }
        }
    }

    void GenerateNPCs()  {
        for(int i =0; i < permutatedClothing.Count; i++) {
            GameObject newNPC = Instantiate(npcPrefab, transform.position, Quaternion.identity);
            if(!newNPC.GetComponent<NPC>()) {
                newNPC.AddComponent<NPC>();
            }

            Vector3 npcPos = Random.insideUnitSphere * spawnRange;
            npcPos.y = 0;

            newNPC.transform.position = npcPos;

            NPC npc = newNPC.GetComponent<NPC>();

            npc.CreateNPC(permutatedClothing[i]);

            npcs.Add(newNPC);
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
