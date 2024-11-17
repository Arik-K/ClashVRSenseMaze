using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DemoManager : MonoBehaviour
{
   
   
    public GameObject maze;
    public GameObject goal;

    public GameObject FalseGoal;
    public GameObject invisibleWall;
    public GameObject GhostWall;
    public List<GameObject> InsideWalls = new List<GameObject>();
    public GameObject player; // the player game object
    public GameObject visionPanel; // to make no vision
    public GameObject instructionPanel; // Reference to the panel containing instructions
    public TextMeshProUGUI textMeshPro;
    public GameObject startPoint;

    public GameObject Left;
    public GameObject Right;

    
    public static int ConditionCount = 0;
    private AudioSource audioSourcePlayer; // the audio source component attached to this game object
    private AudioSource audioSourcePlayerLeft;
    private AudioSource audioSourcePlayerRight;

    public static string maze_name = "Training";
    public static string[] conditions = new string[] { "all", "visual_off", "visual_full_clash" };
  
    private float startTime;
    public float timeBetweenMazes = 120f; // 2 minutes in seconds

    
    // Start is called before the first frame update
    void Start()
    {
        ConditionCount = 0;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        goal.layer = LayerIgnoreRaycast;
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
        maze.SetActive(false);
        startPoint.SetActive(true);
        FalseGoal.SetActive(false);


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
        goal.SetActive(false);
        maze.SetActive(false);
        startPoint.SetActive(true);
        visionPanel.SetActive(false);

        UpdateTextNextLevelScreen(conditions[ConditionCount]);
        
    }

    private void OnStartingPointCollision()
    {
        startPoint.SetActive(false);
        instructionPanel.SetActive(true);
        StartCoroutine(SetNext());
        

        //NextMaze();
        //ActivateCondition(conditions[ConditionCount]);
    }

   /* void Update()
    {
        // Check if 2 minutes have passed
        if (Time.time - startTime >= timeBetweenMazes)
        {
            NextMaze();
        }
    }*/

    public void NextMaze()
    {

        if (ConditionCount == 3)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        // Activate next condition
        ActivateCondition(conditions[ConditionCount]);
    }

    IEnumerator SetNext()
    {
        yield return new WaitForSeconds(5f);
        instructionPanel.SetActive(false);
        goal.SetActive(true);
        maze.SetActive(true);
        NextMaze();
    }

    public void ActivateCondition(string condition)
    {
        switch (condition)
        {
            case "all":
                ApplyVisualAudioHaptic();
                break;
            case "visual_off":
                ApplyVisualOff();
                break;
            case "visual_full_clash":
                ApplyVisualClash();
                break;
        }

        // Increment ConditionCount for the next maze
        ConditionCount++;
    }

    void ApplyVisualAudioHaptic()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        
        audioSourcePlayer.volume = 0.75f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
        
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }

        invisibleWall.SetActive(true);
        GhostWall.SetActive(false);
        invisibleWall.tag = "Wall";
        foreach(GameObject InsideWall in InsideWalls)
        {
            InsideWall.tag = "Wall";   
        }

        Debug.Log("Applying all senses");
    }

    void ApplyVisualOff()
    {
        visionPanel.SetActive(true); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0.75f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        Debug.Log("Applying visual off");

        invisibleWall.SetActive(true);
        GhostWall.SetActive(false);
        invisibleWall.tag = "Invisible";
        foreach(GameObject InsideWall in InsideWalls)
        {
            InsideWall.tag = "Invisible";   
        }
    }

    void ApplyVisualClash()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        
        audioSourcePlayer.volume = 0.75f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
        
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        Debug.Log("Applying visual clash");

        invisibleWall.GetComponent<MeshRenderer> ().enabled = false;
        invisibleWall.SetActive(true);
        GhostWall.SetActive(true);
        FalseGoal.SetActive(true);

        invisibleWall.tag = "Invisible";
        foreach(GameObject InsideWall in InsideWalls)
        {
            InsideWall.tag = "Wall";   
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
            
            case "visual_off":
                textMeshPro.text = "Next Maze:\nTrust Audio and Haptic";
                break;

            case "visual_full_clash":
                textMeshPro.text = "Next Maze:\nTrust Audio and Haptic, Visual Could Be Misleading";
                break;

            default:
                textMeshPro.text = "Next Maze:\nUnknown condition";
                break;
        }
    }

}
