using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogInsertarConversacion
    {
        public ResInsertarConversacion insertarConversacion(ReqInsertarConversacion req)
        {
            var res = new ResInsertarConversacion
            {
                resultado = false,
                error = new List<Error>()
            };

            try
            {
                if (req == null || req.Conversacion == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El request o la conversación es nula"
                    });
                    return res;
                }

                if (req.SesionId == Guid.Empty ||
                    req.Conversacion.Usuario2 == null ||
                    req.Conversacion.Usuario2.UsuarioId <= 0 ||
                    req.Conversacion.Servicio == null ||
                    req.Conversacion.Servicio.ServicioId <= 0)
                {
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

                using (var db = new LinQDataContext())
                {
                    db.SP_InsertarConversacion(
                        req.SesionId,
                        req.Conversacion.Usuario2.UsuarioId,
                        req.Conversacion.Servicio.ServicioId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );
                }

                if (errorID == 0 && idReturn > 0)
                {
                    res.resultado = true;
                    res.mensaje = "Conversación insertada correctamente";
                }
                else
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = errorID ?? (int)enumErrores.excepcionLogica,
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
    }
}
