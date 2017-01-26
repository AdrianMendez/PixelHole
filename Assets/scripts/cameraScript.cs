using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour
{
    public Transform lookAT;
    public float smoothSpeed;
    private Vector3 desiredPosition;

    void LateUpdate()
    {
        desiredPosition = new Vector3(transform.position.x, lookAT.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}