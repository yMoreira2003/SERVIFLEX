using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResListarServiciosPorUsuarioId : ResBase
    {
        public List<Servicio> Servicios { get; set; } 
    }
}
