namespace Wire.Transfer.In.Domain.SeedWorks
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}