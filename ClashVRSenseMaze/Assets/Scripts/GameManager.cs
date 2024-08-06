using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] mazes;
    public GameObject[] startingPoints;
    public GameObject[] goals;
    public GameObject player;
    public GameObject visionPanel;
    public GameObject instructionPanel;

    public static int mazeCount = 0;
    private AudioSource audioSourcePlayer;
    public static string maze_name = "Game";

    private float startTime;
    public float timeBetweenMazes = 120f;
    private WallTouch wallTouchScript;

    void Start()
    {
        mazeCount = 0;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        foreach (GameObject goal in goals)
        {
            goal.layer = LayerIgnoreRaycast;
        }

        // Find and cache a reference to the WallTouch script
        wallTouchScript = FindObjectOfType<WallTouch>();

        // Initial setup
        audioSourcePlayer = player.GetComponent<AudioSource>();
        startTime = Time.time;
        instructionPanel.SetActive(false);

        // Event subscription
        PlayerCollsions playerCollisions = FindObjectOfType<PlayerCollsions>();
        if (playerCollisions != null)
        {
            playerCollisions.onFinish.AddListener(OnFinish);
            playerCollisions.onStartingPointCollision.AddListener(OnStartingPointCollision);
        }
        else
        {
            Debug.LogError("PlayerCollisions script not found!");
        }

        ActivateMaze(mazeCount); 
    }

    private void OnFinish()
    {
        goals[mazeCount].SetActive(false); 
        startingPoints[mazeCount].SetActive(true); 
        visionPanel.SetActive(false);
    }

    private void OnStartingPointCollision()
    {
        startingPoints[mazeCount].SetActive(false); 
        goals[mazeCount].SetActive(true);
        
        // Enable WallTouch after entering a starting point
        if (wallTouchScript != null)
        {
            wallTouchScript.isWallTouchEnabled = true;
        }
        NextMaze(); 
    }

    public void NextMaze()
    {
        if (mazeCount == mazes.Length) 
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        ActivateMaze(mazeCount); 
    }

    public void ActivateMaze(int mazeIndex)
    {
        for (int i = 0; i < mazes.Length; i++)
        {
            mazes[i].SetActive(i == mazeIndex);
            startingPoints[i].SetActive(i == mazeIndex);
            goals[i].SetActive(i == mazeIndex);
        }

        if (mazeIndex >= 0 && mazeIndex < mazes.Length && mazeIndex < startingPoints.Length && mazeIndex < goals.Length)
        {
            mazeCount++;
            player.transform.position = startingPoints[mazeIndex].transform.position;
        }
        else
        {
            Debug.LogError("Invalid maze, starting point, or goal index: " + mazeIndex);
        }
    }





    /*public void UpdateTextNextLevelScreen(string condition_name)
    {
      switch (condition_name)
      {
                        // Deafult case all senses
        case "all" when true:
            textMeshPro.text = "Next Maze: Trust all senses";
            break;
      
        // Only one sensory channel active
        case "audio_only" when true:
            textMeshPro.text = "Next Maze: Only Audio";
              break;
        case "visual_only" when true:
        textMeshPro.text = "Next Maze: Only Visual";
              break;
        case "Haptic_only" when true:
            textMeshPro.text = "Next Maze: Only Haptic";
              break;
        
        // only Two sensory channels - One off
        case "audio_off" when true:
            textMeshPro.text = "Next Maze: Trust Visual and Haptic";
            break;
        case "visual_off" when true:
            textMeshPro.text = "Next Maze: Trust Audio and Haptic";
            break;
        case "Haptic_off" when true:
            textMeshPro.text = "Next Maze: Trust Visual and Audio";
            break;

        // Full clash with one sesnory channel
        case "audio_full_clash" when true:
            textMeshPro.text = "Next Maze: Trust Visual and Haptic, Audio Could Be Misleading";
            break;
        case "visual_full_clash" when true:
            textMeshPro.text = "Next Maze: Trust Audio and Haptic, Visual Could Be Misleading";
            break;
        case "Haptic_full_clash" when true:
            textMeshPro.text = "Next Maze: Trust Visual and Audio, Haptic Could Be Misleading";
            break;

        // Special wall case
        case "invisible" when true:
            textMeshPro.text = "Trust Audio and Haptic, Visual Could Be Misleading";
            break;  

      } 
    
    }*/
    
}
