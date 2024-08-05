using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour
{
    public GameObject maze;
    public GameObject goal;
    public GameObject player; // the player game object
    public GameObject visionPanel; // to make no vision
    public GameObject instructionPanel; // Reference to the panel containing instructions
    public GameObject startPoint;

    
    public static int ConditionCount = 0;
    private AudioSource audioSourcePlayer; // the audio source component attached to this game object

    public static string maze_name = "Training";
    public static string[] conditions = new string[] { "all", "visual_only", "audio_only", "Haptic_only" };
  
    // audio clips to play for raycasting sounds
    private static Vector3 initial_position = new Vector3(-2.21f, 0f, 2.28f);
    private float startTime;
    public float timeBetweenMazes = 120f; // 2 minutes in seconds

    
    // Start is called before the first frame update
    void Start()
    {
        ConditionCount = 0;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        goal.layer = LayerIgnoreRaycast;
        Debug.Log("Current layer: " + gameObject.layer);
        audioSourcePlayer = player.GetComponent<AudioSource>();
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
        goal.SetActive(false);
        maze.SetActive(false);
        startPoint.SetActive(true);
        visionPanel.SetActive(false);
        //instructionPanel.SetActive(true);
        
    }

    private void OnStartingPointCollision()
    {
        startPoint.SetActive(false);
        goal.SetActive(true);
        maze.SetActive(true);

        NextMaze();
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

        if (ConditionCount == 4)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        // Activate next condition
        ActivateCondition(conditions[ConditionCount]);
    }

    public void ActivateCondition(string condition)
    {
        switch (condition)
        {
            case "all":
                ApplyVisualAudioHaptic();
                break;
            case "audio_only":
                ApplyAudio();
                break;
            case "visual_only":
                ApplyVisual();
                break;
            case "Haptic_only":
                ApplyHaptic();
                break;
        }

        // Increment ConditionCount for the next maze
        ConditionCount++;
    }

    void ApplyVisualAudioHaptic()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0.5f;
        Debug.Log("Applying all senses");
    }

    void ApplyVisual()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0f;
        
        Debug.Log("Applying visual only");
    }

    void ApplyAudio()
    {
        visionPanel.SetActive(true); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0.5f;
        Debug.Log("Applying audio only");
    }

    void ApplyHaptic()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0f;
        Debug.Log("Applying haptic only");
    }

}
