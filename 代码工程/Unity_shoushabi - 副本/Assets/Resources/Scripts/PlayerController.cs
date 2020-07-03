using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{

    public float animSpeed = 1.5f;              // anim
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    public bool useCurves = true;             
                                               
    public float useCurvesHeight = 0.5f;

    
    public float forwardSpeed = 7.0f;

    public float backwardSpeed = 2.0f;

    public float rotateSpeed = 2.0f;

    public float jumpPower = 3.0f;
    // collision 组件
    private CapsuleCollider col;
    private Rigidbody rb;
    // 速度
    private Vector3 velocity;
    // collision的高度
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private Animator anim;                          // Animator 
    public AnimatorStateInfo currentBaseState;         // base layer

    private GameObject cameraObject;    // 


    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");


    public int hp;


    // 初期化
    void Start()
    {
        // Animator
        anim = GetComponent<Animator>();
        // CapsuleCollider
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
  
        cameraObject = GameObject.FindWithTag("MainCamera");
        // capsule碰撞体的初始值，高度和中心
        orgColHight = col.height;
        orgVectColCenter = col.center;

        hp = 100;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {   //如果点击了跳跃按键
            //而且目前是处于 locoState状态中的，只有这个状态可以跳跃，即各种前进动作可以跳跃
            if (currentBaseState.fullPathHash == locoState)
            {
                //并且不处于其他动作的转换之中
                if (!anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);  //那就加一个跳跃力，改变人物物体的位置
                    anim.SetBool("Jump", true);     // animator切换到跳跃，通过bool传递
                }
            }
        }
    }


  
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");              // player物体 水平轴的位移量，两次计算的差
        float v = Input.GetAxis("Vertical");                // player物体 垂直轴的位移量，两次计算的差
        anim.SetFloat("Speed", v);                          // 设定Animator的速度变量Speed，v是前后的得到的
        anim.SetFloat("Direction", h);                      // 设定Animator的方向变量Direction, h是左右得到的
        anim.speed = animSpeed;                             // animSpeed = 1.5f 动画的播放速度吧
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 获得当前的动画状态
        rb.useGravity = true;                               // 是否使用重力




        velocity = new Vector3(0, 0, v);      

        velocity = transform.TransformDirection(velocity);

        if (v > 0.1)
        {
            velocity *= forwardSpeed;     
        }
        else if (v < -0.1)
        {
            velocity *= backwardSpeed;  
        }

        


        // 上下
        transform.localPosition += velocity * Time.fixedDeltaTime;

        // 左右
        transform.Rotate(0, h * rotateSpeed, 0);


        // Animator
        // Locomotion中
        // 如果目前处在locoState状态
        if (currentBaseState.fullPathHash == locoState)
        {
        
            if (useCurves)
            {
                resetCollider(); //重置碰撞体位置
            }
        }

        // 如果目前处在jump状态
        else if (currentBaseState.fullPathHash == jumpState)
        {
            //cameraObject.SendMessage("setCameraPositionJumpView");  
                                                                    // 而且不处于动画转换状态
            if (!anim.IsInTransition(0))
            {

               
                if (useCurves)
                {
                   
                    float jumpHeight = anim.GetFloat("JumpHeight");
                    float gravityControl = anim.GetFloat("GravityControl");
                    if (gravityControl > 0)
                        rb.useGravity = false;  

                   
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up); //从人物中心出发向下的向量作为 ray
                    RaycastHit hitInfo;
                    //得到碰撞点的信息 hitInfo ，没设置distance 也没有设置LayerMask（层级遮罩）
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        //uesCurvesHeight =0.5，人物中心离地面大于这个值时我们把这个状态当作是在空中
                        if (hitInfo.distance > useCurvesHeight) //  && hitInfo.collider.tag == "Terrain"
                        {
                            col.height = orgColHight - jumpHeight; // 调整碰撞体的高度：1.5 - jumpheight
                            float adjCenterY = orgVectColCenter.y + jumpHeight;  //0.75 + jumpHeight
                            col.center = new Vector3(0, adjCenterY, 0); // 调整碰撞体的中心
                        }
                        else
                        {
                            // 判定为人物基本结束跳跃动作			
                            resetCollider();
                       }
                    }
                }
                anim.SetBool("Jump", false);
            }
        }
        // IDLE中
        // 目前处于 静止状态的话
        else if (currentBaseState.fullPathHash == idleState)
        {
            if (useCurves)
            {
                resetCollider();
            }
            //这个时候点击了跳跃，也只是触发 休息 动画
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("Rest", true);
            }
        }
        // 如果目前处于 休息 状态
        else if (currentBaseState.fullPathHash == restState)
        {
            //cameraObject.SendMessage("setCameraPositionFrontView");	
            //而且不处于动画切换的过程 就令rest=false
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("Rest", false);
            }
        }
    }

    //void OnGUI()
    //{
    //    GUI.Box(new Rect(Screen.width - 260, 10, 250, 150), "Interaction");
    //    GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
    //    GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
    //    GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
    //    GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
    //    GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
    //    GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
    //}


   
    void resetCollider()
    {
        // 碰撞体位置的重置
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }
}
