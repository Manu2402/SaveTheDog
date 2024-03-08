using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class GUIitem : GameObject
    {
        protected GameObject owner;
        protected Vector2 offset;

        public GUIitem(Vector2 position, GameObject owner, string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, DrawLayer.GUI, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            this.owner = owner;
            sprite.position = position;
            sprite.Camera = CameraMgr.MainCamera;
            offset = position - owner.Position;

            DrawMgr.AddItem(this);
        }

        public override void Destroy()
        {
            base.Destroy();
            owner = null;
        }

    }
}
