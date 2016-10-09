using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public int levelNum;
    public GameObject closeTrigger;
    public bool startDoor = false;
    public bool doorOpen = false;

    LevelManager levelManager;
    IsTriggered isTriggered;
    bool closeTriggered = false;
    BoxCollider doorCollider;

    new Renderer renderer;

	// Use this for initialization
	void Start () {
        doorCollider = GetComponent<BoxCollider>();
        closeTriggered = closeTrigger.GetComponent<IsTriggered>().triggered;
        renderer = GetComponent<Renderer>();
        levelManager = GameObject.Find("MasterController").GetComponent<LevelManager>();
    }
	
	// Update is called once per frame
	void Update () {
        closeTriggered = closeTrigger.GetComponent<IsTriggered>().triggered;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            renderer.material.color = new Color(1f, 0.5f, 0f);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (closeTriggered && other.tag == "Player")
        {
            CloseDoor();
            if (startDoor)
            {
                levelManager.currentLevel = levelNum;
                levelManager.NewLevel();
            }
            else
            {
                levelManager.currentLevel = 0; // Intermediate level to pause all AI
            }
        } else if (other.tag == "Player") {
            renderer.material.color = Color.green;
        }
    }

    public void OpenDoor()
    {
        doorCollider.isTrigger = true;
        renderer.material.color = Color.green;
    }

    public void CloseDoor()
    {
        doorCollider.isTrigger = false;
        renderer.material.color = Color.red; // should this be changed to be the same colour as the walls. To 'hide' the door?
    }
}
