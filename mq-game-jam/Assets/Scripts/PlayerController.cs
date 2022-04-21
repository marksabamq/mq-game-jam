using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private SpriteRenderer ropeSpr;
    [SerializeField] private SpriteRenderer heart;
    public Sprite forwardSprite;
    public Sprite sideSprite;

    public float fadeSpeed = 3;
    private float heartFadeTo = 0;

    [HideInInspector] public bool canMove = true;

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

    private float h, v;
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        bool checkInteract = false;
        if (Input.GetKeyDown(KeyCode.E)) { checkInteract = true; }

        Collider[] inArea = Physics.OverlapSphere(transform.position, 4);
        foreach (Collider b in inArea)
        {
            if (b.GetComponent<NPC>())
            {
                NPC npc = b.GetComponent<NPC>();

                if (npc.moveAway)
                {
                    npc.MoveDir((b.transform.position - transform.position).normalized);
                }
                if (checkInteract)
                {
                    float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(npc.transform.position.x, npc.transform.position.z));
                    if (dist < 5 && Mathf.Abs(transform.position.x - npc.transform.position.x) < 1f)
                    {
                        if (npc.exclaimation)
                        {
                            //stop moving. generate dialogue. show dialogue
                            StateManager.instance.NewSearch(npc);
                            return;
                        }
                        else
                        {
                            StateManager.instance.CheckNPC(npc);
                            return;
                        }
                    }
                }
            }
        }

        heart.color = new Color(heart.color.r, heart.color.g, heart.color.b, Mathf.MoveTowards(heart.color.a, heartFadeTo, Time.deltaTime * fadeSpeed));
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rig.velocity = new Vector3(h, 0, v) * moveSpeed;

            facing = h < 0 ? -1 : h > 0 ? 1 : 0;
        }
    }

    void UpdatedFacing()
    {
        if (m_facing == 0)
        {
            spr.flipX = false;
            ropeSpr.flipX = false;
            spr.sprite = forwardSprite;
        }
        else
        {
            spr.flipX = facing == -1;
            ropeSpr.flipX = facing == -1;
            spr.sprite = sideSprite;
        }
    }

    public void HeartFade(float to)
    {
        heartFadeTo = to;
    }

}
