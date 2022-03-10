using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bigfish : MonoBehaviour
{
    public Vector3 position;
    private Vector3 target1 = new Vector3();
    private Vector3 target2 = new Vector3();
    private int cooldown = 0;
    private void Start()
    {
        position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        transform.position = position;
    }

    void Update()
    {

        if (--cooldown <= 0)
        {
            target1 = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));

            cooldown = (int)Random.Range(60, 120);
        }

        target2 += (target1 - target2) * (float).01;

        position += (target2- position) * (float).01;


        transform.position = position;
    }
}
