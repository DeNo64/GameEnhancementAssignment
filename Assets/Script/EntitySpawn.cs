using UnityEngine;
using System.Collections;

public class EntitySpawn : MonoBehaviour {

    public GameObject entity;

    float time;    

    void Start()
    {
        //Instantiate(entity, transform.position, Quaternion.identity);
    }

    public void spawnEntities(float spawnDelay)
    {
        time += Time.deltaTime;
        if (time >= spawnDelay)
            {
                Instantiate(entity, transform.position, Quaternion.identity);
                GameLogic.entitiesAlive++;
                GameLogic.waveSize--;

                time = 0;
            }
    }
}