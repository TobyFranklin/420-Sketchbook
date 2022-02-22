using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent : MonoBehaviour
{
    public GameObject big;
    private Vector3 position = new Vector3();
    private Vector3 velocity = new Vector3();
    private Vector3 force = new Vector3();
    private float mass = 1;
    private float maxSpeed = 10;
    private float maxForce = 10;

    private Vector3 target = new Vector3();
    private float targetAngle = 0;
    private float targetRadius = 100;
    private float targetSpeed = 0;

    void OnStart()
    {
        position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        mass = Random.Range(50, 100);
        maxForce = Random.Range(5, 15);
        targetAngle = Random.Range(-Mathf.PI, Mathf.PI);
        targetRadius = Random.Range(50, 150);
        maxSpeed = Random.Range(2, 15);
        targetSpeed = maxForce * (float).05;
    }

    void update()
    {

        target = big.transform.position;

        targetAngle += targetSpeed;
        target.x += (targetRadius * Mathf.Cos(targetAngle));
        target.y += (targetRadius * Mathf.Sin(targetAngle));

        doSteeringForce();
        doEuler();
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
