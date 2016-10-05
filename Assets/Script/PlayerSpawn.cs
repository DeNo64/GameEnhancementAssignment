using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {

    public GameObject turrent;
    public GameObject Controller;
    public Transform spawn, goal;

    float time;
    GameObject lastTurrent;

    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetAxis("Fire1") > 0 && Hud.escaped > 0 && Hud.budget >= 10)
        {
            if (time >= 0.5)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    lastTurrent = Instantiate(turrent, new Vector3(hit.point.x, 0.25f, hit.point.z), hit.point.z >= 0 ? Quaternion.identity : Quaternion.identity * Quaternion.Euler(0, 90, 0)) as GameObject;
                    Controller.GetComponent<Grid>().CreateGrid();

                    if (!CheckPath())
                    {
                        DestoryLastTurrent();
                    }
                    else
                    {
                        Hud.budget -= 10;

                        time = 0;
                    }
                }
            }
        }
    }

    bool CheckPath()
    {
        Vector3[] path = Controller.GetComponent<Pathfinding>().FindPath(spawn.position, goal.position);
        if (path != null)
            return true;
        else
            return false;
    }

    public void DestoryLastTurrent()
    {
        if (lastTurrent == null)
            return;
        DestroyImmediate(lastTurrent);
        Controller.GetComponent<Grid>().CreateGrid();
    }
 }