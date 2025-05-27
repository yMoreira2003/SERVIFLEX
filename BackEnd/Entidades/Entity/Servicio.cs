using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Servicio
    {
        public int ServicioId { get; set; }
        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public string Disponibilidad { get; set; }
        public List<SubCategoria> SubCategorias { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

