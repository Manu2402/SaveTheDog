using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    static class ColliderFactory
    {
        public static CircleCollider CreateCircleFor(GameObject obj, bool innerCircle = true)
        {
            float radius;

            if(innerCircle)
            {
                if(obj.HalfWidth < obj.HalfHeight)
                {
                    radius = obj.HalfWidth;
                }
                else
                {
                    radius = obj.HalfHeight;
                }
            }
            else
            {
                radius = (float)Math.Sqrt(obj.HalfWidth * obj.HalfWidth + obj.HalfHeight * obj.HalfHeight);
            }

            return new CircleCollider(obj.RigidBody, radius);
        }

        public static BoxCollider CreateBoxFor(GameObject obj)
        {
            return new BoxCollider(obj.RigidBody, (float)obj.HalfWidth, (float)obj.HalfHeight);
        }
    }
}
