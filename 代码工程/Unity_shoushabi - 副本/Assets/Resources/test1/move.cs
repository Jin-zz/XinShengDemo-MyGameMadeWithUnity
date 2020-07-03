using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{   //废！！！

    //public Camera _maincamera;
    // public GameObject _shanyang;
    GameObject _shanyang;
    int _direction;
    // Use this for initialization
    void Start()
    {
        _shanyang = GameObject.Find("SA_Animal_Goat");
    }

    // Update is called once per frame
    void Update()
    {
        _direction = _shanyang.GetComponent<SheepMove>().MoveDirection;
        if (_direction == 1)
        {
            this.transform.Translate(0, 0, (float)-2);
        }

        if (_direction == -1)
        {
            this.transform.Translate((float)-2, 0, 0);
        }

    }
}
