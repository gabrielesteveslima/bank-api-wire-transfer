namespace Wire.Transfer.In.Domain.SeedWorks
{
    public interface ILogging
    {
        void Error(object message);
        void Warning(object message);
        void Information(object message);
    }
}