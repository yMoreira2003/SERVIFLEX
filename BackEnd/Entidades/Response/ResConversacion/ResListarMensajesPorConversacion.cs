using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResListarMensajesPorConversacion : ResBase
    {
        public List<Mensaje> Mensajes { get; set; }
    }
}
