using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    enum Items
    {
        Boots, Key, Pickaxe, Heart
    }

    class InventoryGUIitem : GUIitem
    {   

        public InventoryGUIitem(Vector2 position, GameObject owner, string texturePath, float spriteWidth = 0, float spriteHeight = 0) : base(position, owner, texturePath, 0, 0, spriteWidth, spriteHeight)
        {
            switch (texturePath)
            {
                case "boots_ico":
                    IsActive = Game.HasBoots; //Flags for inventory
                    break;

                case "key_ico":
                    IsActive = Game.HasKey;
                    break;

                case "pickaxe_ico":
                    IsActive = Game.HasPickaxe;
                    break;

                //Hearts must all be active at the beginning of the game
                case "heart":
                    IsActive = true;
                    break;
            }

            sprite.Camera = CameraMgr.GetCamera("GUI");
        }

    }
}
