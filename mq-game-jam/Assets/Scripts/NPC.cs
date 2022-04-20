using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public SpriteRenderer characterRenderer;
    public SpriteRenderer clothingRenderer;

    public Sprite forwardSprite;
    public Sprite sideSprite;

    private Rigidbody rig;

    private ClothingItem clothingItem;

    private Vector3 moveDir = Vector3.zero;
    private float currMoveTime = 0;
    private float moveTime = 0;

    //-1 left, 0 forward, 1 right
    private int m_facing = 0;
    private int facing
    {
        get { return m_facing; }
        set
        {
            if (m_facing == value) { return; };
            m_facing = value;
            UpdatedFacing();
        }
    }

    void Start() {
        rig = GetComponent<Rigidbody>();
    }

    public void CreateNPC(ClothingItem clothing) {
        clothingItem = clothing;
        clothingRenderer.sprite = clothingItem.clothingItem.sprite;
        clothingRenderer.color = clothing.clothingColour.colour;
    }

    void Update() {
        currMoveTime += Time.deltaTime;
        facing = rig.velocity.x < 0 ? -1 : rig.velocity.x > 0 ? 1 : 0;
    }

    void FixedUpdate() {
        if(currMoveTime < moveTime && currMoveTime != 0) {
            rig.velocity = moveDir;
        } else {
            rig.velocity = Vector3.zero;
        }
    }

    void UpdatedFacing()
    {
        if(m_facing == 0) {
            characterRenderer.flipX = false;
            clothingRenderer.flipX = false;

            clothingRenderer.sprite = clothingItem.clothingItem.sprite;
            characterRenderer.sprite = forwardSprite;
        } else {
            characterRenderer.flipX = m_facing == -1;
            clothingRenderer.flipX = m_facing == -1;

            clothingRenderer.sprite = clothingItem.clothingItem.sideSprite;
            characterRenderer.sprite = sideSprite;
        }
    }

    public void MoveDir(Vector3 moveD) {
        moveDir = moveD;
        moveTime = Random.Range(0.5f,2);
        currMoveTime = 0;
    }
}
