using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Audio;

namespace LastProject
{
    class WinScene : Scene
    {
        enum WinSceneClips
        {
            Menu, LAST
        }

        private Texture texture;
        private Sprite sprite;

        private bool isEnterPressed = true;

        public override void Draw()
        {
            sprite.DrawTexture(texture);
        }

        public override void Start()
        {
            base.Start();

            LoadAssets();

            texture = GfxMgr.GetTexture("winScreen");
            sprite = new Sprite(Game.PixelsToUnits(texture.Width), Game.PixelsToUnits(texture.Height));

            sourceEmitter = new AudioSource[(int)WinSceneClips.LAST];
            clips = new AudioClip[(int)WinSceneClips.LAST];

            LoadSounds();
        }

        private void LoadSounds()
        {
            sourceEmitter[(int)WinSceneClips.Menu] = new AudioSource();
            clips[(int)WinSceneClips.Menu] = GfxMgr.GetClip("Menu");
        }

        public override void Input()
        {
            if (Game.Window.GetKey(KeyCode.Return))
            {
                if (!isEnterPressed)
                {
                    IsPlaying = false;
                    isEnterPressed = true;
                }
            }
            else
            {
                isEnterPressed = false;
            }
        }

        public override void Update()
        {
            sourceEmitter[(int)WinSceneClips.Menu].Stream(clips[(int)WinSceneClips.Menu], Game.DeltaTime);
        }

        public override Scene OnExit()
        {
            texture = null;
            sprite = null;

            GfxMgr.ClearAll();
            ClearAudio();

            return base.OnExit();
        }

        protected void ClearAudio()
        {
            for (int i = 0; i < (int)WinSceneClips.LAST; i++)
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
            GfxMgr.AddTexture("winScreen", "Assets/Graphics/winScreen.png");

            GfxMgr.AddClip("Menu", "Assets/Sounds/Menu.wav");
        }

    }
}
