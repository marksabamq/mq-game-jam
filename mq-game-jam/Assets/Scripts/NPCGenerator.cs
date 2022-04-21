using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour {
    public Transform player;

    public GameObject npcPrefab;
    public float spawnRange = 25;

    [HideInInspector] public List<NPC> npcs = new List<NPC>();

    private ClothingItems[] clothingItems;
    private ClothingColour[] clothingColours = new ClothingColour[] {
        new ClothingColour(new Color(1,0,0), "red"),
        new ClothingColour(new Color(0,0,1), "blue"),
        new ClothingColour(new Color(0,1,0), "green"),
        new ClothingColour(new Color(1,1,0), "yellow"),
        new ClothingColour(new Color(0.5f,0,1), "purple"),
        new ClothingColour(new Color(1,0,1), "pink"),
        new ClothingColour(new Color(0,1,1), "sky blue"),
        new ClothingColour(new Color(0,0.75f,0.5f), "teal"),
        new ClothingColour(new Color(0,0,0), "black")
    };

    private List<ClothingItem> permutatedClothing = new List<ClothingItem>();

    void Start() {
        clothingItems = Resources.LoadAll<ClothingItems>("ClothingItems/");
        GenerateClothes();
        GenerateNPCs();
    }

    void GenerateClothes()
    {
        for (int i = 0; i < clothingItems.Length; i++)
        {
            for (int j = 0; j < clothingColours.Length; j++)
            {
                if (Random.Range(1, 10) < 8) //Randomly skip some permutations
                {
                    permutatedClothing.Add(new ClothingItem(clothingItems[i], clothingColours[j]));
                }
            }
        }

        if (permutatedClothing.Count % 2 == 0) //We need an odd number
        {
            permutatedClothing.RemoveAt(0);
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
            newNPC.transform.SetParent(this.transform);

            NPC npc = newNPC.GetComponent<NPC>();

            npc.CreateNPC(permutatedClothing[i], player);

            npcs.Add(npc);
        }
    }

    public NPC GetRandomNPC(int exclude)
    {
        int rndIndex = Random.Range(0, npcs.Count);
        rndIndex = rndIndex == exclude ? rndIndex + 1 : rndIndex;
        rndIndex = rndIndex >= npcs.Count - 1 ? 0 : rndIndex;

        return npcs[rndIndex];
    }

    public NPC GetNPC(int index)
    {
        return npcs[index];
    }

    public void RemoveNPC(NPC remove)
    {
        npcs.Remove(remove);
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
