using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMove : MonoBehaviour {    //控制游戏主体的移动  包括旋转 前行 平行位移

    public float _movespeed;

    public int MoveDirection;
    public bool isRotate = false;
    public int isTrans = 0;

    float chazhi = (float)7;

    public GameObject _TerrainParent;

    // Use this for initialization
    void Start () {
        MoveDirection = 1;
        _TerrainParent = GameObject.Find("TerrainParent");
    }
	
	// Update is called once per frame
	void Update () {

        this.transform.Translate(0, 0, _movespeed);
        
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0, 0, 1);
        }

        //if (Input.GetKey(KeyCode.S))
        //{
        //    this.transform.Translate(0, 0, -1);
        //}

        //旋转
        if (Input.GetKeyDown(KeyCode.D))  //顺时针旋转
        {
            isRotate = true;
            MoveDirection = -1;
        }

        if (Input.GetKeyDown(KeyCode.A))  //逆时针旋转
        {
            isRotate = true;
            MoveDirection = 1;
        }

        //平移
        if (Input.GetKeyDown(KeyCode.Q) && isTrans> -1)    //左转
        {
            ControlTranslate(this.transform,-1, MoveDirection);
            isTrans--;
        }
        if (Input.GetKeyDown(KeyCode.E) && isTrans < 1) 
        {
            ControlTranslate(this.transform, 1, MoveDirection);
            isTrans++;
        }

        //开始旋转
        if (isRotate == true && _TerrainParent.GetComponent<ControlTParents>().myChildren[1]!=null )
        {
            ControlRotate(this.transform , MoveDirection);
            JiaozhenRotate(this.transform , MoveDirection);
        }


    }

    void OnCollisionEnter(Collision col)
    {

        
        if (col.collider.gameObject.name != "_N_Terrain1" && col.collider.gameObject.name != "_N_Terrain2" && col.collider.gameObject.name != "_N_Terrain3" && col.collider.gameObject.name != "_N_Terrain4")
        {
            //更新UI
            GameObject _UI = GameObject.Find("UI");
            _UI.GetComponent<UI>()._progress = -1;
            Debug.Log("You Lost !! player发生碰撞" + col.collider.gameObject.name);
        }

    }

    void ControlRotate(Transform GG , int R_direction)  //R_direction定义旋转方向 // -1: 0转到90 // +1：90转到0
    {

        float tiltAngle = 90;
        float smooth = 5;
        Quaternion target  = GG.transform.rotation;  //初始化
        if (R_direction == -1)  //按下D
        {
           target = Quaternion.Euler(0, tiltAngle, 0);
        }
        if (R_direction == 1)  //按下A
        {
           target = Quaternion.Euler(0, 0, 0);
        }

        GG.transform.rotation = Quaternion.Slerp(GG.transform.rotation, target, Time.deltaTime * smooth);

        if(GG.transform.rotation== target)
            isRotate = false;

    }


    void ControlTranslate(Transform GG , int T_direction , int Road_direction)
    {
        //T_direction定义平移方向 // 【+1: 右移x++ z-- //-1：左移x-- z++】
        //Road_direction定义路的方向 //1：竖向 x  //-1：横向 z
        if(Road_direction == 1)
        {
            if (T_direction > 0)
            {
                this.transform.Translate(chazhi, 0, 0);
            }
            else
            {
                this.transform.Translate(-chazhi, 0, 0);
            }
        }
        if (Road_direction == -1)
        {
            print("T_direction=" + T_direction);

            if (T_direction > 0)
            {
                this.transform.Translate(chazhi, 0, 0);
            }
            else
            {
                this.transform.Translate(-chazhi, 0, 0);
            }
        }

    }
    void JiaozhenRotate(Transform GG , int Road_direction)
    {
        GameObject ele = _TerrainParent.GetComponent<ControlTParents>().myChildren[1];
        if(ele == null)  ele = _TerrainParent.GetComponent<ControlTParents>().myChildren[0];
        Vector3 dist = GG.position - ele.transform.position;

        if (Road_direction == -1)
        {
            if (dist.z < 33)
            {
                print("+1");
                dist = new Vector3(dist.x, dist.y, (float)37.5-chazhi);
                isTrans = 1;
            }
            else if (dist.z > 42)
            {
                print("+2");
                dist = new Vector3(dist.x, dist.y, (float)37.5 + chazhi);
                isTrans = -1;
            }
            else
            {
                print("+3");
                dist = new Vector3(dist.x, dist.y, (float)37.5);
                isTrans = 0;
            }

        }

        if (Road_direction == 1)
        {
            if (dist.x < 33)
            {
                print("1");
                dist = new Vector3((float)37.5 - chazhi, dist.y, dist.z);
                isTrans = -1;
            }
            else if (dist.x > 42)
            {
                print("2");
                dist = new Vector3((float)37.5 + chazhi, dist.y, dist.z);
                isTrans = 1;
            }
            else
            {
                print("3");
                dist = new Vector3((float)37.5, dist.y, dist.z);
                isTrans = 0;
            }
        }

            GG.position = ele.transform.position + dist;
    }


}
