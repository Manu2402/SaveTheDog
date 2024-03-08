using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class BoxCollider : Collider
    {
        protected float halfWidth;
        protected float halfHeight;

        public float Width { get { return halfWidth * 2; } }
        public float Height { get { return halfHeight * 2; } }

        public BoxCollider(RigidBody owner, int w, int h) : base(owner)
        {
            halfWidth = w * 0.5f;
            halfHeight = h * 0.5f;
        }

        public BoxCollider(RigidBody owner, float halfWidth, float halfHeight) : base(owner)
        {
            this.halfWidth = halfWidth;
            this.halfHeight = halfHeight;
        }

        public override bool Collides(Collider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider other, ref Collision collisionInfo)
        {
            float deltaX = other.Position.X - Math.Max(Position.X - halfWidth, Math.Min(other.Position.X, Position.X + halfWidth));
            float deltaY = other.Position.Y - Math.Max(Position.Y - halfHeight, Math.Min(other.Position.Y, Position.Y + halfHeight));

            return (deltaX * deltaX + deltaY * deltaY) < (other.Radius * other.Radius);
        }

        public override bool Collides(BoxCollider other, ref Collision collisionInfo)
        {
            Vector2 distance = other.Position - Position;

            float deltaX = Math.Abs(distance.X) - (other.halfWidth + halfWidth);

            if(deltaX > 0)
            {
                return false;
            }

            float deltaY = Math.Abs(distance.Y) - (other.halfHeight + halfHeight);

            if (deltaY > 0)
            {
                return false;
            }

            collisionInfo.Type = CollisionType.RectsIntersection;
            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);

            return true;
        }

        public override bool Contains(Vector2 point)
        {
            return
                point.X >= Position.X - halfWidth &&
                point.X <= Position.X + halfWidth &&
                point.Y >= Position.Y - halfHeight &&
                point.Y <= Position.Y + halfHeight;
        }

        public override bool Collides(CompoundCollider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }
    }
}
