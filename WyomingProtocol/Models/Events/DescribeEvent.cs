namespace WyomingProtocol.Models.Events
{
    public sealed class DescribeEvent : IWyomingEvent
    {
        public string Type => Constants.EventTypes.Describe;
    }
}
