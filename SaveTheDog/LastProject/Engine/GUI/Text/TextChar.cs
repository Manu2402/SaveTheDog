using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class TextChar : GameObject
    {

        protected Font font;
        protected char character;

        public char Character { get { return character; } set { character = value; ComputeOffset(); } }

        public TextChar(Vector2 spritePosition, char character, Font f) : base(f.TextureName, DrawLayer.GUI, spriteWidth: Game.PixelsToUnits(f.CharacterWidth), spriteHeight: Game.PixelsToUnits(f.CharacterHeight))
        {
            sprite.position = spritePosition;
            sprite.pivot = Vector2.Zero;
            font = f;
            Character = character;
            IsActive = true;

            frameW = font.CharacterWidth;
            frameH = font.CharacterHeight;

            sprite.Camera = CameraMgr.GetCamera("GUI");

            DrawMgr.AddItem(this);
        }

        protected void ComputeOffset()
        {
            Vector2 textureOffset = font.GetOffset(character);

            textOffsetX = (int)textureOffset.X;
            textOffsetY = (int)textureOffset.Y;
        }
    }
}
