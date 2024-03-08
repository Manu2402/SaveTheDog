using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class InteractMgr //List of interactables
    {

        private static List<IInteractable> items;

        static InteractMgr()
        {
            items = new List<IInteractable>();
        }

        public static void AddItem(IInteractable item)
        {
            items.Add(item);
        }

        public static void RemoveItem(IInteractable item)
        {
            items.Remove(item);
        }

        public static void ClearAll()
        {
            items.Clear();
        }

        public static void Input()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Input();
            }
        }
    }
}
