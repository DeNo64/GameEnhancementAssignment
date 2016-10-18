using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

    public Canvas quit;
    public Button play;
    public Button exit;

    void Start()
    {
        quit = quit.GetComponent<Canvas>();
        play = play.GetComponent<Button>();
        exit = exit.GetComponent<Button>();
        quit.enabled = false;
    }

    public void PressExit()
    {
        quit.enabled = true;
        play.enabled = false;
        exit.enabled = false;
    }

    public void NoPress()
    {
        quit.enabled = false;
        play.enabled = true;
        exit.enabled = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Assignment");
        //SceneManager.UnloadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
