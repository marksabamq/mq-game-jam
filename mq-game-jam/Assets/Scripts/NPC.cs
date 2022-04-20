using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public SpriteRenderer clothingRenderer;
    private Rigidbody rig;

    private ClothingItem clothingItem;

    private Vector3 moveDir = Vector3.zero;
    private float currMoveTime = 0;
    private float moveTime = 0;

    void Start() {
        rig = GetComponent<Rigidbody>();
    }

    public void CreateNPC(ClothingItem clothing) {
        clothingItem = clothing;
        clothingRenderer.sprite = clothingItem.clothingItem.sprite;
    }

    void Update() {
        currMoveTime += Time.deltaTime;
    }

    void FixedUpdate() {
        if(currMoveTime < moveTime && currMoveTime != 0) {
            rig.velocity = moveDir;
        } else {
            rig.velocity = Vector3.zero;
        }
    }

    public void MoveDir(Vector3 moveD) {
        moveDir = moveD;
        moveTime = Random.Range(0.5f,2);
        currMoveTime = 0;
    }
}
