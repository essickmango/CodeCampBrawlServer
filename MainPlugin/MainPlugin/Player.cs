using DarkRift;
using DarkRift.Server;

namespace MainPlugin
{
    public class Player
    {
        public IClient Client;
        public string Name;
        public bool LoggedIn;
        public ushort PlayerId;
        private static ushort nextPlayerId;

        public Game Game;
        public Character Character;


        public static Player CreateGuest(string name, IClient client)
        {
            Player player = new Player();
            player.Client = client;
            player.Name = name;
            player.LoggedIn = false;
            player.PlayerId = nextPlayerId++;

            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(player.PlayerId);
            player.Client.SendMessage(Message.Create((ushort) Tags.SendPlayerId, writer), SendMode.Reliable);

            Server.Players.Add(player.Client.ID, player);

            return player;
        }

        public void DisposePlayer()
        {
            if (Game != null)
            {
                Game.Players.Remove(this);
                Character.DisposeCharacter();
            }
            Server.Players.Remove(Client.ID);

        }

    }
}
