using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    float movementSpeed = 0.05f;

	void Update ()
    {
        Move();
	}

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal * 100, 0, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical * 100);
    }
}
