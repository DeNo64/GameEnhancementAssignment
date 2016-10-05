using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

    public static int entitiesAlive;
    public static int waveSize;

    public GameObject entitySpawn;
    public GameObject controller;

    int originalWaveSize = 10;
    int waveSizeIncreaseValue = 2;
    int originalSpawnDelay = 2;
    bool intermission = true;
    float restartGameDelayTime;
    float spawnDelay;
    float waveSpawnOffset = 0.5f;

    // Update is called once per frame
    void Update ()
    {
        Debug.Log(entitiesAlive +" WS: "+ waveSize);
        
        if (Hud.escaped > 0)
        {
            RunLogic();
        }
        else
        {
            ResetSettings();
        }
    }

    void RunLogic()
    {
        if (waveSize > 0)
        {
            entitySpawn.GetComponent<EntitySpawn>().spawnEntities(spawnDelay);
        }
        else if (!intermission && entitiesAlive == 0)
        {
            Hud.intermission = 10;
            intermission = true;
        }
        else if (intermission && Hud.intermission <= 0)
        {
            Hud.waveNum++;

            waveSize = originalWaveSize;
            waveSize += waveSizeIncreaseValue*Hud.waveNum;
            spawnDelay = originalSpawnDelay;
            spawnDelay *= waveSpawnOffset/Hud.waveNum;
            intermission = false;
        }
    }

    void ResetSettings()
    {
        GameObject[] players2Delete = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] turrents2Delete = GameObject.FindGameObjectsWithTag("Turrent");

        foreach (var player in players2Delete)
            Destroy(player);
        foreach (var turrent in turrents2Delete)
            Destroy(turrent);

        restartGameDelayTime += Time.deltaTime;
        if (Input.GetAxis("Fire1") > 0)
        {
            if (restartGameDelayTime >= 3)
            {
                Hud.escaped = 10;
                Hud.waveNum = 0;
                Hud.score = 0;
                Hud.budget = 0;
                spawnDelay = originalSpawnDelay;

                controller.GetComponent<Grid>().CreateGrid();

                restartGameDelayTime = 0;
            }
        }
    }
}