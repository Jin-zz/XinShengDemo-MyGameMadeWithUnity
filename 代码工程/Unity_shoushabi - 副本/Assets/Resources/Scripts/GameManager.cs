using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //用于各种游戏节点、关卡、成就达成和任务失败等触发
    public GameObject Sprite;
    public Button _Btn_win;
    public GameObject _UI_win;
    public GameObject _UI_img;
    public GameObject _UI_Score;
    public bool isOneOver = false;
    public int score;
    public Button _Imgs_start;
    public Sprite[] aa;
    private bool GG = false;
    public int ii;
    public int time;
    public Sprite a1,a2;
    public int isa1 = 0;
    public GameObject txt_start1;
    public GameObject txt_start2;
    // Use this for initialization
    void Start () {
        Sprite = GameObject.Find("SpriteEmit");
        _UI_win = GameObject.Find("UI_Winn");
        _UI_img = GameObject.Find("OverAN");
        _UI_Score = GameObject.Find("Text_Score");
        _UI_win.SetActive(false);
        score = 0;
        //Button btn = _Btn_win.GetComponent<Button>();
        _Btn_win.onClick.AddListener(OnclickWinButton);
        _Imgs_start.onClick.AddListener(OnclickImgsstartButton);
        a1 = (Sprite)Resources.Load("Textures/1");
        a2 = (Sprite)Resources.Load("Textures/医生");
        // aa = new Sprite[28];
        ii = 0;
        time = 0;
        isa1 = 0;
        txt_start1.SetActive(true);
        txt_start2.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        print(score);
        //捉精灵的任务判断，三只独眼巨人
        if (score == 6)
        {
            Debug.Log("guoguan");
          //  _UI_img.SetActive(false);
            _UI_win.SetActive(true);
            _UI_Score.GetComponent<Text>().text = "Your Score : " + 6 + "/6";
        }
        else
            _UI_Score.GetComponent<Text>().text = "Your Score : " + score + "/6";

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().hp<=0)
        {
            Debug.Log("死亡");
        }

        if (GG)
        {
            if (time % 10 == 0)
            {
                
                if (ii < 19) {
                    _Imgs_start.GetComponent<Image>().sprite = aa[ii];
                }
                ii++;
            }

            time++;
        }

    }



    void OnclickWinButton()
    {
        SceneManager.LoadScene("JZQ");
        _UI_win.SetActive(false);
    }

    void OnclickImgsstartButton()
    {

        if (isa1 == 0)
        {
            _Imgs_start.GetComponent<Image>().color = new Color32(255,255,255,255);
           _Imgs_start.GetComponent<Image>().sprite = a1;
            txt_start1.SetActive(false);
        }
        if (isa1==1)
        {
            _Imgs_start.GetComponent<Image>().sprite = a2;
             txt_start2.SetActive(true);
        }
        if(isa1 == 2)
        {
            _Imgs_start.GetComponent<Image>().sprite = aa[0];
            GG = true;
            txt_start2.SetActive(false);
        }
        if(isa1 == 3)
        {
            _Imgs_start.GetComponent<Image>().enabled = false;
        }
        isa1++;
    }


}
