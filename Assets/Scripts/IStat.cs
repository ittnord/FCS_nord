namespace FCS
{
    public enum StatType
    {
        Hp
    }

    public interface IStat
    {
        StatType Type { get; }

        float Current { get; set; }

        float Max { get; }
    }

    public class Stat : IStat
    {
        private readonly StatType _type;
        private readonly int _max;

        public Stat(StatType type, int initialValue)
        {
            _type = type;
            Current = _max = initialValue;
        }

        public StatType Type { get { return _type; } }
        public float Max { get { return _max; } }
        public float Current { get; set; }
    }
}