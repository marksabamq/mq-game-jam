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

    private Collider col;

    public float fadeSpeed = 3;

    [HideInInspector] public float crowdSize = 10;

    private float heartFadeTo = 0;
    private float ropeFadeTo = 0;
    private float characterFadeTo = 1;

    private Rigidbody rig;

    [HideInInspector] public ClothingItem clothingItem;

    private float flipDeadzone = 0.5f;

    private Vector3 moveDir = Vector3.zero;
    private float currMoveTime = 0;
    private float moveTime = 0;

    private Transform player;

    [HideInInspector] public bool moveAway = true;
    [HideInInspector] public bool exclaimation = false;
    private bool threadConnected = false;
    private LineRenderer thread = null;
    private Transform connectedNPC = null;
    private int currThreadIndex = 0;
    private bool backward;

    private bool found = false;
    private bool recentering = false;

    private float lastFacingUpdate = 0;


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
        rig.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        col = GetComponent<Collider>();
    }

    public void CreateNPC(ClothingItem clothing, Transform p) {
        this.player = p;

        clothingItem = clothing;
        clothingRenderer.sprite = clothingItem.clothingItem.sprite;
        clothingRenderer.color = clothing.clothingColour.colour;
    }

    void Update()
    {
        if (!found)
        {
            float cDist = Vector3.Distance(transform.position, Vector3.zero);
            if (cDist > crowdSize)
            {
                recentering = true;
            }
            if (recentering && cDist < crowdSize * 0.75f)
            {
                recentering = false;
            }
            if (recentering)
            {
                if (moveAway)
                {
                    MoveDirHere((Vector3.zero - transform.position).normalized);
                }
            }

                currMoveTime += Time.deltaTime;

            if (threadConnected)
            {
                Collider[] inArea = Physics.OverlapSphere(transform.position, 3);
                foreach (Collider b in inArea)
                {
                    if (b.GetComponent<NPC>())
                    {
                        NPC npc = b.GetComponent<NPC>();
                        if (npc != this && npc != connectedNPC)
                        {
                            if (npc.moveAway)
                            {
                                npc.MoveDir((b.transform.position - transform.position).normalized);
                            }
                        }
                    }
                }

                Vector3 threadPos = thread.GetPosition(currThreadIndex);
                rig.velocity = (threadPos - transform.position).normalized;

                Vector2 v2Pos = new Vector2(transform.position.x, transform.position.z);

                if (Vector2.Distance(v2Pos, new Vector2(connectedNPC.position.x, connectedNPC.position.z)) < 2)
                {
                    found = true;
                    rig.constraints = RigidbodyConstraints.FreezeAll;
                    if (col != null)
                    {
                        col.enabled = false;
                    }
                    CharacterFade(0.25f);
                    HeartFade(1);
                }

                if (Vector2.Distance(v2Pos, new Vector2(threadPos.x, threadPos.z)) < 0.5f)
                {
                    currThreadIndex = backward ? currThreadIndex - 1 : currThreadIndex + 1;
                    currThreadIndex = currThreadIndex < 0 ? 0 : currThreadIndex > thread.positionCount - 1 ? thread.positionCount - 1 : currThreadIndex;
                }
            }
        }

        facing = rig.velocity.x < -flipDeadzone ? -1 : rig.velocity.x > flipDeadzone ? 1 : 0;

        bool behind = player.position.z < transform.position.z;
        int sortChar = found ? -1 : (behind ? 1 : 4);
        characterRenderer.sortingOrder = found ? -2 : (behind ? 0 : 3);
        clothingRenderer.sortingOrder = sortChar;
        heart.sortingOrder = sortChar;
        rope.sortingOrder = sortChar;

        heart.color = new Color(heart.color.r, heart.color.g, heart.color.b, Mathf.MoveTowards(heart.color.a, heartFadeTo, Time.deltaTime * fadeSpeed));
        rope.color = new Color(rope.color.r, rope.color.g, rope.color.b, Mathf.MoveTowards(rope.color.a, ropeFadeTo, Time.deltaTime * fadeSpeed));
        if (found)
        {
            characterRenderer.color = new Color(
                1, 
                1, 
                1, 
                Mathf.MoveTowards(characterRenderer.color.a, characterFadeTo, Time.deltaTime * fadeSpeed));
            clothingRenderer.color = new Color(
                1,
                1,
                1, 
                Mathf.MoveTowards(clothingRenderer.color.a, characterFadeTo, Time.deltaTime * fadeSpeed));
        }
    }

    void FixedUpdate()
    {
        if (!threadConnected)
        {
            if (currMoveTime < moveTime && currMoveTime != 0)
            {
                rig.velocity = moveDir;
            }
            else
            {
                rig.velocity = Vector3.zero;
            }
        }
    }

    public void ConnectThread(LineRenderer t, NPC connectedTo, bool backwardTraverse)
    {
        threadConnected = true;
        thread = t;
        currThreadIndex = backwardTraverse ? thread.positionCount - 1 : 0;
        connectedNPC = connectedTo.transform;
        backward = backwardTraverse;
    }

    void UpdatedFacing()
    {
        if (Time.time - lastFacingUpdate > 0.3f)
        {
            lastFacingUpdate = Time.time;

            if (m_facing == 0)
            {
                characterRenderer.flipX = false;
                clothingRenderer.flipX = false;
                rope.flipX = false;

                clothingRenderer.sprite = clothingItem.clothingItem.sprite;
                characterRenderer.sprite = forwardSprite;
            }
            else
            {
                characterRenderer.flipX = m_facing == -1;
                clothingRenderer.flipX = m_facing == -1;
                rope.flipX = m_facing == -1;

                clothingRenderer.sprite = clothingItem.clothingItem.sideSprite;
                characterRenderer.sprite = sideSprite;
            }
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
    public void CharacterFade(float to)
    {
        characterFadeTo = to;
    }

    public void MoveDir(Vector3 moveD)
    {
        if (!recentering)
        {
            moveDir = moveD;
            moveTime = Random.Range(0.5f, 2);
            currMoveTime = 0;
        }
    }

    private void MoveDirHere(Vector3 moveD)
    {
        moveDir = moveD;
        moveTime = Random.Range(0.5f, 2);
        currMoveTime = 0;
    }
}
