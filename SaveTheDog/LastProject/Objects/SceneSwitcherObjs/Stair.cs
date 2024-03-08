using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    class Stair : SceneSwitcherObj
    {
        public Stair(string texturePath, Vector2 positionPlayer, bool solid, Scene nextScene, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, positionPlayer, solid, nextScene, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, textOffsetX, textOffsetY, (int)Game.UnitsToPixels(sprite.Width), (int)Game.UnitsToPixels(sprite.Height));
            }
        }

    }
}
