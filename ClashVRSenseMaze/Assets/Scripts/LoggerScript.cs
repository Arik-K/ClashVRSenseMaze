using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class LoggerScript : MonoBehaviour
{
    //private Vector3[] initial_positions = GameManagerScript.initial_position;
    private string[] MazeType;
    private string[] ConditionType;
    private float samplingTime = 0.02f; // sample time in sec
    private string date_string = DateTime.Now.ToString("-dd-MM-yyyy_hh-mm-ss");
    private RaycastHit hit_info;
    public GameObject player; // the player game object
    System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);    
    // Start is called before the first frame update
    void Start()
    {
        string filePath = Application.dataPath + "/" + "Data" + "/" + "Mazes" + "-" + "RawData" + date_string + ".csv";
        string filePath1 = Application.dataPath + "/" + "Data" + "/" + "Mazes-Summary" + date_string +  ".csv";
        File.AppendAllText(filePath1, "maze_type,condition,collision_number,success_maze,completion_time" + Environment.NewLine);
        File.AppendAllText(filePath, "time,x,y,z,rx,ry,rz,maze_type,condition,collision_status,raycast_distance" + Environment.NewLine);
        if(MainMenu.isTraining)
        {
            return;

        }
    }

    public void OnEnable()
    {
        MazeType = GameManagerScript.mazes_name_list;
        ConditionType = GameManagerScript.conditions;
        hit_info = DistancePitchRaycast.hit;
        InvokeRepeating("SampleNow", 0, samplingTime);
    }

    public void ReachLocation(string[] MazeType, string[] ConditionType, int collision_number, float MazeSolveTime)
    {
        int success = 1;
        int maze_index = GameManagerScript.mazeIndex; 
        string filePath1 = Application.dataPath + "/" + "Data" + "/" + "Mazes-Summary" + date_string +  ".csv";
        string logText = string.Format("{0},{1},{2},{3},{4}", MazeType[maze_index], ConditionType[maze_index], collision_number, success, MazeSolveTime);
        File.AppendAllText(filePath1, logText + Environment.NewLine);
    }

    public void FailedReachLocation(string[] MazeType, string[] ConditionType, int collision_number, float MazeSolveTime)
    {
        int success = 0;
        int maze_index = GameManagerScript.mazeIndex; 
        string filePath1 = Application.dataPath + "/" + "Data" + "/" + "Mazes-Summary" + date_string +  ".csv";
        string logText = string.Format("{0},{1},{2},{3},{4}", MazeType[maze_index], ConditionType[maze_index], collision_number, success, MazeSolveTime);
        File.AppendAllText(filePath1, logText + Environment.NewLine);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }

    public void SampleNow()
    {
        MazeType = GameManagerScript.mazes_name_list;
        ConditionType = GameManagerScript.conditions;
        hit_info = DistancePitchRaycast.hit;
        //print(MazeType.Length);
        int maze_index = GameManagerScript.mazeIndex;
        int cur_collision_status = PlayerMovement.collision_status;
        Transform targetTransform = player.GetComponent<Transform>();
        //string timestamp =  Time.time.ToString();
        long cur_time = (long)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        string filePath = Application.dataPath + "/" + "Data" + "/" + "Mazes" + "-" + "RawData" + date_string + ".csv";
        //print("index:" + maze_index);
        //print("Maze:" + MazeType.Length);
        //print("Conditiom:" + ConditionType.Length);
        string logText = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", cur_time, targetTransform.position.x, targetTransform.position.y, targetTransform.position.z, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z, MazeType[maze_index], ConditionType[maze_index], cur_collision_status, hit_info.distance);
        File.AppendAllText(filePath, logText + Environment.NewLine);
        PlayerMovement.collision_status = 0; 
    }

}
