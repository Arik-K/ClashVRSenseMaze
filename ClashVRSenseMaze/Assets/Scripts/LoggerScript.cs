using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class LoggerScript : MonoBehaviour
{
    GameManager gm;
    public GameObject GameManager;

    private float samplingTime = 0.02f; // sample time in sec
    public GameObject player;
    
    private string filePath;
    private float lastCollisionTime = -1f;
    private float collisionCooldown = 0.5f; // 0.5 seconds cooldown between collisions

    void Start()
    {
        string date_string = DateTime.Now.ToString("-dd-MM-yyyy_hh-mm-ss");
        filePath = Path.Combine(Application.persistentDataPath, "collision_log" + date_string + ".csv");
        Debug.Log("File path: " + filePath);

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Time, Collision Type, Object Tag\n");
        }

        if (GameManager != null)
        {
            gm = GameManager.GetComponent<GameManager>();
            if (gm == null)
            {
                Debug.LogError("GameManager component not found on GameManager GameObject!");
            }
        }
        else
        {
            Debug.LogError("GameManager GameObject is not set in the LoggerScript inspector!");
        }

    }

    void Update()
    {
        // Removed the buffer-related logic and timed writes
    }

    void OnApplicationQuit()
    {
        // No need to write the buffer anymore
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        lastCollisionTime = Time.time;

        string time = DateTime.Now.ToString("hh:mm:ss.fff");
        string Condition = gm.conditions[gm.ConditionCount];
        string objectTag = collision.gameObject.tag;

        string logEntry = time + ", " + Condition + ", " + objectTag;

        // Write the log entry directly to the file
        File.AppendAllText(filePath, logEntry + Environment.NewLine);

        Debug.Log("Log written: " + logEntry);
    }

    void OnTriggerEnter(Collider other)
    {
        string time = DateTime.Now.ToString("hh:mm:ss.fff");
        string Condition = gm.conditions[gm.ConditionCount];
        string objectTag = other.gameObject.tag;

        string logEntry = time + ", " + Condition + ", " + objectTag;

        // Write the log entry directly to the file
        File.AppendAllText(filePath, logEntry + Environment.NewLine);

        Debug.Log("Log written: " + logEntry);
    }
}





