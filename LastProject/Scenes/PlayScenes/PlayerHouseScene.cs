using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Audio;

namespace LastProject
{
    enum PlayerHouseNameClips { Overworld, DialogueSFX, PickupSFX, LAST } //Clips of PlayerHouseScene

    class PlayerHouseScene : MapScene
    {
        enum Objs
        {
            Book, Door, Boots
        }

        protected Background bg;
        protected Object[] objects;
        public bool HasPickedUpBoots;

        protected TmxMap TmxMap { get; set; }

        public PlayerHouseScene() : base() { }

        public override void Reload()
        {
            HasPickedUpBoots = false;
        }

        public override void Start()
        {
            LoadAssets();

            sourceEmitter = new AudioSource[(int)PlayerHouseNameClips.LAST];
            clips = new AudioClip[(int)PlayerHouseNameClips.LAST];

            LoadSounds();

            bg = new Background("tileset", "Assets/Config/PlayerHouseSceneMap.tmx");
            TmxMap = bg.TiledMap;
            Map = TmxMap.Map;

            //Camera
            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
            CameraMgr.Init(cameraLimits);

            Player = new Player("PlayerWalk_0");
            CameraMgr.SetTarget(Player);
            Player.SetCamera(CameraMgr.MainCamera);

            LoadPositions();

            if (!HasPickedUpBoots)
            {
                ((PlayerHouseScene)Game.CurrentScene).Map.ToggleNode(17, 16); //Boots 
            }

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

                switch ((Objs)i)
                {
                    case Objs.Book:
                        objects[i] = new Book("book", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case Objs.Door:    //Position of a cell under the door into which the player entered (16, 14)
                        objects[i] = new Door("tileset", new Vector2(16, 14), solid, false, Game.Scenes[(int)ScenesType.Main], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case Objs.Boots:
                        objects[i] = new Boots("boots_ico", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;
                }

                objects[i].Position = new Vector2(x, y) + objects[i].Pivot;
                objects[i].IsActive = true;
            }

            if (HasPickedUpBoots) objects[(int)Objs.Boots].IsActive = false;

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
            InteractMgr.ClearAll();
            FontMgr.ClearAll();
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

            GfxMgr.AddClip("Overworld", "Assets/Sounds/Overworld.wav");
            GfxMgr.AddClip("DialogueSFX", "Assets/Sounds/SFX/DialogueSFX.wav");
            GfxMgr.AddClip("PickupSFX", "Assets/Sounds/SFX/PickupSFX.wav");
        }
    }
}
