using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class NPC : Interactable
    {
        enum Sprites
        {
            Right, Left, Down, Up
        }

        protected Vector2 direction;
        protected float visionRadius;

        protected Texture[] textures;

        public NPC(string texturePath) : base(texturePath, spriteWidth: Game.PixelsToUnits(Game.UnitSize))
        {
            IsActive = true;

            sprite.pivot = Vector2.Zero;

            direction = Forward;
            visionRadius = 5f;

            //Rigidbody
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.NPC;
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Objects);

            frameW = (int)Game.UnitSize;
        }

        public void RemoveNodeOnMe()
        {
            //Remove node on the NPC positions
            ((MapScene)Game.CurrentScene).Map.ToggleNode((int)Position.X, (int)Position.Y);
        }

        //Method to direct the look of the NPC towards the player
        public void LookAt(Player target, Texture[] textures)
        {
            direction = target.Position - Position;
            if (direction.LengthSquared <= visionRadius * visionRadius)
            {
                if (Math.Abs(direction.X) > Math.Abs(direction.Y))
                {
                    if (direction.X > 0)
                    {
                        texture = textures[(int)Sprites.Right];
                    }
                    else
                    {
                        texture = textures[(int)Sprites.Left];
                    }
                }
                else
                {
                    if (direction.Y > 0)
                    {
                        texture = textures[(int)Sprites.Down];
                    }
                    else
                    {
                        texture = textures[(int)Sprites.Up];
                    }
                }
            }
        }

        public override void Update()
        {
            LookAt(((MapScene)Game.CurrentScene).Player, textures);
        }

    }
}
