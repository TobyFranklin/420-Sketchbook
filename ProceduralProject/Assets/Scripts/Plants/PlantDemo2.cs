using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BranchingType
{
    Random,
    Opposite,
    Alternate180,
    Alternate1375,
    WhorledTwo,
    WhorledThree,
}

public class InstanceCollection2{

    public List<CombineInstance> branchInstances = new List<CombineInstance>();
    public List<CombineInstance> leafInstances = new List<CombineInstance>();

    public void AddBranch(Mesh mesh, Matrix4x4 xform){
        branchInstances.Add(new CombineInstance() { mesh = mesh, transform =xform} );
    }
    public void AddLeaf(Mesh mesh, Matrix4x4 xform)
    {
        leafInstances.Add(new CombineInstance() { mesh = mesh, transform = xform });
    }
    public Mesh MakeMultiMesh() {

        Mesh branchMesh = new Mesh();
        branchMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        branchMesh.CombineMeshes(branchInstances.ToArray());

        Mesh leafMesh = new Mesh();
        leafMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        leafMesh.CombineMeshes(leafInstances.ToArray());

        Mesh finalMesh = new Mesh();
        finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        finalMesh.CombineMeshes(new CombineInstance[] {
            new CombineInstance(){ mesh = branchMesh, transform = Matrix4x4.identity},
            new CombineInstance(){ mesh = leafMesh, transform = Matrix4x4.identity},
        }, false);
        return finalMesh;
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlantDemo2 : MonoBehaviour
{

    [Range(0, 100000000)]
    public int seed = 0;

    [Range(2, 25)]
    public int iterations = 5;

    [Range(0, 45)]
    public float turnDegrees = 10;

    [Range(-90, 90)]
    public float twistDegrees = 10;

    [Range(0, 1)]
    public float alignWithParent;

    [Range(1, 10)]
    public int branchNodeDis = 2;

    [Range(1, 10)]
    public int branchNodeTrunk = 1;

    public BranchingType branchType;

    private System.Random randGenerator;

    private float Rand()
    {
        return (float) randGenerator.NextDouble();
    }

    private float Rand(float min, float max)
    {
        return Rand() * (max - min) + min;
    }
    private float RandBell(float min, float max)
    {
        min /= 2;
        max /= 2;

        return Rand(min, max) + Rand(min, max);
    }
    private bool RandBool()
    {
        return (Rand() >= .5f);
    }

    // Start is called before the first frame updates
    void Start()
    {
        Build();
    }

    private void OnValidate()
    {
        Build();
    }
    // Update is called once per frame
    void Build()
    {

        randGenerator = new System.Random(seed);

        // 1. making storage for instances
        InstanceCollection2 instances = new InstanceCollection2();

        // 2. spawn the instances
        Grow(instances, Vector3.zero, Quaternion.identity, new Vector3(.25f, 1, .25f) , iterations);

        // 3. combining the instances together:
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter) meshFilter.mesh = instances.MakeMultiMesh();

    }

    void Grow(InstanceCollection2 instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0, float nodeSpin = 0) {
        if (num < 0) num = 0;
        if (num >= max) return;


        //Make a cube mesh, add a list:

        Matrix4x4 xform = Matrix4x4.TRS(pos, rot, scale);
        instances.AddBranch(MeshTools.MakeCube(), xform);

        // add to num, calc %
        float percentAtEnd = ++num / (float)max;

        Vector3 endpoint = xform.MultiplyPoint(new Vector3(0, 1, 0));

        if ((pos - endpoint).sqrMagnitude < .1f) return; //too small, stop recursion

        bool hasNode = num >= branchNodeTrunk && ((num - branchNodeTrunk - 1) % branchNodeDis == 0);

        if (hasNode)
             {
                  if (branchType == BranchingType.Alternate180) nodeSpin += 180;
                  if (branchType == BranchingType.Alternate1375) nodeSpin += 137.5f;
             }

        {

            Quaternion randRot = rot * Quaternion.Euler(turnDegrees, twistDegrees, 0);
            Quaternion upRot = Quaternion.RotateTowards(rot, Quaternion.identity, 45);

            Quaternion newRot = Quaternion.Lerp(randRot, upRot, percentAtEnd);

            Grow(instances, endpoint, newRot, scale *= .9f, max, num, nodeSpin);
        }

        if (hasNode)
        {

            int howMany = 0;
                float degreesBetweenNodes = 0;

                switch (branchType)
                {
                    case BranchingType.Random:
                        howMany = 1;
                        break;
                    case BranchingType.Opposite:
                        degreesBetweenNodes = 180;
                        howMany = 2;
                        break;
                    case BranchingType.Alternate180:
                        howMany = 1;
                        break;
                    case BranchingType.Alternate1375:
                        howMany = 1;
                        break;
                    case BranchingType.WhorledTwo:
                        degreesBetweenNodes = 180;
                        howMany = 2;
                        break;
                    case BranchingType.WhorledThree:
                        degreesBetweenNodes = 120;
                        howMany = 3;
                        break;
                }

                float lean = Mathf.Lerp(90, 0, alignWithParent);

                for (int i = 0; i < howMany; i++)
                {
                    float spin = nodeSpin + degreesBetweenNodes * i;
                    Quaternion newRot = Quaternion.Euler(lean, spin, 0) * rot;

                 float s = RandBell(.5f, .95f);
                    Grow(instances, endpoint, newRot, scale * s,   max,  num, 90);
                }
            
        }

        
        
    }
}
