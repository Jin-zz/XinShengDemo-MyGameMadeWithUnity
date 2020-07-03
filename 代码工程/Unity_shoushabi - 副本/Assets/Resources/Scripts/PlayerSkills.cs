using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{


    public GameObject skSelect;
    public GameObject camera;

    private Vector3 mousePosOnScreen;
    private Vector3 skSelectPos;

    private Animator anim;


    //技能类型和编号
    public const int STATE_NULL = 0;
    public const int STATE_Stone = 1;
    public const int STATE_RUN = 2;

    //当前选取的技能
    private int NowSkill;
    public GameObject stonePos;
    public GameObject skillWarn;

    // Use this for initialization
    void Start()
    {
        NowSkill = 0;
        anim = GetComponent<Animator>();

        stonePos = GameObject.Find("StonePos");

        skillWarn = GameObject.Find("SkillWarnParent");
    }

    // Update is called once per frame
    void Update()
    {
        //两个if完成skill编号的判断
        if (Input.GetKeyUp("q"))
        {
            NowSkill++;
            if (NowSkill > 1)
            {
                NowSkill = 0;
            }
        }


        if (NowSkill == 1) //stone 技能
        {
            skSelect.SetActive(true);
            mousePosOnScreen = Input.mousePosition; // 获得鼠标在屏幕上的位置
            skSelect.transform.position = mousePosOnScreen; // 技能选取提示跟随鼠标移动
                                                            /* 就 乱七八糟想实现点击怪物结果失败了 
                                                            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
                                                            Debug.Log(mousePosOnScreen + "|" + mousePositionInWorld);
                                                            Debug.Log(mousePosOnScreen.z); // z = 0;
                                                            Vector3 direction = new Vector3(Mathf.Sin(transform.rotation.eulerAngles.y), 0, Mathf.Cos(transform.rotation.eulerAngles.y));

                                                            Ray ray1 = new Ray(mousePositionInWorld, direction); //从人物中心出发
                                                            RaycastHit hitInfo;

                                                            //Debug.Log(direction);
                                                            //Debug.Log(transform.eulerAngles); // 0-360
                                                            Debug.DrawRay(mousePositionInWorld, transform.TransformDirection(Vector3.forward) * 1000, Color.red);

                                                    */


            RaycastHit hitt = new RaycastHit();
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray2, out hitt)) // 如果这个ray2有碰撞
            {
                if (hitt.collider.tag == "Enemy") // 如果是怪物就将标志变小
                {
                    skSelect.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                }
                else
                {
                    skSelect.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                }



                if (Input.GetMouseButtonDown(0) && GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")) // 如果点击了物体
                {
                    if ((transform.position - hitt.point).magnitude < 50) // 距离足够就发射
                    {

                        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HANDUP00_R"))
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Play("HANDUP00_R", 0, 0);

                            GameObject hp_bar = (GameObject)Resources.Load("Prefabs/Stone4Skill");

                            hp_bar.transform.position = stonePos.transform.position;
                            hp_bar.GetComponent<StoneSkill>().playerPos = stonePos.transform.position; // 定义石头投掷的起始点
                            hp_bar.GetComponent<StoneSkill>().aimPos = hitt.point; // 定义石头投掷的目标点

                            Instantiate(hp_bar);
                        }
                    }
                    else
                    {
                        GameObject warn = (GameObject)Resources.Load("Prefabs/SkillWarn");
                        Instantiate(warn);
                    }
                    Debug.Log((hitt.collider.transform.position));


                }
                //Debug.Log(hitt.point);
                //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }



        }
        else
        {
            skSelect.SetActive(false);
        }
    }
}
