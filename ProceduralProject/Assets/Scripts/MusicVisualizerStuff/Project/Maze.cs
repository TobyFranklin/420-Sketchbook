using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Maze : MonoBehaviour
{
    delegate GuyPathfinder.Node LookupDelegate(int x, int y);

    public static Maze singleton{ get; private set; }

    public int size = 20;
    public MazeWall cubePrefab;

    public Transform helperStart;
    public Transform helperEnd;


    private MazeWall[,] cubes;

    private GuyPathfinder.Node[,] nodes;

    void Start()
    {
        if (singleton != null){
            Destroy(gameObject);
            return;
        }

        singleton = this;
        //DontDestroyOnLoad(gameObject);

        MakeGrid();
    }

    private void OnDestroy(){
        if (this == singleton) singleton = null;
    }

    void MakeGrid()
    {
        cubes = new MazeWall[size, size];

        float zoom = 10;
        float amp = 10;

        for(int x = 0; x < size; x++){
            for (int y = 0; y < size; y++)
            {
               // float verticalPosition = Mathf.PerlinNoise(x/zoom, y/zoom) * amp;

               cubes [x,y] = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
    public void MakeNodes()
    {

        nodes = new GuyPathfinder.Node[cubes.GetLength(0), cubes.GetLength(1)];

        for (int x = 0; x < cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubes.GetLength(1); y++)
            {
                GuyPathfinder.Node n = new GuyPathfinder.Node();

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
                GuyPathfinder.Node n = nodes[x, y];

                GuyPathfinder.Node neighbor1 = lookup(x + 1, y);
                GuyPathfinder.Node neighbor2 = lookup(x - 1, y);
                GuyPathfinder.Node neighbor3 = lookup(x, y + 1);
                GuyPathfinder.Node neighbor4 = lookup(x, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);

            }
        }
        //making a path through the dungeon
        //GuyPathfinder.Node start = Lookup(helperStart.position);

        //GuyPathfinder.Node end = Lookup(helperEnd.position);

        //List<GuyPathfinder.Node> path = GuyPathfinder.Solve(start, end);

    }

    public GuyPathfinder.Node Lookup(Vector3 pos){

        if (nodes == null) MakeNodes();

        float w = 1;
        float h = 1;

        pos.x += w / 2;
        pos.z += h / 2;

        int x = (int)(pos.x / w);
        int y = (int)(pos.z / h);

        if (x < 0) return null;
        if (y < 0) return null;
        if (x >= nodes.GetLength(0)) return null;
        if (y >= nodes.GetLength(1)) return null;
        return nodes[x, y];
    }

    public Transform RandomSpot(){
        bool openSpot = false;
        while(!openSpot){
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);

            if(Lookup(new Vector3(x, 0, y)) != null){
                if(cubes[x,y] != null && cubes[x,y].MoveCost < 100){
                    openSpot = true;
                    return cubes[x,y].transform;
               }
            }
            
         }
         return null;
    }
}


[CustomEditor(typeof(Maze))]
class MazeEditor : Editor{

    public override void OnInspectorGUI()
    {
       base.OnInspectorGUI();

        if(GUILayout.Button("find a path")) {
            (target as Maze).MakeNodes();
        }
    }
}
