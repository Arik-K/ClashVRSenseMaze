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
    
    // maze objects
    public Dictionary<GameObject, GameObject> EndPath = new Dictionary<GameObject, GameObject>();
    public List<GameObject> ChangingWalls = new List<GameObject>();
    public List<GameObject> ChangingGoals = new List<GameObject>();
     public List<GameObject> InsideWalls = new List<GameObject>();
    
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
  
    
    // Start is called before the first frame update
    void Start()
    {
        if (ChangingWalls == null)
        {
            Debug.LogError("No parent object assigned for ChaningWalls.");
            return;
        }
        // Check if the ChanginningGoals has been assigned
        if (ChangingGoals == null)
        {
            Debug.LogError("No parent object assigned for ChaningGoals.");
            return;
        }
        
        for (int i = 0; i < ChangingGoals.Count; i++)
        {
            EndPath.Add(ChangingGoals[i], ChangingWalls[i]);
        }
        

        /*// Check if the InsideWalls has been assigned
        if (EndPath == null)
        {
            InsideWalls.Add(GameObject.Find("Wall1"));
            InsideWalls.Add(GameObject.Find("Wall2"));
            InsideWalls.Add(GameObject.Find("Goal1"));
            InsideWalls.Add(GameObject.Find("Goal2"));
        }

        foreach(GameObject wall in InsideWalls)
        {
            DefaultWall(wall);
        }*/

    }


    //Define wall types for changing walls
    void DefaultWall(GameObject wall)
    {
        wall.tag = "Wall";
        wall.layer = LayerMask.NameToLayer("Default");
        wall.GetComponent<MeshRenderer>().enabled = true;
    }
    
    
    //Ghost Walls
    void VisualGhostWall(GameObject wall)
    {
        wall.tag = "VisualGhost";
        wall.layer = LayerMask.NameToLayer("Ignore Raycast");
        wall.GetComponent<MeshRenderer>().enabled = true;
    }

    void AudioGhostWall(GameObject wall)
    {
        wall.tag = "AudioGhost";
        wall.layer = LayerMask.NameToLayer("Default");
        wall.GetComponent<MeshRenderer>().enabled = false;
    }

    void HapticGhostWall(GameObject wall)
    {
        wall.tag = "HapticGhost";
        wall.layer = LayerMask.NameToLayer("Ignore Raycast");
        wall.GetComponent<MeshRenderer>().enabled = false;
    }

    // other wall types

    void InvisibleWall(GameObject wall)
    {
        wall.tag = "Invisble";
        wall.layer = LayerMask.NameToLayer("Default");
        wall.GetComponent<MeshRenderer>().enabled = false;
    }

    void MuteWall(GameObject wall)
    {
        wall.tag = "Mute";
        wall.layer = LayerMask.NameToLayer("Ignore Raycast");
        wall.GetComponent<MeshRenderer>().enabled = true;
    }

    void Intangable(GameObject wall)
    {
        wall.tag = "Intangable";
        wall.layer = LayerMask.NameToLayer("Default");
        wall.GetComponent<MeshRenderer>().enabled = true;
    }


    
    // Maze Mechanism
    // Set walls and goals accordingly

    public void SetPath(int path)
    {
       foreach(GameObject Goal in ChangingGoals)
       {
        Goal.tag = "FalseGoal";
       }
       ChangingGoals[path].tag = "Goal";

       foreach (GameObject wall in ChangingWalls)
       {
        wall.SetActive(true);
       }
       ChangingWalls[path].SetActive(false);
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
                break;*/   
        }

        // Increment ConditionCount for the next maze
        ConditionCount++;
    }


    // Conditions Implemintation
    void ApplyVisualAudioHaptic()
    {
        // Define player senses
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


}






