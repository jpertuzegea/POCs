
namespace QueueMQ.Contingency
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RabbitMQEstatus
    {
        public const string _GUARDADO = "GUARDADO";
        public const string _PENDIENTE = "PENDIENTE";
        public const string _ENVIADO = "ENVIADO";
        public enum Type
        {
            _GUARDADO,
            _PENDIENTE,
            _ENVIADO
        }
        public RabbitMQEstatus()
        {
        }

        public string Map(Type valor)
        {
            switch (valor)
            {
                case Type._GUARDADO: return _GUARDADO;
                case Type._PENDIENTE: return _PENDIENTE;
                case Type._ENVIADO: return _ENVIADO;
                default: return _GUARDADO;
            }
        }
    }
}
