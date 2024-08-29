using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    MazeManager mazeManager;
    public GameObject player; // the player game object
    public GameObject visionPanel; // to make no vision
    public GameObject instructionPanel; // Reference to the panel containing instructions
    public GameObject startPoint;
    public GameObject maze;

    public GameObject Left;
    public GameObject Right;

    
    public static int ConditionCount = 1;
    public static int[] Paths = new int[] { 0, 1, 2, 3 };

    private float startTime;
    public float timeBetweenMazes = 120f; // 2 minutes in seconds
    public TextMeshProUGUI textMeshPro;
    
    // maze objects
    public List<GameObject> ChangingGoals = new List<GameObject>();

    
    // audio sources
    private AudioSource audioSourcePlayer;
    private AudioSource audioSourcePlayerLeft;
    private AudioSource audioSourcePlayerRight;


    public static string[] conditions = new string[] { "all", "visual_only", "audio_only", "haptic_only",
        "visual_off", "audio_off", "haptic_off",
        "visual_full_clash", "audio_full_clash", "haptic_full_clash"};
    List<string> visualTags = new List<string> { "Ivisible", "VisualGhost" };
    List<string> audioTags = new List<string> { "Mute", "AudioGhost" };
    List<string> hapticTags = new List<string> { "Intangable", "TangableGhost" };
    
    void Start()
    {
        ConditionCount = 0;
        mazeManager = maze.GetComponent<MazeManager>();
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        Debug.Log("Current layer: " + gameObject.layer);
        
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = false;
        }

        audioSourcePlayer = player.GetComponent<AudioSource>();
        audioSourcePlayerLeft = Left.GetComponent<AudioSource>();
        audioSourcePlayerRight = Right.GetComponent<AudioSource>();
        startTime = Time.time;

        // Disable instruction panel at start
        instructionPanel.SetActive(false);

        // Find the PlayerCollisions script and subscribe to the onFinish event.
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
    }

    private void OnFinish()
    {
        // Handle the finish event (e.g., start the next maze).
        foreach( GameObject goal in ChangingGoals)
        {
           goal.SetActive(false); 
        }
        maze.SetActive(false);
        visionPanel.SetActive(false);
        startPoint.SetActive(true);
        instructionPanel.SetActive(true);
        UpdateTextNextLevelScreen(conditions[ConditionCount]);
        
    }

    private void OnStartingPointCollision()
    {
        startPoint.SetActive(false);
        instructionPanel.SetActive(false);
        foreach( GameObject goal in ChangingGoals)
        {
           goal.SetActive(true); 
        }
        maze.SetActive(true);

        NextMaze();
        //ActivateCondition(conditions[ConditionCount]);
    }


    void NextMaze()
    {
        if (mazeManager == null)
        {
            Debug.LogError("mazeManager is not assigned.");
            return;
        }        

        if (ConditionCount == conditions.Length)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        // Activate next condition
        mazeManager.ActivateCondition(conditions[ConditionCount]);
        int activePath = mazeManager.SetPath(Paths);
        mazeManager.SetSelectedPath(activePath);
    // Increment ConditionCount for the next maze
        ConditionCount++;

    }




    public void UpdateTextNextLevelScreen(string condition_name)
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
        case "haptic_only" when true:
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

        //Special wall case
        case "invisible" when true:
            textMeshPro.text = "Trust Audio and Haptic, Visual Could Be Misleading";
            break;

      } 
    
    }
    
}
