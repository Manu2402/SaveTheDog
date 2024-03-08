using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using System.Xml;

namespace LastProject
{
    abstract class MapScene : Scene, IMapScene
    {
        public Map Map { get; protected set; }

        //Reference for all MapScene
        public Player Player;

        public HealthGUI Hearts;

        public abstract override void Draw();
        public abstract override void Input();

        public MapScene() { }

        public override void Start()
        {
            base.Start();

            //Player parameters that must be sent between scenes, regardless of the player instance.
            //To make this i used the static variables present in the Game class, in order to assign to the player the temporary data saved.
            if (Player != null)
            {
                Player.MaxSpeed = Game.LastMaxSpeedPlayer;
                Player.Energy = Game.LastEnergyPlayer;
            }
        }

        public override Scene OnExit()
        {
            //Same as Start()
            if (Player != null)
            {
                Game.LastMaxSpeedPlayer = Player.MaxSpeed;
                Game.LastEnergyPlayer = Player.Energy;
            }

            return base.OnExit();
        }

        //Assets used in all MapScenes
        protected override void LoadAssets()
        {
            GfxMgr.AddTexture("tileset", "Assets/Graphics/tileset.png");

            GfxMgr.AddTexture("PlayerWalk_0", "Assets/Graphics/PlayerSheet/WalkUp.png");
            GfxMgr.AddTexture("PlayerWalk_1", "Assets/Graphics/PlayerSheet/WalkDown.png");
            GfxMgr.AddTexture("PlayerWalk_2", "Assets/Graphics/PlayerSheet/WalkLeft.png");
            GfxMgr.AddTexture("PlayerWalk_3", "Assets/Graphics/PlayerSheet/WalkRight.png");
            GfxMgr.AddTexture("Death", "Assets/Graphics/PlayerSheet/Death.png");

            GfxMgr.AddTexture("key_ico", "Assets/Graphics/ICOs/key_ico.png");
            GfxMgr.AddTexture("boots_ico", "Assets/Graphics/ICOs/boots_ico.png");
            GfxMgr.AddTexture("pickaxe_ico", "Assets/Graphics/ICOs/pickaxe_ico.png");

            GfxMgr.AddTexture("heart", "Assets/Graphics/Objects/heart.png");

            GfxMgr.AddTexture("weapons_frame", "Assets/Graphics/weapons_GUI_frame.png");
            GfxMgr.AddTexture("weapon_selection", "Assets/Graphics/weapon_GUI_selection.png");

            GfxMgr.AddTexture("dialogueBar", "Assets/Graphics/Dialogues/DialogueBar.png");
            GfxMgr.AddTexture("dialogueBarName", "Assets/Graphics/Dialogues/DialogueBarName.png");

            FontMgr.AddFont("textSheet", "Assets/Graphics/textSheet.png", 15, 32, 20, 20);
        }

    }
}
