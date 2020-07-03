using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isreach : MonoBehaviour {   //此脚本用于生成地形和障碍物

    GameObject _N_Terrain1;
    GameObject _N_Terrain2;
    GameObject _N_Terrain3;
    GameObject _N_Terrain4;
    GameObject _NTerrainParent;
    GameObject _statcle;
    GameObject _N_Legao1;
    GameObject _N_Legao2;
    GameObject _N_Legao3;
    GameObject _N_Legao4;
  //  GameObject _NLegaoParent;
    // GameObject[] myStatcles;

    GameObject _reachParent;
    GameObject[] myTerrains;
    public GameObject[] myLeGaos;
    public int ii; //用来决定下一块随机生成的地图的编号

    int randomMin = 1;
    int randomMax = 5;


    // Use this for initialization
    void Start () {

        //路障
        _statcle = (GameObject)Resources.Load("Prefabs/WhiteClown");

        _N_Terrain1 = (GameObject)Resources.Load("Prefabs/N_Terrain1");
        _N_Terrain2 = (GameObject)Resources.Load("Prefabs/N_Terrain2");
        _N_Terrain3 = (GameObject)Resources.Load("Prefabs/N_Terrain3");
        _N_Terrain4 = (GameObject)Resources.Load("Prefabs/N_Terrain4");

        _N_Legao1 = (GameObject)Resources.Load("Prefabs/legao_1");
        _N_Legao2 = (GameObject)Resources.Load("Prefabs/legao_2");
        _N_Legao3 = (GameObject)Resources.Load("Prefabs/legao_3");
        _N_Legao4 = (GameObject)Resources.Load("Prefabs/legao_4");

        _NTerrainParent = GameObject.Find("TerrainParent");
        //_NLegaoParent = GameObject.Find("LegaoParent");

        _reachParent = this.transform.parent.gameObject;

        if (_reachParent.name == "_N_Terrain1")  //1 2
        {
            randomMin = 1;
            randomMax = 3;
        }
        if (_reachParent.name == "_N_Terrain2")   //3 4
        {
            randomMin = 3;
            randomMax = 5;
        }
        if (_reachParent.name == "_N_Terrain3")  //1 2
        {
            randomMin = 1;
            randomMax = 3;
        }
        if (_reachParent.name == "_N_Terrain4")  // 3 4
        {
            randomMin = 3;
            randomMax = 5;
        }

        myTerrains = new GameObject[4];
        myTerrains[0] = _N_Terrain1; 
        myTerrains[1] = _N_Terrain2;
        myTerrains[2] = _N_Terrain3;
        myTerrains[3] = _N_Terrain4;

        myLeGaos = new GameObject[4];
        myLeGaos[0] = _N_Legao1;
        myLeGaos[1] = _N_Legao2;
        myLeGaos[2] = _N_Legao3;
        myLeGaos[3] = _N_Legao4;

        ii = (int)Random.Range(randomMin, randomMax);
        //print("下一块地图的编号是："+ii);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log("开始碰撞" + col.collider.gameObject.name);
    //}
    void OnTriggerExit(Collider other)
    {
        Debug.Log("到达" + this.gameObject.name);
        GameObject N_Terrains = myTerrains[ii-1];
        N_Terrains = Instantiate(N_Terrains);   
        N_Terrains.transform.parent = _NTerrainParent.transform;
        N_Terrains.transform.position = _reachParent.transform.position;
        N_Terrains.name = "_N_Terrain" + ii;   //重命名新生成的地形名称

        if (ii == 1)
        {
            N_Terrains.transform.Translate(0, 0, 75);
            //路障
            GameObject N_statcle = Instantiate(_statcle);
            N_statcle.transform.parent = N_Terrains.transform;
            N_statcle.transform.position = N_Terrains.transform.position;
            N_statcle.transform.Translate(-OutputPos(), 0, Random.Range(-40, -75));
        }
        if (ii == 2)
        {
            N_Terrains.transform.Translate(0, 0, 75);
            //乐高
            GameObject N_legao = Instantiate(myLeGaos[(int)Random.Range(0,4)]);
            N_legao.transform.parent = N_Terrains.transform;
            N_legao.transform.position = N_Terrains.transform.position;
            if (Random.Range(-1, 1) < 0)
                N_legao.transform.Translate(-OutputPos(), (float)3.2, Random.Range(0, -45));
            else
                N_legao.transform.Translate(Random.Range(45, 75), (float)3.2, OutputPos());

        }
        if (ii == 3)
        {
            N_Terrains.transform.Translate(75, 0, 0);
            //路障
            GameObject N_statcle = Instantiate(_statcle);
            N_statcle.transform.parent = N_Terrains.transform;
            N_statcle.transform.position = N_Terrains.transform.position;
            if(Random.Range(-1, 1) < 0)
                N_statcle.transform.Translate(Random.Range(0, -45), 0, -OutputPos());
            else
                N_statcle.transform.Translate(-OutputPos(), 0, Random.Range(-45, -75));
        }
        if (ii == 4)
        {
            N_Terrains.transform.Translate(75, 0, 0);
            //乐高
            GameObject N_legao = Instantiate(myLeGaos[(int)Random.Range(0, 4)]);
            N_legao.transform.parent = N_Terrains.transform;
            N_legao.transform.position = N_Terrains.transform.position;
            N_legao.transform.Translate(Random.Range(40, 75), (float)3.2,  OutputPos());
 
        }
    }


    float OutputPos()   //随机取 30 37.5 45 中的一个
    {
        float[] aa = new float[3];
        aa[0] = 30;
        aa[1] = (float)37.5;
        aa[2] = 45;
        return aa[Random.Range(0, 3)];
    }

    float OutputPos1()   //随机取 30 37.5 45 中的一个
    {
        float[] aa = new float[3];
        aa[0] = 32;
        aa[1] = (float)37.5;
        aa[2] = 43;
        return aa[Random.Range(0, 3)];
    }


}
