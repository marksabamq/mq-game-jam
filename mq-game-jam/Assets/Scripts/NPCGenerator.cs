using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour {
    public Transform player;

    public GameObject npcPrefab;
    public float spawnRange = 25;

    public GameObject exclaimation;
    private int exclaimNPC = 0;

    private List<GameObject> npcs = new List<GameObject>();

    private ClothingItems[] clothingItems;
    private ClothingColour[] clothingColours = new ClothingColour[] {
        new ClothingColour(new Color(1,0,0), "red"),
        new ClothingColour(new Color(0,1,0), "blue"),
        new ClothingColour(new Color(0,0,1), "green"),
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

            npc.CreateNPC(permutatedClothing[i], player);

            npcs.Add(newNPC);
        }

        exclaimNPC = Random.Range(0, npcs.Count);
        npcs[exclaimNPC].GetComponent<NPC>().moveAway = false;
    }

    public void GetRandomNPC()
    {

    }

    private void Update()
    {
        exclaimation.transform.position = npcs[exclaimNPC].transform.position;
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
