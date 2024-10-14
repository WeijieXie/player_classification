using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class goal_s : PlayObject
{
    public PlayObject goal;
    public GameObject Aim;
    public float vecChangeRate;//这个数值越大在目标点的穿梭次数就会越多，一般为1
    public float MaxSpeedofGoal;
    public float FleeDistance;
    Vector2 AimPos;
    // Use this for initialization
    void Start()
    {
        AimPos = new Vector2(Aim.transform.position.x, Aim.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        
     
            AimPos = new Vector2(Aim.transform.position.x, Aim.transform.position.z);
        
        Vector2 moveVec = AI_UNSeek(AimPos);
        float length = Mathf.Sqrt(moveVec.x * moveVec.x + moveVec.y * moveVec.y);
        if (length != 0)
        {
            //   Debug.Log("x:" + moveVec.x + "y:" + moveVec.y);
            goal.moveVx += vecChangeRate * moveVec.x / length;
            goal.moveVz += vecChangeRate * moveVec.y / length;

        }
        goal.Move(1, true);
    }
    Vector2 AI_UNSeek(Vector2 targetPos)
    {
        Vector2 disPos = goal.Position - targetPos;
        float distance = Mathf.Sqrt(disPos.x * disPos.x + disPos.y * disPos.y);
        if (distance > FleeDistance)
        {
            return Vector2.zero;
        }
        disPos = disPos.normalized * MaxSpeedofGoal - goal.Velocity;
        return disPos;
    }
}
