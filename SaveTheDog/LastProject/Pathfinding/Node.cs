using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Node
    {
        public int X { get; }
        public int Y { get; }
        public int Cost { get; }
        public List<Node> Neighbours { get; }

        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;

            Neighbours = new List<Node>();
        }

        public void AddNeighbour(Node n)
        {
            Neighbours.Add(n);
        }

        public void RemoveNeighbour(Node n)
        {
            Neighbours.Remove(n);
        }

        public void Destroy()
        {
            for (int i = 0; i < Neighbours.Count; i++)
            {
                if (Neighbours[i] != null)
                {
                    Neighbours[i] = null;
                }
            }

            Neighbours.Clear();
        }

    }
}
