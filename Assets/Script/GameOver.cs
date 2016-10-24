using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public GameObject player;
    public GameObject mastercontroller;

    LevelManager levelManager;
    int respawnTime = 5; // Seconds
    bool caught = false;
    public int timesCaught = 0;

    // Use this for initialization
    void Start()
    {
        levelManager = mastercontroller.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && CheckPlayerVisability(player.transform, other.transform))// && !caught)
        {
            caught = true;
            print("Caught! (Level " + levelManager.currentLevel + ")");
            timesCaught++;
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
        levelManager.ResetKeys();
        levelManager.ClearHud();
        // Commented this out for now because it's easier for testing
        // Has the introduction of the boolean 'caught' fixed this?
        //yield return new WaitForSeconds(respawnTime);   // this wait is breaking the program because if you wait then the coroutine will fire again because the player is still touching the enemy        
        int currentLevel = levelManager.currentLevel;
        levelManager.showPrevious3DText();
        levelManager.OpenLastDoor(currentLevel);
        levelManager.currentLevel = 0;
        caught = false;
        yield return null;
    }

    bool CheckPlayerVisability(Transform player, Transform enemy)
    {
        RaycastHit[] hits;
        var rayDirection = player.position - enemy.position;
        hits = Physics.RaycastAll(enemy.position, rayDirection, Vector3.Distance(player.position, enemy.position));
        if (hits.Length > 0)
        {
            float playerDist = 1001f;
            float closest = 1000f;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.tag == "Player")
                {
                    playerDist = hits[i].distance;
                }
                if (hits[i].distance < closest && hits[i].collider.tag != "PlayerTagRange")
                {
                    closest = hits[i].distance;
                }
            }
            if (playerDist == closest)
            {
                return true;
            }
        }
        return false;
        /*for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.tag == "Player")
            {
                break;
            }
            else
            {
                print(hits[i].collider.tag);
                return false;
            }
        }
    return true;*/
    }
}
