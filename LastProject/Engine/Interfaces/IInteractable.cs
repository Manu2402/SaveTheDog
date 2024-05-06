using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    interface IInteractable //Interface for any obj that is clickable with input
    {
        Vector2 MousePos { get; set; }
        Vector2 ResetPos { get; set; }

        void Input();
        void ResetMousePos();
    }
}
