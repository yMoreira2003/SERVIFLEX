using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ReqInsertarResena
    {
        public Guid SesionId { get; set; }
        public Resena Resena { get; set; }
    }
}
