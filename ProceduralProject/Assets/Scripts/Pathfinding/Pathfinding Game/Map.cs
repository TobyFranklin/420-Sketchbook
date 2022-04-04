using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : MonoBehaviour
{
    delegate MinionPathfinder.Node LookupDelegate(int x, int y);

    public static Map singleton { get; private set; }

    public Ground cubePrefab;

    public int sizeX = 10;
    public int sizeY = 10;

    private Ground[,] cubes;

    private MinionPathfinder.Node[,] nodes;

    public List<GameObject> redTowers = new List<GameObject>();

    public List<GameObject> blueTowers = new List<GameObject>();

    public List<GameObject> redMinions = new List<GameObject>();

    public List<GameObject> blueMinions = new List<GameObject>();



    void Start()
    {

        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        //DontDestroyOnLoad(gameObject);

        MakeGrid();
    }

    private void OnDestroy()
    {
        if (this == singleton) singleton = null;
    }

    private void Update()
    {
    }
    void MakeGrid()
    {
        cubes = new Ground[sizeX, sizeY];

        //float zoom = 10;
        //float amp = 10;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                float verticalPosition = 0;//Mathf.PerlinNoise(x / zoom, y / zoom) * amp;

                cubes[x, y] = Instantiate(cubePrefab, new Vector3(x, verticalPosition, y), Quaternion.identity);
            }
        }
    }

    public void MakeNodes()
    {

        nodes = new MinionPathfinder.Node[cubes.GetLength(0), cubes.GetLength(1)];

        for (int x = 0; x < cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubes.GetLength(1); y++)
            {
                MinionPathfinder.Node n = new MinionPathfinder.Node();

                n.position = cubes[x, y].transform.position;
                n.moveCost = cubes[x, y].MoveCost;

                nodes[x, y] = n;
            }
        }

        LookupDelegate lookup = (x, y) => {
            if (x < 0) return null;
            if (y < 0) return null;
            if (x >= nodes.GetLength(0)) return null;
            if (y >= nodes.GetLength(1)) return null;

            return nodes[x, y];
        };

        for (int x = 0; x < cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubes.GetLength(1); y++)
            {
                MinionPathfinder.Node n = nodes[x, y];

                MinionPathfinder.Node neighbor1 = lookup(x + 1, y);
                MinionPathfinder.Node neighbor2 = lookup(x - 1, y);
                MinionPathfinder.Node neighbor3 = lookup(x, y + 1);
                MinionPathfinder.Node neighbor4 = lookup(x, y - 1);

                //MinionPathfinder.Node neighbor5 = lookup(x + 1, y + 1);
                //MinionPathfinder.Node neighbor6 = lookup(x - 1, y + 1);
                //MinionPathfinder.Node neighbor7 = lookup(x - 1, y - 1);
                //MinionPathfinder.Node neighbor8 = lookup(x + 1, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);

                //if (neighbor5 != null) n.neighbors.Add(neighbor5);
                //if (neighbor6 != null) n.neighbors.Add(neighbor6);
                //if (neighbor7 != null) n.neighbors.Add(neighbor7);
                //if (neighbor8 != null) n.neighbors.Add(neighbor8);

            }
        }
        //making a path through the dungeon
    }

    public MinionPathfinder.Node Lookup(Vector3 pos)
    {

        if (nodes == null) MakeNodes();

        float w = 1;
        float l = 1;

        pos.x += w / 2;
        pos.z += l / 2;

        int x = (int)(pos.x / w);
        int y = (int)(pos.z / l);

        if (x < 0) return null;
        if (y < 0) return null;
        if (x >= nodes.GetLength(0)) return null;
        if (y >= nodes.GetLength(1)) return null;
        return nodes[x, y];
    }
}

[CustomEditor(typeof(Map))]
class MapEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("find a path"))
        {
            (target as Map).MakeNodes();
        }
    }
}
