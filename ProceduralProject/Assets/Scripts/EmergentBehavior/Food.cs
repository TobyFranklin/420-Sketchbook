using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Vector3 position = new Vector3();
    public Spawner spawner;
    public bool randomPos = true;
    public bool targeted = false;
    // Start is called before the first frame update
    void Start()
    {
       if(randomPos) position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Agent(Clone)" && collision.gameObject.GetComponent<Agent>().hasFood == false)
        {
            collision.gameObject.GetComponent<Agent>().hasFood = true;
            spawner.foods.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
