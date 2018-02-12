using DarkRift;
using UnityEngine;

namespace MainPlugin
{
    public class Blood : TickObject
    {
        public STransform Transform;
        public Collider Collider;
        public Vector2 Velocity;
        public Character Owner;

        public ushort Id;

        private const float Gravity = 1f;


        public void OnHit()
        {
            Game.GameTick -= Tick;
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(Id);
            Owner.Owner.Client.SendMessage(Message.Create((ushort) Tags.KillObject, writer), SendMode.Reliable);
            Arealight.CreateA(new CircleCollider(Transform, 1f), Game, 4);

        }

        public static Blood FireBlood(Vector2 position, Game game, Character owner)
        {
            Blood arrow = new Blood();
            arrow.Owner = owner;
            position = new Vector2(position.x+0.2f,position.y);
            arrow.Transform = new STransform(position, 0);
            arrow.Collider = new CircleCollider(arrow.Transform, 0.2f);

            arrow.Velocity = new Vector2(0,0);
            arrow.Game = game;
            arrow.Game.GameTick += arrow.Tick;

            arrow.Id = game.NextObjectId++;

            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write((ushort)5);
            writer.Write(arrow.Id);
            writer.Write(arrow.Transform.Position.x);
            writer.Write(arrow.Transform.Position.y);

            owner.Owner.Client.SendMessage(Message.Create((ushort)Tags.SpawnObject, writer), SendMode.Reliable);

            return arrow;
        }

        public override void Tick()
        {
            Vector2 oldPos = Transform.Position;

            Velocity.y -= Gravity * Clock.DeltaTime;

            Transform.Translate(new Vector2(0, Velocity.y * Clock.DeltaTime));
            if (Game.CollideWithMap(Collider))
            {
                Transform.Translate(new Vector2(0, -Velocity.y * Clock.DeltaTime));
                OnHit();

            }

            Transform.Translate(new Vector2(Velocity.x * Clock.DeltaTime, 0));
            if (Game.CollideWithMap(Collider))
            {
                Transform.Translate(new Vector2(-Velocity.x * Clock.DeltaTime, 0));
                OnHit();
            }

            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(Id);
            writer.Write(Transform.Position.x);
            writer.Write(Transform.Position.y);
            Owner.Owner.Client.SendMessage(Message.Create((ushort) Tags.MoveObject, writer), SendMode.Reliable);

        }


    }
}
