using System;

namespace Wire.Transfer.In.Domain.SeedWorks
{
    public class Builder<T> : IBuilder<T>
    {
        public virtual T Build()
        {
            throw new NotImplementedException();
        }
    }
}