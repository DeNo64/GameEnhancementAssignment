using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public int currentLevel = 0;
    public GameObject[] enemys;
    public GameObject player;

    // Use this for initialization
    void Start () {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }
	
	// Update is called once per frame
	void Update () {
        //print(currentLevel);
	}

    public void ResetEnemys()
    {
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Entity>().ResetEnemy();
        }
    }

    public void OpenLastDoor()
    {
        GameObject.Find("StartDoor" + currentLevel).GetComponent<Door>().OpenDoor();
    }

    public void RespawnPlayer()
    {
        GameObject respawnLoc = GameObject.Find("Level" + currentLevel);
        player.transform.position = respawnLoc.transform.position;
    }

    public void NewLevel()
    {
        switch (currentLevel)
        {
            case 0:
                break;

            default:
                break;
        }


    }
}
