﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public int currentLevel = 0;
    public GameObject[] enemys;
    public GameObject[] keys;
    public GameObject player;
    public GameObject textContainer;
    public Transform respawnLocations;
    public Vector2[] gridTransform;
    public GameObject gameOverObject;
    public GameObject threeDHud;
    public GameObject doors;

    public TextMesh captureText;
    public TextMesh timerText;

    public int currentKeys = 0;

    float gameTimer;
    GameOver gameOverScript;
    int[] keysRequired;

    // Use this for initialization
    void Start () {
        gameTimer = 0.0f;
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        keys = GameObject.FindGameObjectsWithTag("Key");
        
        gameOverScript = gameOverObject.GetComponent<GameOver>();

        keysRequired = new int[doors.transform.childCount];
        // set the required keys to exit doors
        for (int i = 0; i < doors.transform.childCount; i++)
        {
            keysRequired[i] = doors.transform.GetChild(i).FindChild("EndDoor" + (i+1)).GetComponent<Door>().keysRequired;
        }
    }
	
	// Update is called once per frame
	void Update () {
        gameTimer += Time.deltaTime;
        //print(currentLevel);
    }

    public void ResetEnemys()
    {
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Entity>().ResetEnemy();
        }
    }

    public void ResetKeys()
    {
        currentKeys = 0;
        foreach (GameObject key in keys)
        {
            key.GetComponent<Key>().ResetKey();
        }
    }

    public void ClearHud()
    {
        threeDHud.GetComponent<HUD_Keys>().clearKeys();
    }

    public void ResetHud(int currentLevel)
    {
        threeDHud.GetComponent<HUD_Keys>().resetKeys(keysRequired[currentLevel-1]);
    }

    public void OpenLastDoor(int lastLevel)
    {
        GameObject.Find("StartDoor" + (lastLevel)).GetComponent<Door>().OpenDoor();
        GameObject.Find("EndDoor" + (lastLevel)).GetComponent<Door>().LockDoor();
    }

    public void RespawnPlayer()
    {
        GameObject respawnLoc = respawnLocations.GetChild(currentLevel-1).gameObject;
        player.transform.position = respawnLoc.transform.position;
        player.transform.rotation = respawnLoc.transform.rotation;
    }

    public void showPrevious3DText()
    {
        GameObject.Find("Level" + currentLevel + "Text").GetComponent<MeshRenderer>().enabled = true;
    }

    public void update3DText(bool newRoom, int roomNum) // newRoom = Is the room we're entering an actual level?
    {
        if (newRoom)
        {
            GameObject.Find("Level" + roomNum + "Text").GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GameObject.Find("Level" + (roomNum + 1) + "Text").GetComponent<MeshRenderer>().enabled = true;
            if ((currentLevel + 1) == 9)
            {
                captureText.text = "You were caught " + gameOverScript.timesCaught.ToString() + " times.";
                timerText.text = "You took " + Mathf.Floor(gameTimer /60f) + " minutes and " + Mathf.Floor((gameTimer/60-Mathf.Floor(gameTimer / 60))*60) + " seconds to beat the game";
                GameObject.Find("CaptureCountText").GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("TimeCountText").GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void translateGrid()
    {
        this.transform.position = new Vector3(gridTransform[currentLevel].x, 0.5f, gridTransform[currentLevel].y);
        var pathFinder = this.GetComponent<Pathfinding>();
        pathFinder.RefreshGrid();
    }
}
