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

    }

    void update()
    {

        if (--cooldown <= 0)
        {
            target1 = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));

            cooldown = (int)Random.Range(30, 60);
        }

        target2.x += (target1.x - target2.x) * (float).01;
        target2.y += (target1.y - target2.y) * (float).01;
        target2.z += (target1.z - target2.z) * (float).01;

        position.x += (target2.x - position.x) * (float).01;
        position.y += (target2.y - position.y) * (float).01;
        position.z += (target2.z - position.z) * (float).01;
    }
}
