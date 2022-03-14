using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Spawner spawner;
    public Vector3 position = new Vector3();
    private Vector3 velocity = new Vector3();
    private Vector3 force = new Vector3();
    private float mass = 1;
    private float maxForce = 1;

    public Vector3 target = new Vector3();
    public GameObject targetObject;
    private float targetAngle = 0;
    private float targetRadius = 10;
    private float targetSpeed = 0;
    private float maxSpeed;
    private float size;
    Renderer rend;

    private int cooldown = 0;
    public bool hasFood = false;
    public bool randomPos = true;
    public bool targeted = false;

    void Start()
    {
        if(randomPos) position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        velocity = new Vector3(0, 0, 0);
        mass = Random.Range(100, 200);
        maxForce = Random.Range(5, 10);
        targetAngle = Random.Range(-Mathf.PI, Mathf.PI);
        targetRadius = Random.Range(50, 150);
        targetSpeed = maxForce * 5;
        maxSpeed = ((float)Random.Range(100, 120));
        size = Random.Range(3, 6);
        rend = GetComponent<Renderer>();
    }

    void Update()
    {

        if (hasFood) rend.material.color = Color.magenta;
        else rend.material.color = Color.blue;

        if (--cooldown <= 0)
        {
            target = findTarget();

            cooldown = (int)Random.Range(20, 40);
        }

        targetAngle += targetSpeed;


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

        force +=  steeringForce;
    }

    void doEuler()
    {
        //euler integration:
        Vector3 acceleration = force / mass;
        velocity +=  acceleration;
        position += Time.deltaTime *  velocity;
        force *= 0;
    }

    Vector3 findTarget()
    {
        float smallestDist = 999999999;
        int pos = 0;
        bool otherHasFood = false;

        Vector3 newTarget = new Vector3(0, 0, 0);
        bool danger = false;
        int counter = 0;

        for (int e = 0; e < spawner.bigFishes.Count; e++)
        {

            float dist = (spawner.bigFishes[e].transform.position - position).magnitude;

            if (dist < 30)
            {
                if (!danger) newTarget = (spawner.bigFishes[e].transform.position - position) * -1;
                else newTarget += (spawner.bigFishes[e].transform.position - position) * -1;
                counter++;
                danger = true;
            }
        }

        newTarget = newTarget / counter;
        if (danger)
        {
            if (targetObject != null)
            {
                targetObject.GetComponent<Food>().targeted = false;
                targetObject = null;
            }

            return newTarget;
        }
        else if (hasFood)
        {
            for (int i = 0; i < spawner.agents.Count; i++)
            {
                if (spawner.agents[i].GetComponent<Agent>().hasFood)
                {
                    if (spawner.agents[i] == this.gameObject) continue;

                    float dist = (spawner.agents[i].transform.position - position).magnitude;

                    if (dist < smallestDist)
                    {
                        smallestDist = dist;
                        pos = i;
                        otherHasFood = true;
                    }
                }
            }
            if (otherHasFood) return spawner.agents[pos].transform.position;
            else return new Vector3(-100, 0, 0);
        }
        else if (spawner.foods.Count > 0)
        {
            for (int i = 0; i < spawner.foods.Count; i++)
            {
                if (spawner.foods[i].GetComponent<Food>().targeted) continue;

                float dist = (spawner.foods[i].transform.position - position).magnitude;

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
                    targetObject.GetComponent<Food>().targeted = false;
                    targetObject = spawner.foods[pos];
                    targetObject.GetComponent<Food>().targeted = true;
                }
            }
            else
            {
                targetObject = spawner.foods[pos];
                targetObject.GetComponent<Food>().targeted = true;
            }
            return targetObject.transform.position;
        }
        else return new Vector3(0, 0, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Agent(Clone)")
        {

            if (collision.gameObject.GetComponent<Agent>().hasFood && hasFood)
            {
                collision.gameObject.GetComponent<Agent>().hasFood = false;
                hasFood = false;

                GameObject a = Instantiate(spawner.agentPrefab);
                a.GetComponent<Agent>().spawner = spawner;
                a.GetComponent<Agent>().position = position;
                a.GetComponent<Agent>().randomPos = false;
                spawner.agents.Add(a);
                

            }
        }
    }
}
