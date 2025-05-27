using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ReqActualizarDatosUsuario
    {
        public Guid SesionId { get; set; }
        public int ProvinciaId { get; set; }
        public int CantonId { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}
