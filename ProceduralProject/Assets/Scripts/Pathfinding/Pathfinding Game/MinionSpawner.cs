using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public enum teamColor
    {
        blue,
        red,
    }

    public teamColor team;

    public GameObject minionPrefab;

    private float waveInterval = 1;

    private float minionInterval = 1;

    private int[] minionTypes = new int[] { 1, 0, 1, 0 };

    private int nextMinion = 0;

    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

        if (team == teamColor.blue) rend.material.color = Color.blue;

        if (team == teamColor.red) rend.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

        waveInterval -= Time.deltaTime;
        if (waveInterval <= 0)
        {
            minionInterval -= Time.deltaTime;
            if(minionInterval <= 0)
            {
                spawnWave();
                minionInterval = 10;
            }
        }
    }

    void spawnWave()
    {
        // Debug.Log("SPawn");

        Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (team == teamColor.red) offset.x +=2;
        else if (team == teamColor.blue) offset.x -= 2;

        GameObject minion = Instantiate(minionPrefab, offset, Quaternion.identity);
        minion.GetComponent<Minion>().makeStats(minionTypes[nextMinion], (int)team);


        if (team == teamColor.red)Map.singleton.redMinions.Add(minion);
        else if (team == teamColor.blue) Map.singleton.blueMinions.Add(minion);

        nextMinion += 1;
        if (nextMinion >= minionTypes.Length)
        {
            waveInterval = 10;
            nextMinion = 0;
        }
    }

}

