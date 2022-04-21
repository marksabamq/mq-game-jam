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
        linePoints.Clear();
    }

    private void OnEnable()
    {
        if(lr == null)
        {
            lr = GetComponent<LineRenderer>();
        }
    }

    public void CreateThread(Transform start, Transform end, bool move)
    {
        tiedStart = start;
        tiedEnd = end;
        moving = move;

        lr.positionCount = 0;
        linePoints.Clear();

        AddPos(tiedStart.position);
    }

    private void Update()
    {
        if(moving)
        {
            float dist = Vector3.Distance(tiedEnd.position, prevPos);
            if(dist > newPointDist)
            {
                AddPos(tiedEnd.position);
            }
        }
    }

    public void AddPos(Vector3 pos)
    {
        //get dist from prev pos. add points inbetween

        prevPos = pos;
        linePoints.Add(prevPos);
        lr.positionCount = linePoints.Count;
        lr.SetPositions(linePoints.ToArray());
    }
}
