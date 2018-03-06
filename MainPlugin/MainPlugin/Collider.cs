using System;
using UnityEngine;

namespace MainPlugin
{
    public abstract class Collider
    {
        public STransform Transform;

        public abstract bool IsColliding(Collider other);

        public Vector2 GetReflection(Vector2 velocity, Collider other)
        {
            if (other.GetType() == typeof(BoxCollider))
            {
                BoxCollider box = (BoxCollider) other;
                Vector2 middleDis = new Vector2(Math.Abs(Transform.Position.x - box.Transform.Position.x),
                    Math.Abs(Transform.Position.y - box.Transform.Position.y));

                if (Math.Abs(middleDis.x - box.Size.x / 2) <= Math.Abs(middleDis.y - box.Size.y / 2))
                {
                    return new Vector2(velocity.x * (-1), velocity.y);
                }

                return new Vector2(velocity.x, velocity.y * (-1));

            }

            if (other.GetType() == typeof(CircleCollider))
            {
                CircleCollider circle = (CircleCollider) other;
                Vector2 middleDis = new Vector2(Transform.Position.x - circle.Transform.Position.x,
                    Transform.Position.y - circle.Transform.Position.y);
                //siehe OneNote CCB > CCB > Reflection für grafische Darstellung der Herleitung der Gleichung
                Vector2 reflection = Vector2.Dot(middleDis, velocity) * 2 * middleDis - velocity;
                return reflection;
            }
            Server.Instance.Log(velocity.ToString());
            Server.Instance.Log(other.ToString());
            Server.Instance.Log("This is a Bug in GetReflection.. ur welcome :3");
            return Vector2.down;
        }
    }


    public class BoxCollider : Collider
    {
        public Vector2 Size;

        public BoxCollider(STransform transform, Vector2 size)
        {
            Transform = transform;
            Size = size;
        }

        public override bool IsColliding(Collider other)
        {
            if (other.GetType() == typeof(BoxCollider))
            {
                return CollisionManager.CollideBoxColliders(this, (BoxCollider) other);
            }
            if (other.GetType() == typeof(CircleCollider))
            {
                return CollisionManager.CollideCircleAndBoxColliders(this, (CircleCollider) other);
            }
            return false;
        }

    }

    public class CircleCollider : Collider
    {
        public float Radius;

        public CircleCollider(STransform transform, float radius)
        {
            Transform = transform;
            Radius = radius;
        }

        public override bool IsColliding(Collider other)
        {
            if (other.GetType() == typeof(BoxCollider))
            {
                return CollisionManager.CollideCircleAndBoxColliders((BoxCollider) other, this);
            }
            return false;
        }

    }

}
