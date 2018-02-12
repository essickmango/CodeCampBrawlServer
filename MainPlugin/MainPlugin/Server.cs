using System;
using System.Collections.Generic;
using System.Threading;
using DarkRift;
using DarkRift.Server;
using UnityEngine;
using LogType = DarkRift.LogType;
using ThreadPriority = System.Threading.ThreadPriority;

namespace MainPlugin
{
    public class Server: Plugin
    {
        public static Server Instance;
        public static Dictionary<ushort, Player> Players = new Dictionary<ushort, Player>();
        public Dictionary<string, Game> Games = new Dictionary<string, Game>();

        public Server(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            Instance = this;

            Thread gameBackgroundThread = new Thread(Clock.StartBackgroundloop);
            gameBackgroundThread.Priority = ThreadPriority.AboveNormal;
            gameBackgroundThread.IsBackground = true;
            gameBackgroundThread.Start();

            ClientManager.ClientConnected += OnClientConnect;
            ClientManager.ClientDisconnected += OnClientDisconnect;
        }

        public override Version Version => new Version(1,0,0);
        public override bool ThreadSafe => true;

        public void Log(string s)
        {
            WriteEvent(s, LogType.Info);

        }

        void OnClientConnect(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessage;

        }

        void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e)
        {
            e.Client.MessageReceived -= OnMessage;
        }



        void OnMessage(object sender, MessageReceivedEventArgs e)
        {
            Message message = e.GetMessage();
            if (message != null)
            {
                switch (message.Tag)
                {
                    case (ushort)Tags.GuestJoin:
                        GuestJoin(sender, message);
                        break;
                    case (ushort)Tags.CreateTestGame:
                        CreateTestGame(sender, message);
                        break;
                    case (ushort)Tags.WalkL:
                        SetWalk(sender, message, false);
                        break;
                    case (ushort)Tags.WalkR:
                        SetWalk(sender, message, true);
                        break;
                    case (ushort)Tags.Jump:
                        SetJump(sender, message);
                        break;
                    case (ushort)Tags.ShootBeacon:
                        ShootBeacon(sender, message);
                        break;
                    case (ushort)Tags.JoinGame:
                        JoinGame(sender, message);
                        break;
                    case (ushort)Tags.ShootArrow:
                        ShootArrow(sender, message);
                        break;
                    case (ushort)Tags.StopL:
                        StopWalk(sender, message, false);
                        break;
                    case (ushort)Tags.StopR:
                        StopWalk(sender, message, true);
                        break;
                }
            }
        }


        void GuestJoin(object sender, Message message)
        {
            IClient client = (IClient)sender;
            DarkRiftReader reader = message.GetReader();
            string name = reader.ReadString();
            Player.CreateGuest(name, client);
        }

        void CreateTestGame(object sender, Message message)
        {
            IClient client = (IClient)sender;
            DarkRiftReader reader = message.GetReader();

            string gameName = reader.ReadString();
            GameCreationData tesTdata = GameCreationData.CreateSimple(new List<MapObject>()
            {
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(0,-4.5f),0 ),new Vector2(20,1) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(0,-2f),0 ),new Vector2(3,0.5f) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(3,-2f),0 ),new Vector2(1,5f) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(20,1),0 ),new Vector2(1,25) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(-20,1),0 ),new Vector2(1,25) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(0,13),0 ),new Vector2(50,1) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(0,-12),0 ),new Vector2(50,1) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(-7.25f,-0.75f),0 ),new Vector2(8.5f,0.5f) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(-7.5f,-8),0 ),new Vector2(20,1) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(-13,-6),0 ),new Vector2(3,0.5f) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(-13,-9),0 ),new Vector2(3,1.75f) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(18,-10.5f),0 ),new Vector2(3.5f,2.5f) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(14,-8),0 ),new Vector2(1,1) ),0 ),
                MapObject.CreatePreset(new BoxCollider(new STransform(new Vector2(11.5f,-6),0 ),new Vector2(1,1) ),0 ),
            }, new List<Vector2>()
            {
                new Vector2(0,4),
                new Vector2(18,3.5f),
                new Vector2(-18,-9.5f),
                new Vector2(0,9),
                new Vector2(-15,9),
            });

            Game g = Game.CreateGame(gameName, tesTdata);
            Games.Add(g.Name,g);
            g.JoinPlayer(Players[client.ID]);
        }

        void JoinGame(object sender, Message message)
        {
            IClient client = (IClient)sender;
            DarkRiftReader reader = message.GetReader();
            string gameName = reader.ReadString();
            Game game = Games[gameName];
            if (game != null)
            {
                game.JoinPlayer(Players[client.ID]);
            }
        }


        void SetWalk(object sender, Message message, bool direction)
        {
            IClient client = (IClient) sender;
            Player p = Players[client.ID];
            if (p.Game != null)
            {
                if (direction)
                {
                    p.Character.WalkR = true;
                }
                else
                {
                    p.Character.WalkL = true;
                }
            }
        }

        void StopWalk(object sender, Message message, bool direction)
        {
            IClient client = (IClient)sender;
            Player p = Players[client.ID];
            if (p.Game != null)
            {
                if (direction)
                {
                    p.Character.WalkR = false;
                }
                else
                {
                    p.Character.WalkL = false;
                }
            }
        }

        void SetJump(object sender, Message message)
        {
            IClient client = (IClient) sender;
            Player p = Players[client.ID];
            if (p.Game != null)
            {
                p.Character.Jumped = true;
            }
        }


        void ShootBeacon(object sender, Message message)
        {
            DarkRiftReader reader = message.GetReader();
            IClient client = (IClient)sender;
            Player p = Players[client.ID];
            if (p.Game != null)
            {
                if (p.Character.LastCastBeacon + 120 > p.Game.Frame)
                {
                    return;
                }
                p.Character.LastCastBeacon = p.Game.Frame;
                Vector2 dir = new Vector2(reader.ReadSingle(),reader.ReadSingle());
                BeaconArrow a = BeaconArrow.FireArrow(p.Character.Transform.Position, dir, p.Game, p.Character);
            }
        }

        void ShootArrow(object sender, Message message)
        {
            DarkRiftReader reader = message.GetReader();
            IClient client = (IClient)sender;
            Player p = Players[client.ID];
            if (p.Game != null)
            {
                if (p.Character.LastCastArrow + 20 > p.Game.Frame)
                {
                    return;
                }
                p.Character.LastCastArrow = p.Game.Frame;
                Vector2 dir = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                Arrow a = Arrow.FireArrow(p.Character.Transform.Position, dir, p.Game, p.Character);
            }
        }
    }
}
