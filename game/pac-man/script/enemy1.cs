using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemy1 : MonoBehaviour
{

    public GameObject target;
    public float MIN_trackingRate;//最小的追踪向量改变率
    public float MIN_TrackingDis;
    public float MAX_trackingVel;
    public float moveVx;//x方向的速度
    public float moveVz;//z方向的速度
                        // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((Mathf.Atan(moveVx / moveVz) * (-180 / Mathf.PI)));

        //  LookAtTarget();
        //  this.transform.position += new Vector3(moveVx * Time.deltaTime, moveVy * Time.deltaTime, 0);
        
        Track_AIAdvanced();
       
    }
    

    /// <summary>
    /// 追踪算法
    /// </summary>
    void Track_AIAdvanced()
    {
        //计算与追踪目标的方向向量
        float vx = target.transform.position.x - this.transform.position.x;
        float vz = target.transform.position.z - this.transform.position.z;

        float length = PointDistance_2D(vx, vz);
        //如果达到距离就追踪
        if (length < MIN_TrackingDis)
        {
            vx = MIN_trackingRate * vx / length;
            vz = MIN_trackingRate * vz / length;
            moveVx += vx;
            moveVz += vz;

            //增加一点扰动
            if (Random.Range(1, 10) == 1)
            {
                vx = Random.Range(-1, 1);
                vz = Random.Range(-1, 1);
                moveVx += vx;
                moveVz += vz;
            }
            length = PointDistance_2D(moveVx, moveVz);

            //如果导弹飞的速度太快就让它慢下来
            if (length > MAX_trackingVel)
            {
                //让它慢下来
                moveVx *= 0.75f;
                moveVz *= 0.75f;
            }

        }
        //如果不在追踪范围内，随机运动
        else
        {
            if (Random.Range(1, 10) == 1)
            {
                vx = Random.Range(-2, 2);
                vz = Random.Range(-2, 2);
                moveVx += vx;
                moveVz += vz;
            }
            length = PointDistance_2D(moveVx, moveVz);

            //如果导弹飞的速度太快就让它慢下来
            if (length > MAX_trackingVel)
            {
                //让它慢下来
                moveVx *= 0.75f;
                moveVz *= 0.75f;
            }
        }


        this.transform.position += new Vector3(moveVx * Time.deltaTime ,0 ,moveVz * Time.deltaTime);
    }

    float PointDistance_2D(float x, float y)
    {
        //使用了泰勒展开式来计算，有3.5%的误差,直接使用开方计算会比较慢，但是测试了我的电脑好像没有什么变化可能是数据量不大体现不出来
        /*x = Mathf.Abs(x);
        y = Mathf.Abs(y);
        float mn = Mathf.Min(x, y);//获取x,y中最小的数
        float result = x + y - (mn / 2) - (mn / 4) + (mn / 8);*/

        float result = Mathf.Sqrt(x * x + y * y);
        return result;
    }
}