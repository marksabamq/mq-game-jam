using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 3;
    [SerializeField] private SpriteRenderer spr;

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

    }

    void FixedUpdate() {
        rig.velocity = new Vector3(h, 0, v) * moveSpeed;
    }
}
