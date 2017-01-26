using UnityEngine;
using System.Collections;

public class enemyBehaviour : MonoBehaviour
{

    private Transform _playerScript;
    public float _speed;
    private Vector2 toTarget;
    private bool follow;

    void Start ()
    {
        follow = false;
        _playerScript = GameObject.Find("Player").GetComponent<Transform>();
    }
	
	void Update ()
    {
        if(follow)
        {
            toTarget = _playerScript.transform.position - transform.position;
            transform.Translate(toTarget * _speed * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            follow = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            follow = false;
        }
    }
}
