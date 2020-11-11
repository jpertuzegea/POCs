using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Models.Result.Enum
{
    /// <summary>
    /// Tipos de error
    /// </summary>
    [DataContract]
    public enum ErrorType
    {
        /// <summary>
        /// Respuesta satisfactoria
        /// </summary>
        [EnumMember]
        Ok = 0,

        /// <summary>
        /// No encontrado
        /// </summary>
        [EnumMember]
        NotFound = 1,

        /// <summary>
        /// Falla técnica
        /// </summary>
        [EnumMember]
        TechnicalFault = 2,

        /// <summary>
        /// FAlla de negocio
        /// </summary>
        [EnumMember]
        BusinessFault = 3,

        /// <summary>
        /// FAlla de negocio
        /// </summary>
        [EnumMember]
        DataFailed = 4
    }
}
