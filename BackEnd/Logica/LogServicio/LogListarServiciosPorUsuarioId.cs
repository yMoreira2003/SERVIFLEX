using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarServiciosPorUsuarioId
    {
        public ResListarServiciosPorUsuarioId listarServicios(ReqListarServiciosPorUsuarioId req)
        {
            ResListarServiciosPorUsuarioId res = new ResListarServiciosPorUsuarioId();
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

                if (req.Usuario.UsuarioId <= 0)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.idFaltante,
                        Message = "Usuario ID inválido"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarServiciosPorUsuarioIdResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarServiciosPorUsuarioId(
                        req.Usuario.UsuarioId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Servicios = factoriaListaServicios(resultado);
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

        private List<Servicio> factoriaListaServicios(List<SP_ListarServiciosPorUsuarioIdResult> lista)
        {
            var serviciosDict = new Dictionary<int, Servicio>();

            foreach (var item in lista)
            {
                if (!serviciosDict.ContainsKey(item.SERVICIO_ID))
                {
                    var servicio = new Servicio
                    {
                        ServicioId = item.SERVICIO_ID,
                        Titulo = item.TITULO,
                        Descripcion = item.DESCRIPCION,
                        Precio = item.PRECIO,
                        Disponibilidad = item.DISPONIBILIDAD,
                        CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                        UpdatedAt = item.UPDATED_AT ?? DateTime.MinValue,
                        Usuario = new Usuario
                        {
                            UsuarioId = item.USUARIO_ID ?? 0,
                            Nombre = item.USUARIO_NOMBRE,
                            Apellido1 = item.USUARIO_APELLIDO1,
                            Apellido2 = item.USUARIO_APELLIDO2
                        },
                        Categoria = new Categoria
                        {
                            CategoriaId = item.CATEGORIA_ID ?? 0,
                            Nombre = item.CATEGORIA_NOMBRE
                        },
                        SubCategorias = new List<SubCategoria>()
                    };
                    serviciosDict.Add(servicio.ServicioId, servicio);
                }

                if (item.SUB_CATEGORIA_ID.HasValue)
                {
                    var sub = new SubCategoria
                    {
                        SubCategoriaId = item.SUB_CATEGORIA_ID.Value,
                        Nombre = item.SUB_CATEGORIA_NOMBRE
                    };

                    if (!serviciosDict[item.SERVICIO_ID].SubCategorias.Any(s => s.SubCategoriaId == sub.SubCategoriaId))
                    {
                        serviciosDict[item.SERVICIO_ID].SubCategorias.Add(sub);
                    }
                }
            }
            return serviciosDict.Values.ToList();
        }
    }
}
