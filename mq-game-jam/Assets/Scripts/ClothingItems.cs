using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Clothing Item", menuName = "Create clothing item")]
public class ClothingItems : ScriptableObject {
    public Sprite sprite;
    public Sprite sideSprite;

    public string clothingName;
    public bool prefixA = false;


    [Tooltip("Use tag <color> to replace with generated colour")]
    public string[] customReferences;
}
