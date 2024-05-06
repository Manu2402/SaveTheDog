using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Pickaxe : Object
    {
        private bool pickedUp;

        public Pickaxe(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (!pickedUp)
                {
                    if (HasInteractDialogue(DialogueName.PickaxePickup))
                    {
                        ((MapScene)Game.CurrentScene).Player.InventoryGUI.PickUp(Items.Pickaxe);
                        ((DogHouseScene)Game.CurrentScene).Map.ToggleNode(33, 34); //Pickaxe
                        ((DogHouseScene)Game.CurrentScene).HasPickedUpPickaxe = true;

                        pickedUp = true;
                        IsActive = false;

                        //Sound
                        Game.CurrentScene.sourceEmitter[(int)PlayerHouseNameClips.PickupSFX].Play(Game.CurrentScene.clips[(int)PlayerHouseNameClips.PickupSFX]);
                    }
                } 
            }
        }
    }
}
