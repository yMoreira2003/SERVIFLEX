using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ReqInsertarConversacion
    {
        public Guid SesionId { get; set; }
        public Conversacion Conversacion { get; set; }
    }
}
