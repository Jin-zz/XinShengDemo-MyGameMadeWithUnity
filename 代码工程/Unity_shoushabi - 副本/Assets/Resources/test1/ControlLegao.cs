using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLegao : MonoBehaviour {
    private float speed;
	// Use this for initialization
	void Start () {
        speed = 60;

    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject _UI = GameObject.Find("UI");  //更新UI
        _UI.GetComponent<UI>()._progress = _UI.GetComponent<UI>()._progress + 10;
        Destroy(this.gameObject);
    }
}
