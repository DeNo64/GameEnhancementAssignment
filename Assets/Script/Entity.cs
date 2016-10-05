using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{

    public int HitPoints = 10;
    public float movementSpeed = 0.02f;
    public GameObject player;

    Vector3[] path;
    int targetIndex;
    Pathfinding pathFinding;
    Vector3 currentWaypoint;
    float time;

    List<Vector3> waypoints = new List<Vector3>();
    bool atWaypoint = false;

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
            currentWaypoint = player.transform.position;

            path = pathFinding.FindPath(transform.position, currentWaypoint);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    void OnDestroy()
    {
        StopCoroutine("FollowPath");
        GameLogic.entitiesAlive--;
    }

    void Start()
    {
        var enemyWaypoints = GameObject.Find(this.gameObject.name + "Waypoints").transform;
        for (int i = 0; i < enemyWaypoints.childCount; i++)
            waypoints.Add(enemyWaypoints.GetChild(i).position);

        currentWaypoint = GetClosestWaypoint();

        pathFinding = GameObject.Find("MasterController").GetComponent<Pathfinding>();
        path = pathFinding.FindPath(transform.position, currentWaypoint);

        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
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
            if (hit.transform == player.transform)
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