using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquareType
{
    Open,

    Wall, 

    Slime
}
public class Square : MonoBehaviour
{
    public Transform wall;
    public Transform slime;
    BoxCollider box;
    public SquareType type = SquareType.Open;
    public float MoveCost {
        get{
            if (type == SquareType.Open) return 1;
            if (type == SquareType.Wall) return 9999;
            if (type == SquareType.Slime) return 10;
            return 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        UpdateArt();
    }

    // Update is called once per frame
    void OnMouseDown(){

        //changes this terrain cubes state
        type += 1;
        if ((int)type > 2) type = 0;

        //change the artwork of the terrain cube
        UpdateArt();

        //rebuild our array of nodes
        if(Maze.singleton) Maze.singleton.MakeNodes();
    }

    void UpdateArt(){

        bool isShowingWall = (type == SquareType.Wall);

        float y = isShowingWall ? .44f : 0f;
        float h = isShowingWall ? 1.1f : .2f;
        box.size = new Vector3(1, h, 1);
        box.center = new Vector3(0, y, 0);

        if (wall) wall.gameObject.SetActive(isShowingWall);
        if (slime) slime.gameObject.SetActive(type == SquareType.Slime);
    }
}
