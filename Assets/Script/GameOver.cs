using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public GameObject player;
    LevelManager levelManager;
    int respawnTime = 5;

	// Use this for initialization
	void Start () {
        levelManager = GameObject.Find("MasterController").GetComponent<LevelManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && CheckPlayerVisability(player.transform, other.transform))
        {
            print("Caught!");
            // Play Death Animation
            // Show "Caught" text on screen (Fade in?)
            // Wait a few (5?) seconds and Respawn at the last checkpoint (Show Timer Until Respawn)
            StartCoroutine("RespawnTimer");
        }
    }

    IEnumerator RespawnTimer()
    {
        levelManager.RespawnPlayer();
        levelManager.ResetEnemys();
        int currentLevel = levelManager.currentLevel;
        levelManager.currentLevel = 0;
        yield return new WaitForSeconds(respawnTime);   // this wait is breaking the program because if you wait then the coroutine will fire again because the player is still touching the enemey        
        levelManager.OpenLastDoor(currentLevel);        // fixed the WaitForSeconds bug very badly but YOLO its just Uni
        yield return null;
    }

    bool CheckPlayerVisability(Transform player, Transform enemy)
    {
        RaycastHit[] hits;
        var rayDirection = player.position - enemy.position;
        hits = Physics.RaycastAll(enemy.position, rayDirection, Vector3.Distance(player.position, enemy.position));
        if (hits.Length > 0) // Sometimes this doesn't detect the wall so the player dies when in cover.
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.tag == "Player")
                {
                    break;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
}
