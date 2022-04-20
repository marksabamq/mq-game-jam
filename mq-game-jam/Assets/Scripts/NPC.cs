using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public SpriteRenderer characterRenderer;
    public SpriteRenderer clothingRenderer;
    public SpriteRenderer heart;
    public SpriteRenderer rope;

    public Sprite forwardSprite;
    public Sprite sideSprite;

    public float fadeSpeed = 3;

    private float heartFadeTo = 0;
    private float ropeFadeTo = 0;

    private Rigidbody rig;

    private ClothingItem clothingItem;

    private float flipDeadzone = 0.5f;

    private Vector3 moveDir = Vector3.zero;
    private float currMoveTime = 0;
    private float moveTime = 0;

    private Transform player;

    [HideInInspector] public bool moveAway = true;
    [HideInInspector] public bool exclaimation = false;

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

    public void CreateNPC(ClothingItem clothing, Transform p) {
        this.player = p;

        clothingItem = clothing;
        clothingRenderer.sprite = clothingItem.clothingItem.sprite;
        clothingRenderer.color = clothing.clothingColour.colour;
    }

    void Update() {
        currMoveTime += Time.deltaTime;
        facing = rig.velocity.x < -flipDeadzone ? -1 : rig.velocity.x > flipDeadzone ? 1 : 0;

        characterRenderer.sortingOrder = player.position.z < transform.position.z ? 0 : 3;
        clothingRenderer.sortingOrder = player.position.z < transform.position.z ? 1 : 4;

        heart.color = new Color(heart.color.r, heart.color.g, heart.color.b, Mathf.MoveTowards(heart.color.a, heartFadeTo, Time.deltaTime * fadeSpeed));
        rope.color = new Color(rope.color.r, rope.color.g, rope.color.b, Mathf.MoveTowards(rope.color.a, ropeFadeTo, Time.deltaTime * fadeSpeed));
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

    public void FadeRope(float to)
    {
        ropeFadeTo = to;
    }

    public void HeartFade(float to)
    {
        heartFadeTo = to;
    }

    public void MoveDir(Vector3 moveD) {
        moveDir = moveD;
        moveTime = Random.Range(0.5f,2);
        currMoveTime = 0;
    }
}
