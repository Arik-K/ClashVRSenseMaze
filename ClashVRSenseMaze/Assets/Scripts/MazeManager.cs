using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class MazeManager : MonoBehaviour
{

    // The array of materials you want to use.
    //public Material[] materials; 

    //gameObjects for player
    public GameObject visionPanel;// for visual off
    public GameObject OverLay; //A no peek Gaurdian
    public GameObject player;
    public GameObject Left;
    public GameObject Right;
    public GameObject gameManager;
    
    // maze objects
    //public Dictionary<GameObject, GameObject> EndPath = new Dictionary<GameObject, GameObject>();
    public List<GameObject> ChangingWalls = new List<GameObject>();
    public List<GameObject> ChangingGoals = new List<GameObject>();
    public List<GameObject> InsideWalls = new List<GameObject>();
    
    // Audio Sources
    private AudioSource audioSourcePlayer;
    private AudioSource audioSourcePlayerLeft;
    private AudioSource audioSourcePlayerRight;
    private AudioSource audioSourcePlayerCollision;

     

    /*public static string[] conditions = new string[] { "all", "visual_only", "audio_only", "haptic_only",
        "visual_off", "audio_off", "haptic_off",
        "visual_full_clash", "audio_full_clash", "haptic_full_clash", "Invisible"};
     */   
    // Start is called before the first frame update
    void Awake()
    {
        int[] Paths = new int[] { 0, 1, 2, 3 };
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
            //EndPath.Add(ChangingGoals[i], ChangingWalls[i]);
        }

        audioSourcePlayer = player.GetComponent<AudioSource>();
        audioSourcePlayerLeft = Left.GetComponent<AudioSource>();
        audioSourcePlayerRight = Right.GetComponent<AudioSource>();
        audioSourcePlayerCollision = gameManager.GetComponent<AudioSource>();

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
        //wall.layer = LayerMask.NameToLayer("Ignore Raycast");
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
        //wall.layer = LayerMask.NameToLayer("Ignore Raycast");
        wall.GetComponent<MeshRenderer>().enabled = false;
    }

    // other wall types
    void InvisibleWall(GameObject wall)
    {
        wall.tag = "Invisible";
        wall.layer = LayerMask.NameToLayer("Default");
        wall.GetComponent<MeshRenderer>().enabled = false;
    }

    void MuteWall(GameObject wall)
    {
        wall.tag = "Mute";
        //wall.layer = LayerMask.NameToLayer("Ignore Raycast");
        wall.GetComponent<MeshRenderer>().enabled = true;
    }

    void Intangable(GameObject wall)
    {
        wall.tag = "Intangable";
        wall.layer = LayerMask.NameToLayer("Default");
        wall.GetComponent<MeshRenderer>().enabled = true;
    }


    
    // Maze Mechanism
    // Set walls and goals 
    public int SetPath(int[] paths)
    {
        // Check if paths list is not empty
        if (paths == null || paths.Length == 0)
        {
            Debug.LogError("Paths list is empty or null!");
            return -1;
        }

        // Create a Random object to pick a random index
        System.Random random = new System.Random();
        int randomIndex = random.Next(paths.Length);  // Get a random index
        int path = paths[randomIndex];
        

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

       return path;

    }
    public void setWalls(string wallType)
    {
        
        switch (wallType){
            case "Default" when true:

                foreach(GameObject InsideWall in InsideWalls)
                {
                    DefaultWall(InsideWall);     
                }
                foreach(GameObject ChangingWall in ChangingWalls)
                {
                    DefaultWall(ChangingWall);     
                }
            break;

            case "Vision" when true:

                foreach(GameObject InsideWall in InsideWalls)
                {
                    DefaultWall(InsideWall);     
                }
                
                foreach(GameObject ChangingWall in ChangingWalls)
                {
                    InvisibleWall(ChangingWall);     
                }
                ChangingWalls[GameManager.path].SetActive(true);
                VisualGhostWall(ChangingWalls[GameManager.path]);
            break;
            
            case "Audio" when true:

                foreach(GameObject InsideWall in InsideWalls)
                {
                    DefaultWall(InsideWall);     
                }

                ChangingWalls[GameManager.path].SetActive(true);
                foreach(GameObject ChangingWall in ChangingWalls)
                {
                    MuteWall(ChangingWall);
                }

                AudioGhostWall(ChangingWalls[GameManager.path]);
            break;            
            
            case "Haptic" when true:

                foreach(GameObject InsideWall in InsideWalls)
                {
                    DefaultWall(InsideWall);     
                }
                
                ChangingWalls[GameManager.path].SetActive(true);
                foreach(GameObject ChangingWall in ChangingWalls)
                {
                    Intangable(ChangingWall);     
                }

                HapticGhostWall(ChangingWalls[GameManager.path]);
            break;

            case "Invisible" when true:
                
                foreach(GameObject InsideWall in InsideWalls)
                {
                    InvisibleWall(InsideWall);     
                }
                foreach(GameObject ChangingWall in ChangingWalls)
                {
                    InvisibleWall(ChangingWall);     
                }
            break;
        }

    }

        public void PlayerModalityAndLimits(bool EnableVision,bool EnableOverLay,float PlayerSound, float HandsSound, bool touch)
        {
            visionPanel.SetActive(EnableVision);
            OverLay.SetActive(EnableOverLay);

            audioSourcePlayer.volume = PlayerSound;
            audioSourcePlayerCollision.volume = PlayerSound;
            audioSourcePlayerLeft.volume = HandsSound;
            audioSourcePlayerRight.volume = HandsSound;

            WallTouch[] wallTouches = FindObjectsOfType<WallTouch>(); 
            foreach (WallTouch wallTouch in wallTouches)
            {
                wallTouch.isWallTouchEnabled = touch;
            }
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
            case "haptic_full_clash" when true:
                ApplyHapticlFullClash();
                break;

            // Special wall case
            case "invisible" when true:
                ApplyInvisible();
                break;  
        }



    }


    // Conditions Implemintation
    void ApplyVisualAudioHaptic()
    {
        PlayerModalityAndLimits(false, true, 0.75f, 0.5f, true);
        setWalls("Default");
        Debug.Log("Applying all senses");
    }
    void ApplyVisual()
    {
        PlayerModalityAndLimits(false, true, 0f, 0f, false);
        setWalls("Default");
        Debug.Log("Applying visual only");
    }

    void ApplyAudio()
    {
        PlayerModalityAndLimits(true, false, 0.75f, 0.5f, false);
        setWalls("Default");
        Debug.Log("Applying audio only");
    }

    void ApplyHaptic()
    {
        PlayerModalityAndLimits(true, true, 0f, 0f, true);
        setWalls("Default");
        Debug.Log("Applying haptic only");
    }

    void ApplyAudioOff()
    {
        PlayerModalityAndLimits(false, true, 0f, 0f, true);
        setWalls("Default");
        Debug.Log("Applying Audio off");

    }
    
    void ApplyVisualOff()
    {
        PlayerModalityAndLimits(true, true, 0.75f, 0.5f, true);
        setWalls("Default");
        Debug.Log("Applying viusal off");

    }

    void ApplyHapticOff()
    {
        PlayerModalityAndLimits(false, true, 0.75f, 0.5f, false);
        setWalls("Default");        
        Debug.Log("Applying haptic off");

    }

    void ApplyVisualFullClash()
    {
        PlayerModalityAndLimits(false, true, 0.75f, 0.5f, true);
        setWalls("Vision");
        Debug.Log("Applying Full Visual Clash");

    }
    void ApplyAudioFullClash()
    {
        PlayerModalityAndLimits(false, true, 0.75f, 0.5f, true);
        setWalls("Audio");
        Debug.Log("Applying Full Audio Clash");
    }
    void ApplyHapticlFullClash()
    {
        PlayerModalityAndLimits(false, true, 0.75f, 0.5f, true);
        setWalls("Haptic");
        Debug.Log("Applying Full Haptic Clash");
    }

    void ApplyInvisible()
    {
        PlayerModalityAndLimits(false, true, 0.75f, 0.5f, true);
        setWalls("Invisible");
        Debug.Log("Applying Invisible");
    }


}






