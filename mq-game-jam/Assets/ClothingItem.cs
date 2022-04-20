using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingItem {
    public ClothingItems clothingItem;
    public color clothingColour;
    public string clothingDescription;

    public ClothingItem(ClothingItems clothingItem, color clothingColour, string clothingDescription) {
        this.clothingItem = clothingItem;
        this.clothingColour = clothingColour;
        this.clothinDescription = clothingDescription;
    }
}
    