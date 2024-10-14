using UnityEngine;
using System.Collections;

public class PlayObject : MonoBehaviour
{
    [HideInInspector]
    public float moveVx;//x方向的分速度
    [HideInInspector]
    public float moveVz;//y方向的分速度
    public float MaxSpeed;
    /// <summary>
    /// 2维坐标(x,y)
    /// </summary>
    public Vector2 Position
    {
        get
        {
            return new Vector2(this.transform.position.x, this.transform.position.z);
        }
    }
    private Vector2 _vHeading;
    /// <summary>
    /// //设置导弹的前进方向的归一化向量m_vHeading
    /// </summary>
    public Vector2 vHeading
    {
        get
        {
            float length = Mathf.Sqrt(moveVx * moveVx + moveVz * moveVz);
            if (length != 0)
            {
                _vHeading.x = moveVx / length;
                _vHeading.y = moveVz / length;
            }
            return _vHeading;
        }
    }
    private Vector2 _vSide;
    /// <summary>
    /// 前进方向的垂直向量
    /// </summary>
    public Vector2 vSide
    {
        get
        {
            _vSide.x = -vHeading.y;
            _vSide.y = vHeading.x;
            return _vSide;
        }
    }

    /// <summary>
    /// 速度向量
    /// </summary>
    public Vector2 Velocity
    {
        get
        {
            return new Vector2(moveVx, moveVz);
        }
    }
    /// <summary>
    /// 速度标量
    /// </summary>
    public float Speed
    {
        get
        {
            return Mathf.Sqrt(moveVx * moveVx + moveVz * moveVz);
        }
    }
    public float MaxSpeedRate;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
  
    public void Move(float speedRate, bool isLookAtVelocityVector)
    {
        this.transform.position += new Vector3(moveVx * Time.deltaTime,0, moveVz * Time.deltaTime) * speedRate;
        //  Debug.Log("x:" + m_postion.x + "y:" + m_postion.y);
        //调整导弹的朝向是的它和速度矢量合成方向一样
        if (isLookAtVelocityVector)
        {
            LookAtVelocityVector();
        }
    }
    /// <summary>
    /// 使得物体始终朝向矢量速度的方向
    /// </summary>
    void LookAtVelocityVector()
    {
        float yAngles = Mathf.Atan(moveVx / moveVz) * (-180 / Mathf.PI);
        if (moveVz == 0)
        {
            yAngles = moveVx > 0 ? -90 : 90;
            //跟以往的计算角度不同的是，这里加了moveVx==0的独立判断，这样可以在不控制的时候保持原状态
            if (moveVx == 0)
            {
                yAngles = this.transform.rotation.eulerAngles.y;
            }
        }

        if (moveVz < 0)
        {
            yAngles = yAngles - 180;
        }
        Vector3 tempAngles = new Vector3(0, yAngles,0);
        Quaternion tempQua = this.transform.rotation;
        tempQua.eulerAngles = tempAngles;
        this.transform.rotation = tempQua;
    }
}

