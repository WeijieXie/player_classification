using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemy1 : MonoBehaviour
{

    public GameObject target;
    public float MIN_trackingRate;//��С��׷�������ı���
    public float MIN_TrackingDis;
    public float MAX_trackingVel;
    public float moveVx;//x������ٶ�
    public float moveVz;//z������ٶ�
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
    /// ׷���㷨
    /// </summary>
    void Track_AIAdvanced()
    {
        //������׷��Ŀ��ķ�������
        float vx = target.transform.position.x - this.transform.position.x;
        float vz = target.transform.position.z - this.transform.position.z;

        float length = PointDistance_2D(vx, vz);
        //����ﵽ�����׷��
        if (length < MIN_TrackingDis)
        {
            vx = MIN_trackingRate * vx / length;
            vz = MIN_trackingRate * vz / length;
            moveVx += vx;
            moveVz += vz;

            //����һ���Ŷ�
            if (Random.Range(1, 10) == 1)
            {
                vx = Random.Range(-1, 1);
                vz = Random.Range(-1, 1);
                moveVx += vx;
                moveVz += vz;
            }
            length = PointDistance_2D(moveVx, moveVz);

            //��������ɵ��ٶ�̫�������������
            if (length > MAX_trackingVel)
            {
                //����������
                moveVx *= 0.75f;
                moveVz *= 0.75f;
            }

        }
        //�������׷�ٷ�Χ�ڣ�����˶�
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

            //��������ɵ��ٶ�̫�������������
            if (length > MAX_trackingVel)
            {
                //����������
                moveVx *= 0.75f;
                moveVz *= 0.75f;
            }
        }


        this.transform.position += new Vector3(moveVx * Time.deltaTime ,0 ,moveVz * Time.deltaTime);
    }

    float PointDistance_2D(float x, float y)
    {
        //ʹ����̩��չ��ʽ�����㣬��3.5%�����,ֱ��ʹ�ÿ��������Ƚ��������ǲ������ҵĵ��Ժ���û��ʲô�仯�������������������ֲ�����
        /*x = Mathf.Abs(x);
        y = Mathf.Abs(y);
        float mn = Mathf.Min(x, y);//��ȡx,y����С����
        float result = x + y - (mn / 2) - (mn / 4) + (mn / 8);*/

        float result = Mathf.Sqrt(x * x + y * y);
        return result;
    }
}