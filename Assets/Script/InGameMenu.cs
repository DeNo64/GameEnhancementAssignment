using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

    public Canvas quit;
    public Button resume;
    public Button exit;
    public GameObject player;

    Canvas thisCanvas;

    void Start()
    {
        quit = quit.GetComponent<Canvas>();
        resume = resume.GetComponent<Button>();
        exit = exit.GetComponent<Button>();
        thisCanvas = this.gameObject.GetComponent<Canvas>();
        quit.enabled = false;
    }

    public void PressQuit()
    {
        quit.enabled = true;
        resume.enabled = false;
        exit.enabled = false;
    }

    public void NoPress()
    {
        quit.enabled = false;
        resume.enabled = true;
        exit.enabled = true;
    }

    public void PressResume()
    {
        ShowMenu(false);
        player.GetComponent<Player>().mouseVisable = false;        
    }

    public void LeaveGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowMenu(bool showBool)
    {
        thisCanvas.enabled = showBool;
        resume.enabled = showBool;
        exit.enabled = showBool;
        quit.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Assignment");
    }
}
