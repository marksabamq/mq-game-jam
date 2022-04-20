using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClothingItem {
    public ClothingItems clothingItem;
    public Color clothingColour;
    public string clothingDescription;

    public ClothingItem(ClothingItems clothingItem, Color clothingColour, string clothingDescription) {
        this.clothingItem = clothingItem;
        this.clothingColour = clothingColour;
        this.clothingDescription = clothingDescription;
    }
}
    