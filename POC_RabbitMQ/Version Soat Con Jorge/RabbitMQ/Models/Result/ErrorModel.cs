using Models.Result.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Models.Result
{
    [DataContract]
    public class ErrorModel
    {
        public ErrorModel()
        {
        }

        [JsonIgnore]
        public int Id { get; set; }

        [DataMember]
        public IEnumerable<string> ErrorDescription { get; set; }

        [DataMember]
        public ErrorType ErrorType { get; set; }


        [DataMember]
        public Exception Exception { get; set; }


        private ErrorModel(IEnumerable<string> ErrorDescription, ErrorType ErrorType, Exception Exception, int Id = 0)
        {
            this.Id = Id;
            this.ErrorDescription = ErrorDescription;
            this.ErrorType = ErrorType;
            this.Exception = Exception;
        }

        public static ErrorModel CreateErrorModel(IEnumerable<string> errorDescription, ErrorType ErrorType, Exception exception = null, int Id = 0)
        {
            return new ErrorModel(errorDescription, ErrorType, exception, Id);
        }
        public static ErrorModel CreateErrorModel(string errorDescription, ErrorType ErrorType, Exception exception = null, int Id = 0)
        {
            return new ErrorModel(new List<string> { errorDescription }, ErrorType, exception, Id);
        }
    }

}
