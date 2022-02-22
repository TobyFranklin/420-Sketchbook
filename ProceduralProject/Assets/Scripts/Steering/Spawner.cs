using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject agentPrefab;
    public GameObject bigFishprefab;
    private GameObject b;

    void Start()
    {
        GameObject b = bigFishprefab;
        Instantiate(b);
    }

    void update()
    {
        
            agent a = new agent();
            a.big = b;
            Instantiate(a);
        
    }
}
