using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour
{

    public Vector3 OriginPoint;

    public float walkSpeed; // 行走速度
    public bool renewWalkAim = true; // 是否寻找新的随机游走的目标

    public float randomWalkAimX; // 随机游走的新位置x
    public float randomWalkAimZ; // 随机游走的新位置z，一起构成了二维平面上的点
    public Vector3 aimPos;

    public float lifetime;
    public float createTime;

    public bool isGet = false;



    private void Start()
    {
        walkSpeed = 6.0f;
        OriginPoint = transform.GetComponentInParent<Transform>().position; // 要围绕着中心点运动，不能超过一定范围

        lifetime = 20f;
        createTime = Time.time;

        transform.parent = GameObject.Find("SpriteEmit").transform;
    }

    private void FixedUpdate()
    {
        RandomWalk();
        if(Time.time- createTime > lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //精灵的随机游走，重点是要在一个范围内游走，所以在随机的基础上加上距离判断。
    void RandomWalk()
    {
        if (renewWalkAim)//初始就是true，必然先执行
        {
            float addPosZ;
            float addPosX;

            float newPosX;
            float newPosZ;

            addPosX = Random.Range(5.0f, 10.0f) * Mathf.Round(Random.Range(-2, 2)); // 得到的叠加量在（-20，20）之间

            addPosZ = Random.Range(5.0f, 10.0f) * Mathf.Round(Random.Range(-2, 2)); // 

            newPosX = transform.position.x + addPosX;
            newPosZ = transform.position.z + addPosZ;

            //如果这个新坐标在 originPos为圆心 r=20的半径之内
            if (Mathf.Abs((new Vector3(newPosX, transform.position.y, newPosZ) - OriginPoint).magnitude) < 16)  // sprite的活动范围
            {
                randomWalkAimX = transform.position.x + addPosX;
                randomWalkAimZ = transform.position.z + addPosZ;
                renewWalkAim = false; // 得到了新的目标位置，就保持目标位置不刷新直到到达了此目标位置
                aimPos = new Vector3(randomWalkAimX, transform.position.y, randomWalkAimZ);
                Debug.Log("随机游走的当前目标位置" + "(" + randomWalkAimX + "," + randomWalkAimZ + ")");
            }
            else
            {
                //什么都不做，一直true，慢慢动动啥的
            }
        }
        //如果顺利获得了新的aimPos
        else 
        {
            // 如果已经到达了目标位置附近，就把renewWalkAim设置为true
            if (Mathf.Abs((transform.position - aimPos).magnitude) < 1)
            {
                renewWalkAim = true;
            }
        }
        WalkTo(transform.position, aimPos);
    }

    //用于randomwalk() 随机游走函数。
    void WalkTo(Vector3 currentPos, Vector3 aimPos) // 目前的位置和目标位置,都应该是vector2，然后高度看地形高度
    {
        //float height;
        //height = Terrain.activeTerrain.SampleHeight(transform.position); // 获取位置的地图高度
        //                                                                 //Debug.Log(height);

        Vector3 walkDirection = aimPos - currentPos; //计算平面方向
        walkDirection = walkDirection.normalized;  //方向单位化

        //walkDirection.y = height;  //position的高度为地形高度
        transform.LookAt(aimPos);  // 转向面对目的地的方向

        //play 行走 的动画
        //this.GetComponent<Animation>().Play("walk");

        transform.Translate(walkDirection * walkSpeed * Time.fixedDeltaTime, Space.World); // 向目的方向位移，这里只是x和z轴
                                                                                           //Vector3 updatePos = transform.position; // 改变行走的高度为地形高度
                                                                                           //updatePos.y = height;
                                                                                           //transform.position = updatePos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") // 如果被玩家触碰，就消失。
        {
            Destroy(this.gameObject);
            isGet = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().score ++;
            Debug.Log("!!!!yesa" + GameObject.Find("GameManager").GetComponent<GameManager>().score);
        }
    }

}
