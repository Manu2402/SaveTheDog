using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    abstract class SceneSwitcherObj : Object //Class for any object that has conseguences the change of scene
    {
        public Scene NextScene;
        protected Vector2 positionPlayer; //For change the player's position for spawn it in the right cell

        public SceneSwitcherObj(string texturePath, Vector2 positionPlayer, bool solid, Scene nextScene, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            //I use a temp position, that's (-1f, -1f), an invalid position like coordinates
            this.positionPlayer = positionPlayer != Game.PositionFake ? positionPlayer : Game.PositionFake;
            NextScene = nextScene;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            Vector2 dist = ((MapScene)Game.CurrentScene).Player.Position - (Position - Pivot);
            if (dist.LengthSquared <= 0.5f)
            {
                //Change scene and set player pos
                ((MapScene)Game.CurrentScene).NextScene = NextScene;
                ((MapScene)Game.CurrentScene).IsPlaying = false;
                Game.PositionPlayer = positionPlayer;
            }
        }

    }
}
