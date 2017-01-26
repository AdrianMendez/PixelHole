using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyMaps : MonoBehaviour
{
    private Transform _playerScript;
    void Start ()
    {
        _playerScript = GameObject.Find("Player").GetComponent<Transform>();
    }
	
	void Update ()
    {
		if(transform.position.y - 100 >= _playerScript.transform.position.y)
        {
            Destroy(gameObject);
        }
	}
}
