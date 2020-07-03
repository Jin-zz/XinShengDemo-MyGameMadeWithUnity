using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWarn : MonoBehaviour {
    public GameObject skillWarn;
    public float timeC;
	// Use this for initialization
	void Start () {
        timeC = Time.time;
        skillWarn = GameObject.Find("SkillWarnParent");
        transform.parent = skillWarn.transform;
        transform.localPosition = new Vector3(0, -1, 0);
	}
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.time - timeC;

        this.GetComponent<Image>().color = new Color(255, 255, 255, (1 - (deltaTime)));
        this.transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color(207,0,0, (1 - (deltaTime)));

        if(deltaTime > 2)
        {
            //this.gameObject.SetActive(false);
            Destroy(this);
        }
	}
}
