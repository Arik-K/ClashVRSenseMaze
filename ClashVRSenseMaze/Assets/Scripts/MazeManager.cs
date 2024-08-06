using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeManager : MonoBehaviour
{
    public Material[] materials; // The array of materials you want to use.
    public GameObject noVision;// for visual off
    public GameObject player;
    private GameObject[] phantom_maze_list;  
    private Camera playerCamera;

    private int currentTextureIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ActivateMaze(string curr_maze)
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
    }

    /*public void ActivateCondition(string maze, string condition)
    {
        AudioSource audioSourcePlayer = player.GetComponent<AudioSource>();
        
        
        // Change Material
        //int randomIndex = Random.Range(0, materials.Length); // Generate a random index within the materials array length
        // ChangeWallsMaterial(maze_name, randomIndex); // Set the initial material by passing the desired index.  
        print(maze);

        switch (condition)
        {
            // Deafult case all senses
            case "all" when true:
                ApplyVisualAudioHaptic(maze_name);
                break;
            
            // Only one sensory channel active
            case "audio_only" when true:
                ApplyAudioOnly(maze_name);
                break;
            case "visual_only" when true:
                ApplyVisualOnly(maze_name);
                break;
            case "Haptic_only" when true:
                ApplyHapticlOnly(maze_name);
                break;
            
            // only Two sensory channels - One off
            case "audio_off" when true:
                ApplyAudioOnly(maze_name);
                break;
            case "visual_off" when true:
                ApplyVisualOnly(maze_name);
                break;
            case "Haptic_off" when true:
                ApplyHapticlOnly(maze_name);
                break;

            // Full clash with one sesnory channel
            case "audio_full_clash" when true:
                ApplyContraVisual(maze_name);
                break;
            case "visual_full_clash" when true:
                ApplyVisualOnly(maze_name);
                break;
            case "Haptic_full_clash" when true:
                ApplyHapticlOnly(maze_name);
                break;

            // Special wall case
            case "invisible" when true:
                ApplyInvisible(maze_name);
                break;   
        }

    }


    public void WallMaterialChange()
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

    void DeactivateWalls(Transform parent, string tag)
    {
        GameObject[] walls = GetChildGameObjectsWithTag(parent, tag);
        foreach (GameObject wall in walls)
        {
            if (wall != null)
            {
                wall.gameObject.SetActive(false);
            }
        }
    }

    void ActivateWalls(Transform parent, string tag)
    {
        GameObject[] walls = GetChildGameObjectsWithTag(parent, tag);
        foreach (GameObject wall in walls)
        {
            if (wall != null)
            {
                wall.gameObject.SetActive(true);
            }
        }
    }

    GameObject[] GetChildGameObjectsWithTag(Transform parent, string tag)
    {
        List<GameObject> taggedObjects = new List<GameObject>();
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                taggedObjects.Add(child.gameObject);
            }
        }
        return taggedObjects.ToArray();
        
    }
    */

}
