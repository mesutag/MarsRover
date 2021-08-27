using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Application
{
    public class CustomApplicationException : Exception
    {
        public CustomApplicationException(string reason) : base(reason)
        {
        }
        public static void ThrowValidationException(List<ValidationFailure> validationFailures) 
        {
            throw new CustomApplicationException(string.Join("\n", validationFailures.Select(p => p.ErrorMessage)));
        }
    }
}
