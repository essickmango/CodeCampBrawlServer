using System;
using UnityEngine;

namespace MainPlugin
{
    class CollisionManager
    {

        public static bool CollideBoxColliders(BoxCollider col1, BoxCollider col2)
        {
            Vector2 min1 = new Vector2(col1.Transform.Position.x - col1.Size.x/2, col1.Transform.Position.y - col1.Size.y/2 );
            Vector2 max1 = new Vector2(col1.Transform.Position.x + col1.Size.x/2, col1.Transform.Position.y + col1.Size.y/2);

            Vector2 min2 = new Vector2(col2.Transform.Position.x - col2.Size.x / 2, col2.Transform.Position.y - col2.Size.y / 2);
            Vector2 max2 = new Vector2(col2.Transform.Position.x + col2.Size.x / 2, col2.Transform.Position.y + col2.Size.y / 2);
            if ((min1.x >= min2.x && min1.x < max2.x) ||(max1.x <= max2.x && max1.x > min2.x))
            {
                if ((min1.y >= min2.y && min1.y < max2.y) || (max1.y <= max2.y && max1.y > min2.y))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CollideCircleAndBoxColliders(BoxCollider box, CircleCollider circle)
        {
            float distX = Math.Abs(circle.Transform.Position.x - box.Transform.Position.x);
            float distY = Math.Abs(circle.Transform.Position.y - box.Transform.Position.y);

            //If the distance is greater than halfCircle +halfRect, then they are too far apart to be colliding
            if (distX > (box.Size.x / 2 + circle.Radius) || distY > (box.Size.y / 2 + circle.Radius))
            {
                return false;
            }

            if (distX <= (box.Size.x / 2) || distY <= (box.Size.y / 2))
            {
                return true;
            }

            double cornerdistanceSqr = Math.Pow((distX - box.Size.x / 2), 2) + Math.Pow((distY - box.Size.y / 2), 2);
            return (cornerdistanceSqr <= Math.Pow((circle.Radius), 2));
        }


        public static bool IsPointInBox(Vector2 point, BoxCollider box)
        {
            if (point.x < (box.Transform.Position.x + box.Size.x / 2) && point.x > (box.Transform.Position.x - box.Size.x / 2))
            {
                if (point.y < (box.Transform.Position.y + box.Size.y / 2) && point.y > (box.Transform.Position.y - box.Size.y / 2))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
