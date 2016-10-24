using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    const float MOVESPEED = 4f;
    const float STRAFESPEED = 2f;
    const float ROTSPEED = 200f;
    const float FORCEMULTI = 3000f;
    Rigidbody player;
    public bool mouseVisable = false;

    public GameObject inGameMenu;
    InGameMenu script;
    
    internal Animator animator;

    void Start()
    {
        player = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        script = inGameMenu.GetComponent<InGameMenu>();

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (animator != null)
        {
            animator.SetBool("WPushed", Input.GetKey(KeyCode.W));
            animator.SetBool("SPushed", Input.GetKey(KeyCode.S));
            animator.SetBool("APushed", Input.GetKey(KeyCode.A));
            animator.SetBool("DPushed", Input.GetKey(KeyCode.D));

            animator.SetBool("TPushed", Input.GetKey(KeyCode.T));
            animator.SetBool("YPushed", Input.GetKey(KeyCode.Y));
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseVisable = !mouseVisable;
            script.ShowMenu(mouseVisable);
        }
        if (!mouseVisable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Move();
        }
        else
            Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = (mouseVisable);
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
            //player.AddForce((-transform.right * (STRAFESPEED * Time.deltaTime * FORCEMULTI)) + player.position);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.MovePosition((transform.right * (STRAFESPEED * Time.deltaTime)) + player.position); // Right
            //player.AddForce((transform.right * (STRAFESPEED * Time.deltaTime * FORCEMULTI)) + player.position);
        }
        if (Input.GetKey(KeyCode.W))
        {
            player.MovePosition((transform.forward * (MOVESPEED * Time.deltaTime)) + player.position); // Forward
            //player.AddForce((transform.forward * (MOVESPEED * FORCEMULTI * Time.deltaTime)) + player.position);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.MovePosition((-transform.forward * (MOVESPEED * Time.deltaTime)) + player.position); // Backward
            //player.AddForce((-transform.forward * (MOVESPEED * FORCEMULTI * Time.deltaTime)) + player.position);
        }

        //Vector3 eulerRot = new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * ROTSPEED;
        //Quaternion deltaRotation = Quaternion.Euler(eulerRot);

        //player.MoveRotation(player.rotation * deltaRotation);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * ROTSPEED);
        player.velocity = new Vector3(0f, 0f, 0f);        
    }
}
