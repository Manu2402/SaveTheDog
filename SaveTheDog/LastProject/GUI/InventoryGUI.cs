using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class InventoryGUI : GameObject
    {
        protected InventoryGUIitem[] items;
        protected string[] textureNames = { "boots_ico", "key_ico", "pickaxe_ico" };

        protected float itemWidth;

        public InventoryGUI(Vector2 position) : base("weapons_frame", DrawLayer.GUI)
        {
            DrawMgr.AddItem(this);

            sprite.pivot = Vector2.Zero;
            sprite.position = position;
            sprite.Camera = CameraMgr.GetCamera("GUI");

            items = new InventoryGUIitem[textureNames.Length];

            itemWidth = Game.PixelsToUnits(Game.UnitSize);

            float itemPosY = position.Y + HalfHeight;
            float itemsHorizontalDistance = Game.PixelsToUnits(7);

            for (int i = 0; i < items.Length; i++)
            {
                Vector2 itemPos = new Vector2((position.X + itemsHorizontalDistance + itemWidth * 0.5f + itemWidth * i) * 2.3f, itemPosY);
                items[i] = new InventoryGUIitem(itemPos, this, textureNames[i]);
            }

        }

        public void PickUp(Items item)
        {
            switch (item)
            {
                case Items.Boots:
                    Game.HasBoots = true; //Turn on the Flag
                    items[(int)Items.Boots].IsActive = true; //SetActive in inventory
                    break;

                case Items.Key:
                    Game.HasKey = true;
                    items[(int)Items.Key].IsActive = true;
                    break;

                case Items.Pickaxe:
                    Game.HasPickaxe = true;
                    items[(int)Items.Pickaxe].IsActive = true;
                    break;
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Destroy();
            }

            items = null;
        }

    }
}
