using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roamer : MonoBehaviour
{
    public Transform moveTarget;

    private List<GuyPathfinder.Node> pathToTarget = new List<GuyPathfinder.Node>();

    private bool shouldCheckAgain = true;

    private float checkAgainIn = 0;

    private LineRenderer line;

    private Material mat;

    SimpleViz2 viz2;
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {   
        viz2 = SimpleViz2.viz2;
        body = GetComponent<Rigidbody>();
        mat = GetComponent<MeshRenderer>().material;
        line = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        viz2 = SimpleViz2.viz2;
        GetComponent<MeshRenderer>().material.SetFloat("_MusicChange", viz2.avgAmp);
        if(moveTarget == null){
        moveTarget = Maze.singleton.RandomSpot();
        }

        checkAgainIn -= Time.deltaTime;
        if(checkAgainIn <= 0){
            shouldCheckAgain = true;
            checkAgainIn = 1;
        }

        if(shouldCheckAgain) FindPath();
        MoveAlongPath();
    }

    private void MoveAlongPath(){
        if (pathToTarget == null) return;
        if (pathToTarget.Count < 2) return;

        // grab first item in path and move to that node 

        Vector3 target = pathToTarget[1].position;
        target.y += 1;

        transform.position = Vector3.Lerp(transform.position, target, .02f);

        float d = (target - transform.position).magnitude;

        if(d < .25f){
            shouldCheckAgain = true;
        }
    }

    private void FindPath()
    {
        shouldCheckAgain = false;

        if (moveTarget && Maze.singleton)
        {
            GuyPathfinder.Node start = Maze.singleton.Lookup(transform.position);
            GuyPathfinder.Node end = Maze.singleton.Lookup(moveTarget.position);

            if (start == null || end == null || start == end) {
                pathToTarget.Clear();
                moveTarget = null;
                return;
            }

            pathToTarget = GuyPathfinder.Solve(start, end);

            //?
            //Rendering the path on a LineRenderer
            Vector3[] positions = new Vector3[pathToTarget.Count];

            for (int i = 0; i < pathToTarget.Count; i++)
            {
                positions[i] = pathToTarget[i].position + new Vector3(0, .5f, 0);
            }
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MPlayer>())
        {
            Application.Quit();

        }
    }
}
