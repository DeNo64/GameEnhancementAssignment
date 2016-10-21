using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    LevelManager levelManager;
    bool collected = false;

    new Renderer renderer;
    SphereCollider collider;

    // Use this for initialization
    void Start () {
        levelManager = GameObject.Find("MasterController").GetComponent<LevelManager>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<SphereCollider>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter(Collision other)
    {
        if (!collected && other.collider.tag == "Player")
        {
            levelManager.currentKeys++;
            collected = true;
            renderer.enabled = false;
            collider.enabled = false;
        }
    }

    public void ResetKey()
    {
        collected = false;
        renderer.enabled = true;
        collider.enabled = true;
    }
}
