//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;


public class ThirdPersonCamera : MonoBehaviour
{
    public float smooth = 10f;      // カメラモーションのスムーズ化用変数
    Transform standardPos;          // the usual position for the camera, specified by a transform in the game
    Transform frontPos;         // Front Camera locater
    Transform jumpPos;          // Jump Camera locater

    // スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
    bool bQuickSwitch = false;  //Change Camera Position Quickly


    //鼠标控制视角：
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes m_axes = RotationAxes.MouseXAndY; // 0
    public float m_sensitivityX = 1f;
    public float m_sensitivityY = 1f;

    // 水平方向的 镜头转向
    public float m_minimumX = -180f;
    public float m_maximumX = 180f;
    // 垂直方向的 镜头转向 (这里给个限度 最大仰角为45°)
    public float m_minimumY = -20f;
    public float m_maximumY = 30f;

    public GameObject player;

    float m_rotationY = 0f;


    void Start()
    {
        // 各参照の初期化
        standardPos = GameObject.Find("CamPos").transform;

        if (GameObject.Find("FrontPos"))
            frontPos = GameObject.Find("FrontPos").transform;

        if (GameObject.Find("JumpPos"))
            jumpPos = GameObject.Find("JumpPos").transform;

        //カメラをスタートする
        transform.position = standardPos.position;
        transform.forward = standardPos.forward;

        setCameraPositionNormalView();

        // 防止 刚体影响 镜头旋转
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        player = GameObject.FindWithTag("Player");
    }


    void FixedUpdate()  // このカメラ切り替えはFixedUpdate()内でないと正常に動かない
    {

        //Debug.Log(standardPos.forward);
        Vector3 playerPos = player.transform.position;

        if (Input.GetButton("Fire1"))   // left Ctlr 切换到前面的视角
        {
            // Change Front Camera
            setCameraPositionFrontView();
        }

        else if (Input.GetButton("Fire2"))  //Alt 切换到跳跃视角 跳跃视角就是这个alt仰视视角
        {
            // Change Jump Camera
            setCameraPositionJumpView();
        }

        else if (Input.GetMouseButton(1)) // 鼠标右键可以旋转视角
        {
            float m_rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_sensitivityX; //鼠标水平方向，y轴的旋转
            m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
            m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY); //仰角在-30，30之间，x轴的旋转

            //transform.localEulerAngles = new Vector3(-m_rotationY, m_rotationX, 0);

            transform.RotateAround(playerPos, new Vector3(0f, 1f, 0f), Input.GetAxis("Mouse X") * m_sensitivityX);
            transform.position = standardPos.position;
            //transform.RotateAround(playerPos, new Vector3(1f, 0f, 0f), -Input.GetAxis("Mouse Y") * m_sensitivityY);

        }

        else
        {
            // return the camera to standard position and direction 返回普通视角
            setCameraPositionNormalView();
        }

        //if (Input.GetKeyDown("z")) //重置视角
        //{
        //    transform.position = standardPos.position;
        //    transform.forward = standardPos.forward;
        //}


    }

    void setCameraPositionNormalView()
    {
        if (bQuickSwitch == false)
        {
            // the camera to standard position and direction. smooth用于动画
            transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);
            transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
        }
        else
        { //quickchange 不需要lerp制造动画
          // the camera to standard position and direction / Quick Change
            transform.position = standardPos.position;
            transform.forward = standardPos.forward;
            bQuickSwitch = false;
        }
    }


    void setCameraPositionFrontView()
    {
        // Change Front Camera
        bQuickSwitch = true;
        transform.position = frontPos.position;
        transform.forward = frontPos.forward;
    }

    void setCameraPositionJumpView() //很low，这里不用
    {
        // Change Jump Camera
        bQuickSwitch = false;
        //		transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);	
        //		transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);		
    }
}
