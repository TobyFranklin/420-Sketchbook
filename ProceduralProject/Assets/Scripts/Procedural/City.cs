using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public Building buildingPrefab;
    public GameObject roadPrefab;
    public GameObject groundPrefab;
    public GameObject carPrefab;

    public GameObject[] benchPrefabs;

    public GameObject[] streetLightPrefabs;
    public GameObject[] hydrantPrefabs;
    public GameObject[] trashPrefabs;


    [Range(2, 5)]
    public int buildingMinHeight;

    [Range(4, 10)]
    public int buildingMaxHeight;


    public Plant plantPrefab;


    [Range(10, 40)]
    public int cityWidth = 20;

    [Range(10, 40)]
    public int cityLength = 20;

    private int blockWidth = 2;

    private int blockLength = 2;

    private int[,] city;

    private void Start()
    {
        city = new int[cityWidth, cityLength];
        GenerateCity();
    }

    private void GenerateCity()
    {
        for (int x = 0; x < cityWidth; x++)
        {
            for (int z = 0; z < cityLength; z++)
            {
                if (city[x, z] == 0) SetBlock(x, z);

            }
        }

        for (int x = 0; x < cityWidth; x++)
        {
            for (int z = 0; z < cityLength; z++)
            {
                buildCity(x,z);
            }
        }

    }
    private void SetBlock(int xPos, int zPos) {

        int blockType = Random.Range(1, 3);

        blockWidth = 3;
        blockLength = 3;

        for (int x = xPos - 1; x < xPos + blockWidth + 1; x++) {
            for (int z = zPos - 1; z < zPos + blockLength + 1; z++) {

                if (x > cityWidth && z > cityLength && x < 0 && z < 0) continue;
                SetType(x, z, blockType);

                if (x == xPos + blockWidth || z == zPos + blockLength || x == xPos - 1 || z == zPos - 1) SetType(x, z, 5);

                Vector3 pos = new Vector3(x, 0, z);

            }
        }

    }

    private void buildCity(int x, int z){
        Vector3 pos = new Vector3(x, 0, z);

        switch (GetType(x, z))
        {
            case 1:
                Building b = buildingPrefab;
                b.Build(buildingMinHeight, buildingMaxHeight);

                Vector3 offset = new Vector3(0, 0, 0);
                Quaternion rotOffset = Quaternion.Euler(0, 0, 0);

                ///ROTATIoN STUFF
               /// if (x == xPos) { rotOffset = Quaternion.Euler(0, 180, 0); offset = new Vector3(-1, 0, -1); }
               /// if (z == zPos) { rotOffset = Quaternion.Euler(0, 90, 0); offset = new Vector3(0, 0, -1); }
               /// if (z == zPos + blockLength - 1) { rotOffset = Quaternion.Euler(0, -90, 0); offset = new Vector3(-1, 0, 0); }

                Instantiate(b, pos + offset, Quaternion.identity * rotOffset);

                break;

            case 2:
                Instantiate(groundPrefab, pos - new Vector3(.5f, 0, .5f), Quaternion.identity);
                int random = Random.Range(1, 101);

                if (random < 40)
                {
                    Plant plant = plantPrefab;
                    plant.Build();

                    Instantiate(plant, pos - new Vector3(.5f, -.15f, .5f), Quaternion.identity);
                }
                else if (random < 60)
                {
                    GameObject clone = benchPrefabs[Random.Range(0, benchPrefabs.Length)];
                    Instantiate(clone, pos - new Vector3(.5f, -.15f, .5f), Quaternion.Euler(0, Random.Range(0, 361), 0));
                }
                break;

            case 5:
                Instantiate(roadPrefab, pos - new Vector3(.5f, 0, .5f), Quaternion.identity);
                int rand = Random.Range(1, 101);
                if (rand < 20)
                {
                    int turn = 0;
                    ///ROTATIoN STUFF   if (x == xPos + blockWidth) turn = 90;
                    Instantiate(carPrefab, pos - new Vector3(.5f, -.08f, .5f), Quaternion.Euler(-90, (Random.Range(0, 2) * 180) + turn, 0));
                }
                else if (rand < 40)
                {
                    GameObject clone = streetLightPrefabs[Random.Range(0, streetLightPrefabs.Length)];

                    Instantiate(clone, pos - new Vector3(.5f, -.01f, .1f), Quaternion.Euler(0, 180, 0));


                    ///ROTATIoN STUFF
                    /*
                    if (x == xPos + blockWidth && z == zPos + blockLength)
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.9f, -.01f, .9f), Quaternion.Euler(0, 135, 0));

                    }
                    else if (x == xPos + blockWidth)
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.9f, -.01f, .5f), Quaternion.Euler(0, 90, 0));
                        else Instantiate(clone, pos - new Vector3(.1f, -.01f, .5f), Quaternion.Euler(0, 270, 0));
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.5f, -.01f, .9f), Quaternion.Euler(0, 0, 0));
                        else Instantiate(clone, pos - new Vector3(.5f, -.01f, .1f), Quaternion.Euler(0, 180, 0));
                    }
                    */

                }
                else if (rand < 50)
                {
                    GameObject clone = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

                    Instantiate(clone, pos - new Vector3(.5f, -.01f, .1f), Quaternion.Euler(0, 180, 0));

                    ///ROTATIoN STUFF
                    /*
                    if (x == xPos + blockWidth && z == zPos + blockLength)
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.9f, -.01f, .9f), Quaternion.Euler(0, 135, 0));

                    }
                    else if (x == xPos + blockWidth)
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.9f, -.01f, .5f), Quaternion.Euler(0, 90, 0));
                        else Instantiate(clone, pos - new Vector3(.1f, -.01f, .5f), Quaternion.Euler(0, 270, 0));
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.5f, -.01f, .9f), Quaternion.Euler(0, 0, 0));
                        else Instantiate(clone, pos - new Vector3(.5f, -.01f, .1f), Quaternion.Euler(0, 180, 0));
                    }
                    */

                }
                else if (rand < 60)
                {
                    GameObject clone = hydrantPrefabs[Random.Range(0, hydrantPrefabs.Length)];
                    Instantiate(clone, pos - new Vector3(.5f, -.01f, .1f), Quaternion.Euler(0, 180, 0));

                    /*
                    if (x == xPos + blockWidth && z == zPos + blockLength)
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.9f, -.01f, .9f), Quaternion.Euler(0, 135, 0));

                    }
                    else if (x == xPos + blockWidth)
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.9f, -.01f, .5f), Quaternion.Euler(0, 90, 0));
                        else Instantiate(clone, pos - new Vector3(.1f, -.01f, .5f), Quaternion.Euler(0, 270, 0));
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 1) Instantiate(clone, pos - new Vector3(.5f, -.01f, .9f), Quaternion.Euler(0, 0, 0));
                        else Instantiate(clone, pos - new Vector3(.5f, -.01f, .1f), Quaternion.Euler(0, 180, 0));
                    }
                    */

                }

                break;
        }
    }
    private void SetType(int xPos, int zPos, int type)
    {
        if (xPos < cityWidth && zPos < cityLength && xPos > 0 && zPos > 0) city[xPos, zPos] = type;
    }

    private int GetType(int xPos, int zPos)
    {
        if (xPos < cityWidth && zPos < cityLength && xPos > 0 && zPos > 0) return city[xPos, zPos];
        return -1;
    }


}
