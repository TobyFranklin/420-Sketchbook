using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonController : MonoBehaviour
{
    public Transform moveTarget;

    private List<Pathfinder.Node> pathToTarget = new List<Pathfinder.Node>();

    private bool shouldCheckAgain = true;

    private float checkAgainIn = 0;

    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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

        transform.position = Vector3.Lerp(transform.position, target, .005f);

        float d = (target - transform.position).magnitude;

        if(d < .25f){
            shouldCheckAgain = true;
        }
    }

    private void FindPath()
    {
        shouldCheckAgain = false;

        if (moveTarget && GridController.singleton)
        {
            Pathfinder.Node start = GridController.singleton.Lookup(transform.position);
            Pathfinder.Node end = GridController.singleton.Lookup(moveTarget.position);

            if (start == null || end == null || start == end) {
                pathToTarget.Clear();
                return;
            }

            pathToTarget = Pathfinder.Solve(start, end);

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
}
