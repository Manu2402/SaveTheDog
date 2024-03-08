using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using System.Xml;
using Aiv.Audio;

namespace LastProject
{
    enum MainNameClips { Overworld, DialogueSFX, OpenDoorSFX, RockSFX, LAST } //Clips of MainScene

    class MainScene : MapScene
    {
        #region ENUMS

        enum ObjectGroups //Type of ObjectGroups in XML
        {
            NPC, Objs, LAST
        }

        enum NPCs //ObjectGroups[0]
        {
            Princess, Player, LAST
        }

        enum Objs //ObjectGroups[1]
        {
            DoorOpenOne, DoorOpenTwo, DoorCloseOne, Rock, LAST
        }


        #endregion

        protected NPC princess;

        protected Background bg;
        protected Object[] objects;

        protected Vector2 PlayerPos;
        protected Vector2 PrincessPos;

        public bool FirstLoad = true; //FirstLoad to understand if the scene is the first load or not
        public bool DogHouseDoorUnlocked = false;

        protected TmxMap TmxMap { get; set; }

        public MainScene() : base() { }

        public override void Reload()
        {
            FirstLoad = true;
        }

        public override void Start()
        {
            base.Start();

            LoadAssets();

            sourceEmitter = new AudioSource[(int)MainNameClips.LAST];
            clips = new AudioClip[(int)MainNameClips.LAST];
            LoadSounds();

            //Camera
            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
            CameraMgr.Init(cameraLimits);
            CameraMgr.AddCamera("GUI");

            bg = new Background("tileset", "Assets/Config/MainSceneMap.tmx");

            //Getting map created in tmxMap contained in bg
            TmxMap = bg.TiledMap;
            Map = TmxMap.Map;

            //Get positions of player and princess by the XML file
            LoadPositions();

            //Create objects by the XML file
            LoadObjects();

            //Player
            if (!FirstLoad) //Set the position and the sprite based on FirstLoad
            {
                Player = new Player("PlayerWalk_1");
                Player.Position = Game.PositionPlayer;
                Player.Energy = Game.LastEnergyPlayer;
            }
            else
            {
                Player = new Player("PlayerWalk_2");
                Player.Position = PlayerPos;
            }

            //Hearts
            Hearts = new HealthGUI(new Vector2(1f, 4f));
            Hearts.IsActive = true;

            FirstLoad = false;

            CameraMgr.SetTarget(Player);
            Player.SetCamera(CameraMgr.MainCamera);

            //Princess
            princess = new Princess("princessIdle_0");
            princess.Position = PrincessPos;
            princess.RemoveNodeOnMe();
        }

        protected void LoadSounds()
        {
            for (int i = 0; i < clips.Length; i++)
            {
                switch ((MainNameClips)i)
                {
                    case MainNameClips.Overworld:
                        sourceEmitter[i] = new AudioSource(42);
                        clips[i] = GfxMgr.GetClip("Overworld");
                        break;

                    case MainNameClips.DialogueSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("DialogueSFX");
                        break;

                    case MainNameClips.OpenDoorSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("OpenDoor");
                        break;

                    case MainNameClips.RockSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("RockSFX");
                        break;
                }
            }
        }

        protected void LoadPositions()
        {
            XmlNode mapNode = TmxMap.mapNode;
            XmlNodeList objGroupNode = mapNode.SelectNodes("objectgroup");
            XmlNodeList NPCNodes = objGroupNode[(int)ObjectGroups.NPC].SelectNodes("object");

            for (int i = 0; i < NPCNodes.Count; i++)
            {
                int x = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(NPCNodes[i], "x"));
                int y = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(NPCNodes[i], "y"));

                switch ((NPCs)i)
                {
                    case NPCs.Princess:
                        PrincessPos.X = x;
                        PrincessPos.Y = y;
                        break;

                    case NPCs.Player:
                        PlayerPos.X = x;
                        PlayerPos.Y = y;
                        break;
                }
            }
        }

        protected void LoadObjects()
        {
            XmlNode mapNode = TmxMap.mapNode;
            XmlNodeList objGroupNode = mapNode.SelectNodes("objectgroup");
            XmlNodeList ObjsNodes = objGroupNode[(int)ObjectGroups.Objs].SelectNodes("object");
            Object[] objects = new Object[ObjsNodes.Count];

            for (int i = 0; i < ObjsNodes.Count; i++)
            {
                int x = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(ObjsNodes[i], "x"));
                int y = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(ObjsNodes[i], "y"));
                int width = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(ObjsNodes[i], "width"));
                int height = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(ObjsNodes[i], "height"));

                bool solid = TmxMap.GetBoolAttribute(ObjsNodes[i], "solid");

                switch ((Objs)i)
                {
                    case Objs.DoorOpenOne:
                        objects[i] = new Door("tileset", Game.PositionFake, solid, false, Game.Scenes[(int)ScenesType.PlayerHouse], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case Objs.DoorOpenTwo:
                        objects[i] = new Door("tileset", Game.PositionFake, solid, false, Game.Scenes[(int)ScenesType.PrincessHouse], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case Objs.DoorCloseOne:
                        objects[i] = new Door("tileset", Game.PositionFake, solid, true, Game.Scenes[(int)ScenesType.DogHouse], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case Objs.Rock:
                        objects[i] = new Rock("tileset", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;
                }

                objects[i].Position = new Vector2(x, y) + objects[i].Pivot;
                objects[i].IsActive = true;
            }

            if (DogHouseDoorUnlocked) ((Door)objects[(int)Objs.DoorCloseOne]).SetOpenDoor();

            this.objects = objects;
        }

        protected override void LoadAssets()
        {
            base.LoadAssets();

            GfxMgr.AddTexture("princessIdle_0", "Assets/Graphics/PrincessSheet/PrincessIdleRight.png");
            GfxMgr.AddTexture("princessIdle_1", "Assets/Graphics/PrincessSheet/PrincessIdleLeft.png");
            GfxMgr.AddTexture("princessIdle_2", "Assets/Graphics/PrincessSheet/PrincessIdleDown.png");
            GfxMgr.AddTexture("princessIdle_3", "Assets/Graphics/PrincessSheet/PrincessIdleUp.png");

            GfxMgr.AddClip("Overworld", "Assets/Sounds/Overworld.wav");
            GfxMgr.AddClip("DialogueSFX", "Assets/Sounds/Sfx/DialogueSFX.wav");
            GfxMgr.AddClip("OpenDoor", "Assets/Sounds/Sfx/OpenDoor.wav");
        }

        public override void Input()
        {
            Player.UpdateStateMachine();
            InteractMgr.Input();
        }

        public override void Update()
        {
            sourceEmitter[(int)MainNameClips.Overworld].Stream(clips[(int)MainNameClips.Overworld], Game.Window.DeltaTime);

            PhysicsMgr.Update();
            UpdateMgr.Update();
            CameraMgr.Update();

            PhysicsMgr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            base.OnExit();

            Player.Destroy();
            princess.Destroy();
            bg.Destroy();

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].Destroy();
            }

            TmxMap = null;

            UpdateMgr.ClearAll();
            PhysicsMgr.ClearAll();
            DrawMgr.ClearAll();
            GfxMgr.ClearAll();
            FontMgr.ClearAll();
            CameraMgr.ClearAll();
            InteractMgr.ClearAll();
            ClearAudio();

            Hearts.Destroy();

            Game.PrevScene = this;

            //OnExit() of Scene
            return base.OnExit();
        }

        protected void ClearAudio()
        {
            for (int i = 0; i < (int)MainNameClips.LAST; i++)
            {
                sourceEmitter[i].Stop();
                sourceEmitter[i] = null;
                clips[i] = null;
            }

            sourceEmitter = null;
            clips = null;
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }
    }
}
