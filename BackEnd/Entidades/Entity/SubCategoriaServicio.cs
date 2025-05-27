using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class SubCategoriaServicio
    {
        public int SubCateServId { get; set; }
        public Servicio Servicio { get; set; }
        public SubCategoria SubCategoria { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
