using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMaozi : MonoBehaviour {

    public Text _Text_Start;
    private int xuhao;
    public GameObject _UI_Start;
    public GameObject _TaskText;
    public GameObject _UI_Score;
    // Use this for initialization
    void Start () {
        xuhao = 0;
        _UI_Score = GameObject.Find("Text_Score");
        _UI_Score.GetComponent<Text>().color =new Color32(255,255,255,0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if(xuhao == 0)
        {
            _Text_Start.GetComponent<Text>().text = "帽子: 嘿，艾卡，我的朋友。";
        }
        if (xuhao == 1)
        {
            _Text_Start.GetComponent<Text>().text = "艾卡: 大飞碟，我这是在哪？我爸爸呢？";
        }
        if (xuhao == 2)
        {
            _Text_Start.GetComponent<Text>().text = "帽子: 我也好久没见到他了。";
        }
        if (xuhao == 3)
        {
            _Text_Start.GetComponent<Text>().text = "艾卡: 那你知道我要去哪找他么？";
        }
        if (xuhao == 4)
        {
            _Text_Start.GetComponent<Text>().text = "帽子：前面有一块巨石祭祀台，你可以去那收集精魄，获得神灵的指引";
        }
        if (xuhao == 5)
        {
            _UI_Start.SetActive(false);
            _TaskText.SetActive(true);
            _UI_Score.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        }


        xuhao++;

    }
}
