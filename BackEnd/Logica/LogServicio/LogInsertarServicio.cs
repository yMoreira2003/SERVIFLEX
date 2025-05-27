using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogInsertarServicio
    {
        public ResInsertarServicio insertarServicio(ReqInsertarServicio req)
        {
            ResInsertarServicio res = new ResInsertarServicio();
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

                if (req.SesionId == Guid.Empty || req.Servicio == null ||
                    req.Servicio.Categoria?.CategoriaId <= 0 ||
                    string.IsNullOrWhiteSpace(req.Servicio.Titulo))
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.parametrosInvalidos,
                        Message = "Datos obligatorios faltantes o inválidos"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    db.SP_InsertarServicio(
                        req.SesionId,
                        req.Servicio.Categoria.CategoriaId,
                        req.Servicio.Titulo,
                        req.Servicio.Descripcion,
                        req.Servicio.Precio,
                        req.Servicio.Disponibilidad,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );

                    if (errorID == 0 && idReturn.HasValue && idReturn > 0)
                    {
                        res.ServicioId = idReturn.Value;
                        res.resultado = true;
                        res.Mensaje = "Servicio insertado correctamente.";

                        // Asociar subcategorías si existen
                        if (req.Servicio.SubCategorias != null && req.Servicio.SubCategorias.Count > 0)
                        {
                            foreach (SubCategoria subCategoria in req.Servicio.SubCategorias)
                            {
                                int? subIdReturn = 0;
                                int? subErrorId = 0;
                                string subErrorDesc = "";

                                db.SP_InsertarSubCategoriaServicio(
                                    res.ServicioId,
                                    subCategoria.SubCategoriaId,
                                    ref subIdReturn,
                                    ref subErrorId,
                                    ref subErrorDesc
                                );

                                if (subErrorId != 0)
                                {
                                    res.resultado = false;
                                    res.error.Add(new Error
                                    {
                                        ErrorCode = (int)enumErrores.excepcionLogica,
                                        Message = $"Error al asociar SubCategoría ID {subCategoria.SubCategoriaId}: {subErrorDesc}"
                                    });
                                }
                            }
                        }
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
    }
}
