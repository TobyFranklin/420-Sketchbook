using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum teamColor
    {
        blue,
        red,
    }

    public teamColor team;
    public float damage = 999;

    public Vector3 position = new Vector3();
    private Vector3 velocity = new Vector3();
    private Vector3 force = new Vector3();
    private float mass = 1;
    private float maxForce = 1;

    public Transform target;
    private float targetAngle = 0;
    private float targetSpeed = 0;
    private float maxSpeed;
    private float size;
    Renderer rend;


    void Start()
    {
        velocity = new Vector3(0, 0, 0);
        mass = Random.Range(100, 200);
        maxForce = 5;
        targetAngle = Random.Range(-Mathf.PI, Mathf.PI);
        targetSpeed = maxForce * 5;
    }

    void Update()
    {
        if(target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        targetAngle += targetSpeed;


        doSteeringForce();
        doEuler();

        transform.position = position;

        transform.rotation = Quaternion.LookRotation(velocity);
        transform.localScale = new Vector3(size, size, size);
    }

    public void MakeStats(Transform newTarget, float teams, float big, float speed, Vector3 loc, float dmg)
    {
        damage = dmg;

        position = loc;

        target = newTarget;

        team = (teamColor)teams;

        rend = GetComponent<Renderer>();

        if (team == teamColor.blue) rend.material.color = Color.blue;

        if (team == teamColor.red) rend.material.color = Color.red;

        size = big;

        maxSpeed = speed;
    }

    void doSteeringForce()
    {

        //find desired velocity
        //desired velocity = clamp(target position - current position)

        Vector3 desiredVelocity = target.position - position;
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
        Vector3 acceleration = force / mass;
        velocity += acceleration;
        position += Time.deltaTime * velocity;
        force *= 0;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent == target) {
            if (collision.transform.parent.GetComponent<Minion>())
            {
                collision.transform.parent.GetComponent<Minion>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
            else if (collision.transform.parent.GetComponent<Player>())
            {
                collision.transform.parent.GetComponent<Player>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
            else if (collision.transform.parent.GetComponent<Tower>())
            {
                collision.transform.parent.GetComponent<Tower>().TakeDamage(damage);
                Destroy(this.gameObject);
            }

        }
    }
}
