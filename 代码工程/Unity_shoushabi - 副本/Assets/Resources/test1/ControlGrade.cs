using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGrade : MonoBehaviour {

    private int progress;
    GameObject _UI;
    // Use this for initialization
    void Start () {
        _UI = GameObject.Find("UI");

    }
	
	// Update is called once per frame
	void Update () {
        progress = _UI.GetComponent<UI>()._progress;
        if (progress >= 0)
        {
            if (progress < 100)
            {
                this.GetComponent<TextMesh>().fontSize = 40;
                this.GetComponent<TextMesh>().text = progress + "%";
            }
            else
            {
                this.GetComponent<TextMesh>().fontSize = 20;
                this.GetComponent<TextMesh>().text = "You Win !";
            }
        }
        else
        {
            this.GetComponent<TextMesh>().fontSize = 20;
            this.GetComponent<TextMesh>().text = "You Lost !";
        }
    }
}
