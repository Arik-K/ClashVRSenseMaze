using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LoggerScript : MonoBehaviour
{
    public GameManager gm;
    public GameObject player;
    public Camera playerCamera;
    public GameObject leftHand;
    public GameObject rightHand;

    private string condition = "";   
    private string filePathPrefix;
    private string playerFilePath;
    private string leftHandFilePath;
    private string rightHandFilePath;
    private float lastLogTime;
    private const float LogInterval = 0.02f; // 50Hz (1/50 = 0.02 seconds)

    private Dictionary<GameObject, Collider> ignoredColliders = new Dictionary<GameObject, Collider>();

    void Start()
    {
        string date_string = DateTime.Now.ToString("-dd-MM-yyyy_HH-mm-ss");
        filePathPrefix = Path.Combine(Application.persistentDataPath, "game_log" + date_string);
        
        playerFilePath = filePathPrefix + "_player.csv";
        leftHandFilePath = filePathPrefix + "_lefthand.csv";
        rightHandFilePath = filePathPrefix + "_righthand.csv";

        InitializeCSVFile(playerFilePath);
        InitializeCSVFile(leftHandFilePath);
        InitializeCSVFile(rightHandFilePath);

        if (gm == null)
        {
            Debug.LogError("GameManager is not set in the LoggerScript inspector!");
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not set in the LoggerScript inspector!");
        }

        // Store references to the colliders you want to ignore
        ignoredColliders[leftHand] = leftHand.GetComponents<Collider>()[0];
        ignoredColliders[rightHand] = rightHand.GetComponents<Collider>()[0];
    }

    void InitializeCSVFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "UnixTimestamp,Path,Condition,CollisionObject,PosX,PosY,PosZ,RotX,RotY,RotZ\n");
        }
        Debug.Log("File initialized: " + filePath);
    }

    void Update()
    {
        if (Time.time - lastLogTime >= LogInterval)
        {
            LogData();
            lastLogTime = Time.time;
        }
    }

    private void LogData()
    {
        long unixTimestamp = DateTimeToUnixTimestamp(DateTime.Now);
        int currentPath = GameManager.path;

        if (gm.ConditionCount - 1 < 0)
        {
            condition = gm.conditions[0];
        }
        else
        {
            condition = gm.conditions[gm.ConditionCount - 1];
        }

        LogObjectData(player, playerFilePath, unixTimestamp, currentPath, useCamera: true);
        LogObjectData(leftHand, leftHandFilePath, unixTimestamp, currentPath);
        LogObjectData(rightHand, rightHandFilePath, unixTimestamp, currentPath);
    }

    private void LogObjectData(GameObject obj, string filePath, long timestamp, int currentPath, bool useCamera = false)
    {
        Vector3 position;
        Vector3 rotation;

        if (useCamera && playerCamera != null)
        {
            position = playerCamera.transform.position;
            rotation = playerCamera.transform.eulerAngles;
        }
        else
        {
            position = obj.transform.position;
            rotation = obj.transform.eulerAngles;
        }

        string collisionObject = CheckCollision(obj);

        string logEntry = string.Format("{0},{1},{2},{3},{4:F2},{5:F2},{6:F2},{7:F2},{8:F2},{9:F2}",
            timestamp, currentPath, condition, collisionObject,
            position.x, position.y, position.z,
            rotation.x, rotation.y, rotation.z);

        File.AppendAllText(filePath, logEntry + Environment.NewLine);
    }

    private string CheckCollision(GameObject obj)
    {
        Collider objCollider;
        
        if (obj == player)
        {
            // For player, use the collider on the player object
            objCollider = player.GetComponent<Collider>();
        }
        else
        {
            // For hands, use all colliders except the ignored one
            Collider[] objColliders = obj.GetComponents<Collider>();
            objCollider = objColliders.Length > 0 ? objColliders[0] : null;
        }

        if (objCollider == null)
        {
            Debug.LogWarning($"No Collider found on {obj.name}. Collision check skipped.");
            return "NoCollider";
        }

        List<string> collidedTags = new List<string>();

        Collider[] colliders = Physics.OverlapBox(
            objCollider.bounds.center, 
            objCollider.bounds.extents, 
            obj.transform.rotation
        );

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != obj && 
                !collider.CompareTag("Untagged") && 
                !collider.CompareTag("MainCamera") && 
                !collider.CompareTag("Player") && 
                !collider.CompareTag("Hand"))
            {
                collidedTags.Add(collider.tag);
            }
        }

        if (collidedTags.Count > 0)
        {
            return string.Join("|", collidedTags);
        }

        return "None";
    }

    private long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}




