using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread : MonoBehaviour
{
    public Transform tiedStart;
    public Transform tiedEnd;
    public Vector3 tiedEndOffset;

    public bool moving;
    public float newPointDist = 0.2f;
    private Vector3 prevPos;

    private List<Vector3> linePoints = new List<Vector3>();

    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    private void Update()
    {
        if(moving)
        {
            float dist = Vector3.Distance(tiedEnd.position, prevPos);
            if(dist > newPointDist)
            {
                prevPos = tiedEnd.position;
                linePoints.Add(prevPos);
                lr.positionCount = linePoints.Count;
                lr.SetPositions(linePoints.ToArray());
            }
        }
    }
}
