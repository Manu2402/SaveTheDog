using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class HealthGUI : GameObject
    {
        private int numHearts;
        protected InventoryGUIitem[] items;
        protected string textureName;
        protected float itemWidth;

        public HealthGUI(Vector2 position) : base("weapons_frame", DrawLayer.GUI)
        {
            DrawMgr.AddItem(this);

            sprite.pivot = Vector2.Zero;
            sprite.position = position;
            sprite.Camera = CameraMgr.GetCamera("GUI");

            numHearts = ((MapScene)Game.CurrentScene).Player.Energy;

            textureName = "heart";

            items = new InventoryGUIitem[numHearts];

            itemWidth = Game.PixelsToUnits(Game.UnitSize);

            float itemPosY = position.Y + HalfHeight;
            float itemsHorizontalDistance = Game.PixelsToUnits(7);

            for (int i = 0; i < numHearts; i++)
            {
                Vector2 itemPos = new Vector2((position.X + itemsHorizontalDistance + itemWidth * 0.5f + itemWidth * i) * 2.3f, itemPosY);
                items[i] = new InventoryGUIitem(itemPos, this, textureName);
            }
        }
        
        public void GetDamage(int damage)
        {
            ((MapScene)Game.CurrentScene).Player.AddDamage(damage);
            items[--numHearts].IsActive = false;            
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
