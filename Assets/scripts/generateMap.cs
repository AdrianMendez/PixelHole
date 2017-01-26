using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateMap : MonoBehaviour
{
    public List<GameObject> _maps;
    [SerializeField] private int _random01;
    [SerializeField] private int _random02;
    private int _position01;
    private int _position02;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "ground")
        {
            ///GENERATE 2 RANDOM
            _random01 = Random.Range(0, 9); //<--0-10
            _random02 = Random.Range(0, 9); //<--0-10

            ///POSITIONS OF 2 NEW MAPS
            _position01 = 0;
            _position02 = 60;

            ///GENERATE FIRST MAP
            GameObject map01 = Instantiate(_maps[_random01]) as GameObject;
            map01.transform.position = new Vector3(0, transform.position.y - 40 - _position01, 0);

            ///GENERATE SECOND MAP
            GameObject map02 = Instantiate(_maps[_random02]) as GameObject;
            map02.transform.position = new Vector3(0, transform.position.y - 40 - _position02, 0);

            /// DESTROY PREVIOUS MAPS
            

            ///MOVE THIS TRANSFORM DOWN TO MAKE NEW MAPS
            transform.position = new Vector3(0, transform.position.y - 120, 0);
        }
    }
}