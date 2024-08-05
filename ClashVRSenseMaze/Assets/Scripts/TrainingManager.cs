using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour
{
    public GameObject maze;
    public GameObject Goal;
    public GameObject player; // the player game object
    public GameObject VisionPanel; // to make no vision
    public GameObject instructionPanel; // Reference to the panel containing instructions

    
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
        ConditionCount = -1;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        Goal.layer = LayerIgnoreRaycast;
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
        }
        else
        {
            Debug.LogError("PlayerCollisions script not found!");
        }

        NextMaze();
    }

    private void OnFinish()
    {
        // Handle the finish event (e.g., start the next maze).
        NextMaze(); 
    }

    void Update()
    {
        // Check if 2 minutes have passed
        if (Time.time - startTime >= timeBetweenMazes)
        {
            //NextMaze();
        }
    }

    public void NextMaze()
    {

        if (ConditionCount == 4)
        {
            SceneManager.LoadScene("MainMenu");
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
        VisionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0.5f;
        Debug.Log("Applying all senses");
    }

    void ApplyVisual()
    {
        VisionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0f;
        Debug.Log("Applying visual only");
    }

    void ApplyAudio()
    {
        VisionPanel.SetActive(true); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0.5f;
        Debug.Log("Applying audio only");
    }

    void ApplyHaptic()
    {
        VisionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0f;
        Debug.Log("Applying haptic only");
    }

    // Method to start the next level after showing instructions
    public void NextLevel()
    {
        instructionPanel.SetActive(true); // Show instructions panel
        // Optionally, you can add additional logic before starting the maze
        StartCoroutine(StartMazeAfterDelay());
    }

    IEnumerator StartMazeAfterDelay()
    {
        // Wait for 3 seconds (or adjust as needed)
        yield return new WaitForSeconds(3f);

        // Hide instruction panel
        instructionPanel.SetActive(false);

        // Start the maze
        NextMaze();
    }

}
