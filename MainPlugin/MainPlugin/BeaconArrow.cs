using DarkRift;
using UnityEngine;

namespace MainPlugin
{
    class BeaconArrow: TickObject
    {
        public STransform Transform;
        public Collider Collider;
        public Vector2 Velocity;
        public Character Owner;

        public ushort Id;

        private const float Gravity = 4f;
        private const float ShootStrenght = 15f;

        private bool ignited;

        public Light Light;

        public void OnHit(Collider other)
        {
            //       Game.GameTick -= Tick;

            //       Light?.Dispose();

            Velocity = Collider.GetReflection(Velocity, other);

        }

        public void Ignite()
        {
            ignited = true;
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(Id);
            Owner.Owner.Client.SendMessage(Message.Create((ushort)Tags.KillObject, writer), SendMode.Reliable);


            //trandform to light arrow
            Id = Game.NextObjectId++;
            writer = DarkRiftWriter.Create();
            writer.Write((ushort)1);
            writer.Write(Id);
            writer.Write(Transform.Position.x);
            writer.Write(Transform.Position.y);

            Game.SendMessageToAll(Message.Create((ushort)Tags.SpawnObject, writer));

            Light = Light.Create(new CircleCollider(Transform, 2f), Game);

        }

        public static BeaconArrow ShootArrow(Vector2 position, Vector2 direction, Game game, Character owner)
        {
            BeaconArrow arrow = new BeaconArrow();
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
            writer.Write((ushort)0);
            writer.Write(arrow.Id);
            writer.Write(arrow.Transform.Position.x);
            writer.Write(arrow.Transform.Position.y);

            owner.Owner.Client.SendMessage(Message.Create((ushort) Tags.SpawnObject, writer), SendMode.Reliable);

            return arrow;
        }


        public override void Tick()
        {
            base.Tick();
            if (FramesAlive == 120 && !ignited)
            {
                Ignite();
            }
            if (FramesAlive == 240)
            {
                Dispose();
                DarkRiftWriter writer = DarkRiftWriter.Create();
                writer.Write(Id);
                Game.SendMessageToAll(Message.Create((ushort)Tags.KillObject, writer));
                Light = Arealight.CreateA(new CircleCollider(Transform, 3f), Game, 2);
            }

            Vector2 oldPos = Transform.Position;

            Velocity.y -= Gravity * Clock.DeltaTime;

            if (Velocity.y < 0 && !ignited)
            {
             //   Ignite();   
            }

            Transform.Translate(new Vector2(0, Velocity.y * Clock.DeltaTime));
            Collider other = Game.CollideWithMapReturnCollider(Collider);
            if (other != null)
            {
                Transform.Translate(new Vector2(0, -Velocity.y * Clock.DeltaTime));
                OnHit(other);
                    
            }

            Transform.Translate(new Vector2(Velocity.x * Clock.DeltaTime, 0));
            other = Game.CollideWithMapReturnCollider(Collider);
            if (other != null)
            {
                Transform.Translate(new Vector2(-Velocity.x * Clock.DeltaTime, 0));
                OnHit(other);
            }

            if (!ignited)
            {
                DarkRiftWriter writer = DarkRiftWriter.Create();
                writer.Write(Id);
                writer.Write(Transform.Position.x);
                writer.Write(Transform.Position.y);
                Owner.Owner.Client.SendMessage(Message.Create((ushort)Tags.MoveObject, writer), SendMode.Reliable);
            }
            else
            {
                DarkRiftWriter writer = DarkRiftWriter.Create();
                writer.Write(Id);
                writer.Write(Transform.Position.x);
                writer.Write(Transform.Position.y);
                Game.SendMessageToAll(Message.Create((ushort)Tags.MoveObject, writer));
            }

        }
    }
}
