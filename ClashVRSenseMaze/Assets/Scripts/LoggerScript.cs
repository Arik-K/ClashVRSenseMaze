using System;
using System.IO;
using UnityEngine;

public class LoggerScript : MonoBehaviour
{
    public GameManager gm;
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;

    public string condition = "";   
    private string filePath;
    private float lastCollisionTime = -1f;
    private float collisionCooldown = 0.5f; // 0.5 seconds cooldown between collisions
    private float startTime; // To track when the game started

    void Start()
    {
        startTime = Time.time; // Record the start time of the game
        string date_string = DateTime.Now.ToString("-dd-MM-yyyy_HH-mm-ss");
        filePath = Path.Combine(Application.persistentDataPath, "collision_log" + date_string + ".csv");
        Debug.Log("File path: " + filePath);

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Timeframe,Path,Condition,CollisionSource,ObjectTag,PosX,PosY,PosZ,RotX,RotY,RotZ\n");
        }

        if (gm == null)
        {
            Debug.LogError("GameManager is not set in the LoggerScript inspector!");
        }

    }

    public void LogCollision(GameObject collidedObject, string collisionSource)
    {
        // Ignore collisions with objects tagged as "Hand" or "Untagged"
        if (collidedObject.CompareTag("Hand") || collidedObject.CompareTag("Untagged") || collidedObject.CompareTag("Player"))
            return;
            
        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        lastCollisionTime = Time.time;

        float elapsedTime = Time.time - startTime; // Calculate elapsed time since game started
        int CurrPath = GameManager.path; // Assuming gm.currentPath is an int
        if (gm.ConditionCount-1 < 0){
            condition = gm.conditions[0];
        }
        else{
            condition = gm.conditions[gm.ConditionCount-1];
        }
        
        string objectTag = collidedObject.tag;

        Vector3 position;
        Vector3 rotation;

        switch (collisionSource)
        {
            case "LeftHand":
                position = leftHand.transform.position;
                rotation = leftHand.transform.eulerAngles;
                break;
            case "RightHand":
                position = rightHand.transform.position;
                rotation = rightHand.transform.eulerAngles;
                break;
            default: // Player body
                position = player.transform.position;
                rotation = player.transform.eulerAngles;
                break;
        }

        string logEntry = string.Format("{0:F3},{1},{2},{3},{4},{5:F2},{6:F2},{7:F2},{8:F2},{9:F2},{10:F2}",
            elapsedTime, CurrPath, condition, collisionSource, objectTag,
            position.x, position.y, position.z,
            rotation.x, rotation.y, rotation.z);

        File.AppendAllText(filePath, logEntry + Environment.NewLine);
        Debug.Log("Log written: " + logEntry);
    }

    void OnCollisionEnter(Collision collision)
    {
        LogCollision(collision.gameObject, "Body");
    }

    void OnTriggerEnter(Collider other)
    {
        LogCollision(other.gameObject, "Body");
    }
}




