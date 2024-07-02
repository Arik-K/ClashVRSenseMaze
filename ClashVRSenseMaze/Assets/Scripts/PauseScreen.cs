using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;
using System;
using System.Runtime.CompilerServices;

public class PauseScreen : MonoBehaviour
{
    public InputActionReference OpenMenuAction;
    public GameObject vPauseMenu;
    public GameObject NextLevelCanvas;
    public bool isPaused;

    //public GameManagerScript gms;
    //private TrainingScript ts;
    //private MazeManager mm;
    //private StreamOutlet outlet_finish;
    //private string[] sample_start = {""};
    
    // Define Toggle Button for Menu
    void Awake()
    {
        OpenMenuAction.action.Enable();
        OpenMenuAction.action.performed -= ToggleMenu;
    }

    private void OnDestroy()
    {
        OpenMenuAction.action.Disable();
        OpenMenuAction.action.performed -= ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        vPauseMenu.SetActive(false);
        NextLevelCanvas.SetActive(false);
        //gms = FindObjectOfType<GameManagerScript>();
        //ts = FindObjectOfType<TrainingScript>();
       //mm = FindObjectOfType<MazeManager>();
    }

    // Update is called once per frame

    public void QuitGame()
    {
         Application.Quit();
    }

    public void PauseGame()
    {
        vPauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;  
    }

    public void ResumeGame()
    {
        vPauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;  
  
    }

        public void GoToMainMenu()
    {
         Time.timeScale = 1f;
         SceneManager.LoadScene("MainMenu");   
    }

    public void NextMazeButton()
    {
        //mm.TriggerRotationStop();
        //if(MainMenu.isTraining)
            {
                //ts.NextMaze();
                AudioListener.volume = 1f;
                NextLevelCanvas.SetActive(false);
                
            }
       // else
            {
                //gms.NextMaze();
                AudioListener.volume = 1f;
                NextLevelCanvas.SetActive(false);

            }
    }
}
