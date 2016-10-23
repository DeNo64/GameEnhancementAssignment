using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    public GameObject player;
    public float camLocBehind;
    public float camLocAbove;

    // Use this for initialization
    void Start()
    {
        transform.LookAt(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        updateCam();
    }

    void LateUpdate()
    {
        transform.LookAt(player.transform);
        transform.localRotation = Quaternion.Euler(21.907f, 0f, 0f);
    }

    void updateCam()
    {
        transform.position = player.transform.position/15;
        transform.rotation = player.transform.rotation;
        transform.Translate(Vector3.back * camLocBehind/15);
        transform.Translate(Vector3.up * camLocAbove/15);
        transform.LookAt(player.transform);
    }
}
