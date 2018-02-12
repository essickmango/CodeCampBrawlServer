using System.Collections.Generic;
using DarkRift;
using UnityEngine;

namespace MainPlugin
{
    public class GameUpdateMessage
    {

        public List<Vector2> CharacterPositions;
        public List<ushort> CharacterIds;

        public static GameUpdateMessage Create()
        {
            GameUpdateMessage g = new GameUpdateMessage();
            g.CharacterIds = new List<ushort>();
            g.CharacterPositions = new List<Vector2>();
            return g;
        }


        public void AddCharacterPosUpdate(ushort id, Vector2 pos)
        {
            CharacterPositions.Add(pos);
            CharacterIds.Add(id);
        }

        public void Reset()
        {
            CharacterPositions.Clear();
            CharacterIds.Clear();
        }

        public Message GetMessage()
        {
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write((ushort)CharacterPositions.Count);
            for (int i = 0; i < CharacterPositions.Count; i++)
            {
                writer.Write(CharacterIds[i]);
                writer.Write(CharacterPositions[i].x);
                writer.Write(CharacterPositions[i].y);
            }
            Reset();
            return Message.Create((ushort)Tags.TickUpdate, writer);
        }

        

    }
}
