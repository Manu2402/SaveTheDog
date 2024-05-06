using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    class Rock : Object
    {
        private bool firstInteraction = true;

        public Rock(string texturePath, bool solid, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, solid, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight) { }

        public override void Update()
        {
            //Precise selection of the situation-specific dialogue
            if (Game.HasPickaxe && firstInteraction)
            {
                if (HasInteractDialogue(DialogueName.RockDestroyedFirst))
                {
                    firstInteraction = false;
                }
            }
            else if (Game.HasPickaxe && !firstInteraction)
            {
                HasInteractDialogue(DialogueName.RockDestroyed);
            }
            else
            {
                HasInteractDialogue(DialogueName.Rock);
            }
        }
    }
}
