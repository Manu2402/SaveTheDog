using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace LastProject
{
    class Dog : NPC
    {
        private bool endDialogue = false;
        private int numTextures;

        public Dog(string texturePath) : base(texturePath)
        {
            numTextures = 4;
            textures = new Texture[numTextures];

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = GfxMgr.GetTexture($"dogIdle_{i}");
            }

            UpdateMgr.AddItem(this);
            DrawMgr.AddItem(this);
        }

        public override void Update()
        {
            base.Update();

            if (HasInteractDialogue(DialogueName.Dog) && !endDialogue)
            {
                Game.CurrentScene.NextScene = Game.Scenes[(int)ScenesType.Win];
                endDialogue = true;
            }

            if (((DogHouseScene)Game.CurrentScene).Player.Fsm.GetCurrentState().GetType() != typeof(DialogueState) && endDialogue)
            {
                Game.CurrentScene.IsPlaying = false;
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
