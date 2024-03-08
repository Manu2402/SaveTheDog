using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;
using Aiv.Fast2D;
using System.Xml;
using OpenTK;

namespace LastProject
{
    enum PrincessHouseNameClips { Overworld, DialogueSFX, LAST } //Clips of PrincessHouseScene  

    enum ObjsInPrincessHouseScene //Public because this enum will also be exploited in other classes
    {
        Book, Door, Pot, Stair
    }

    class PrincessHouseScene : MapScene
    {
        protected Background bg;

        public Object[] objects { get; protected set; } //Public in get for Pot class
        protected TmxMap TmxMap { get; set; }

        public PrincessHouseScene() : base() { }

        public bool UnlockedStair = false; //Different states in this house
        public bool GetIntoByUnderground = false;

        public override void Reload()
        {
            UnlockedStair = false;
            GetIntoByUnderground = false;
        }

        public override void Start()
        {
            LoadAssets();

            sourceEmitter = new AudioSource[(int)PrincessHouseNameClips.LAST];
            clips = new AudioClip[(int)PrincessHouseNameClips.LAST];

            LoadSounds();

            bg = new Background("tileset", "Assets/Config/PrincessHouseSceneMap.tmx");
            TmxMap = bg.TiledMap;
            Map = TmxMap.Map;

            //Camera
            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
            CameraMgr.Init(cameraLimits);

            switch (Game.PrevScene.GetType().Name)
            {
                case "UndergroundScene":
                    Player = new Player("PlayerWalk_1");
                    Player.Position = Game.PositionPlayer;
                    break;

                case "MainScene":
                    Player = new Player("PlayerWalk_0");
                    LoadPositions();
                    break;
            }

            CameraMgr.SetTarget(Player);
            Player.SetCamera(CameraMgr.MainCamera);

            LoadObjects();

            base.Start();

            //Hearts
            Hearts = new HealthGUI(new Vector2(1f, 4f));
            Hearts.IsActive = true;
        }

        protected void LoadPositions()
        {
            XmlNode mapNode = TmxMap.mapNode;
            XmlNodeList objGroupNode = mapNode.SelectNodes("objectgroup");
            //0 is the single position, that is the player's position
            XmlNode playerNode = objGroupNode[0].SelectSingleNode("object");

            int x = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(playerNode, "x"));
            int y = (int)Game.PixelsToUnits(TmxMap.GetIntAttribute(playerNode, "y"));

            Player.Position = new Vector2(x, y);
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

                switch ((ObjsInPrincessHouseScene)i)
                {
                    case ObjsInPrincessHouseScene.Book:
                        objects[i] = new Book("book", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case ObjsInPrincessHouseScene.Door:
                        objects[i] = new Door("tileset", new Vector2(24, 14), solid, false, Game.Scenes[(int)ScenesType.Main], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case ObjsInPrincessHouseScene.Pot:
                        objects[i] = new Pot("pot", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case ObjsInPrincessHouseScene.Stair:
                        objects[i] = new Stair("tileset", new Vector2(24, 18), solid, Game.Scenes[(int)ScenesType.Underground], DrawLayer.Middleground, 96, 241, spriteWidth: width, spriteHeight: height);
                        break;
                }

                objects[i].Position = new Vector2(x, y) + objects[i].Pivot;
                objects[i].IsActive = true;
            }

            ((Pot)objects[(int)ObjsInPrincessHouseScene.Pot]).RemoveNodeOnMe();
            ((Stair)objects[(int)ObjsInPrincessHouseScene.Stair]).IsActive = false;

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
            for (int i = 0; i < (int)PrincessHouseNameClips.LAST; i++)
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
            GfxMgr.AddTexture("pot", "Assets/Graphics/Objects/pot.png");

            GfxMgr.AddClip("Overworld", "Assets/Sounds/Overworld.wav");
            GfxMgr.AddClip("DialogueSFX", "Assets/Sounds/SFX/DialogueSFX.wav");
        }
    }
}

