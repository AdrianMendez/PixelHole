using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class shoot : MonoBehaviour
{
    private float _speed;
    private float _timeDestroyer;
    public GameObject _prefabExplosion;

    void Start ()
    {
        _speed = 0.35f;
    }
	
	void Update ()
    {
        this.transform.position -= this.transform.up * this._speed;
        _timeDestroyer++;
        if (_timeDestroyer >= 60)
        {
            GameObject explosion = (GameObject)Instantiate(_prefabExplosion);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
            _timeDestroyer = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "destroyable_ground")
        {
            GameObject explosion = (GameObject)Instantiate(_prefabExplosion);
            explosion.transform.position = transform.position;
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "enemy_ghost" || other.gameObject.tag == "enemy_spike")
        {
            GameObject explosion = (GameObject)Instantiate(_prefabExplosion);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "ground" || other.gameObject.tag == "destroyable_ground")
        {
            GameObject bullet02 = (GameObject)Instantiate(_prefabExplosion);
            bullet02.transform.position = transform.position;
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "wall")
        {
            GameObject bullet02 = (GameObject)Instantiate(_prefabExplosion);
            bullet02.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
