using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class CompoundCollider : Collider
    {
        public Collider BoundingCollider;
        protected List<Collider> colliders;

        public CompoundCollider(RigidBody owner, Collider boundingCollider) : base(owner)
        {
            BoundingCollider = boundingCollider;
            colliders = new List<Collider>();
            Offset = Vector2.Zero;
        }

        public virtual bool InnerCollidersCollide(Collider collider, ref Collision collisionInfo)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (collider.Collides(colliders[i], ref collisionInfo))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void AddCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public override bool Collides(Collider other, ref Collision collisionInfo)
        {
            return other.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider other, ref Collision collisionInfo)
        {
            return (other.Collides(BoundingCollider, ref collisionInfo) && InnerCollidersCollide(other, ref collisionInfo));
        }

        public override bool Collides(BoxCollider other, ref Collision collisionInfo)
        {
            return (other.Collides(BoundingCollider, ref collisionInfo) && InnerCollidersCollide(other, ref collisionInfo));
        }

        public override bool Collides(CompoundCollider other, ref Collision collisionInfo)
        {
            if (BoundingCollider.Collides(other.BoundingCollider, ref collisionInfo))
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    for (int j = 0; j < other.colliders.Count; j++)
                    {
                        if (colliders[i].Collides(other.colliders[j], ref collisionInfo))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override bool Contains(Vector2 point)
        {
            if (BoundingCollider.Contains(point))
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    if (colliders[i].Contains(point))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
