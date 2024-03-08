using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    abstract class Collider
    {
        public Vector2 Offset;
        public RigidBody RigidBody;
        public Vector2 Position { get { return RigidBody.Position + Offset; } }

        public Collider(RigidBody owner)
        {
            RigidBody = owner;
            Offset = new Vector2(RigidBody.GameObject.HalfWidth, RigidBody.GameObject.HalfHeight);

        }

        public abstract bool Collides(Collider collider, ref Collision collisionInfo);
        public abstract bool Collides(CircleCollider collider, ref Collision collisionInfo);
        public abstract bool Collides(BoxCollider collider, ref Collision collisionInfo);
        public abstract bool Collides(CompoundCollider collider, ref Collision collisionInfo);

        public abstract bool Contains(Vector2 point);
    }
}
