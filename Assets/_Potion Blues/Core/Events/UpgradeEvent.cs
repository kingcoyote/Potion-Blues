using PotionBlues.Definitions;

namespace PotionBlues
{
    public class UpgradeEvent : IEvent
    {
        public UpgradeEventType Type;
        public UpgradeCardDefinition Upgrade;

        public UpgradeEvent(UpgradeEventType type, UpgradeCardDefinition def)
        {
            Type = type;
            Upgrade = def;
        }
    }

    public enum UpgradeEventType
    {
        Purchased,
        Unlocked,
        Sold,
    }
}