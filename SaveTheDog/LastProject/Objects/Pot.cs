using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Pot : Object
    {
        public Pot(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);
        }

        public void RemoveNodeOnMe()
        {
            //Remove node on the pot position
            ((PrincessHouseScene)Game.CurrentScene).Map.ToggleNode((int)Position.X, (int)Position.Y);
        }

        public override void Update()
        {
            if (!((PrincessHouseScene)Game.CurrentScene).UnlockedStair)
            {
                if (HasInteractDialogue(DialogueName.Pot))
                {
                    //Activate the stair
                    ((PrincessHouseScene)Game.CurrentScene).UnlockedStair = true;
                    ((PrincessHouseScene)Game.CurrentScene).objects[(int)ObjsInPrincessHouseScene.Stair].IsActive = true;
                }
            }
        }


    }
}
