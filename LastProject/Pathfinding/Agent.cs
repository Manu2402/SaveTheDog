using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class Agent
    {
        private Actor owner;

        public int X { get { return Convert.ToInt32(owner.Position.X); } }
        public int Y { get { return Convert.ToInt32(owner.Position.Y); } }

        private List<Node> path;
        private Node current;
        private Node target;

        public Node Target { get => target; set => target = value; }

        public Agent(Actor owner)
        {
            target = null;
            this.owner = owner;
        }

        public void SetPath(List<Node> newPath)
        {
            path = newPath;
            if (path.Count > 0 && target == null)
            {
                target = path[0];
                path.RemoveAt(0);
            }
            else if (path.Count > 0)
            {
                int dist = Math.Abs(path[0].X - target.X) + Math.Abs(path[0].Y - target.Y);
                if (dist > 1)
                {
                    path.Insert(0, current);
                }
            }
        }

        public void Update()
        {
            Vector2 direction = Vector2.Zero;
            if (target != null)
            {
                Vector2 destination = new Vector2(target.X, target.Y);
                direction = destination - owner.Position;
                float distance = direction.Length;

                if (distance < 0.05f)
                {
                    owner.Position = destination;
                    current = target;

                    if (path.Count == 0)
                    {
                        target = null;
                    }
                    else
                    {
                        target = path[0];
                        path.RemoveAt(0);
                    }
                }
                else
                {
                    owner.Position += direction.Normalized() * owner.MaxSpeed * Game.DeltaTime;
                }
            }

            if (direction != Vector2.Zero)
            {
                owner.Velocity = direction.Normalized();
            }
            else
            {
                owner.Velocity = direction;
            }
        }

        public void Destroy()
        {
            owner = null;
            target = null;

            path.Clear();
            path = null;

            if (current != null) { current.Destroy(); }
        }

    }
}
