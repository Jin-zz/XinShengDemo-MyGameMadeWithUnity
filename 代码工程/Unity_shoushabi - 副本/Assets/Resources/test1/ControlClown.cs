using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlClown : MonoBehaviour {

    private GameObject _MainPlayer;
    private float dist;
    private Animator anim;
    // Use this for initialization
    void Start () {
        _MainPlayer = GameObject.Find("Player");  //游戏对象
        anim = this.transform.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos_player = _MainPlayer.transform.position;
        Vector3 pos_clown = this.transform.position;
        dist = Vector3.Distance(pos_clown, pos_player);  //计算怪物与游戏对象的距离
        this.transform.LookAt(_MainPlayer.transform);

        //if (dist < 25 && dist > 10)
        //{
        //    anim.SetInteger("isTurn", 1);
        //    // this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
        //    this.transform.LookAt(_MainPlayer.transform);
        //}
        //else
        //{
        //    anim.SetInteger("isTurn", 0);
        //}

        if (dist < 50)
        {
           // print(dist);
            anim.SetBool("isAttack", true);
        }
        else anim.SetBool("isAttack", false);

    }
}
