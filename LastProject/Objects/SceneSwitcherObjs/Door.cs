using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using Aiv.Audio;

namespace LastProject
{
    class Door : SceneSwitcherObj
    {
        private bool isLocked;

        public Door(string texturePath, Vector2 positionPlayer, bool solid, bool isLocked, Scene nextScene, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, positionPlayer, solid, nextScene, layer, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            this.isLocked = isLocked;
        }

        public override void Update()
        {
            if (isLocked)
            {
                if (Game.HasKey)
                {
                    if (HasInteractDialogue(DialogueName.DoorUnlocked))
                    {
                        SetOpenDoor();

                        //Sound
                        ((MainScene)Game.CurrentScene).sourceEmitter[(int)MainNameClips.OpenDoorSFX].Play(((MainScene)Game.CurrentScene).clips[(int)MainNameClips.OpenDoorSFX]);
                    }
                }
                else
                {
                    HasInteractDialogue(DialogueName.DoorLocked);
                }
            }
        }

        public void SetOpenDoor()
        {
            isLocked = false;
            ((MainScene)Game.Scenes[(int)ScenesType.Main]).DogHouseDoorUnlocked = true;

            DrawMgr.AddItem(this);
            textOffsetX = 79;
            textOffsetY = 176;
            frameW = (int)Game.UnitSize;
            frameH = (int)Game.UnitSize;

            SetAvailableOpenDoor();
        }

        private void SetAvailableOpenDoor()
        {
            //If the open door is drawn then i can access it in pathfinding
            if (DrawMgr.Contains(this))
            {
                ((MainScene)Game.CurrentScene).Map.ToggleNode((int)Position.X, (int)Position.Y);
                Game.CurrentScene.NextScene = Game.Scenes[(int)ScenesType.DogHouse];
            }
        }

    }
}
