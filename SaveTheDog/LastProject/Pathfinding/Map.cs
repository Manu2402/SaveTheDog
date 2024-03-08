using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class Map //Game Map
    {
        private Dictionary<Node, Node> cameFrom;
        private Dictionary<Node, int> costSoFar;
        private PriorityQueue frontier;

        private int width, height;
        private int[] cells;

        public Node[] Nodes { get; }

        public Map(int w, int h, int[] cells)
        {
            width = w;
            height = h;
            this.cells = cells;

            Nodes = new Node[cells.Length];

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] <= 0) continue;
                int x = i % width;
                int y = i / height;
                Nodes[i] = new Node(x, y, cells[i]);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = x + y * width;

                    if (Nodes[index] == null) continue;

                    AddNeighbours(Nodes[index], x, y);
                }
            }

        }

        public void AddNeighbours(Node n, int x, int y)
        {
            //Top
            CheckNeighbour(n, x, y - 1);
            //Bottom
            CheckNeighbour(n, x, y + 1);
            //Left
            CheckNeighbour(n, x - 1, y);
            //Right
            CheckNeighbour(n, x + 1, y);
        }

        public void CheckNeighbour(Node n, int x, int y)
        {
            if (x < 0 || x >= width) return;
            if (y < 0 || y >= height) return;

            int index = x + y * width;

            Node neighbour = Nodes[index];

            if (neighbour != null)
            {
                n.AddNeighbour(neighbour);
            }
        }

        private void AddNode(int x, int y, int cost = 1)
        {
            int index = x + y * width;
            Node node = new Node(x, y, cost);
            Nodes[index] = node;
            AddNeighbours(node, x, y);

            foreach (Node adj in node.Neighbours)
            {
                adj.AddNeighbour(node);
            }

            cells[index] = cost;
        }

        private void RemoveNode(int x, int y)
        {
            int index = x + y * width;
            Node node = GetNode(x, y);

            foreach (Node adj in node.Neighbours)
            {
                adj.RemoveNeighbour(node);
            }

            Nodes[index] = null;
            cells[index] = 0;
        }

        private Node GetNode(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return null;
            return Nodes[x + y * width];
        }

        public List<Node> GetPath(int startX, int startY, int endX, int endY)
        {
            List<Node> path = new List<Node>();

            Node start = GetNode(startX, startY);
            Node end = GetNode(endX, endY);

            if (start == null || end == null) return path;

            AStar(start, end);

            if (!cameFrom.ContainsKey(end))
            {
                return path;
            }

            Node currentNode = end;
            while (currentNode != cameFrom[currentNode])
            {
                path.Add(currentNode);
                currentNode = cameFrom[currentNode];
            }

            path.Reverse();

            return path;
        }

        public void ToggleNode(int x, int y)
        {
            Node n = GetNode(x, y);
            if (n == null)
            {
                AddNode(x, y);
            }
            else
            {
                RemoveNode(x, y);
            }
        }

        public void AStar(Node start, Node end)
        {
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();
            frontier = new PriorityQueue();

            cameFrom[start] = start;
            costSoFar[start] = 0;
            frontier.Enqueue(start, Heuristic(start, end));

            while (!frontier.IsEmpty)
            {
                Node currentNode = frontier.Dequeue();

                if (currentNode == end) return;

                foreach (Node nextNode in currentNode.Neighbours)
                {
                    int newCost = costSoFar[currentNode] + nextNode.Cost;
                    if (!costSoFar.ContainsKey(nextNode) || costSoFar[nextNode] > newCost)
                    {
                        cameFrom[nextNode] = currentNode;
                        costSoFar[nextNode] = newCost;
                        int priority = newCost + Heuristic(nextNode, end);
                        frontier.Enqueue(nextNode, priority);
                    }
                }
            }
        }

        private int Heuristic(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        public void Destroy()
        {
            if (cameFrom != null)
            {
                cameFrom.Clear();
                cameFrom = null;
            }

            if (costSoFar != null)
            {
                costSoFar.Clear();
                costSoFar = null;
            }

            if (frontier != null)
            {
                frontier.ClearAll();
                frontier = null;
            }

            for (int i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i] != null)
                {
                    Nodes[i].Destroy();
                }
            }
        }

    }
}
