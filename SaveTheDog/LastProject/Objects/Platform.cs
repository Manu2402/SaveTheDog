using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Platform : Object
    {
        private bool isPressed;
        private int damage;

        public bool IsPressed { get => isPressed; set => isPressed = value; }

        public Platform(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);

            frameW = (int)Game.UnitSize;
            damage = 1;
        }

        public override void Draw()
        {
            if (IsActive)
            {
                if (isPressed)
                {
                    sprite.DrawTexture(texture, (int)Game.UnitSize, 0, frameW, frameH);
                }
                else
                {
                    sprite.DrawTexture(texture, 0, 0, frameW, frameH);
                }
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            Vector2 dist = ((MapScene)Game.CurrentScene).Player.Position - (Position - Pivot);
            if (dist.LengthSquared <= 0.5f)
            {
                if (!isPressed)
                {
                    isPressed = true;

                    //Algorithm for pressure in the correct order of the platforms
                    if (PlatformMgr.GetPlatformAtIndex(PlatformMgr.CounterPlatformPressed) == this)
                    {
                        //Sound
                        Game.CurrentScene.sourceEmitter[(int)UndergroundNameClips.PlatformSFX].Play(Game.CurrentScene.clips[(int)UndergroundNameClips.PlatformSFX]);
                        
                        PlatformMgr.CounterPlatformPressed++;
                    }
                    else
                    {
                        //Sound
                        Game.CurrentScene.sourceEmitter[(int)UndergroundNameClips.Hurt].Play(Game.CurrentScene.clips[(int)UndergroundNameClips.Hurt]);

                        PlatformMgr.CounterPlatformPressed = 0;
                        PlatformMgr.ResetPlatforms();
                        ((MapScene)Game.CurrentScene).Hearts.GetDamage(damage);
                    }
                }
            }
        }

    }
}
