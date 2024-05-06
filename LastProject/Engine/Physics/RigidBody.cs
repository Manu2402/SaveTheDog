using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    enum RigidBodyType { Player = 1, NPC = 2, Objects = 4 }

    class RigidBody
    {
        protected uint collisionMask;

        public GameObject GameObject;   // Owner
        public Collider Collider;

        public bool IsGravityAffected;
        public bool IsCollisionAffected = true;
        public bool IsActive { get { return GameObject.IsActive; } }

        public Vector2 Velocity;

        public Vector2 Position { get { return GameObject.Position; } }

        public RigidBodyType Type;

        public RigidBody(GameObject owner)
        {
            GameObject = owner;
            PhysicsMgr.AddItem(this);
        }

        public void Update()
        {
            if (IsGravityAffected)
            {
                Velocity.Y += PhysicsMgr.G * Game.DeltaTime;
            }

            GameObject.Position += Velocity * Game.DeltaTime;
        }

        public bool Collides(RigidBody other, ref Collision collisionInfo)
        {
            return Collider.Collides(other.Collider, ref collisionInfo);
        }

        public void AddCollisionType(RigidBodyType type)
        {
            collisionMask |= (uint)type;
        }

        public void AddCollisionType(uint type)
        {
            collisionMask |= type;
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }

        public void Destroy()
        {
            GameObject = null;
            if (Collider != null)
            {
                Collider.RigidBody = null;
                Collider = null;
            }

            PhysicsMgr.RemoveItem(this);
        }
    }
}
