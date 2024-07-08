using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject mazes;
    MazeManager mm;
    Logger ls;
    TrainingManager ts;
    PauseScreen pm;

    public GameObject NextLevelCanvas1;
    public List<Transform> all_mazes;
    public GameObject Goal;
    public static int mazeIndex = -1;
    public static int count = 0;

    private string temp_str;
    private Transform temp_trans;
    private AudioSource audioSource; // the audio source component attached to this game object
    private string[] MazeType_temp = new string[]  {} ;
    private string[] ConditionType_temp = new string[]  {} ;

    public TextMeshProUGUI textMeshPro;
    public static string[] mazes_name_list =  new string[]  {} ;
    public static string[] conditions = new string[] {};
    public GameObject player; // the player game object
    // audio clips to play for raycasting sounds
    public AudioClip[] audioClips;
    private static Vector3 maze1_intial_location = new Vector3(0.62f, 0.219f, -1.23f);
    private static Vector3 maze2_intial_location = new Vector3(0.62f, 0.219f, 0.83f);
    private static Vector3 maze3_intial_location = new Vector3(0.62f, 0.219f, 0.83f);
    // private static Vector3 maze4_intial_location = new Vector3(-0.59f, 0.219f, 0.86f); 

    // Start is called before the first frame update
      void Start()
      {   
        //gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        Goal.layer = LayerIgnoreRaycast;

        Debug.Log("Current layer: " + gameObject.layer);
        mm = mazes.GetComponent<MazeManager>();
        audioSource = player.GetComponent<AudioSource>();
       // MainMenu.isTraining = false;
        //NextMaze();
      }
    // Update is called once per frame
    void Update()
    {
        
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
        case "Haptic_only" when true:
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

        // Special wall case
        case "invisible" when true:
            textMeshPro.text = "Trust Audio and Haptic, Visual Could Be Misleading";
            break;  

      } 
    
    }
    
}
