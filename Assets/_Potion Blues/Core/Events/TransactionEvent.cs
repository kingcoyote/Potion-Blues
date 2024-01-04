namespace PotionBlues.Events
{
    public class TransactionEvent : IEvent
    {
        public float Amount;
        public string Description;
    }
}