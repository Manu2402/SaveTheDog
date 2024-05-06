using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    interface IMapScene //Interface for scenes that implements Map for pathfinding
    {
        Map Map { get; }
    }
}
