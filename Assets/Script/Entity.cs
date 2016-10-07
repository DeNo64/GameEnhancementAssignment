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

    Vector3 spawnLoc;
    LevelManager levelManager;
    Vector3[] path;
    int targetIndex;
    Pathfinding pathFinding;
    Vector3 currentWaypoint;
    float time = 0.0f;

    List<Vector3> waypoints = new List<Vector3>();
    bool atWaypoint = false;

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

        currentWaypoint = GetClosestWaypoint();

        pathFinding = GameObject.Find("MasterController").GetComponent<Pathfinding>();
        path = pathFinding.FindPath(transform.position, currentWaypoint);

        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    public void ResetEnemy()
    {
        StopCoroutine("FollowPath");
        transform.position = spawnLoc;
        currentWaypoint = GetClosestWaypoint();
        path = pathFinding.FindPath(transform.position, currentWaypoint);
        StartCoroutine("FollowPath");
    }

    void Update()
    {
        if (HitPoints <= 0)
        {
            Hud.score++;
            Hud.budget += 10;
            Destroy(this.gameObject);
        }

        if (atWaypoint)
        {
            currentWaypoint = waypoints[GetIndexOfWaypoint(currentWaypoint) + 1];
            atWaypoint = false;

            path = pathFinding.FindPath(transform.position, currentWaypoint);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

        if(CheckPlayerVisability())
        {
            time += Time.deltaTime;
            currentWaypoint = player.transform.position;
            if (time > 0.2f)
            {
                path = pathFinding.FindPath(transform.position, currentWaypoint);
                if (path != null)
                {
                    StopCoroutine("FollowPath");
                    StartCoroutine("FollowPath");
                }
                time = 0.0f;
            }
        }
    }

    void OnDestroy()
    {
        StopCoroutine("FollowPath");
        GameLogic.entitiesAlive--;
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
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movementSpeed * (1 + (0.5f * Hud.waveNum)));
            }
            yield return null;
        }
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
            Hud.escaped--;
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
                else {
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

    Vector3 GetClosestWaypoint()
    {
        Vector3 closestWaypoint = waypoints[0];
        foreach (var waypoint in waypoints)
        {
            float currentDistance = Vector3.Distance(transform.position, closestWaypoint);
            float nextDistance = Vector3.Distance(transform.position, waypoint);

            if (currentDistance > nextDistance)
            {
                closestWaypoint = waypoint;
            }
        }
        if (Vector3.Distance(transform.position, closestWaypoint) < 1)
        {
            closestWaypoint = waypoints[GetIndexOfWaypoint(closestWaypoint) + 1];
        }
        return closestWaypoint;
    }

    bool CheckPlayerVisability()
    {
        RaycastHit hit;
        var rayDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
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
            return false;
    }
}