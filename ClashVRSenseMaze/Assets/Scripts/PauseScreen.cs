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
    public InputActionProperty OpenMenuAction;
    public GameObject vPauseMenu;
    public GameObject menuCanvas;


    //public GameManagerScript gms;
    //private TrainingScript ts;
    //private MazeManager mm;
    //private StreamOutlet outlet_finish;
    //private string[] sample_start = {""};
    
    // Define Toggle Button for Menu

    void Update()
    {
        if(OpenMenuAction.action.WasPressedThisFrame())
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            vPauseMenu.SetActive(!menuCanvas.activeSelf);
        }
    }

}
