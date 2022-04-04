using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type
{
    Open,

    Wall,

    Slime
}
public class Ground : MonoBehaviour
{
    public Transform wall;
    public Transform slime;
    BoxCollider box;

    public bool occupied  = false;

    public type type = type.Open;
    public float MoveCost
    {
        get
        {

            if (type == type.Open) return 1;
            if (type == type.Wall) return 9999;
            if (type == type.Slime) return 10;
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
    void OnMouseDown()
    {

        //changes this terrain cubes state
        type += 1;
        if ((int)type > 2) type = 0;

        //change the artwork of the terrain cube
        UpdateArt();

        //rebuild our array of nodes
        if (Map.singleton) Map.singleton.MakeNodes();
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Player.instance.target = transform;
        }
    }

    void UpdateArt()
    {

        bool isShowingWall = (type == type.Wall);

        float y = isShowingWall ? .44f : 0f;
        float h = isShowingWall ? 1.1f : .2f;

        if (wall) wall.gameObject.SetActive(isShowingWall);
        if (slime) slime.gameObject.SetActive(type == type.Slime);
    }

    void OnTriggerEnter(Collider colide)
    {

        occupied = true;

   }
   void OnTriggerExit(Collider colide)
   {

        occupied = false;

   }
}
