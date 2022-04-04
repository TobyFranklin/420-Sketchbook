using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCam : MonoBehaviour
{

    public static OverheadCam instance;

    public Camera cam;
    private void Awake()
    {
        instance = this;
    }

    public float speed = 12f;
    public float jumpHeight = 20f;

    Vector3 velocity;
    bool isGrounded;


    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(1, 0, 0) * x + new Vector3(0, 0, 1) * z;

        transform.position +=(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump")) velocity.y = jumpHeight;
        else if (Input.GetKey(KeyCode.C)) velocity.y = -jumpHeight;
        else velocity.y = 0;

        transform.position += (velocity * Time.deltaTime);
    }
}
