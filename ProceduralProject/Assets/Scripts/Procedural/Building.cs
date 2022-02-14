using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Building : MonoBehaviour
{

    public GameObject[] bottomPieces;
    public GameObject[] middlePieces;
    public GameObject[] topPieces;

    public int minPieces = 4;

    public int maxPieces = 10;

    private List<CombineInstance> pieces;

    private Mesh finalMesh;

    private BoxCollider boxCollider;

    // Start is called before the first frame update
    public void OnValidate()
    {
        Build(minPieces, maxPieces);
    }

    public void Build(int min, int max)
    {

        boxCollider = GetComponent<BoxCollider>();

        pieces = new List<CombineInstance>();

        int buildingSize = Random.Range(minPieces, maxPieces + 1);

        float heightOffset = SpawnPiece(bottomPieces, 0, 0, 0);

        for (int x = 2; x < buildingSize; x++){
            heightOffset += SpawnPiece(middlePieces, heightOffset, 0, 0);
        }

        heightOffset += SpawnPiece(topPieces, heightOffset, 0, 0);

        boxCollider.size = new Vector3(1, heightOffset, 1);
        boxCollider.center = new Vector3(-.5f, heightOffset/2, -.5f);

        finalMesh = new Mesh();
        finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        finalMesh.CombineMeshes(pieces.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter) meshFilter.mesh = finalMesh;

    }

    float SpawnPiece(GameObject[] pieceArray, float inputHeight, float xPos, float zPos)
    {
        CombineInstance piece = new CombineInstance();

        GameObject clone = pieceArray[Random.Range(0, pieceArray.Length)];

        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().sharedMesh;
        Bounds baseBounds = cloneMesh.bounds;

        float addedHeight = baseBounds.size.y;

        piece.mesh = cloneMesh;
        piece.transform = Matrix4x4.TRS(this.transform.position + new Vector3 (xPos, inputHeight, zPos), transform.rotation, Vector3.one);

        pieces.Add(piece);

        return addedHeight;
    }

 
}
