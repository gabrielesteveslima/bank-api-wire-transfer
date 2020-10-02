using System;
using System.Collections.Generic;

namespace Wire.Transfer.In.Application.Configuration.Validation
{
    /// <summary>
    ///     Represents any Infrastructure errors
    /// </summary>
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message, List<object> errors) : base(message)
        {
            Errors = errors;
        }

        public List<object> Errors { get; }
    }
}