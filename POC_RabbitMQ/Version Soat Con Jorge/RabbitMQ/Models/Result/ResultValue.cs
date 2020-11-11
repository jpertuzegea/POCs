using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Result
{
    public sealed class ResultValue<T, U> : IResult<T, U>
    {
        public readonly T Value;

        public ResultValue(T value)
        {
            if (value == null) throw new Exception("ResultValue value cannot be null");
            Value = value;
        }
    }

}
