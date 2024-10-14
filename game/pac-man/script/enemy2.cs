
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
        //���ȼ������ߵľ���
        Vector2 ToPursuit =new Vector2(m_pursuitTarget.transform.position.x, m_pursuitTarget.transform.position.z) - m_pursuiter.Position;
        //�ֲ������ǰ�����������ĵ��
        float RelativeHeading = DotProduct(m_pursuiter.vHeading,  m_pursuitTarget.vHeading);
        //������ߵľ�����׷���ߵľֲ�����ǰ�������ͶӰ�����㣬��ô׷����Ӧ��ֱ����׷�ٶ����ƶ��������20�Ǳ�׷�ٶ���ķ������׷���ߵĳ�����18��(cos(18)=0.95)�ͱ���Ϊ������ŵ�
        if (DotProduct(ToPursuit, m_pursuiter.vHeading) > 0 && RelativeHeading < -0.95f)
        {
            //            Debug.Log("relativeHeading:" + RelativeHeading);
            return AI_Seek(new Vector2(m_pursuitTarget.transform.position.x, m_pursuitTarget.transform.position.z));
        }
        //Ԥ�ⱻ׷���ߵ�λ�ã�Ԥ���ʱ�������ڱ�׷������׷���ߵľ��룬������׷���ߵ��ٶȺ͵�ǰ������׷���ߵı�Ԥ��λ��(���ƿڰ�,��������!)
        float toPursuitLenght = Mathf.Sqrt(ToPursuit.x * ToPursuit.x + ToPursuit.y * ToPursuit.y);
        float LookAheadTime = toPursuitLenght / (m_pursuiter.MaxSpeed + 5);//�ٶ�
        //Ԥ���λ��,��ʵ���λ�û�һֱ�ڱ�׷�ٶ���ľֲ�����ǰ��
        Vector2 predictionPos = new Vector2(m_pursuitTarget.transform.position.x, m_pursuitTarget.transform.position.z) + m_pursuitTarget.Velocity * LookAheadTime;
        Debug.Log("preditionx:" + predictionPos.x + "preditiony:" + predictionPos.y);
        AimPredictionPos.transform.position = new Vector3(predictionPos.x,0, predictionPos.y);
        return AI_Seek(predictionPos);
    }

    Vector2 AI_Seek(Vector2 TargetPos)
    {
        //����Ŀ��λ����׷�ٶ���λ�õ��������ҽ����һ��
        Vector2 DesiredVelocity = (TargetPos - m_pursuiter.Position).normalized * m_pursuiter.MaxSpeed;
        //ֱ������Ϳ��Եõ�һ���м�Ĺ�������������ֱ����Ӳ�ĸı�
        DesiredVelocity = DesiredVelocity - m_pursuiter.Velocity;
        return DesiredVelocity;
    }
  
  
    float DotProduct(Vector2 A, Vector2 B)
    {
        return A.x * B.x + A.y * B.y;
    }
}