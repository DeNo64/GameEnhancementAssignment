using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public int HitPoints = 10;
    public float movementSpeed = 0.02f;
    public GameObject player;
    public string waypointPath;
    public int gameLevel;
        
    float sightSpeedMult = 1.5f;
    Vector3 spawnLoc;
    LevelManager levelManager;
    Vector3[] path;
    int targetIndex;
    Pathfinding pathFinding;
    Vector3 currentWaypoint;
    float time = 0.0f;

    // finite state machine
    enum entityAlertState
    {
        Calm = 0,
        Suspicious = 1,
        Alert = 2
    }
    entityAlertState currentState = entityAlertState.Calm;
    float calmDownTime = 0.5f;
    Renderer renderer;
    float SusToCalmTime = 0f;
    float SusToAlertTime = 0f;

    List<Vector3> waypoints = new List<Vector3>();
    bool atWaypoint = false;
    bool followingPlayer = false;

    void Start()
    {
        spawnLoc = transform.position;
        levelManager = GameObject.Find("MasterController").GetComponent<LevelManager>();
        Transform enemyWaypoints = GameObject.Find("EnemyWaypoints/WaypointSet" + waypointPath).transform;
        for (int i = 0; i < enemyWaypoints.childCount; i++)
        {
            waypoints.Add(enemyWaypoints.GetChild(i).position);
            //print(enemyWaypoints.GetChild(i).name);
        }

        currentWaypoint = GetClosestWaypoint(transform);

        pathFinding = GameObject.Find("MasterController").GetComponent<Pathfinding>();
        //path = pathFinding.FindPath(transform.position, currentWaypoint);

        //StopCoroutine("FollowPath");
        //StartCoroutine("FollowPath");

        renderer = GetComponent<Renderer>();
    }

    public void ResetEnemy()
    {
        StopCoroutine("FollowPath");
        transform.position = spawnLoc;
        currentWaypoint = GetClosestWaypoint(transform);
        followingPlayer = false;
        currentState = entityAlertState.Calm;
    }

    public void FindPath()
    {
        path = pathFinding.FindPath(transform.position, currentWaypoint);
        StartCoroutine("FollowPath");
    }

    void Update()
    {
        CheckFiniteStateMachine();      // done the Brody way :P

        //if (gameLevel == levelManager.currentLevel)
        //{
        //    if (CheckPlayerVisability())
        //    {
        //        time += Time.deltaTime;
        //        currentWaypoint = player.transform.position;
        //        if (time > 0.2f)
        //        {
        //            path = pathFinding.FindPath(transform.position, currentWaypoint);
        //            if (path == null)
        //            {
        //                throw new System.Exception("Path Was Not Found!");
        //            }

        //            StopCoroutine("FollowPath");
        //            StartCoroutine(JustWait(2));
        //            time = 0.0f;
        //        }
        //        followingPlayer = true;
        //    }
        //    else if (atWaypoint)
        //    {
        //        if (false)
        //        {
        //            currentWaypoint = GetClosestWaypoint(player.transform);
        //            followingPlayer = false;
        //        }
        //        else
        //        {
        //            currentWaypoint = waypoints[GetIndexOfWaypoint(currentWaypoint) + 1];
        //        }
                
        //        //print(GetIndexOfWaypoint(currentWaypoint) + 1);
        //        atWaypoint = false;

        //        path = pathFinding.FindPath(transform.position, currentWaypoint);
        //        StopCoroutine("FollowPath");
        //        StartCoroutine("FollowPath");
        //    }
        //}
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (gameLevel == levelManager.currentLevel)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        atWaypoint = true;
                        targetIndex = 0;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                float calculatedMoveSpeed = movementSpeed;
                if (followingPlayer)
                {
                    calculatedMoveSpeed = movementSpeed * sightSpeedMult;
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, calculatedMoveSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }

    IEnumerator JustWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine("FollowPath");
        yield return null;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            HitPoints--;
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Finish")
        {
            Destroy(this.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                //Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    int GetIndexOfWaypoint(Vector3 waypoint)
    {
        int index = waypoints.IndexOf(waypoint);

        if (index + 1 >= waypoints.Count())
            index = -1;

        return index;
    }

    Vector3 GetClosestWaypoint(Transform currentPos)
    {
        Vector3 closestWaypoint = waypoints[0];
        foreach (var waypoint in waypoints)
        {
            float currentDistance = Vector3.Distance(currentPos.position, closestWaypoint);
            float nextDistance = Vector3.Distance(currentPos.position, waypoint);

            if (currentDistance > nextDistance)
            {
                closestWaypoint = waypoint;
            }
        }
        if (Vector3.Distance(currentPos.position, closestWaypoint) < 1)
        {
            closestWaypoint = waypoints[GetIndexOfWaypoint(closestWaypoint) + 1];
        }
        return closestWaypoint;
    }

    bool CheckPlayerVisability()
    {
        RaycastHit[] hits;
        var rayDirection = player.transform.position - transform.position;
        hits = Physics.RaycastAll(transform.position, rayDirection, Vector3.Distance(player.transform.position, transform.position));
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

        /*if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            Debug.DrawRay(transform.position, rayDirection, Color.black);
            if (hit.collider.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
            return false; */
    }

    void CheckFiniteStateMachine()
    {
        if(currentState == entityAlertState.Calm)
        {
            renderer.material.color = Color.blue;
            if (atWaypoint)
            {
                currentWaypoint = waypoints[GetIndexOfWaypoint(currentWaypoint) + 1];
            
                //print(GetIndexOfWaypoint(currentWaypoint) + 1);
                atWaypoint = false;
                followingPlayer = false;

                path = pathFinding.FindPath(transform.position, currentWaypoint);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }

            if (CheckPlayerVisability())
            {
                // play some audio queue or something
                currentState = entityAlertState.Suspicious;
            }
        }
        else if (currentState == entityAlertState.Suspicious)
        {
            renderer.material.color = Color.yellow;
            StopCoroutine("FollowPath");

            if (CheckPlayerVisability())
            {
                SusToCalmTime = 0;
                SusToAlertTime += Time.deltaTime;
                if (SusToAlertTime > calmDownTime)
                {
                    if (CheckPlayerVisability())
                    {
                        currentState = entityAlertState.Alert;
                        atWaypoint = false;
                    }
                    SusToAlertTime = 0.0f;
                }
            }
            else
            {
                SusToAlertTime = 0f;
                SusToCalmTime += Time.deltaTime;
                if (SusToCalmTime > calmDownTime)
                {
                    if (!CheckPlayerVisability())
                    {
                        currentState = entityAlertState.Calm;

                        currentWaypoint = GetClosestWaypoint(transform);
                        path = pathFinding.FindPath(transform.position, currentWaypoint);
                        StopCoroutine("FollowPath");
                        StartCoroutine("FollowPath");
                    }
                    SusToCalmTime = 0.0f;
                }
            }                                 
        }
        else if (currentState == entityAlertState.Alert)
        {
            renderer.material.color = Color.red;

            if (CheckPlayerVisability())
            {
                time += Time.deltaTime;
                if (time > 0.2f)
                {
                    currentWaypoint = player.transform.position;
                    path = pathFinding.FindPath(transform.position, currentWaypoint);
                    if (path == null)
                    {
                        throw new System.Exception("Path Was Not Found!");
                    }
                    followingPlayer = true;

                    StopCoroutine("FollowPath");
                    StartCoroutine("FollowPath");
                    time = 0.0f;
                }
            }

            if (atWaypoint)
            {
                currentState = entityAlertState.Suspicious;
            }
        }
    }
}