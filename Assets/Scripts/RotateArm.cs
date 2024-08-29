using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public float rotationSpeed;

    public void ArmRotate()
    {
        StartCoroutine(RotateOverTime());
    }

    private IEnumerator RotateOverTime()
    {
        float rotationAmount = 0f;

        while (rotationAmount < 360f)
        {
            float rotateStep = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotateStep);
            rotationAmount += rotateStep;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
