using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class Book : Object
    {
        public Book(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            DrawMgr.AddItem(this);
        }

        public override void Update()
        {
            //According to which scene i am, a different text is displayed (always acting on the book)
            if (((MapScene)Game.CurrentScene).GetType() == typeof(PlayerHouseScene))
            {
                HasInteractDialogue(DialogueName.BookPlayerHouse);
            }
            else if(((MapScene)Game.CurrentScene).GetType() == typeof(PrincessHouseScene))
            {
                HasInteractDialogue(DialogueName.BookPrincessHouse);
            }
        }
    }
}
