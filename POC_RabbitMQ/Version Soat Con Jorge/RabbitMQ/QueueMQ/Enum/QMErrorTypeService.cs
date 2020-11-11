using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Tipos de error
    /// </summary>
    [DataContract]
    public enum ErrorTypeService
    {
        /// <summary>
        /// Respuesta satisfactoria
        /// </summary>
        [EnumMember]
        Ok = 0,

        /// <summary>
        /// No se encontraron registros
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
