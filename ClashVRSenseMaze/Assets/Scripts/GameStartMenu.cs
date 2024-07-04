using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;


    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button optionButton;
    public Button trainingButton;
    public Button quitButton;

    private const int MainGameSceneIndex = 1;
    private const int TrainingSceneIndex = 2;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        // Hook events
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(EnableOption);
        trainingButton.onClick.AddListener(StartTraining);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(MainGameSceneIndex);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);

    }

    public void EnableOption()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }


    public void StartTraining()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(TrainingSceneIndex);
    }
}
