using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ReqInsertarMensaje
    {
        public Guid SesionId { get; set; }
        public Mensaje Mensaje { get; set; }
    }
}
