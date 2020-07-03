using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBlood : MonoBehaviour
{
    public GameObject player;



    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(player.transform.position);

        int hp = transform.parent.GetComponent<BossArtificialIdiot>().hp;
        
        transform.GetChild(1).localScale = new Vector3(hp *(1f/100f), 0.2f, 1);
        transform.GetChild(1).localPosition = new Vector3(((100-hp) * 0.01f) / 2 * 3.62f, 6, 0.1f);
    }
}
