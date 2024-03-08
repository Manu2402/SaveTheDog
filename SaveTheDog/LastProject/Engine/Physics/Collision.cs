using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    enum CollisionType
    {
        None, RectsIntersection, CirclesIntersection, CirleRectIntersection
    }

    struct Collision
    {
        public Vector2 Delta;
        public GameObject Collider;
        public CollisionType Type;
    }
}
