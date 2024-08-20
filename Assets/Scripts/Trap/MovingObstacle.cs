using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;

    public GameObject ways;
    public Transform[] waysPoint;
    int pointIndex;
    int pointCount;
    int direction = 1;
    private void Awake()
    {
        waysPoint = new Transform[ways.transform.childCount];
        //add to array
        for(int i = 0; i < ways.transform.childCount; i++)
        {
            waysPoint[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }
    private void Start()
    {
        pointCount = waysPoint.Length;
        pointIndex = 1;
        targetPos = waysPoint[pointIndex].transform.position;
    }
    private void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        if(transform.position == targetPos)
        {
            NextPoint();
        }
    }
    void NextPoint()
    {
        if(pointIndex == pointCount - 1)
        {
            direction = -1;
        }
        if(pointIndex == 0)
        {
            direction = 1;
        }
        pointIndex += direction;
        targetPos = waysPoint[pointIndex].transform.position;
    }
}
