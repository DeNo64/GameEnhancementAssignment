using UnityEngine;
using System.Collections;

public class DriftFix : MonoBehaviour {

    Vector3 initialLoc;

	// Use this for initialization
	void Start () {
        initialLoc = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = initialLoc;
    }
}
