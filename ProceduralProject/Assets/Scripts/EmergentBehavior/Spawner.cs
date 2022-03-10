using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject agentPrefab;
    public GameObject bigFishprefab;

    private GameObject b;
    private List<GameObject> agents = new List<GameObject>();

    void Start()
    {
        b = Instantiate(bigFishprefab);
    }

    void Update()
    {
        
            if(agents.Count < 1)
        {
            GameObject a = Instantiate(agentPrefab);
            a.GetComponent<agent>().bigFish = b;
            agents.Add(a);
        }
    }
}
