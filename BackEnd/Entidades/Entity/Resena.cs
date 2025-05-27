using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Resena
    {
        public int ResenaId { get; set; }
        public Servicio Servicio { get; set; }
        public Usuario Usuario { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

