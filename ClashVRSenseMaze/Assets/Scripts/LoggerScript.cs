using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class LoggerScript : MonoBehaviour
{
   
   GameManager gm;
    //private Vector3[] initial_positions = GameManagerScript.initial_position;
    private string[] MazeType;
    private string[] ConditionType;
    private float samplingTime = 0.02f; // sample time in sec
    private string date_string = DateTime.Now.ToString("-dd-MM-yyyy_hh-mm-ss");
    private RaycastHit hit_info;
    public GameObject player; // the player game object
    System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);    
      // Path to the CSV file
    private string filePath;

    void Start()
    {
        // Define the file path for the CSV file
        filePath = Application.dataPath + "/Data/collision_log" + date_string +".csv";

        // Check if the file already exists, if not, create it and add headers
        if (!File.Exists(filePath))
        {
            // Add a header to the CSV file
            File.WriteAllText(filePath, "Time, Collision Type, Object Tag\n");
        }
    }

    // Called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter(Collision collision)
    {
        // Log the collision time, type, and object tag to the CSV
        string time = Time.time.ToString(); // Get the time since the start of the game
        string collisionType = "Collision";
        string objectTag = collision.gameObject.tag; // Get the tag of the colliding object

        // Format the data as a CSV line
        string logEntry = time + ", " + collisionType + ", " + objectTag + "\n";

        // Append the log entry to the CSV file
        File.AppendAllText(filePath, logEntry);

        // Optionally log to console as well for debugging
        Debug.Log("Logged: " + logEntry);
    }

    // Called when the collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // Log the trigger time, type, and object tag to the CSV
        string time = Time.time.ToString(); // Get the time since the start of the game
        string collisionType = "Trigger";
        string objectTag = other.gameObject.tag; // Get the tag of the triggering object

        // Format the data as a CSV line
        string logEntry = time + ", " + collisionType + ", " + objectTag + "\n";

        // Append the log entry to the CSV file
        File.AppendAllText(filePath, logEntry);

        // Optionally log to console as well for debugging
        Debug.Log("Logged: " + logEntry);
    }
}



