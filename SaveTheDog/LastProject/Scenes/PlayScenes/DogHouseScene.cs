using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using Aiv.Audio;
using System.Xml;

namespace LastProject
{
    class DogHouseScene : MapScene
    {
        #region ENUMS

        enum ObjectGroups //Type of ObjectGroups in XML
        {
            NPC, Objs, LAST
        }

        enum NPCs //ObjectGroups[0]
        {
            Dog, Player, LAST
        }

        enum Objs //ObjectGroups[1]
        {
            Door, Pickaxe
        }

        #endregion

        protected Background bg;
        protected Object[] objects;
        public bool HasPickedUpPickaxe;

        protected NPC dog;

        protected Vector2 PlayerPos;
        protected Vector2 DogPos;

        protected TmxMap TmxMap { get; set; }

        public DogHouseScene() : base() { }

        public override void Reload()
        {
            HasPickedUpPickaxe = false;
        }

        public override void Start()
        {
            LoadAssets();

            sourceEmitter = new AudioSource[(int)PlayerHouseNameClips.LAST]; //Same as PlayerHouse
            clips = new AudioClip[(int)PlayerHouseNameClips.LAST];

            LoadSounds();

            bg = new Background("tileset", "Assets/Config/DogHouseSceneMap.tmx"); 
            TmxMap = bg.TiledMap;
            Map = TmxMap.Map;

            //Camera
            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
            CameraMgr.Init(cameraLimits);
            LoadPositions();

            //Player
            Player = new Player("PlayerWalk_0");
            CameraMgr.SetTarget(Player);
            Player.SetCamera(CameraMgr.MainCamera);
            Player.Position = PlayerPos;

            if (!HasPickedUpPickaxe)
            {
                ((DogHouseScene)Game.CurrentScene).Map.ToggleNode(33, 34); //Pickaxe
            }

            LoadObjects();

            base.Start();

            //Hearts
            Hearts = new HealthGUI(new Vector2(1f, 4f));
            Hearts.IsActive = true;

            //Dog
            dog = new Dog("dogIdle_2");
            dog.Position = DogPos;
            dog.RemoveNodeOnMe();
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
                    case NPCs.Dog:
                        DogPos.X = x;
                        DogPos.Y = y;
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
            XmlNode objGroupNode = mapNode.SelectNodes("objectgroup")[1];
            XmlNodeList ObjsNodes = objGroupNode.SelectNodes("object");

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
                    case Objs.Door:
                        objects[i] = new Door("tileset", new Vector2(32, 14), solid, false, Game.Scenes[(int)ScenesType.Main], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case Objs.Pickaxe:
                        objects[i] = new Pickaxe("pickaxe_ico", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;
                }

                objects[i].Position = new Vector2(x, y) + objects[i].Pivot;
                objects[i].IsActive = true;
            }

            if (HasPickedUpPickaxe) objects[(int)Objs.Pickaxe].IsActive = false;

            this.objects = objects;
        }

        protected void LoadSounds()
        {
            for (int i = 0; i < clips.Length; i++)
            {
                switch ((PlayerHouseNameClips)i)
                {
                    case PlayerHouseNameClips.Overworld:
                        sourceEmitter[i] = new AudioSource(42);
                        clips[i] = GfxMgr.GetClip("Overworld");
                        break;

                    case PlayerHouseNameClips.DialogueSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("DialogueSFX");
                        break;

                    case PlayerHouseNameClips.PickupSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("PickupSFX");
                        break;
                }
            }
        }

        public override void Input()
        {
            Player.UpdateStateMachine();
            InteractMgr.Input();
        }

        public override void Update()
        {
            sourceEmitter[(int)PlayerHouseNameClips.Overworld].Stream(clips[(int)PlayerHouseNameClips.Overworld], Game.Window.DeltaTime);

            PhysicsMgr.Update();
            UpdateMgr.Update();
            CameraMgr.Update();

            PhysicsMgr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            base.OnExit();

            Player.Destroy();
            dog.Destroy();
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
            InteractMgr.ClearAll();
            CameraMgr.ClearAll();
            ClearAudio();

            Hearts.Destroy();

            Game.PrevScene = this;

            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }

        protected void ClearAudio()
        {
            for (int i = 0; i < (int)PlayerHouseNameClips.LAST; i++)
            {
                sourceEmitter[i].Stop();
                sourceEmitter[i] = null;
                clips[i] = null;
            }

            sourceEmitter = null;
            clips = null;
        }

        protected override void LoadAssets()
        {
            base.LoadAssets();

            GfxMgr.AddTexture("book", "Assets/Graphics/Objects/book.png");
            GfxMgr.AddTexture("dogIdle_0", "Assets/Graphics/DogSheet/DogIdleRight.png");
            GfxMgr.AddTexture("dogIdle_1", "Assets/Graphics/DogSheet/DogIdleLeft.png");
            GfxMgr.AddTexture("dogIdle_2", "Assets/Graphics/DogSheet/DogIdleDown.png");
            GfxMgr.AddTexture("dogIdle_3", "Assets/Graphics/DogSheet/DogIdleUp.png");

            GfxMgr.AddClip("Overworld", "Assets/Sounds/Overworld.wav");
            GfxMgr.AddClip("DialogueSFX", "Assets/Sounds/SFX/DialogueSFX.wav");
            GfxMgr.AddClip("PickupSFX", "Assets/Sounds/SFX/PickupSFX.wav");
        }
    }
}
