using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public int levelNum;
    public GameObject closeTrigger;
    public bool startDoor = false;
    public bool doorOpen = false;
    public int keysRequired = 0;

    LevelManager levelManager;
    IsTriggered isTriggered;
    bool closeTriggered = false;
    BoxCollider doorCollider;

    Color currentColor;

    new Renderer renderer;

	// Use this for initialization
	void Start () {
        doorCollider = GetComponent<BoxCollider>();
        closeTriggered = closeTrigger.GetComponent<IsTriggered>().triggered;
        renderer = GetComponent<Renderer>();
        levelManager = GameObject.Find("MasterController").GetComponent<LevelManager>();
        if (keysRequired != 0)
        {
            renderer.material.color = new Color(0f, 0.5f, 1f);
            //currentColor = new Color(0f, 0.5f, 1f);
            doorCollider.isTrigger = false;
            doorOpen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        closeTriggered = closeTrigger.GetComponent<IsTriggered>().triggered;
        if (keysRequired != 0) { 
            if (levelManager.currentLevel == levelNum && levelManager.currentKeys == keysRequired && !doorOpen)
            {
                OpenDoor();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            renderer.material.color = new Color(1f, 0.5f, 0f);
            //currentColor = new Color(1f, 0.5f, 0f);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (closeTriggered && other.tag == "Player")
        {
            CloseDoor();
            if (startDoor)
            {
                levelManager.update3DText(true, levelNum);
                levelManager.currentLevel = levelNum;
                levelManager.translateGrid();
                GameObject enemies = GameObject.Find("Enemys");
                for (int i = 0; i < enemies.transform.childCount; i++)
                {
                    Entity enemyScript = enemies.transform.GetChild(i).GetComponent<Entity>();
                    if (enemyScript.gameLevel == levelNum)
                    {
                        enemyScript.enabled = true;
                        enemyScript.ResetEnemy();
                        enemyScript.FindPath();
                    }
                    else enemyScript.enabled = false;
                }
            }
            else if (levelManager.currentKeys == keysRequired)
            {
                levelManager.update3DText(false, 0);
                levelManager.currentLevel = 0; // Intermediate level to pause all AI
                levelManager.currentKeys = 0;
            }
        } else if (other.tag == "Player") {
            renderer.material.color = Color.green;
            //currentColor = Color.green;
        }
    }

    public void OpenDoor()
    {
        doorCollider.isTrigger = true;
        renderer.material.color = Color.green;
        //currentColor = Color.green;
        doorOpen = true;
    }

    public void CloseDoor()
    {
        doorCollider.isTrigger = false;
        renderer.material.color = Color.red; // should this be changed to be the same colour as the walls. To 'hide' the door?
        //currentColor = Color.red;
        doorOpen = false;
    }
}
