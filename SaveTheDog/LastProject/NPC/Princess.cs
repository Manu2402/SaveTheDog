using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace LastProject
{
    class Princess : NPC
    {
        private int numTextures;

        public Princess(string texturePath) : base(texturePath)
        {
            numTextures = 4;
            textures = new Texture[numTextures];

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = GfxMgr.GetTexture($"princessIdle_{i}");
            }

            UpdateMgr.AddItem(this);
            DrawMgr.AddItem(this);
        }

        public override void Update()
        {
            base.Update();

            if (Game.HasKey)
            {
                HasInteractDialogue(DialogueName.PricessAfterPickedUpKey);
            }
            else
            {
                HasInteractDialogue(DialogueName.Princess);
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = null;
            }

            textures = null;
        }

    }
}
