using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public int currentLevel = 0;
    public GameObject[] enemys;
    public GameObject player;
    public Transform respawnLocations;

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

    public void OpenLastDoor(int lastLevel)
    {
        GameObject.Find("StartDoor" + (lastLevel)).GetComponent<Door>().OpenDoor();
        GameObject.Find("EndDoor" + (lastLevel)).GetComponent<Door>().OpenDoor();  // If you're in the door when the enemy catches you the door breaks
    }

    public void RespawnPlayer()
    {
        GameObject respawnLoc = respawnLocations.GetChild(currentLevel-1).gameObject;
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
