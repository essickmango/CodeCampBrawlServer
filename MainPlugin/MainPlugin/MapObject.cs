namespace MainPlugin
{
    public class MapObject
    {
        public STransform Transform;
        public Collider Collider;
        public ushort Id;
        public enum  Ids
        {
           Wall = 0 
        }

        public static MapObject CreatePreset(Collider collider, ushort id)
        {
            MapObject m = new MapObject();
            m.Transform = collider.Transform;
            m.Collider = collider;
            m.Id = id;

            return m;
        }

    }
}
