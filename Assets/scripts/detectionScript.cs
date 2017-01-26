using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionScript : MonoBehaviour
{
    public GameObject _ghost;
    public GameObject _prefabExplosion;

    void OnTriggerEnter2D(Collider2D other)
    {
        playerScript _playerScript = GameObject.Find("Player").GetComponent<playerScript>();

        if (other.tag == "bullet")
        {
            if (gameObject.tag == "destroyable_ground") _playerScript._score2 += 200;
            if (gameObject.tag == "enemy_spike") _playerScript._score2 += 600;
            if (gameObject.tag == "enemy_ghost") _playerScript._score2 += 400;
            Destroy(_ghost);
        }
        if(other.tag == "Player")
        {
            if (gameObject.tag == "enemy_ghost") 
            {
                _playerScript._score2 += 800;
                Destroy(_ghost);
                GameObject explosion = (GameObject)Instantiate(_prefabExplosion);
                explosion.transform.position = transform.position;
            }
        }
    }
}
