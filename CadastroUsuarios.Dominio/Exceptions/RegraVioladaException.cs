using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroUsuarios.Dominio.Exceptions
{

    [Serializable]
    public class RegraVioladaException : Exception
    {
        public RegraVioladaException() { }
        public RegraVioladaException(string message) : base(message) { }
        public RegraVioladaException(string message, Exception inner) : base(message, inner) { }
        protected RegraVioladaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
