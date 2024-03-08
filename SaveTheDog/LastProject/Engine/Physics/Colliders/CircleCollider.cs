using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    class CircleCollider : Collider
    {
        public float Radius;

        public CircleCollider(RigidBody owner, float radius) : base(owner)
        {
            Radius = radius;
        }

        public override bool Collides(Collider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider other, ref Collision collisionInfo)
        {
            Vector2 dist = other.Position - Position;
            return (dist.LengthSquared <= Math.Pow(other.Radius + Radius, 2));
        }

        public override bool Collides(BoxCollider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CompoundCollider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Contains(Vector2 point)
        {
            Vector2 distFromCenter = point - Position;
            return distFromCenter.LengthSquared <= (Radius * Radius);
        }
    }
}
