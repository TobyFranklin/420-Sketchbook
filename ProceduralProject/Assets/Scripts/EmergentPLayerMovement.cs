using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergentPLayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Spawner spawner;

    public float speed = 12f;
    public float jumpHeight = 20f;

    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump")) velocity.y = jumpHeight;
        else if (Input.GetKey(KeyCode.C)) velocity.y = -jumpHeight;
        else velocity.y = 0;

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject a = Instantiate(spawner.agentPrefab);
            a.GetComponent<Agent>().spawner = spawner;
            a.GetComponent<Agent>().position = transform.position + transform.forward * 50; 
            a.GetComponent<Agent>().randomPos = false;
            spawner.agents.Add(a);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            GameObject b = Instantiate(spawner.bigFishprefab);
            b.GetComponent<Bigfish>().spawner = spawner;
            b.GetComponent<Bigfish>().position = transform.position + transform.forward * 50;
            b.GetComponent<Bigfish>().randomPos = false;
            spawner.bigFishes.Add(b);
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            GameObject f = Instantiate(spawner.foodPrefab);
            f.GetComponent<Food>().spawner = spawner;
            f.GetComponent<Food>().position = transform.position + transform.forward * 50;
            f.GetComponent<Food>().randomPos = false;
            spawner.foods.Add(f);
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
