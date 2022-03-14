using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject agentPrefab;
    public GameObject bigFishprefab;
    public GameObject foodPrefab;

    public List<GameObject> bigFishes = new List<GameObject>();
    public List<GameObject> agents = new List<GameObject>();
    public List<GameObject> foods = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {

        if (bigFishes.Count < 2)
        {
            GameObject b = Instantiate(bigFishprefab);
            b.GetComponent<Bigfish>().spawner = this;
            bigFishes.Add(b);
        }

        if (agents.Count < 5)
        {
            GameObject a = Instantiate(agentPrefab);
            a.GetComponent<Agent>().spawner = this;
            agents.Add(a);
        }

        if (foods.Count < 5)
        {
            GameObject f = Instantiate(foodPrefab);
            f.GetComponent<Food>().spawner = this;
            foods.Add(f);
        }
    }
}
