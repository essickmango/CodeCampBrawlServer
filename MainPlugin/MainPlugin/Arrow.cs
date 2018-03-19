using System;
using DarkRift;
using UnityEngine;

namespace MainPlugin
{
    public class Arrow : TickObject
    {
        public STransform Transform;
        public Collider Collider;
        public Vector2 Velocity;
        public Character Owner;

        public int Frame;

        public ushort Id;

        private const float Gravity = 2f;
        private const float ShootStrenght = 14f;

        public static Arrow FireArrow(Vector2 position, Vector2 direction, Game game, Character owner)
        {
            Arrow  arrow = new Arrow();
            arrow.Owner = owner;
            arrow.Transform = new STransform(position, 0);
            arrow.Collider = new CircleCollider(arrow.Transform, 0.3f);

            Vector2 force = direction;
            force.Normalize();
            force *= ShootStrenght;
            arrow.Velocity = force;
            arrow.Game = game;
            arrow.Game.GameTick += arrow.Tick;

            arrow.Id = game.NextObjectId++;

            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write((ushort)3);
            writer.Write(arrow.Id);
            writer.Write(arrow.Transform.Position.x);
            writer.Write(arrow.Transform.Position.y);

            arrow.Frame = game.Frame;

            game.SendMessageToAll(Message.Create((ushort)Tags.SpawnObject, writer));

            return arrow;
        }



        public override void Tick()
        {
            if (Game.Frame > Frame+600)
            {
                Game.GameTick -= Tick;
                Dispose();
            }

            Vector2 oldPos = Transform.Position;

            Velocity.y -= Gravity * Clock.DeltaTime;

            Transform.Translate(new Vector2(Velocity.x * Clock.DeltaTime, Velocity.y * Clock.DeltaTime));

            //if hit player disapear and do dmg
            Character hit = Game.HitEnemyCharacter(Collider, Owner);
            bool hitMapObject = Game.CollideWithMap(Collider);
            if (hit!= null)
            {
                //do dmg
                hit.TakeDmg(40, Owner);
                Dispose();
            }

            if (hitMapObject)
            {
                Dispose();
            }

            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(Id);
            writer.Write(Transform.Position.x);
            writer.Write(Transform.Position.y);
            Game.SendMessageToAll(Message.Create((ushort) Tags.MoveObject, writer));


        }
        public override void Dispose()
        {
            base.Dispose();
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(Id);
            Game.SendMessageToAll(Message.Create((ushort)Tags.KillObject, writer));
        }

    }
}
