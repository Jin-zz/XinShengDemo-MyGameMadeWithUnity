using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossArtificialIdiot : MonoBehaviour
{

    //boss 主要的任务就是寻找到主角并且接近她攻击她。
    //1、boss 有视野范围，一定距离之内才可以主动追击主角
    //2、boss 有狗,狗主要靠嗅觉，狗会狂吠吸引boss来到狗的位置寻找主角
    //3、狗的视野略大于boss，这样可以令boss来到狗身边在继续寻找。不会到了狗就已经发现了boss
    //boss有几个状态：1追逐主角 2不知所措 原地不动 3随机行走（也是初始状态）

    public GameObject gameManager;
    public Animation monsterAnim; // 怪物的动画
    public Vector3 OriginPoint;  // 怪物的出生位置，用于局限他在一个范围内
    public GameObject player; //主角物体

    //定义怪物的四种行为：不知所措、行走、奔跑、攻击
    public const int STATE_STAND = 0;
    public const int STATE_WALK = 1;
    public const int STATE_RUN = 2;
    public const int STATE_ATTACK = 3;
    public const int STATE_DEATH = 4;
    private int NowState;    //怪物当前状态  
    public int deathTime;



    // 定义怪物属性
    private string monName = "侯姆沃克";
    //public float damage;
    public int hp;

    public float distanceWorld; // 主角和boss的距离
    public float bossSightRange; // boss的视野范围，进入则开始跑步追击
    public float bossAttackRange;

    public float runSpeed; // 跑步速度
    public float walkSpeed; // 行走速度
    public bool renewWalkAim = true; // 是否寻找新的随机游走的目标
    public float r; // boss游走范围

    public Ray ray; // boss到主角的射线投射
    public Ray[] rays = new Ray[1000];
    public RaycastHit bossSightInfo; //boos到主角射线投射的信息

    public float randomWalkAimX; // 随机游走的新位置x
    public float randomWalkAimZ; // 随机游走的新位置z，一起构成了二维平面上的点
    public Vector3 aimPos;

    public bool isDie; // 人死不能复生


    public float smooth;

    private void Start()
    {
        NowState = STATE_WALK;
        OriginPoint = transform.position;
        monsterAnim = GetComponent<Animation>();
        runSpeed = 4f; // 跑步速度
        walkSpeed = 2f; // 行走速度
        bossSightRange = 30f; // boss的视野范围，进入则开始跑步追击
        bossAttackRange = 5f;
        hp = 100;
        r = 20;

        isDie = false;

        player = GameObject.FindGameObjectWithTag("Player");


        smooth = 2;
    }



    private void FixedUpdate()
    {

        //IsBossSee(transform.position,player.transform.position);


        // 计算得到两个人物之间的距离
        distanceWorld = (transform.position - player.GetComponent<Transform>().position).magnitude;
        // origin是boss眼睛，方向是从boss位置到角色位置的射线
        ray = new Ray(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), player.GetComponent<Transform>().position - transform.position);
        bossSightInfo = new RaycastHit();

        //显示到主角的连线
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), player.GetComponent<Transform>().position - transform.position, Color.red);
        //Debug.Log(bossSightInfo);


        //先判断目前的状态：
        //if (!gameManager.GetComponent<GameManager>().isOneOver) //如果第一关还没通过，boss就执行自己的智能，如果通过了，就freeze
        //{

        if (!isDie)
        {
            if (bossAttackRange < distanceWorld && distanceWorld < bossSightRange) // 根据距离判定现在的状态, 在感知范围之内而且没达到攻击距离，就进行追击
            {
                if (Physics.Raycast(ray, out bossSightInfo)) //true always
                {
                    if (bossSightInfo.collider.tag == "Player") // 如果看到了player，则进入追击状态
                    {
                        NowState = STATE_RUN;
                    }
                    else // 没看到player，就进入游走。可以是在一开始进入了范围，但没看到，仍保持游走；看到开始追击，player躲起来了，改变状态为游走。
                    {
                        NowState = STATE_WALK;
                    }
                }
            }
            else if (bossAttackRange >= distanceWorld)
            {
                NowState = STATE_ATTACK;
            }
            else
            {
                NowState = STATE_WALK;
            }
            if (hp <= 0)
            {
                NowState = STATE_DEATH;
            }


            //根据状态选择函数执行
            if (NowState == STATE_RUN)
            {

                monsterAnim["walk"].speed = 1.5f;
                monsterAnim.Play("walk");
                cheasePlayer();
            }
            else if (NowState == STATE_WALK)
            {
                //初始状态，主角在视野范围之外，进行随机游走。
                monsterAnim["walk"].speed = 1.0f;
                monsterAnim.Play("walk");
                randomWalk();
            }
            else if (NowState == STATE_STAND)
            {
                if (Random.Range(0f, 1f) > 0.5)
                {
                    NowState = STATE_WALK;
                }
                else
                {

                }
            }
            else if (NowState == STATE_ATTACK)
            {

                BossAttack(player.GetComponent<Transform>().position);
                //Debug.Log("bite");

            }
            else if (NowState == STATE_DEATH)
            {
                monsterAnim.Play("death");
                
                //isDie = true;
                deathTime++;

                if (deathTime == 130)
                {
                    monsterAnim["death"].speed = 0f;
                }

            }
        }


    }

    //跑向主角并进行距离判断，距离小于攻击距离就开始攻击
    void cheasePlayer()
    {
        RunTo(transform.position, player.GetComponent<Transform>().position);
    }


    //5.29 14:08 完成随机游走！！！！！！！
    void randomWalk()
    {
        if (renewWalkAim)//初始就是true，必然先执行
        {
            float newPosX;
            float newPosZ;

            newPosX = Random.Range(OriginPoint.x - r, OriginPoint.x + r);
            newPosZ = Random.Range(OriginPoint.z - r, OriginPoint.z + r);
            renewWalkAim = false;
            aimPos = new Vector3(newPosX, transform.position.y, newPosZ);

        }
        else
        {
            // 如果已经到达了目标位置附近，就把renewWalkAim设置为true
            if ((transform.position - aimPos).magnitude < 4)
            {
                renewWalkAim = true;
            }
            else
            {

                //Debug.Log((transform.position - aimPos).magnitude);
            }
        }
        WalkTo(transform.position, aimPos);
    }


    void WhoAmI()
    {
        //play 一个 不知所措的 动画，不做任何动作。
    }

    //用于randomwalk() 随机游走函数。
    void WalkTo(Vector3 currentPos, Vector3 aimPos) // 目前的位置和目标位置,都应该是vector2，然后高度都是物体的当前y值
    {
        //float height;
        //height = Terrain.activeTerrain.SampleHeight(transform.position); // 获取位置的地图高度
        //Debug.Log(height);

        Vector3 walkDirection = aimPos - currentPos; //计算平面方向
        walkDirection = walkDirection.normalized;  //方向单位化

        //walkDirection.y = height;  //position的高度为地形高度
        transform.LookAt(aimPos);  // 转向面对目的地的方向

        // Ray rayA = new Ray(transform.position, player.GetComponent<Transform>().position - transform.position);
        // RaycastHit bossAim = new RaycastHit();

        Debug.DrawRay(transform.position, aimPos - currentPos, Color.red);

        //play 行走 的动画
        //this.GetComponent<Animation>().Play("walk");

        transform.Translate(walkDirection * walkSpeed * Time.fixedDeltaTime, Space.World); // 向目的方向位移，这里只是x和z轴
    }

    //用于chease函数
    void RunTo(Vector3 currentPos, Vector3 playerPos)
    {
        //float height;
        //height = Terrain.activeTerrain.SampleHeight(transform.position); // 获取位置的地图高度

        Vector3 walkDirection = playerPos - currentPos; //计算平面方向
        walkDirection = walkDirection.normalized;  //方向单位化

        //walkDirection.y = height;  //position的高度为地形高度
        transform.LookAt(playerPos);  // 转向面对目的地的方向

        //play 跑动 的动画
        //this.GetComponent<Animation>().Play("run");

        //transform.Translate(walkDirection * runSpeed * Time.fixedDeltaTime, Space.World); // 向目的方向位移，这里只是x和z轴
        transform.position += (walkDirection * runSpeed * Time.fixedDeltaTime);
        //Vector3 updatePos = transform.position;
        //updatePos.y = height;
        //transform.position = updatePos;

    }


    void BossAttack(Vector3 playerPos)
    {

        transform.LookAt(playerPos);  // 转向面对目的地的方向
                                      //play attack 动画

        monsterAnim.Play("bite");

        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DAMAGED00"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Play("DAMAGED00", 0, 0);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().hp -= 10;
        }


        //Animation anim;
        //anim = transform.GetChild(2).GetComponent<Animation>();
        //anim.Play("bite");

    }

    void IsBossSee(Vector3 currentPos, Vector3 aimPos) // 判断怪物的视野内是不是有玩家，有的话就去攻击（锥形视野。
                                                       // 参数是当前原始当前坐标 原始目的坐标
    {
        //rays[0] = new Ray(new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), player.GetComponent<Transform>().position - transform.position);
        bool isSee = false;

        // y轴的高度取消，怪物始终平视
        currentPos = new Vector3(currentPos.x, 0, currentPos.z);
        aimPos = new Vector3(aimPos.x, 0, aimPos.z);

        Vector3 mainDir = aimPos - currentPos; // 方向是 xz平面上的向量，y =0
        Vector3 mainDirAC = mainDir + new Vector3(0, 1, 0); // ac ，用于获得一个垂直地面的平面

        Vector3 mainDirNormal = Vector3.Cross(mainDir, mainDirAC); // 通过平面获取 法向量
        mainDirNormal = mainDirNormal.normalized; // 法向量归一，（是平行于地面的法向量
        Vector3 mainY = new Vector3(0, 1, 0); // 与法向量构成了垂直于地面的一个面

        for (int i = -30; i < 30; i += 5) // 选出这个面上的 点
        {
            for (int j = -30; j < 30; j += 5)
            {
                Vector3 a = mainDir + i * mainY + j * mainDirNormal;
                rays[(j + 30)/5 + (i + 30)*8/5] = new Ray(currentPos, a);
            }
        }

        RaycastHit hitInformation = new RaycastHit();

        for (int i = -5; i < 5; i ++)
        {
            for (int j = -5; j < 5; j ++)
            {
                Debug.DrawRay(transform.position, rays[j+i*10].direction );
                Debug.Log("ray");
                // 对于每一条射线，只要有 一条 碰撞了且是同 玩家 碰撞，那就可以
                if ((Physics.Raycast(rays[(j + 30) / 5 + (i + 30) * 8 / 5], out hitInformation, bossSightRange)) && hitInformation.collider.tag == "Player") 
                {
                    isSee = true;
                    Debug.Log("!i see");
                    
                }
                else
                {
                    //return false;
                }
            }
        }
    }

    void Freeze()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            if (hp > 0)
            {
                hp -= 2; //减少十滴血
                Debug.Log("-10");
            }



        }
    }
}
