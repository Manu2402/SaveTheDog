using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class DialogueBar : GameObject
    {
        private Texture dialoguebarNameTex;
        private Sprite dialoguebarName;
        private TextObject text;
        private TextObject nameText;

        public DialogueBar(string texturePath, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            dialoguebarNameTex = GfxMgr.GetTexture("dialogueBarName");
            dialoguebarName = new Sprite(Game.PixelsToUnits(dialoguebarNameTex.Width), Game.PixelsToUnits(dialoguebarNameTex.Height));

            Pivot = Vector2.Zero;
            Position = new Vector2(0f, Game.PixelsToUnits(600f));
            dialoguebarName.position = new Vector2(0f, Game.PixelsToUnits(540f));

            sprite.Camera = CameraMgr.GetCamera("GUI");
            dialoguebarName.Camera = CameraMgr.GetCamera("GUI");

            text = new TextObject(new Vector2(Position.X + 1f, Position.Y + 1f));
            nameText = new TextObject(new Vector2(Position.X + 2.5f, Position.Y - 2.5f));

            text.IsActive = true;
            nameText.IsActive = true;

            DrawMgr.AddItem(this);
        }

        //Change Text
        public void SetText(string text, string nameText)
        {
            if (this.text.Text != text) this.text.Destroy();
            if (this.nameText.Text != nameText) this.nameText.Destroy();

            this.text.Text = text.ToString();
            this.nameText.Text = nameText.ToString();
        }

        public override void Draw()
        {
            base.Draw();
            if (IsActive)
            {
                dialoguebarName.DrawTexture(dialoguebarNameTex);
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            text.Destroy();
            nameText.Destroy();

            dialoguebarName = null;
            dialoguebarNameTex = null;
        }
    }
}
