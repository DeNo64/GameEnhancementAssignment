using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    const float MOVESPEED = 4f;
    const float STRAFESPEED = 2f;
    const float ROTSPEED = 200f;
    Rigidbody player;

    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        Move();
	}

    void Move()
    {
        /*float horizontal = Input.GetAxis("Horizontal") * MOVESPEED * Time.deltaTime;
        player.MovePosition(new Vector3(horizontal * 100, 0, 0) + player.position);

        float vertical = Input.GetAxis("Vertical") * MOVESPEED * Time.deltaTime;
        player.MovePosition(new Vector3(0, 0, vertical * 100) + player.position);*/

        if (Input.GetKey(KeyCode.A))
        {
            player.MovePosition((-transform.right * (STRAFESPEED * Time.deltaTime)) + player.position); // Left
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.MovePosition((transform.right * (STRAFESPEED * Time.deltaTime)) + player.position); // Right
        }
        if (Input.GetKey(KeyCode.W))
        {
            player.MovePosition((transform.forward * (MOVESPEED * Time.deltaTime)) + player.position); // Forward
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.MovePosition((-transform.forward * (MOVESPEED * Time.deltaTime)) + player.position); // Backward
        }

        //Vector3 eulerRot = new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * ROTSPEED;
        //Quaternion deltaRotation = Quaternion.Euler(eulerRot);

        //player.MoveRotation(player.rotation * deltaRotation);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * ROTSPEED);
        player.velocity = new Vector3(0f, 0f, 0f);
    }
}
