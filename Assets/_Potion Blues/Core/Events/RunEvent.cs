namespace PotionBlues
{
    public class RunEvent : IEvent
    {
        public RunEventType Type;

        public RunEvent(RunEventType type)
        {
            Type = type;
        }
    }

    public enum RunEventType
    {
        Created,
        DayPreview,
        DayStarted,
        ShopClosed,
        DayEnded,
        DayReview,
        RunReview,
        Ended
    }
}