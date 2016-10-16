using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public int currentLevel = 0;
    public GameObject[] enemys;
    public GameObject player;
    public GameObject textContainer;
    public Transform respawnLocations;
    public Vector2[] gridTransform;

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
        player.transform.rotation = respawnLoc.transform.rotation;
    }

    public void showPrevious3DText()
    {
        GameObject.Find("Level" + currentLevel + "Text").GetComponent<MeshRenderer>().enabled = true;
    }

    public void update3DText(bool newRoom, int roomNum) // Is the room we're entering an actual level?
    {
        if (newRoom)
        {
            GameObject.Find("Level" + roomNum + "Text").GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GameObject.Find("Level" + (currentLevel + 1) + "Text").GetComponent<MeshRenderer>().enabled = true;
        }
        
    }

    public void translateGrid()
    {
        this.transform.position = new Vector3(gridTransform[currentLevel].x, 0.5f, gridTransform[currentLevel].y);
        var pathFinder = this.GetComponent<Pathfinding>();
        pathFinder.RefreshGrid();
    }
}
