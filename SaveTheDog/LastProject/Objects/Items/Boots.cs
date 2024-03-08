using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Boots : Object
    {
        private bool pickedUp;

        public Boots(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);
        }

        public override void Update()
        {
            if (!pickedUp)
            {
                if (HasInteractDialogue(DialogueName.BootsPickup))
                {
                    ((MapScene)Game.CurrentScene).Player.InventoryGUI.PickUp(Items.Boots);
                    ((PlayerHouseScene)Game.CurrentScene).Map.ToggleNode(17, 16); //Boots 
                    ((PlayerHouseScene)Game.CurrentScene).HasPickedUpBoots = true;

                    ((MapScene)Game.CurrentScene).Player.SpeedUp();

                    pickedUp = true;
                    IsActive = false;

                    //Sound
                    Game.CurrentScene.sourceEmitter[(int)PlayerHouseNameClips.PickupSFX].Play(Game.CurrentScene.clips[(int)PlayerHouseNameClips.PickupSFX]);
                }
            }
        }

    }
}
