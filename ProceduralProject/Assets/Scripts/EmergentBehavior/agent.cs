using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent : MonoBehaviour
{
    public GameObject bigFish;
    private Vector3 position = new Vector3();
    private Vector3 velocity = new Vector3();
    private Vector3 force = new Vector3();
    private float mass = 1;
    private float maxForce = 1;

    private Vector3 target = new Vector3();
    private float targetAngle = 0;
    private float targetRadius = 10;
    private float targetSpeed = 0;
    private float maxSpeed;
    void Start()
    {
        position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        mass = Random.Range(10, 20);
        maxForce = Random.Range(5, 10);
        targetAngle = Random.Range(-Mathf.PI, Mathf.PI);
        targetRadius = Random.Range(50, 150);
        targetSpeed = maxForce * 5;
        maxSpeed = .1f;
    }

    void Update()
    {
        target = bigFish.transform.position;

        targetAngle += targetSpeed;


        doSteeringForce();
        doEuler();

        transform.position = position;

        transform.rotation = Quaternion.LookRotation(velocity);
    }

    void doSteeringForce()
    {

        //find desired velocity
        //desired velocity = clamp(target position - current position)

        Vector3 desiredVelocity = target - position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // find steering force
        // steering force = desired velocity

        Vector3 steeringForce = desiredVelocity - velocity;
        steeringForce.Normalize();


        steeringForce *= maxSpeed;
        Debug.Log(steeringForce);

        force += steeringForce;
    }

    void doEuler()
    {
        //euler integration:
        Vector3 acceleration = force /mass;
        velocity += acceleration;
        position += velocity;
        force *=0 ;
    }
}
