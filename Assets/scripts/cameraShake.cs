using UnityEngine;
using System.Collections;

public class cameraShake : MonoBehaviour
{

    public Transform camTransform;

    public bool shakeActive;
    public float shakeAmount = 0.7f;
    Vector2 originalPos;
    public float framesCounter = 0;

    void Awake()
    {
        originalPos = camTransform.localPosition;
    }

    void Update ()
    {
        if (shakeActive)
        {
            framesCounter++;
            if(framesCounter >= 5)
            {
                framesCounter = 0;
                shakeActive = false;
            }
            camTransform.localPosition = originalPos + Random.insideUnitCircle * shakeAmount;
        }
        else
        {
            camTransform.localPosition = originalPos;
        }
    }
    public void CameraShake()
    {
        shakeActive = true;
    }
}
