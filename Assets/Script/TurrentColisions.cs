using UnityEngine;
using System.Collections;

public class TurrentColisions : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Turrent>().player.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Turrent>().player.Remove(col.gameObject);
        }
    }
}