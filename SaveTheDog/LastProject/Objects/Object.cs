using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    abstract class Object : Interactable
    {
        public Object(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            sprite.pivot = Vector2.Zero;

            if (solid) //Add Rigidbody
            {
                RigidBody = new RigidBody(this);
                RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
                RigidBody.Type = RigidBodyType.Objects;
                RigidBody.AddCollisionType(RigidBodyType.NPC | RigidBodyType.Player);
            }

            UpdateMgr.AddItem(this);
        }
    }
}
