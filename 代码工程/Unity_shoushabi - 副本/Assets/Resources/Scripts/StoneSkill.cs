using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkill : MonoBehaviour {

    public Vector3 playerPos;
    public Vector3 aimPos;
    public float smooth;

    public float createTime;


	// Use this for initialization
	void Start ()
    {
        AddForceByDistance(playerPos, aimPos);
        smooth = 3;

        createTime = Time.time;

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void FixedUpdate()
    {
        //10s后石头消失
        if (Time.time - createTime > 10)
        {
            Destroy(this.gameObject);
        }
    }

    void AddForceByDistance(Vector3 playerPos,Vector3 aimPos)
    {
        float distance = (playerPos - aimPos).magnitude;

        float distanceXY = (new Vector3(playerPos.x, playerPos.y, 0) - new Vector3(aimPos.x, aimPos.y, 0)).magnitude;
        //Debug.Log("distance:"+distanceXY);
        //x y坐标下 二维向量的夹角和最佳角，第三维度变成0不影响
        Vector3 dirXY = new Vector3(aimPos.x, aimPos.y, 0) - new Vector3(playerPos.x, playerPos.y, 0);

        Transform forwardTo = GameObject.Find("FrontPos").transform;

        float thetaOriXY = Mathf.Atan(dirXY.y/dirXY.x)*Mathf.Rad2Deg;

        float a = Vector3.Angle(dirXY, new Vector3(1, 0 ,0)); // 0-180 验证是正确的
        float thetaBestXY = (thetaOriXY/2) + 45;

        Vector3 dirInFact = new Vector3(Mathf.Cos(thetaBestXY * Mathf.Deg2Rad), Mathf.Sin(thetaBestXY * Mathf.Deg2Rad),0);

        //Debug.Log("thetaBestXY"+thetaBestXY);  
        //转换坐标系后的高“高度”,可以计算出时间。
        float abnewHeightXY = distanceXY * Mathf.Abs(Mathf.Sin((45 - (thetaOriXY / 2)) * Mathf.Deg2Rad)); // thetaOriXY+90-thetaBestXY = this

        float t =Mathf.Sqrt( (2 * abnewHeightXY) / (9.8f * Mathf.Abs(Mathf.Cos(thetaBestXY* Mathf.Deg2Rad))));
        //由时间和飞行长度上计算出力的大小
        float abnewLegthXY = distanceXY * Mathf.Abs(Mathf.Cos((45 - (thetaOriXY / 2)) * Mathf.Deg2Rad)); // distance in new axies
        //float fXY = 2 * abnewLegthXY + 10 * Mathf.Sin(thetaBestXY * Mathf.Deg2Rad);
        //Vector3 fXYFinal = dirXY.normalized * fXY *100;

        float vXY = (abnewLegthXY+(0.5f*9.8f*Mathf.Pow(t,2))) / t ;
        Vector3 vXYFinal = dirInFact.normalized * vXY;

        //Debug.Log("dirXY:" + dirXY);
        //Debug.Log("dirInFact:" + dirInFact);

        //Debug.Log("thetaOriXY:" + thetaOriXY);
        //Debug.Log("thetaBestXY:" + thetaBestXY);
        //Debug.Log("abnewHeightXY:"+abnewHeightXY);
        //Debug.Log("time:" + t);
        //Debug.Log(2 * abnewHeightXY);
        //Debug.Log("10 * Mathf.Cos(thetaBestXY):"+ 10 * Mathf.Cos(thetaBestXY));
        //Debug.Log("abnewLegthXY:" + abnewLegthXY);
        //Debug.Log("______________");




        float distanceXZ = (new Vector3(playerPos.x, 0, playerPos.z) - new Vector3(aimPos.x, 0, aimPos.z)).magnitude;
        //Debug.Log("distance:" + distanceXZ);
        //x y坐标下 二维向量的夹角和最佳角，第三维度变成0不影响
        Vector3 dirXZ = new Vector3(aimPos.x, 0, aimPos.z) - new Vector3(playerPos.x, 0, playerPos.z);

        float fXZ = 5;
        //Vector3 fXZFinal = dirXZ.normalized * fXZ;
        //Vector3 fFinal = fXYFinal + fXZFinal;

        float vXZ = distanceXZ / t;
        Vector3 vXZFinal = dirXZ.normalized * vXZ;

        GetComponent<Rigidbody>().velocity = vXZFinal + new Vector3(0,vXYFinal.y,0);
    }

}
