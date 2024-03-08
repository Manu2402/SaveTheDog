using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    interface IDrawable
    {
        DrawLayer Layer { get; }
        void Draw();
    }
}
