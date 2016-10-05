using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turrent : MonoBehaviour {

    public enum TurrentState
    {
        Idle,
        Aim,
        Fire
    }

    public GameObject bullet;
    public float rotateSpeed = 0.5f;
    public float moveSpeed = 0.01f;

    public List<GameObject> player;
    public GameObject hud;

    private TurrentState currentState;
    private float time;


    // Use this for initialization
    void Start () {
        currentState = TurrentState.Idle;
        player = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckForDeadEntities();
        CheckStateMachine();
    }

    void Shoot()
    {
        time += Time.deltaTime;
        if (time >= 0.25)
        {
            GameObject thisBullet = Instantiate(bullet, transform.GetChild(0).GetChild(0).position, Quaternion.identity) as GameObject;
            thisBullet.GetComponent<Bullet>().direction = thisBullet.transform.position - this.transform.GetChild(0).position;

            time = 0;
        }
    }

    void Rotate(float rotateSpeed)
    {
        Quaternion rotate = Quaternion.LookRotation(new Vector3(player[0].transform.position.x - transform.position.x, 0, player[0].transform.position.z - transform.position.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * rotateSpeed);
    }

    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        player.Add(col.gameObject);
    //    }
    //}

    //void OnTriggerExit(Collider col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        player.Remove(col.gameObject);
    //    }
    //}

    void CheckForDeadEntities()
    {
        if (player.Count != 0)
        {
            foreach (GameObject playerObject in player.ToArray())
            {
                if (playerObject.GetComponent<Entity>().HitPoints <= 0)
                {
                    player.Remove(playerObject);
                }
            }
        }
    }
    
    void CheckStateMachine()
    {
        switch (currentState)
        {
            case TurrentState.Idle:
                if (player.Count != 0)
                {
                    currentState = TurrentState.Aim;
                }
                break;
            case TurrentState.Aim:
                if (player.Count == 0)
                {
                    currentState = TurrentState.Idle;
                    break;
                }
                else if (Vector3.Angle(new Vector3(transform.forward.x, 0, transform.forward.z),
                                       new Vector3(player[0].transform.position.x, 0, player[0].transform.position.z)
                                     - new Vector3(transform.position.x, 0, transform.position.z))
                                     < 10)
                {
                    currentState = TurrentState.Fire;
                    break;
                }

                Rotate(rotateSpeed);
                break;
            case TurrentState.Fire:
                if (player.Count == 0)
                {
                    currentState = TurrentState.Idle;
                    break;
                }
                else if (Vector3.Angle(new Vector3(transform.forward.x, 0, transform.forward.z),
                                       new Vector3(player[0].transform.position.x, 0, player[0].transform.position.z)
                                     - new Vector3(transform.position.x, 0, transform.position.z))
                                     > 10)
                {
                    currentState = TurrentState.Aim;
                    break;
                }

                Rotate(rotateSpeed * 5);
                Shoot();
                break;
        }
    }
}
