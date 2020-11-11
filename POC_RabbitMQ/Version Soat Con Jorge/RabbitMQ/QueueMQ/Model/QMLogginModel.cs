
namespace QueueMQ.Model
{
    using System;

    public class QMLogginModel
    {
        public struct LogginStruct
        {
            private const string _ALL = "ALL";
            private const string _VACIO = "";
            private string _Default;
            private bool isValid;
            public string Default { get { if (isValid) return _Default; else return String.Empty; } set { if (value.Equals(_ALL) || value.Equals(_VACIO)) { _Default = value; isValid = true; } } }
        }
        public LogginStruct LogLevel;
    }
}
