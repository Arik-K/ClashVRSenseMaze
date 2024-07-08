using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour
{
    public GameObject maze;
    MazeManager mm;

    public GameObject Goal;
    public static int ConditionCount = 0;
    private AudioSource audioSourcePlayer; // the audio source component attached to this game object
    GameObject VisionPanel; // to make no vision
    public static string maze_name = "Training";
    public static string[] conditions = new string[] { "all", "visual_only", "audio_only", "Haptic_only" };
    public GameObject player; // the player game object
    // audio clips to play for raycasting sounds
    private static Vector3 initial_position = new Vector3(-2.21f, 0f, 2.28f);
    private float startTime;
    public float timeBetweenMazes = 120f; // 2 minutes in seconds

    public GameObject instructionPanel; // Reference to the panel containing instructions

    // Start is called before the first frame update
    void Start()
    {
        ConditionCount = 0;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        Goal.layer = LayerIgnoreRaycast;
        Debug.Log("Current layer: " + gameObject.layer);

        mm = maze.GetComponent<MazeManager>();
        audioSourcePlayer = player.GetComponent<AudioSource>();
        startTime = Time.time;

        // You may want to initialize VisionPanel if it's not already set in the Inspector
        // VisionPanel = GameObject.FindWithTag("YourVisionPanelTag");

        // Disable instruction panel at start
        instructionPanel.SetActive(false);
    }

    void Update()
    {
        // Check if 2 minutes have passed
        if (Time.time - startTime >= timeBetweenMazes)
        {
            NextMaze();
        }
    }

    public void NextMaze()
    {
        AudioSource NextLevelSound = GameObject.FindWithTag("Ground").GetComponent<AudioSource>();
        Transform targetTransform = player.GetComponent<Transform>();
        CharacterController controller_player = player.GetComponent<CharacterController>();

        if (ConditionCount == 4)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Reset timer
        startTime = Time.time;

        NextLevelSound.Play();

        controller_player.enabled = false;
        targetTransform.position = initial_position;
        controller_player.enabled = true;

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
        VisionPanel.SetActive(true); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0.5f;
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

        // Method to handle when the player reaches the goal
    public void ReachedGoal()
    {
        // Call NextMaze to proceed to the next maze
        NextMaze();
    }
}
