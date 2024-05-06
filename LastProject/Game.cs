using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    static class Game
    {
        //Variables
        public static Window Window;

        //Properties
        public static float OptimalScreenHeight;
        public static float UnitSize { get; private set; }
        public static float OptimalUnitSize { get; private set; }
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static float ScreenCenterX { get { return Window.OrthoWidth * 0.5f; } }
        public static float ScreenCenterY { get { return Window.OrthoHeight * 0.5f; } }
        public static Vector2 ScreenCenter { get { return new Vector2(ScreenCenterX, ScreenCenterY); } }

        #region Utilities

        //Support variables for certain moments, for lack of time there will be no refactoring

        public static Scene[] Scenes { get; set; } //List of Scenes, used for get/assign a specific scene indicated with her enum
        
        public static Scene PrevScene;

        public static Vector2 PositionFake = new Vector2(-1f, -1f); //Position Fake

        //Variables of player that i need for all scenes
        public static float LastMaxSpeedPlayer;
        public static int LastEnergyPlayer;
        public static Vector2 PositionPlayer = PositionFake; //Start fake position
        public static bool HasBoots;
        public static bool HasKey;
        public static bool HasPickaxe;

        #endregion

        public static void Init()
        {
            Window = new Window(801, 800, "LastProject", false); //801 for fill completely the window
            Window.SetVSync(false);
            Window.SetDefaultViewportOrthographicSize(50);

            OptimalScreenHeight = 800;
            UnitSize = Window.Height / Window.OrthoHeight; //16
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight; //16

            //SCENES
            Scenes = new Scene[(int)ScenesType.LAST];

            //Assign the scenes
            TitleScene titleScene = new TitleScene("titleScreen","selector");
            Scenes[(int)ScenesType.Title] = titleScene;
            MainScene mainScene = new MainScene();
            Scenes[(int)ScenesType.Main] = mainScene;
            PlayerHouseScene playerHouseScene = new PlayerHouseScene();
            Scenes[(int)ScenesType.PlayerHouse] = playerHouseScene;
            PrincessHouseScene princessHouseScene = new PrincessHouseScene();
            Scenes[(int)ScenesType.PrincessHouse] = princessHouseScene;
            UndergroundScene undergroundScene = new UndergroundScene();
            Scenes[(int)ScenesType.Underground] = undergroundScene;
            DogHouseScene dogHouseScene = new DogHouseScene();
            Scenes[(int)ScenesType.DogHouse] = dogHouseScene;
            GameOverScene gameOverScene = new GameOverScene();
            Scenes[(int)ScenesType.GameOver] = gameOverScene;
            WinScene winScene = new WinScene();
            Scenes[(int)ScenesType.Win] = winScene;

            titleScene.NextScene = mainScene;
            mainScene.NextScene = null; //NextScene decided runtime
            playerHouseScene.NextScene = null; //NextScene decided runtime
            princessHouseScene.NextScene = null; //NextScene decided runtime
            undergroundScene.NextScene = null; //NextScene decided runtime
            gameOverScene.NextScene = mainScene;
            winScene.NextScene = null;

            CurrentScene = titleScene;
        }

        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / OptimalUnitSize;
        }

        public static float UnitsToPixels(float unitsSize)
        {
            return unitsSize * OptimalUnitSize;
        }

        public static void Play()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                //FPS
                Window.SetTitle($"Save The Dog!");

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();

                    if (nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                // INPUT
                CurrentScene.Input();

                // UPDATE
                CurrentScene.Update();

                // DRAW
                CurrentScene.Draw();

                Window.Update();
            }
        }
    }
}
