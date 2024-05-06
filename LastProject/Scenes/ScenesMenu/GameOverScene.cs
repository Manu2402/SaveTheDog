using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using Aiv.Audio;

namespace LastProject
{
    class GameOverScene : TitleScene
    {
        public GameOverScene() : base("gameOverScreen", "selector") { }

        protected override void LoadAssets()
        {
            base.LoadAssets();
            GfxMgr.AddTexture("gameOverScreen", "Assets/Graphics/gameOverScreen.png");
        }

        public override Scene OnExit()
        {
            //Reset parameters
            Game.HasBoots = false;
            Game.HasKey = false;
            Game.HasPickaxe = false;
            Game.LastEnergyPlayer = 3;
            Game.LastMaxSpeedPlayer = 6f;

            for (int i = 0; i < Game.Scenes.Length; i++)
            {
                Game.Scenes[i].Reload();
            }

            return base.OnExit();
        }

    }
}
