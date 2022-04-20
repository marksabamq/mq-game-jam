using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;
    public Vector3 followOffset;
    public float moveSpeed = 1;
    public float maxDist = 1;

    // Update is called once per frame
    void LateUpdate()
    {
        float dist = Vector2.Distance(new Vector2(follow.position.x, follow.position.z), new Vector2(transform.position.x, transform.position.z));
        moveSpeed = (dist / maxDist) + 1;

        transform.position = Vector3.MoveTowards(transform.position, follow.position + followOffset, moveSpeed * Time.deltaTime);
    }
}
