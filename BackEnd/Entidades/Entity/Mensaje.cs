using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Mensaje
    {
        public int MensajeId { get; set; }
        public Conversacion Conversacion { get; set; }
        public Usuario Usuario { get; set; }
        public string Contenido { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
