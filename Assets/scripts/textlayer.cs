using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textlayer : MonoBehaviour 
{

    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    void Awake() {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }

    // Update is called once per frame
    void Update () 
    {
		
	}
}
