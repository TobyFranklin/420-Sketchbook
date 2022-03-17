using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public class Node
    {
        public Vector3 position;
        public float G { get; private set; } //Distance from start
        public float H { get; private set; } //Distance from end
        public float F //Total Cost
        {
            get
            {
                return G + H;
            }
        }

        public float moveCost = 1;

        public List<Node> neighbors = new List<Node>();

        private Node _parent;
        public Node parent {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (parent != null)
                {
                    G = _parent.G + moveCost;
                }
                else G = 0;
            }
        }

        public void DoHeuristic(Node end)
        {
            Vector3 d = end.position - this.position;
            H = d.magnitude;
        }

    }
        public static List<Node> Solve(Node start, Node end)
        {

            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();

            start.parent = null;
            open.Add(start);
            //1. travel from start to end

            while(open.Count > 0) {
               
                // find node with least F
                float bestF = 0;
                Node current = null;

                foreach(Node n in open){

                    if(n.F < bestF || current == null) {
                        current = n;
                        bestF = n.F;
                    }
                }

                if(current == end){
                    break;
                }

                bool isDone = false;

                foreach(Node neighbor in current.neighbors){
                    if (!closed.Contains(neighbor))
                    {
                        if (!open.Contains(neighbor)) {

                            open.Add(neighbor);
                            neighbor.parent = current;

                            if(neighbor == end){
                                isDone = true;
                            }

                            neighbor.DoHeuristic(end);

                            //set G and H
                        }
                        else{//Node already in Open

                            //Todo: if G cost is lower, change neighbor's parent

                            //if this path to neighbor has lower G
                            //than prevouis path to neighbor. . .
                        if(current.G + neighbor.moveCost < neighbor.G)
                            {
                                neighbor.parent = current;
                            }
                        }
                    }
                }
                if (isDone) break;
            }
            //2. travel from end to start, building path

            List<Node> path = new List<Node>();


            for(Node temp = end; temp!= null; temp = temp.parent){
                path.Add(temp);
            }

            //3. reverse path
            path.Reverse();

            return path;
        }

    }

