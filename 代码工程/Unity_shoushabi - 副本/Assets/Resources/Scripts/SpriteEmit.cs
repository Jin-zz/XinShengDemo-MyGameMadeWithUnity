using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEmit : MonoBehaviour {

    private float emitTime ;
    private float lastTime;

	// Use this for initialization
	void Start () {

        emitTime = 5f;
        lastTime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time -lastTime > emitTime && transform.childCount<10)
        {
            lastTime = Time.time;

            GameObject hp_bar = (GameObject)Resources.Load("Prefabs/Sprite");
            Instantiate(hp_bar);
            hp_bar.transform.position = transform.position; // 初始化位置信息
        }

	}
}
