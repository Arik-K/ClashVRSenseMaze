using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class MazeManager : MonoBehaviour
{
    // The array of materials you want to use.
    public Material[] materials; 

    //gameObjects
    public GameObject visionPanel;// for visual off
    public GameObject player;
    
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
    
    private int ConditionCount;
    private int currentTextureIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*public void ActivateMaze(string curr_maze)
    {
        print("Maze - " + curr_maze);
        // Enable only current maze walls
        foreach (Transform maze in transform)
        {
            if (maze.name == curr_maze)
            {
                maze.gameObject.SetActive(true);
            }
            else
            {
                maze.gameObject.SetActive(false);
            }
        }
    }*/

    
    public void NextMaze()
    {

        if (ConditionCount == conditions.Length)
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
            // Deafult case all senses
            case "all" when true:
                ApplyVisualAudioHaptic();
                break;
            
            // Only one sensory channel active
            case "audio_only" when true:
                ApplyAudio();
                break;
            case "visual_only" when true:
                ApplyVisual();
                break;
            case "haptic_only" when true:
                ApplyHaptic();
                break;
            
            // only Two sensory channels - One off
            case "audio_off" when true:
                ApplyAudioOff();
                break;
            case "visual_off" when true:
                ApplyVisualOff();
                break;
            case "haptic_off" when true:
                ApplyHapticOff();
                break;

            // Full clash with one sesnory channel
            case "audio_full_clash" when true:
                ApplyAudioFullClash();
                break;
            case "visual_full_clash" when true:
                ApplyVisualFullClash();
                break;
            case "Haptic_full_clash" when true:
                ApplyHapticlFullClash();
                break;

            // Special wall case
            /*case "invisible" when true:
                ApplyInvisible();
                break;   */
        }

        // Increment ConditionCount for the next maze
        ConditionCount++;
    }

    void ApplyVisualAudioHaptic()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        
        audioSourcePlayer.volume = 0.25f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
        
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        Debug.Log("Applying all senses");
    }

    void ApplyVisual()
    {
        visionPanel.SetActive(false); // Assuming VisionPanel is properly initialized in Start()
        audioSourcePlayer.volume = 0f;
        audioSourcePlayerLeft.volume = 0f;
        audioSourcePlayerRight.volume = 0f;

        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = false;
        }
        Debug.Log("Applying visual only");
    }

    void ApplyAudio()
    {
        visionPanel.SetActive(true); // Assuming VisionPanel is properly initialized in Start()
        
        audioSourcePlayer.volume = 0.25f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
        
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = false;
        }
        Debug.Log("Applying audio only");
    }

    void ApplyHaptic()
    {
        visionPanel.SetActive(true); // Assuming VisionPanel is properly initialized in Start()
        
        audioSourcePlayer.volume = 0f;
        audioSourcePlayerLeft.volume = 0f;
        audioSourcePlayerRight.volume = 0f;
       
       WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        Debug.Log("Applying haptic only");
    }

    void ApplyAudioOff()
    {
        // Visual Conditions
        visionPanel.SetActive(false);

        // Audio Conditions
        audioSourcePlayer.volume = 0f;
        audioSourcePlayerLeft.volume = 0f;
        audioSourcePlayerRight.volume = 0f;
       
       // Haptic Consditions
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        Debug.Log("Applying Audio off");

    }
    
    void ApplyVisualOff()
    {
        // Visual Conditions
        visionPanel.SetActive(true);

        // Audio Conditions
        audioSourcePlayer.volume = 0.25f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
       
       // Haptic Consditions
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        Debug.Log("Applying viusal off");

    }

    void ApplyHapticOff()
    {
        // Visual Conditions
        visionPanel.SetActive(false);

        // Audio Conditions
        audioSourcePlayer.volume = 0.25f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
       
       // Haptic Consditions
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = false;
        }
        Debug.Log("Applying haptic only");

    }

    

    void ApplyVisualFullClash()
    {
        ApplyVisualAudioHaptic();




        
        Debug.Log("Applying Full Visual Clash");

    }

        void ApplyAudioFullClash()
    {
        // Visual Conditions
        visionPanel.SetActive(false);

        // Audio Conditions
        audioSourcePlayer.volume = 0.25f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
       
       // Haptic Conditions
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        
        Debug.Log("Applying Full Visual Clash");

    }
    void ApplyHapticlFullClash()
    {
        // Visual Conditions
        visionPanel.SetActive(false);

        // Audio Conditions
        audioSourcePlayer.volume = 0.25f;
        audioSourcePlayerLeft.volume = 0.5f;
        audioSourcePlayerRight.volume = 0.5f;
       
       // Haptic Conditions
        WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
        foreach (WallTouch wallTouch in wallTouches)
        {
            wallTouch.isWallTouchEnabled = true;
        }
        
        Debug.Log("Applying Full Haptic Clash");
    }

    

    
    

    /*public void WallMaterialChange()
    {

    }

    //Define every Maze Condition
    void ApplyVisualAudioHaptic(string maze_name)
    {
        AudioSource audioSourcePlayer = player.GetComponent<AudioSource>();   
        
        Transform maze_transform = GameObject.Find(maze_name).transform;

        // Turn off all visual walls
        DeactivateWalls(maze_transform, "GhostWall");
        DeactivateWalls(maze_transform, "InvisiWall");

        // Turn off all audio walls
        DeactivateWalls(maze_transform, "GhostSoundWall");
        DeactivateWalls(maze_transform, "MuteWall");

        // Turn on all morph walls
        ActivateWalls(maze_transform, "MorphWall");

        // Make sure Obstruction of view is off
        ObscureObj.SetActive(false);

        // Make sure listener is ON
        audioSource3.volume = 0.5f;

        print("all");
    }
    */



}
