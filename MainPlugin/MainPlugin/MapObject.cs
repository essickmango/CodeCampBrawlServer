namespace MainPlugin
{
    public class MapObject
    {
        public STransform Transform;
        public Collider Collider;
        public ushort Type;
        public enum  TypeIds
        {
           Wall = 0,
           Background = 5,
        }

        public static MapObject CreatePreset(Collider collider, ushort type)
        {
            MapObject m = new MapObject();
            m.Transform = collider.Transform;
            m.Collider = collider;
            m.Type = type;

            return m;
        }

    }
}
