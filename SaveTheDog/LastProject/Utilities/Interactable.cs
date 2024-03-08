using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    //Implements IInteractable
    class Interactable : GameObject, IInteractable
    {
        public Interactable(string texturePath, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            ResetPos = Game.PositionFake; //Invalid position == Out of window
            InteractMgr.AddItem(this);
        }

        public Vector2 MousePos { get; set; }
        public Vector2 ResetPos { get; set; }

        public void Input()
        {
            if (Game.Window.MouseLeft)
            {
                MousePos = Game.Window.MousePosition;
            }
        }

        public void ResetMousePos()
        {
            MousePos = ResetPos;
        }

        //If i interacted with the mouse on an Interactable object and the player is quite close to it, i start the specific dialog
        protected bool HasInteractDialogue(DialogueName name)
        {  
            bool hasInteract = false;
            if (RigidBody.Collider.Contains(MousePos))
            {
                Player player = ((MapScene)Game.CurrentScene).Player;
                Vector2 dist = ((MapScene)Game.CurrentScene).Player.Position - (Position - Pivot);

                if (!(player.Fsm.GetCurrentState() is DialogueState) && dist.LengthSquared <= 1f)
                {
                    Game.CurrentScene.sourceEmitter[(int)MainNameClips.DialogueSFX].Play(Game.CurrentScene.clips[(int)MainNameClips.DialogueSFX]);
                    player.Fsm.GoTo(StateEnum.DIALOGUE);
                    ((DialogueState)player.Fsm.GetState(StateEnum.DIALOGUE)).SetDialogue(name);

                    hasInteract = true;
                }
            }

            ResetMousePos();

            return hasInteract;
        }

    }
}
