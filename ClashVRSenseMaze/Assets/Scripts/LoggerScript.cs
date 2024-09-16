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
   // private string date_string = DateTime.Now.ToString("-dd-MM-yyyy_hh-mm-ss");
    public GameObject player;
    
    private string filePath;
    private List<string> logBuffer = new List<string>();
    private float writeInterval = 5.0f; // Write every 5 seconds
    private float timeSinceLastWrite = 0f;
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
        timeSinceLastWrite += Time.deltaTime;
        if (timeSinceLastWrite >= writeInterval)
        {
            WriteLogBufferToFile();
            timeSinceLastWrite = 0f;
        }
    }

    void OnApplicationQuit()
    {
        WriteLogBufferToFile(); // Ensure all logs are written on exit
    }

    private void WriteLogBufferToFile()
    {
        if (logBuffer.Count > 0)
        {
            File.AppendAllLines(filePath, logBuffer);
            logBuffer.Clear();
        }
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
        logBuffer.Add(logEntry);

        Debug.Log("Buffered Log: " + logEntry);
    }

    void OnTriggerEnter(Collider other)
    {
        string time = DateTime.Now.ToString("hh:mm:ss.fff");
        string Condition = gm.conditions[gm.ConditionCount];
        string objectTag = other.gameObject.tag;

        string logEntry = time + ", " + Condition + ", " + objectTag;
        logBuffer.Add(logEntry);

        Debug.Log("Buffered Log: " + logEntry);
    }
}




