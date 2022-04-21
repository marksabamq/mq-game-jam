using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClothingItem {
    public ClothingItems clothingItem;
    public ClothingColour clothingColour;

    public ClothingItem(ClothingItems clothingItem, ClothingColour clothingColour)
    {
        this.clothingItem = clothingItem;
        this.clothingColour = clothingColour;
    }
}
    