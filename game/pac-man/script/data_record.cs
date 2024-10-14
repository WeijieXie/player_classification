using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class data_record : MonoBehaviour
{
    public int TestNO;
    private string TimeNow;
    private string format = "MM-dd-HH-mm-ss";
    public player player;
    public GameObject e1;
    public GameObject e2;
    public GameObject goal;
    public GameObject des;
   

    StreamWriter sw;

    int frameCounter = 1;

    private DateTime startTime = DateTime.UtcNow;


    // Start is called before the first frame update
    void Start()
    {
        TimeNow = GetTime().ToString(format);
        string fileName = "record" + TestNO + "_" + TimeNow + ".csv";

        string dirpath = Path.Combine(Application.dataPath, "data_recordings");

        if (!Directory.Exists(dirpath))
        {
            Directory.CreateDirectory(dirpath);
        }


        sw = new StreamWriter(Path.Combine(dirpath, fileName), true);
        sw.WriteLine("lable"+","+ "No" + "," + "Timestamp" + "," + "player_x" + "," + "player_y" 
            + "," + "enemy1_to_player_x" + "," + "enemy1_to_player_y" + "," 
            + "enemy2_to_player_x" + "," + "enemy2_to_player_y" + "," 
            + "goal_to_player_x" + "," + "goal_to_player_y" + ","      
            + "goal_to_des_x" + "," + "goal_to_des_y" + ","
            + "player_to_des_x" + "," + "player_to_des_y" + ","
            + "input_x" + "," + "input_y" + "," + "input_z" + "," + "input_w" + "," + "input_all"
            
            );
    }

    // Update is called once per frame
    void Update()
    {
        sw.WriteLine(TestNO +"," + frameCounter + "," + GetTimeStampMilliSecond() 
            + "," + player.transform.position.x + "," + player.transform.position.z 
            + "," + (e1.transform.position.x - player.transform.position.x) 
            + "," + (e1.transform.position.z - player.transform.position.z) 
            + "," + (e2.transform.position.x - player.transform.position.x) 
            + "," + (e2.transform.position.z - player.transform.position.z) 
            + "," + (goal.transform.position.x - player.transform.position.x) 
            + "," + (goal.transform.position.z - player.transform.position.z)
            + "," + (des.transform.position.x - goal.transform.position.x)
            + "," + (des.transform.position.z - goal.transform.position.z)
            + "," + (des.transform.position.x - player.transform.position.x)
            + "," + (des.transform.position.z - player.transform.position.z)

            + "," + player.input.x
            + "," + player.input.y
            + "," + player.input.z
            + "," + player.input.w
            + "," + (player.input.x*8+ player.input.y * 4+player.input.z * 2+player.input.w * 1)
            );
        frameCounter++;



    }
    public static System.DateTime GetTime()
    {
        return System.DateTime.Now;
    }

    public long GetTimeStampMilliSecond()
    {
        TimeSpan ts = DateTime.UtcNow - startTime;
        try
        {
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            Debug.Log($"GetTimeStampMilliSecond Error = {ex}");
            return 0;
        }
    }
}
