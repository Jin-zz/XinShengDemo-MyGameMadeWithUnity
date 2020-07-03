using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTParents : MonoBehaviour {

    public GameObject[] myChildren;
    
    // Use this for initialization
    void Start () {
        myChildren = new GameObject[4];
        myChildren[0] = GameObject.Find("_N_Terrain1");
    }
	
	// Update is called once per frame
	void Update () {
        int i = 0;
        foreach (Transform child in transform)
        {
            // GameObject.Destroy(child);

            if (i == 3)
            {
                GameObject.Destroy(myChildren[0]);
            }

            myChildren[i] = child.gameObject;
            i++;
            //print(i);
        }
    }
}
