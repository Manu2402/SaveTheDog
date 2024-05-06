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
    enum UndergroundNameClips { Underworld, DialogueSFX, PlatformSFX, PickupSFX, Hurt, LAST } //Clips of UndergroundScene

    enum ObjsInUndergroundScene { Platform_0, Platform_1, Platform_2, Platform_3, Platform_4, Stair, Key } //Public

    class UndergroundScene : MapScene
    {
        protected Background bg;
        protected Object[] objects;

        public bool AfterPressedAllPlatformState = false;
        public bool HasPickedUpKey = false;

        protected TmxMap TmxMap { get; set; }

        public override void Reload()
        {
            AfterPressedAllPlatformState = false;
            HasPickedUpKey = false;
        }

        public override void Start()
        {
            ((PrincessHouseScene)Game.Scenes[(int)ScenesType.PrincessHouse]).GetIntoByUnderground = true;

            DrawMgr.InitRenderTexture();
            DrawMgr.AddFX("Vignette", new DarkFX());
            
            LoadAssets();

            sourceEmitter = new AudioSource[(int)UndergroundNameClips.LAST];
            clips = new AudioClip[(int)UndergroundNameClips.LAST];
            LoadSounds();

            bg = new Background("tileset", "Assets/Config/UndergroundSceneMap.tmx");

            //Getting map created in tmxMap contained in bg
            TmxMap = bg.TiledMap;
            Map = TmxMap.Map;

            //Camera
            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
            CameraMgr.Init(cameraLimits);
            CameraMgr.AddCamera("GUI");

            Player = new Player("PlayerWalk_1");
            CameraMgr.SetTarget(Player);
            Player.SetCamera(CameraMgr.MainCamera);

            LoadPositions();

            LoadObjects();

            if (!HasPickedUpKey)
            {
                ((UndergroundScene)Game.CurrentScene).Map.ToggleNode(24, 31); //Key
            }

            base.Start();

            //Hearts
            Hearts = new HealthGUI(new Vector2(1f, 4f));
            Hearts.IsActive = true;
        }

        public void SpawnKey()
        {
            objects[(int)ObjsInUndergroundScene.Key].IsActive = true;
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

                switch ((ObjsInUndergroundScene)i)
                {
                    case ObjsInUndergroundScene.Platform_0:
                    case ObjsInUndergroundScene.Platform_1:
                    case ObjsInUndergroundScene.Platform_2:
                    case ObjsInUndergroundScene.Platform_3:
                    case ObjsInUndergroundScene.Platform_4:

                        objects[i] = new Platform("platform", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        
                        if (AfterPressedAllPlatformState)
                        {
                            ((Platform)objects[i]).IsPressed = true;
                            PlatformMgr.CounterPlatformPressed++;
                        }

                        PlatformMgr.AddItem((Platform)objects[i]);

                        break;

                    case ObjsInUndergroundScene.Stair:
                        objects[i] = new Stair("tileset", new Vector2(24, 19), solid, Game.Scenes[(int)ScenesType.PrincessHouse], DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;

                    case ObjsInUndergroundScene.Key:
                        objects[i] = new Key("key_ico", solid, DrawLayer.Middleground, spriteWidth: width, spriteHeight: height);
                        break;
                }

                objects[i].Position = new Vector2(x, y) + objects[i].Pivot;
                objects[i].IsActive = true;
            }

            objects[(int)ObjsInUndergroundScene.Key].IsActive = false;

            this.objects = objects;
        }

        protected void LoadSounds()
        {
            for (int i = 0; i < clips.Length; i++)
            {
                switch ((UndergroundNameClips)i)
                {
                    case UndergroundNameClips.Underworld:
                        sourceEmitter[i] = new AudioSource(42);
                        clips[i] = GfxMgr.GetClip("Underworld");
                        break;

                    case UndergroundNameClips.DialogueSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("DialogueSFX");
                        break;

                    case UndergroundNameClips.PlatformSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("PlatformSFX");
                        break;

                    case UndergroundNameClips.PickupSFX:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("PickupSFX");
                        break;

                    case UndergroundNameClips.Hurt:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("Hurt");
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
            sourceEmitter[(int)UndergroundNameClips.Underworld].Stream(clips[(int)UndergroundNameClips.Underworld], Game.Window.DeltaTime);

            PhysicsMgr.Update();
            UpdateMgr.Update();
            CameraMgr.Update();

            PhysicsMgr.CheckCollisions();

            PlatformMgr.Update();
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
            DrawMgr.ClearEffects();
            GfxMgr.ClearAll();
            InteractMgr.ClearAll();
            FontMgr.ClearAll();
            CameraMgr.ClearAll();
            PlatformMgr.ClearAll();
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
            for (int i = 0; i < (int)UndergroundNameClips.LAST; i++)
            {
                if (sourceEmitter != null)
                {
                    sourceEmitter[i].Stop();
                    sourceEmitter[i] = null;
                    clips[i] = null;
                }
            }

            sourceEmitter = null;
            clips = null;
        }

        protected override void LoadAssets()
        {
            base.LoadAssets();

            GfxMgr.AddTexture("platform", "Assets/Graphics/Objects/platform.png");

            GfxMgr.AddClip("Underworld", "Assets/Sounds/Underworld.wav");
            GfxMgr.AddClip("DialogueSFX", "Assets/Sounds/Sfx/DialogueSFX.wav");
            GfxMgr.AddClip("PlatformSFX", "Assets/Sounds/Sfx/PlatformActivated.wav");
            GfxMgr.AddClip("PickupSFX", "Assets/Sounds/SFX/PickupSFX.wav");
            GfxMgr.AddClip("Hurt", "Assets/Sounds/SFX/Hurt.wav");
        }

    }
}
