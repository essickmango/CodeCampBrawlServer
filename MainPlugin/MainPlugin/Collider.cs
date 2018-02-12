using UnityEngine;

namespace MainPlugin
{
   public abstract class Collider
   {
       public STransform Transform;

       public abstract bool IsColliding(Collider other);
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
                return CollisionManager.CollideCircleAndBoxColliders(this, (CircleCollider)other);
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
