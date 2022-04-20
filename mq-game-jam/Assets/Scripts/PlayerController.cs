using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 3;
    [SerializeField] private SpriteRenderer spr;
    public Sprite forwardSprite;
    public Sprite sideSprite;

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

    private float h,v; 
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start() {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Collider[] inArea = Physics.OverlapSphere(transform.position, 5);
        foreach(Collider b in inArea)
        {
            if(b.GetComponent<NPC>())
            {
                b.GetComponent<NPC>().MoveDir((b.transform.position - transform.position).normalized);
            }
        }
    }

    void FixedUpdate() {
        rig.velocity = new Vector3(h, 0, v) * moveSpeed;
        facing = h < 0 ? -1 : h > 0 ? 1 : 0;
    }

    void UpdatedFacing() {
        if (m_facing == 0)
        {
            spr.flipX = false;
            spr.sprite = forwardSprite;
        }
        else
        {
            spr.flipX = facing == -1;
            spr.sprite = sideSprite;
        }
    }
}
