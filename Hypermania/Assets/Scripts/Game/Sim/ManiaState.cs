using System.Collections.Generic;
using MemoryPack;
using Utils.SoftFloat;

namespace Game.Sim
{
    [MemoryPackable]
    public partial struct ManiaNote
    {
        public int Id;
        public int Length;
    }

    public enum ManiaEventKind
    {
        Hit,
        Missed,
    }

    [MemoryPackable]
    public partial struct ManiaEvent
    {
        public ManiaEventKind Kind;

        /// <summary>
        /// If the note was hit, how many ticks away from the exact timing it was hit
        /// </summary>
        public int Offset;

        /// <summary>
        /// If the note was missed, if it was hit early or late
        /// </summary>
        public bool Early;
    }

    [MemoryPackable]
    public partial struct ManiaSim
    {
        public const int MAX_NOTES = 200;

        public ManiaSim(int numKeys) { }

        public void Poll(GameInput input, List<ManiaEvent> outEvents) { }
    }
}
