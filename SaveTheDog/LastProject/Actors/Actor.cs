using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LastProject
{
    abstract class Actor : GameObject
    {
        //Variables
        protected int energy;
        protected float maxSpeed;
        protected int maxEnergy;

        //Properties
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public Vector2 Velocity { get; set; }

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, textOffsetX: textOffsetX, textOffsetY: textOffsetY, spriteWidth: spriteWidth, spriteHeight: spriteHeight)
        {
            maxEnergy = 3;
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
            else
            {
                //Reset player on the start point
                ((MapScene)Game.CurrentScene).Player.ResetSpawnUnderground();
            }

        }

        public virtual void OnDie() { }

        public virtual void Reset()
        {
            Energy = maxEnergy;
        }

    }
}
