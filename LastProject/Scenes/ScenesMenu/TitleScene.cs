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
    //List of Sounds per AudioClips
    enum TitleNameClips { Menu, MenuCursor, LAST } //Clips of TitleScene

    class TitleScene : Scene
    {
        //Background images
        protected Texture texture;
        protected Sprite sprite;

        //Selection
        protected Texture selectionTexture;
        protected Sprite selectionSprite;

        //Paths
        protected string texturePath;
        protected string selectionPath;

        //Selector positions
        protected Vector2 upPosition;
        protected Vector2 downPosition;

        protected bool isUp;
        protected bool isEnterPressed;

        public TitleScene(string texturePath, string selectionPath)
        {
            this.texturePath = texturePath;
            this.selectionPath = selectionPath;
        }

        public override void Start()
        {
            base.Start();

            LoadAssets();

            //Positions for selection
            upPosition = new Vector2(Game.PixelsToUnits(275f), Game.PixelsToUnits(439f));
            downPosition = new Vector2(Game.PixelsToUnits(275f), Game.PixelsToUnits(606f));

            sourceEmitter = new AudioSource[(int)TitleNameClips.LAST];
            clips = new AudioClip[(int)TitleNameClips.LAST];

            //Initialize audioclips and audiosources
            for (int i = 0; i < clips.Length; i++)
            {
                switch ((TitleNameClips)i)
                {
                    case TitleNameClips.Menu:
                        sourceEmitter[i] = new AudioSource(42);
                        clips[i] = GfxMgr.GetClip("menu");
                        break;

                    case TitleNameClips.MenuCursor:
                        sourceEmitter[i] = new AudioSource();
                        clips[i] = GfxMgr.GetClip("menuCursor");
                        break;
                }
            }

            isUp = true;

            //Load image
            texture = GfxMgr.GetTexture(texturePath);
            sprite = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);

            //Load image selection hand
            selectionTexture = GfxMgr.GetTexture(selectionPath);
            selectionSprite = new Sprite(Game.PixelsToUnits(selectionTexture.Width), Game.PixelsToUnits(selectionTexture.Height));
            selectionSprite.scale = new Vector2(0.3f);
            selectionSprite.pivot = new Vector2(selectionSprite.Width * 0.5f, selectionSprite.Height * 0.5f);

            selectionSprite.position = upPosition;
        }

        protected override void LoadAssets()
        {
            GfxMgr.AddTexture("titleScreen", "Assets/Graphics/titleScreen.png");
            GfxMgr.AddTexture("selector", "Assets/Graphics/backhand.png");

            GfxMgr.AddClip("menu", "Assets/Sounds/menu.wav");
            GfxMgr.AddClip("menuCursor", "Assets/Sounds/Sfx/MenuCursor01.wav");
        }

        //Handler selector
        public override void Input()
        {
            if (Game.Window.GetKey(KeyCode.Up) && !isUp)
            {
                isUp = true;
                selectionSprite.position = upPosition;
                sourceEmitter[(int)TitleNameClips.MenuCursor].Play(clips[(int)TitleNameClips.MenuCursor]);
            }
            else if (Game.Window.GetKey(KeyCode.Down) && isUp)
            {
                isUp = false;
                selectionSprite.position = downPosition;
                sourceEmitter[(int)TitleNameClips.MenuCursor].Play(clips[(int)TitleNameClips.MenuCursor]);
            }

            if (Game.Window.GetKey(KeyCode.Return))
            {
                if (!isEnterPressed)
                {
                    isEnterPressed = true;

                    if (!isUp)
                    {
                        NextScene = null;
                    }

                    IsPlaying = false;
                }
            }
            else
            {
                isEnterPressed = false;
            }
        }

        protected void ClearAudio()
        {
            for (int i = 0; i < (int)TitleNameClips.LAST; i++)
            {
                sourceEmitter[i].Stop();
                sourceEmitter[i] = null;
                clips[i] = null;
            }

            sourceEmitter = null;
            clips = null;
        }

        public override Scene OnExit()
        {
            texture = null;
            sprite = null;

            ClearAudio();

            GfxMgr.ClearAll();

            Game.PrevScene = this;

            return base.OnExit();
        }

        public override void Update()
        {
            sourceEmitter[(int)TitleNameClips.Menu].Stream(clips[(int)TitleNameClips.Menu], Game.DeltaTime);
        }

        public override void Draw()
        {
            sprite.DrawTexture(texture);
            selectionSprite.DrawTexture(selectionTexture);
        }
    }
}
