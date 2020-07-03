using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamera : MonoBehaviour {

    // GameObject ParentOfCamera;
    // public Animation AnimatorOfCamera;
    // Use this for initialization
    bool GG = false;
    void Start () {
       // ParentOfCamera = this.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
       
        AnimatorStateInfo stateInfo = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Camera"))
        {
            this.GetComponent<Animator>().SetBool("isEnCamera", false);
        }
        if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.A))  //旋转动画
        {
            this.GetComponent<Animator>().SetBool("isEnCamera", true);

        }




    }
}
