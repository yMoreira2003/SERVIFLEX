using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public Provincia Provincia { get; set; }
        public Canton Canton { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string FotoPerfil { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Contrasena { get; set; }
        public string Salt { get; set; }
        public int Verificacion { get; set; }
        public bool Activo { get; set; }
        public bool PerfilCompleto { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
