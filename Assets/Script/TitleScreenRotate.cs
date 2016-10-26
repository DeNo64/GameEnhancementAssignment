using UnityEngine;
using System.Collections;

public class TitleScreenRotate : MonoBehaviour {
    
    const int ROTATIONSPEED = 100;

    void Update()
    {
        transform.Rotate(Vector3.up, ROTATIONSPEED * Time.deltaTime);
    }
}
