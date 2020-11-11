using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Model
{
    using System;
    using System.Diagnostics;

    public class QueueManagerLogEntry : Exception
    {
        /// <summary>
        /// Excepción genérica a ser manejada por todo el proyecto de ElectronicBilling, no se debe manejar exepciones de ningun otro tipo.
        /// </summary>
        /// <param name="Action">Accción o evento que se estaba ejecutando</param>
        /// <param name="Origin">Worker o proceso que originó la excepción</param>
        /// <param name="Description">Descripción del evento, se puede incluir el exception.Message</param>
        /// <param name="Data">Data que se estaba manipulando cuando courrió el evento</param>
        /// <param name="ex">Excepcion opcional, se puede incluir la excepción de ser así se incoporará la InnerException al cuerpo de esta expeción genérica.</param>
        public QueueManagerLogEntry(string Action, string Origin, string Description, string Data, Exception ex = null) : base(
            "\n" +
           "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy") + "\n" +
           "Hora: " + DateTime.Now.ToString("HH:mm:ss.fff") + "\n" +
           "Accion: " + Action + "\n" +
           "Origen: " + Origin + "\n" +
           "Descripcion: " + Description + "\n" +
           "Message: " + (ex != null ? ex.Message : "") + "\n" +
           "Data: " + Data + "\n" +
           "Metodo: " + (new StackTrace().GetFrame(1).GetMethod().Name),
           ex?.InnerException
        )
        {

        }
    }
}
