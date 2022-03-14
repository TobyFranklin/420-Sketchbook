using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bigfish : MonoBehaviour
{
    public Spawner spawner;
    public Vector3 position;
    private Vector3 velocity = new Vector3();
    private Vector3 force = new Vector3();
    private float mass = 1;
    public GameObject targetObject;
    private Vector3 target = new Vector3();
    private int cooldown = 0;
    private float maxSpeed;
    public float splitSize = 15;
    public float size;
    public bool randomPos = true;
    public bool hasFood = false;
    public bool targeted = false;

    private void Start()
    {
        if (randomPos) position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        velocity = new Vector3(0, 0, 0);
        mass = Random.Range(40, 60);
        maxSpeed = ((float)Random.Range(60, 80));
        size = Random.Range(5, 6);
    }

    void Update()
    {
        if (size > splitSize) hasFood = true;

        if (--cooldown <= 0)
        {
            target = findTarget();

            cooldown = (int)Random.Range(90, 120);
        }



        doSteeringForce();
        doEuler();

        transform.position = position;

        transform.rotation = Quaternion.LookRotation(velocity);

        transform.localScale = new Vector3(size, size, size);
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
        Vector3 acceleration = force / mass / (size / splitSize);
        velocity += acceleration;
        position += velocity * Time.deltaTime;
        force *= 0;
    }

    Vector3 findTarget()
    {
        float smallestDist = 999999999;
        int pos = 0;
        bool otherHasFood = false;

        if (hasFood) {

            for (int i = 0; i < spawner.bigFishes.Count; i++)
            {
                if (spawner.bigFishes[i].GetComponent<Bigfish>().hasFood)
                {
                    if (spawner.bigFishes[i] == this.gameObject) continue;

                    float dist = (spawner.bigFishes[i].transform.position - position).magnitude;

                    if (dist < smallestDist)
                    {
                        smallestDist = dist;
                        pos = i;
                        otherHasFood = true;
                    }
                }
            }
            if (otherHasFood) return spawner.bigFishes[pos].transform.position;
            else return new Vector3(100, 0, 0);
        }
        else if (spawner.agents.Count > 0)
        {
            for (int i = 0; i < spawner.agents.Count; i++)
            {
                if (spawner.agents[i].GetComponent<Agent>().targeted) continue;

                bool skip = false;

                for (int v = 0; v < spawner.agents.Count; v++)
                {
                    float otherDist = (spawner.agents[i].transform.position - spawner.agents[v].transform.position).magnitude;

                    if (otherDist < 30 && spawner.agents[v].GetComponent<Agent>().targeted) skip = true;
                }

                if (skip) continue;


                float dist = (spawner.agents[i].transform.position - position).magnitude;

                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    pos = i;
                }
            }

            if (targetObject != null)
            {
                if (smallestDist < (targetObject.transform.position - position).magnitude)
                {
                    targetObject.GetComponent<Agent>().targeted = false;
                    targetObject = spawner.agents[pos];
                    targetObject.GetComponent<Agent>().targeted = true;
                }
            }
            else
            {
                targetObject = spawner.agents[pos];
                targetObject.GetComponent<Agent>().targeted = true;
            }
            return targetObject.transform.position;

        }
        else return new Vector3(0, 0, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Agent(Clone)")
        {
            size += 1;

                 spawner.agents.Remove(collision.gameObject);
                 Destroy(collision.gameObject);

        }

        if (collision.gameObject.name == "BigFish(Clone)")
        {

            if (collision.gameObject.GetComponent<Bigfish>().hasFood && hasFood)
            {
                collision.gameObject.GetComponent<Bigfish>().hasFood = false;
                hasFood = false;


                GameObject b = Instantiate(spawner.bigFishprefab);
                b.GetComponent<Bigfish>().size = Random.Range(3, 4) + (size/2 + collision.gameObject.GetComponent<Bigfish>().size/2)/4;
                b.GetComponent<Bigfish>().spawner = spawner;
                b.GetComponent<Bigfish>().position = position;
                b.GetComponent<Bigfish>().randomPos = false;
                spawner.bigFishes.Add(b);

                size = size/2;
                collision.gameObject.GetComponent<Bigfish>().size = collision.gameObject.GetComponent<Bigfish>().size/ 2;
            }
        }

    }
}

