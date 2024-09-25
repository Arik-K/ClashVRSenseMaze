using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public List<(int, string)> ShuffledCombinations { get; private set; } = new List<(int, string)>();
    MazeManager mazeManager;
    WallTouch touch;
    public GameObject player; // the player game object
    public GameObject visionPanel; // to make no vision
    public GameObject instructionPanel; // Reference to the panel containing instructions
    public GameObject startPoint;
    public GameObject maze;
    public GameObject Ground;

    public GameObject Left;
    public GameObject Right;

    public int ConditionCount = 0;
    public static int[] Paths = new int[] { 0, 1, 2, 3 };
    public static int path;

    private float startTime;
    private float finishTime;
    public float timeBetweenMazes = 180f; // 3 minutes in seconds

    public TextMeshProUGUI textMeshPro;
    
    // maze objects
    public List<GameObject> ChangingGoals = new List<GameObject>();

    // audio sources
    private AudioSource audioSourcePlayer;
    private AudioSource audioSourcePlayerLeft;
    private AudioSource audioSourcePlayerRight;
    private AudioSource NextLevelCall;

    public string[] conditions = new string[] { 
        "all", "visual_only", "audio_only", "haptic_only",
        "visual_off", "audio_off", "haptic_off",
        "visual_full_clash", "audio_full_clash", "haptic_full_clash",
        "invisible"
    };

    List<string> visualTags = new List<string> {"Ivisible", "VisualGhost"};
    List<string> audioTags = new List<string> { "Mute", "AudioGhost" };
    List<string> hapticTags = new List<string> { "Intangable", "TangableGhost" };

    private List<(int, string)> shuffledCombinations;

    void Start()
    {
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
        NextLevelCall = Ground.GetComponent<AudioSource>();

        startTime = Time.time;

        // Disable all
        maze.SetActive(false);
        startPoint.SetActive(true);
        foreach(GameObject goal in ChangingGoals)
        {
           goal.SetActive(false); 
        }

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

        GenerateShuffledCombinations();
        ConditionCount = 0;
        if (ShuffledCombinations.Count > 0)
        {
            UpdateTextNextLevelScreen(ShuffledCombinations[ConditionCount].Item2);
        }
        else{
            Debug.LogError("ShuffledCombinations is empty!");
            }

    }
    

    public void GenerateShuffledCombinations()
    {
        var combinations = new List<(int, string)>();
        foreach (int path in Paths)
        {
            foreach (string condition in conditions)
            {
                combinations.Add((path, condition));
            }
        }

        // Log the original list
        Debug.Log("Original Combinations:");
        for (int i = 0; i < combinations.Count; i++)
        {
            Debug.Log($"{i + 1}: Path {combinations[i].Item1}, Condition: {combinations[i].Item2}");
        }
        Debug.Log($"Total combinations: {combinations.Count}");

        // Shuffle the combinations
        System.Random rng = new System.Random();
        ShuffledCombinations = combinations.OrderBy(x => rng.Next()).ToList();

        // Log the shuffled list
        Debug.Log("\nShuffled Combinations:");
        for (int i = 0; i < ShuffledCombinations.Count; i++)
        {
            Debug.Log($"{i + 1}: Path {ShuffledCombinations[i].Item1}, Condition: {ShuffledCombinations[i].Item2}");
        }
        Debug.Log($"Total combinations after shuffling: {ShuffledCombinations.Count}");
    }
    private void OnFinish()
    {
        // ###BUG FIX### - reset audio and haptic so it won't keep going after finishing the level
        mazeManager.PlayerModalityAndLimits(false, true, 0.0f, 0.0f, false);

        if(ConditionCount >= ShuffledCombinations.Count)
        {
            textMeshPro.text = "Thank you for Participating :)";
            SceneManager.LoadScene("MainMenuScene");
            return;
        }
        
        // Handle the finish event (e.g., start the next maze).
        foreach(GameObject goal in ChangingGoals)
        {
           goal.SetActive(false); 
        }
        maze.SetActive(false);
        visionPanel.SetActive(false);
        startPoint.SetActive(true);
        UpdateTextNextLevelScreen(ShuffledCombinations[ConditionCount].Item2);
    }

    private void OnStartingPointCollision()
    {
        startPoint.SetActive(false);
        instructionPanel.SetActive(true);
        StartCoroutine(SetNextMaze());
    }

    IEnumerator SetNextMaze()
    {
        yield return new WaitForSeconds(5f);
        
        instructionPanel.SetActive(false);
        foreach(GameObject goal in ChangingGoals)
        {
           goal.SetActive(true); 
        }
        maze.SetActive(true);

        NextMaze();
    }

    void NextMaze()
    {
        if (mazeManager == null)
        {
            Debug.LogError("mazeManager is not assigned.");
            return;
        }        

        if (ConditionCount < ShuffledCombinations.Count)
        {
            // Get the next combination
            var combination = ShuffledCombinations[ConditionCount];
            
            // Activate next condition
            path = mazeManager.SetPath(new int[] { combination.Item1 });
            mazeManager.ActivateCondition(combination.Item2);

            // Increment ConditionCount for the next maze
            ConditionCount++;
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void UpdateTextNextLevelScreen(string condition_name)
    {
        switch (condition_name)
        {
            // Default case all senses
            case "all":
                textMeshPro.text = "Next Maze:\nTrust all senses";
                break;
          
            // Only one sensory channel active
            case "audio_only":
                textMeshPro.text = "Next Maze:\nOnly Audio";
                break;
            case "visual_only":
                textMeshPro.text = "Next Maze:\nOnly Visual";
                break;
            case "haptic_only":
                textMeshPro.text = "Next Maze:\nOnly Haptic";
                break;
            
            // only Two sensory channels - One off
            case "audio_off":
                textMeshPro.text = "Next Maze:\nTrust Visual and Haptic";
                break;
            case "visual_off":
                textMeshPro.text = "Next Maze:\nTrust Audio and Haptic";
                break;
            case "haptic_off":
                textMeshPro.text = "Next Maze:\nTrust Visual and Audio";
                break;

            // Full clash with one sensory channel
            case "audio_full_clash":
                textMeshPro.text = "Next Maze:\nTrust Visual and Haptic, Audio Could Be Misleading";
                break;
            case "visual_full_clash":
                textMeshPro.text = "Next Maze:\nTrust Audio and Haptic, Visual Could Be Misleading";
                break;
            case "haptic_full_clash":
                textMeshPro.text = "Next Maze:\nTrust Visual and Audio, Haptic Could Be Misleading";
                break;

            //Special wall case
            case "invisible":
                textMeshPro.text = "Next Maze:\nTrust Audio and Haptic, Visual Could Be Misleading";
                break;

            default:
                textMeshPro.text = "Next Maze:\nUnknown condition";
                break;
        }
    }
}