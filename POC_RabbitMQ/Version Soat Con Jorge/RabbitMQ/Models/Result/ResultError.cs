using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Result
{
    public sealed class ResultError<T, U> : IResult<T, U>
    {
        public readonly U ErrorValue;

        public ResultError(U value)
        {
            if (value == null) throw new Exception("ResultError value cannot be null");
            ErrorValue = value;
        }
    }
}
