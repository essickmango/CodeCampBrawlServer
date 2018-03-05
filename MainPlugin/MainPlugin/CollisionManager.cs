using System;
using UnityEngine;

namespace MainPlugin
{
    class CollisionManager
    {

        public static bool CollideBoxColliders(BoxCollider col1, BoxCollider col2)
        {
            Vector2 middleDis = new Vector2(Math.Abs(col1.Transform.Position.x - col2.Transform.Position.x), Math.Abs(col1.Transform.Position.y - col2.Transform.Position.y));

            if (middleDis.x <= (col1.Size.x + col2.Size.x) / 2)
            {
                if (middleDis.y <= (col1.Size.y + col2.Size.y) / 2)
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

            //If the distance is greater than halfCircle + halfRect, then they are too far apart to be colliding
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


        public static Vector2 MoveObjToWall(Vector2 Velocity, Collider Coll, MapObject Wall)
        {
            Vector2 WallSize;
            Vector2 ObjSize;
            if (Wall.Collider.GetType() == typeof(BoxCollider))
            {
                WallSize = new Vector2(((BoxCollider)Wall.Collider).Size.x, ((BoxCollider)Wall.Collider).Size.y);
            }
            else
            {
                float WallRadius = ((CircleCollider)Wall.Collider).Radius;
                WallSize = new Vector2(WallRadius, WallRadius); //yes, I'm creating a square
            }

            if (Coll.GetType() == typeof(BoxCollider))
            {
                ObjSize = new Vector2(((BoxCollider)Coll).Size.x, ((BoxCollider)Coll).Size.y);
            }
            else
            {
                float ObjRadius = ((CircleCollider)Coll).Radius;
                ObjSize = new Vector2(ObjRadius, ObjRadius);
            }
            float yDistance = ObjSize.y + WallSize.y + 0.001f; //minimum distance between Objects
            float xDistance = ObjSize.x + WallSize.x + 0.001f;

            Vector2 VelocityTry = new Vector2(Velocity.x, Velocity.y);
            //Try to multiply vector until x is next to wall, then check if y is in Obj
            VelocityTry *= (Math.Abs(Wall.Collider.Transform.Position.x - Coll.Transform.Position.x) - ObjSize.x - WallSize.x - 0.001f) / Velocity.x;
            Vector2 Point = VelocityTry + Coll.Transform.Position;

            if (Point.y >= Wall.Transform.Position.y - yDistance && Point.y <= Wall.Transform.Position.y + yDistance) 
            {
                return VelocityTry;
            }
            else
            {
                //the same with y, checking x, should give result if x didnt work
                VelocityTry = new Vector2(Velocity.x, Velocity.y);
                VelocityTry *= (Math.Abs(Wall.Collider.Transform.Position.y - Coll.Transform.Position.y) - ObjSize.y - WallSize.y - 0.001f) / Velocity.y;
                Point = VelocityTry + Coll.Transform.Position;
                if (Point.x >= Wall.Transform.Position.x - xDistance && Point.x <= Wall.Transform.Position.y + xDistance)
                {
                    return VelocityTry;
                }
            }
            //if nothing works
            return Velocity;
        }

    }
}
