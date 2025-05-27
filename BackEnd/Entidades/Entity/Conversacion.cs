using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Conversacion
    {
        public int ConversacionId { get; set; }
        public Usuario Usuario1 { get; set; }
        public Usuario Usuario2 { get; set; }
        public Servicio Servicio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

