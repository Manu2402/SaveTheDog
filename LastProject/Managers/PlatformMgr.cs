using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    static class PlatformMgr
    {
        private static List<Platform> items;
        private static int numPlatforms = 5;
        public static int CounterPlatformPressed = 0;

        static PlatformMgr()
        {
            items = new List<Platform>();
        }

        public static void AddItem(Platform item)
        {
            items.Add(item);
        }

        public static void RemoveItem(Platform item)
        {
            items.Remove(item);
        }

        public static Platform GetPlatformAtIndex(int index)
        {
            if (index < numPlatforms)
            {
                return items[index];
            }

            return null;
        }

        public static void ResetPlatforms()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].IsPressed = false;
            }
        }

        public static void ClearAll()
        {
            items.Clear();
        }

        public static void Update()
        {
            if (CounterPlatformPressed >= numPlatforms && !((UndergroundScene)Game.CurrentScene).HasPickedUpKey)
            {
                //Get pressed for the rest of the game and spawn key
                ((UndergroundScene)Game.CurrentScene).AfterPressedAllPlatformState = true;
                ((UndergroundScene)Game.CurrentScene).SpawnKey();
            }
        }

    }
}
