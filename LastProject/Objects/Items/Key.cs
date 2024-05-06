using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Key : Object
    {
        private bool pickedUp;

        public Key(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);
        }

        public override void Update()
        {
            if (!pickedUp)
            {
                if (HasInteractDialogue(DialogueName.KeyPickup))
                {
                    ((MapScene)Game.CurrentScene).Player.InventoryGUI.PickUp(Items.Key);
                    ((UndergroundScene)Game.CurrentScene).Map.ToggleNode(24, 31); //Key
                    ((UndergroundScene)Game.CurrentScene).HasPickedUpKey = true;

                    pickedUp = true;
                    IsActive = false;

                    //Sound
                    Game.CurrentScene.sourceEmitter[(int)UndergroundNameClips.PickupSFX].Play(Game.CurrentScene.clips[(int)UndergroundNameClips.PickupSFX]);
                }
            }
        }
    }
}
