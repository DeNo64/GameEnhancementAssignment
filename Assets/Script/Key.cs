using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    const int ROTATIONSPEED = 100;

    LevelManager levelManager;
    bool collected = false;

    new Renderer renderer;
    //SphereCollider collider;
    Collider collider;  
    

    // Use this for initialization
    void Start () {
        levelManager = GameObject.Find("MasterController").GetComponent<LevelManager>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, ROTATIONSPEED * Time.deltaTime);
	}

    void OnCollisionEnter(Collision other)
    {
        if (!collected && other.collider.tag == "Player")
        {
            levelManager.currentKeys++;
            collected = true;
            //renderer.enabled = false;
            collider.enabled = false;
            this.gameObject.SetActive(false);
        }
    }

    public void ResetKey()
    {
        collected = false;
        //renderer.enabled = true;
        collider.enabled = true;
        this.gameObject.SetActive(true);
    }
}
