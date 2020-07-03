using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour {

    public Text _Text_progress;
    public Button _Btn_start;
    public Button _Btn_over;
    public Button _Btn_win;
    public Button _Btn_help;
    public int _progress;
    public GameObject _Player;
    private Vector3 PlayerStartPos;

    public GameObject UI_Start;
    public GameObject UI_Over;
    public GameObject UI_Win;
    public GameObject UI_Help;
    
    public Animator _chan_animtor;


    // Use this for initialization
    void Start () {
        UI_Start = GameObject.Find("UI_Start");
        UI_Over = GameObject.Find("UI_Over");
        UI_Win = GameObject.Find("UI_Win");
        UI_Help = GameObject.Find("UI_Help");

        UI_Start.SetActive(true);
        UI_Over.SetActive(false);
        UI_Win.SetActive(false);
        UI_Help.SetActive(false);
        _Text_progress.enabled = false;

        _progress = 0;
        _Player.GetComponent<SheepMove>()._movespeed = 0;
        PlayerStartPos = _Player.transform.position;
        _Btn_start.onClick.AddListener(onClickStartBtn);
        _Btn_over.onClick.AddListener(onClickOverBtn);
        _Btn_win.onClick.AddListener(onClickWinBtn);
        _Btn_help.onClick.AddListener(onClickHelpBtn);

        _chan_animtor = GameObject.Find("unitychan").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_progress >= 0)
        {
            if(_progress < 100)
            {
                
                //_chan_animtor.Play("WALK00_R");
                _Text_progress.GetComponent<Text>().text = " Your progress: " + _progress + "%";
            }
               
            else
            {
                _chan_animtor.SetBool("isWalk", false);
                _Text_progress.GetComponent<Text>().text = " You Win ! ";
                if (_Player.GetComponent<SheepMove>()._movespeed > 0)
                {
                    _Player.GetComponent<SheepMove>()._movespeed = _Player.GetComponent<SheepMove>()._movespeed - (float)0.1;
                }
                else
                {
                    _Player.GetComponent<SheepMove>()._movespeed = 0;
                    UI_Win.SetActive(true);
                }
            }
                
        }
        else
        {
            _chan_animtor.SetBool("isWalk", false);

            _Text_progress.GetComponent<Text>().text = "My Friend , You Lost";
            if (_Player.GetComponent<SheepMove>()._movespeed > 0)
            {
                _Player.GetComponent<SheepMove>()._movespeed = _Player.GetComponent<SheepMove>()._movespeed - (float)0.05;
            }
            // else StartGame();
            else
            {
                _Player.GetComponent<SheepMove>()._movespeed = 0;
                if(UI_Start.activeSelf == false)
                {
                    UI_Over.SetActive(true);
                }
                
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))  //帮助
        {
            UI_Help.SetActive(true);
            _Player.GetComponent<SheepMove>()._movespeed = 0;
        }

    }

    void StartGame()
    {
        _chan_animtor.SetBool("isWalk", true);
        _progress = 0;
        _Player.transform.position = PlayerStartPos;
        _Player.transform.rotation = Quaternion.Euler(0, 0, 0);
        _Player.GetComponent<SheepMove>().isTrans = 0;
        GameObject _TParents = GameObject.Find("TerrainParent");
        foreach (Transform child in _TParents.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    void onClickStartBtn()
    {        
        UI_Start.SetActive(false);
        _Text_progress.enabled = true;
        _Player.GetComponent<SheepMove>()._movespeed = (float)0.5;
        StartGame();

    }
    void onClickOverBtn()
    {
        UI_Over.SetActive(false);
        UI_Start.SetActive(true);
        //_Player.GetComponent<SheepMove>()._movespeed = (float)0.5;
    }

    void onClickWinBtn()
    {
        //在此添加结束场景
        SceneManager.LoadScene("ENDING");
    }

    void onClickHelpBtn()
    {
        UI_Help.SetActive(false);
        _Player.GetComponent<SheepMove>()._movespeed = (float)0.5;
    }
}
