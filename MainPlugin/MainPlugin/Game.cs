using System;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Random = System.Random;

namespace MainPlugin
{
    public class Game: IDisposable
    {
        public ushort GameId;
        private static ushort nextGameId;
        public string Name;
        public List<Player> Players;
        public List<MapObject> MapObjects;
        public List<Vector2> SpawnPoints;

        public List<Light> Lights;

        public ushort NextObjectId;

        public delegate void GameLogic();
        public event GameLogic GameTick;

        public GameUpdateMessage UpdateMessage;

        public int Frame;

        public static Game CreateGame(string gameName, GameCreationData gameCreationData)
        {
            Game game = new Game();
            game.Lights = new List<Light>();
            game.GameId = nextGameId++;
            game.Players = new List<Player>();
            game.Name = gameName;
            game.MapObjects = gameCreationData.MapObjects;
            game.SpawnPoints = gameCreationData.SpawnPoints;
            game.UpdateMessage = GameUpdateMessage.Create();
            Clock.Tick += game.Tick;

            return game;
        }


        public void JoinPlayer(Player player)
        {
            Character c = Character.Create(GetRandomSpawnPoint(), this, player);
            Players.Add(player);
            player.Game = this;
            player.Character = c; 
            player.Client.SendMessage(GetGameStartMessage(), SendMode.Reliable);
        }

        public Vector2 GetRandomSpawnPoint()
        {
            Random rnd = new Random();
            int r = rnd.Next(SpawnPoints.Count);
            return SpawnPoints[r];
        }

        public Message GetGameStartMessage()
        {
            DarkRiftWriter writer = DarkRiftWriter.Create();

            writer.Write(GameId);

            writer.Write((ushort)MapObjects.Count);
            foreach (MapObject go in MapObjects)
            {
                writer.Write(go.Type);
                if (go.Type == (ushort)MapObject.TypeIds.Wall)
                {
                    writer.Write(go.Transform.Position.x);
                    writer.Write(go.Transform.Position.y);
                    BoxCollider b = (BoxCollider) go.Collider;
                    writer.Write(b.Size.x);
                    writer.Write(b.Size.y);
                }


            }

            writer.Write((byte)Players.Count);
            foreach (Player p in Players)
            {
                writer.Write(p.Name);
                writer.Write(p.Character.Owner.PlayerId);
            }


            Message m = Message.Create((ushort) Tags.GameStartMessage, writer);

            return m;


        }

        public void Tick()
        {
            Frame++;
            foreach (Player p in Players)
            {
                p.Character.Tick();
            }

            GameTick?.Invoke();

            SendMessageToAll(UpdateMessage.GetMessage());
        }


        public void SendMessageToAll(Message message)
        {
            foreach (Player p in Players)
            {
                p.Client.SendMessage(message, SendMode.Reliable);
            }
        }


        public bool CollideWithMap(Collider col)
        {
            foreach (MapObject m in MapObjects)
            {
                if (col.IsColliding(m.Collider))
                {
                    return true;
                }
            }
            return false;

        }


        public Collider CollideWithMapReturnCollider(Collider col) //returns the colliding object
        {
            foreach (MapObject m in MapObjects)
            {
                if (col.IsColliding(m.Collider))
                {
                    return m.Collider;
                }
            }
            return null;
        }

        public bool IsEnlighted(Collider col)
        {
            foreach (Light l in Lights)
            {
                if (col.IsColliding(l.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        public Character HitEnemyCharacter(Collider col, Character own)
        {
            foreach (Player p in Players)
            {
                if (p.Character == own)
                {
                   continue;
                }
                if (col.IsColliding(p.Character.Collider))
                {
                    return p.Character;
                }
            }
            return null;
        }


        public void Dispose()
        {
            Clock.Tick -= Tick;
        }

    }
}
