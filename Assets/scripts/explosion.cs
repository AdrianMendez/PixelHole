using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explosion : MonoBehaviour
{
    private float _timeDestroyer;

	void Update ()
    {
        _timeDestroyer++;
        if (_timeDestroyer >= 30)
        {
            Destroy(gameObject);
            _timeDestroyer = 0;
        }
    }
}
