
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemy2 : PlayObject
{
    public PlayObject m_pursuiter;
    public PlayObject m_pursuitTarget;
    public GameObject AimPredictionPos;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //  Vector2 moveVec = AI_Seek(m_pursuitTarget.Position);
        Vector2 moveVec = AI_PredictionPursuit();
        m_pursuiter.moveVx += moveVec.x;
        m_pursuiter.moveVz += moveVec.y;
        m_pursuiter.Move(1, true);
    }

    Vector2 AI_PredictionPursuit()
    {
        //首先计算两者的距离
        Vector2 ToPursuit =new Vector2(m_pursuitTarget.transform.position.x, m_pursuitTarget.transform.position.z) - m_pursuiter.Position;
        //局部坐标的前进方向向量的点积
        float RelativeHeading = DotProduct(m_pursuiter.vHeading,  m_pursuitTarget.vHeading);
        //如果两者的距离在追逐者的局部向量前进方向的投影大于零，那么追逐者应该直接向被追踪对象移动，这里的20是被追踪对象的反方向和追踪者的朝向在18度(cos(18)=0.95)就被认为是面对着的
        if (DotProduct(ToPursuit, m_pursuiter.vHeading) > 0 && RelativeHeading < -0.95f)
        {
            //            Debug.Log("relativeHeading:" + RelativeHeading);
            return AI_Seek(new Vector2(m_pursuitTarget.transform.position.x, m_pursuitTarget.transform.position.z));
        }
        //预测被追踪者的位置，预测的时间正比于被追踪者与追踪者的距离，反比与追踪者的速度和当前靠近被追踪者的被预测位置(好绕口啊,慢慢理解吧!)
        float toPursuitLenght = Mathf.Sqrt(ToPursuit.x * ToPursuit.x + ToPursuit.y * ToPursuit.y);
        float LookAheadTime = toPursuitLenght / (m_pursuiter.MaxSpeed + 5);//速度
        //预测的位置,其实这个位置会一直在被追踪对象的局部坐标前方
        Vector2 predictionPos = new Vector2(m_pursuitTarget.transform.position.x, m_pursuitTarget.transform.position.z) + m_pursuitTarget.Velocity * LookAheadTime;
        Debug.Log("preditionx:" + predictionPos.x + "preditiony:" + predictionPos.y);
        AimPredictionPos.transform.position = new Vector3(predictionPos.x,0, predictionPos.y);
        return AI_Seek(predictionPos);
    }

    Vector2 AI_Seek(Vector2 TargetPos)
    {
        //计算目标位置与追踪对象位置的向量并且将其归一化
        Vector2 DesiredVelocity = (TargetPos - m_pursuiter.Position).normalized * m_pursuiter.MaxSpeed;
        //直接相减就可以得到一个中间的过渡向量，避免直接生硬的改变
        DesiredVelocity = DesiredVelocity - m_pursuiter.Velocity;
        return DesiredVelocity;
    }
  
  
    float DotProduct(Vector2 A, Vector2 B)
    {
        return A.x * B.x + A.y * B.y;
    }
}