using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


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

    
    public static int ConditionCount = 0;
    public static int[] Paths = new int[] { 0, 1, 2, 3 };
    public static int path;

    private float startTime;
    private float finishTime;
    public float timeBetweenMazes = 120f; // 2 minutes in seconds

    public TextMeshProUGUI textMeshPro;
    
    // maze objects
    public List<GameObject> ChangingGoals = new List<GameObject>();

    
    // audio sources
    private AudioSource audioSourcePlayer;
    private AudioSource audioSourcePlayerLeft;
    private AudioSource audioSourcePlayerRight;


    public static string[] conditions = new string[] { "all", //"visual_only", "audio_only", "haptic_only",
       // "visual_off", "audio_off", "haptic_off",
        //"visual_full_clash", 
       // "audio_full_clash", 
        //"haptic_full_clash", 
        //"invisible"
        };

    List<string> conditionList;
    List<string> visualTags = new List<string> {"Ivisible", "VisualGhost"};
    List<string> audioTags = new List<string> { "Mute", "AudioGhost" };
    List<string> hapticTags = new List<string> { "Intangable", "TangableGhost" };
    
    void Start()
    {
        conditionList = new List<string>(conditions); 
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

        // Disable all
        //instructionPanel.SetActive(false);
        maze.SetActive(false);
        startPoint.SetActive(true);
        foreach( GameObject goal in ChangingGoals)
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

        UpdateTextNextLevelScreen(conditions[ConditionCount]);

    }

        // Method to get a random condition from the list and remove it
    string GetRandomCondition()
    {
        if (conditionList.Count == 0)
        {
            Debug.LogWarning("No more conditions to choose from.");
            return null; // Handle this case as needed
        }

        // Get a random index
        int randomIndex = Random.Range(0, conditionList.Count);

        // Get the condition at the random index
        string chosenCondition = conditionList[randomIndex];

        // Remove the chosen condition from the list
        conditionList.RemoveAt(randomIndex);

        return chosenCondition;
    }


    private void OnFinish()
    {
        if(ConditionCount >= conditions.Length)
        {
             textMeshPro.text = "Thank you for Participating :)";
        }
        
        // Handle the finish event (e.g., start the next maze).
        foreach( GameObject goal in ChangingGoals)
        {
           goal.SetActive(false); 
        }
        maze.SetActive(false);
        visionPanel.SetActive(false);
        startPoint.SetActive(true);
       //instructionPanel.SetActive(true);
        UpdateTextNextLevelScreen(conditions[ConditionCount]);
        
    }

    private void OnStartingPointCollision()
    {
        startPoint.SetActive(false);
        instructionPanel.SetActive(true);
        StartCoroutine(SetNextMaze());

        //ActivateCondition(conditions[ConditionCount]);
    }

    IEnumerator SetNextMaze()
    {
        yield return new WaitForSeconds(4f);
        
        instructionPanel.SetActive(false);
        foreach( GameObject goal in ChangingGoals)
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

        if (ConditionCount == conditions.Length)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        // Activate next condition
        path = mazeManager.SetPath(Paths);
        mazeManager.ActivateCondition(conditions[ConditionCount]);
        // Increment ConditionCount for the next maze
        ConditionCount++;

    }

    public void UpdateTextNextLevelScreen(string condition_name)
    {
      switch (condition_name)
      {
                        // Deafult case all senses
        case "all" when true:
            textMeshPro.text = "Next Maze:\nTrust all senses";
            break;
      
        // Only one sensory channel active
        case "audio_only" when true:
            textMeshPro.text = "Next Maze:\nOnly Audio";
              break;
        case "visual_only" when true:
        textMeshPro.text = "Next Maze:\nOnly Visual";
              break;
        case "haptic_only" when true:
            textMeshPro.text = "Next Maze:\nOnly Haptic";
              break;
        
        // only Two sensory channels - One off
        case "audio_off" when true:
            textMeshPro.text = "Next Maze:\nTrust Visual and Haptic";
            break;
        case "visual_off" when true:
            textMeshPro.text = "Next Maze:\nTrust Audio and Haptic";
            break;
        case "Haptic_off" when true:
            textMeshPro.text = "Next Maze:\nTrust Visual and Audio";
            break;

        // Full clash with one sesnory channel
        case "audio_full_clash" when true:
            textMeshPro.text = "Next Maze:\nTrust Visual and Haptic, Audio Could Be Misleading";
            break;
        case "visual_full_clash" when true:
            textMeshPro.text = "Next Maze:\nTrust Audio and Haptic, Visual Could Be Misleading";
            break;
        case "Haptic_full_clash" when true:
            textMeshPro.text = "Next Maze:\nTrust Visual and Audio, Haptic Could Be Misleading";
            break;

        //Special wall case
        case "invisible" when true:
            textMeshPro.text = "Next Maze:\nTrust Audio and Haptic, Visual Could Be Misleading";
            break;

      } 
    
    }
    
}
