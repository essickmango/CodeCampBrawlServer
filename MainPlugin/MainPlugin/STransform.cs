using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainPlugin
{
    public class STransform
    {
        public Vector2 Position;
        public float Rotation;

        public STransform(Vector2 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
        }


        public void Translate(Vector2 direction)
        {
            Position += direction;
        }
    }
}
