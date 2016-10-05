using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public GameObject turrent;
    public float bulletSpeed = 0.5f;
    public int lifeTime = 1;

    public Vector3 direction;
    private float time;

    // Update is called once per frame
    void Update()
    {
        Travel();
        time += Time.deltaTime;
        if (time >= lifeTime) Destroy(gameObject);
    }

    void Travel()
    { 
        transform.Translate(direction * bulletSpeed);
    }
}