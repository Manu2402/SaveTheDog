using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace LastProject
{
    enum DrawLayer { Background, Middleground, Playground, Foreground, GUI, LAST }

    static class DrawMgr
    {
        private static List<IDrawable>[] items;
        private static RenderTexture sceneTexture;
        private static Sprite scene;

        private static Dictionary<string, PostProcessingEffect> postFX;

        static DrawMgr()
        {
            items = new List<IDrawable>[(int)DrawLayer.LAST];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new List<IDrawable>();
            }
        }

        public static void InitRenderTexture()
        {
            sceneTexture = new RenderTexture(Game.Window.Width, Game.Window.Height);
            scene = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);
            scene.Camera = CameraMgr.GetCamera("GUI");

            postFX = new Dictionary<string, PostProcessingEffect>();
        }

        public static void AddFX(string fxName, PostProcessingEffect fx)
        {
            if (!postFX.ContainsKey(fxName))
            {
                postFX.Add(fxName, fx);
            }
        }

        public static void RemoveFX(string fxName)
        {
            postFX.Remove(fxName);
        }

        private static void ApplyPostFX()
        {
            foreach (var item in postFX)
            {
                sceneTexture.ApplyPostProcessingEffect(item.Value);
            }
        }

        public static bool Contains(IDrawable item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Contains(item)) return true;
            }

            return false;
        }

        public static void AddItem(IDrawable item)
        {
            items[(int)item.Layer].Add(item);
        }

        public static void RemoveItem(IDrawable item)
        {
            items[(int)item.Layer].Remove(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }
        }

        public static void ClearEffects()
        {
            sceneTexture = null;
            scene = null;
            postFX.Clear();
            postFX = null;
        }


        public static void Draw()
        {
            if (Game.CurrentScene.GetType() == typeof(UndergroundScene))
            {
                Game.Window.RenderTo(sceneTexture);
                for (int i = 0; i < items.Length; i++)
                {
                    if ((DrawLayer)i == DrawLayer.GUI)
                    {
                        ApplyPostFX();
                        Game.Window.RenderTo(null);
                        scene.DrawTexture(sceneTexture);
                    }

                    for (int j = 0; j < items[i].Count; j++)
                    {
                        items[i][j].Draw();
                    }
                }
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    for (int j = 0; j < items[i].Count; j++)
                    {
                        items[i][j].Draw();
                    }
                }
            }
        }
    }
}
