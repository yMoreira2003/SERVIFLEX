using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogObtenerServicio
    {
        public ResObtenerServicio obtenerServicio(ReqObtenerServicio req)
        {
            ResObtenerServicio res = new ResObtenerServicio();
            res.error = new List<Error>();

            try
            {
                if (req == null)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (req.Servicio.ServicioId <= 0)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.idFaltante,
                        Message = "Servicio ID inválido"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ObtenerServicioResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ObtenerServicio(
                        req.Servicio.ServicioId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado.Any())
                {
                    res.Servicio = factoriaServicio(resultado);
                    res.resultado = true;
                }
                else
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.excepcionLogica,
                        Message = errorDescripcion
                    });
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error.Add(new Error
                {
                    ErrorCode = (int)enumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }
            return res;
        }

        private Servicio factoriaServicio(List<SP_ObtenerServicioResult> lista)
        {
            var primero = lista.First();

            var servicio = new Servicio
            {
                ServicioId = primero.SERVICIO_ID,
                Titulo = primero.TITULO,
                Descripcion = primero.DESCRIPCION,
                Precio = primero.PRECIO,
                Disponibilidad = primero.DISPONIBILIDAD,
                CreatedAt = primero.CREATED_AT ?? DateTime.MinValue,
                UpdatedAt = primero.UPDATED_AT ?? DateTime.MinValue,
                Usuario = new Usuario
                {
                    UsuarioId = primero.USUARIO_ID ?? 0,
                    Nombre = primero.USUARIO_NOMBRE,
                    Apellido1 = primero.USUARIO_APELLIDO1,
                    Apellido2 = primero.USUARIO_APELLIDO2
                },
                Categoria = new Categoria
                {
                    CategoriaId = primero.CATEGORIA_ID ?? 0,
                    Nombre = primero.CATEGORIA_NOMBRE
                },
                SubCategorias = new List<SubCategoria>()
            };

            foreach (var item in lista)
            {
                if (item.SUB_CATEGORIA_ID.HasValue)
                {
                    var sub = new SubCategoria
                    {
                        SubCategoriaId = item.SUB_CATEGORIA_ID.Value,
                        Nombre = item.SUB_CATEGORIA_NOMBRE
                    };

                    if (!servicio.SubCategorias.Any(s => s.SubCategoriaId == sub.SubCategoriaId))
                    {
                        servicio.SubCategorias.Add(sub);
                    }
                }
            }
            return servicio;
        }
    }
}
