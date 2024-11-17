using Sirenix.OdinInspector;
using UnityEngine;

public class Rope2DCreator : MonoBehaviour
{
    [SerializeField, Range(2, 50)]
    private int segmentsCount = 2;

    public Transform pointA;
    public Transform pointB;
    public HingeJoint2D hingePrefab;
    public Transform trackedObject;
    private LineRenderer line;

    [HideInInspector]
    public Transform[] segments;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        if (line != null)
        {
            line.enabled = true;
        }
    }

    private Vector2 GetSegmentPosition(int segmentIndex)
    {
        Vector2 posA = pointA.position;
        Vector2 posB = pointB.position;
        float fraction = 1f / segmentsCount;
        return Vector2.Lerp(posA, posB, fraction * segmentIndex);
    }

    [Button]
    void GenerateRope()
    {
        segments = new Transform[segmentsCount];

        for (int i = 0; i < segmentsCount; i++)
        {
            var currJoint = Instantiate(hingePrefab, GetSegmentPosition(i), Quaternion.identity, this.transform);
            segments[i] = currJoint.transform;

            if (i > 0) // not first hinge
            {
                int prevIndex = i - 1;
                currJoint.connectedBody = segments[prevIndex].GetComponent<Rigidbody2D>();
            }
        }
    }

    [Button]
    void DeleteSegments()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        segments = null;
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < segmentsCount; i++)
        {
            Vector2 posAtIndex = GetSegmentPosition(i);
            Gizmos.DrawSphere(posAtIndex, 0.1f);
        }
    }

    private void Update()
    {
        CheckTrackedObjectDestroyed();
    }

    private void CheckTrackedObjectDestroyed()
    {
        if (trackedObject == null)
        {
            if (line != null)
            {
                line.enabled = false;
            }
        }
    }
}
