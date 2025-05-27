using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogInsertarResena
    {
        public ResInsertarResena InsertarResena(ReqInsertarResena req)
        {
            var res = new ResInsertarResena
            {
                resultado = false,
                error = new List<Error>(),
                mensaje = ""
            };

            try
            {
                if (req == null || req.Resena == null || req.SesionId == Guid.Empty)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El request, la reseña o la sesión es inválida"
                    });
                    return res;
                }

                if (req.Resena.Calificacion < 1 || req.Resena.Calificacion > 5)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.datosInvalidos,
                        Message = "La calificación debe estar entre 1 y 5"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    db.SP_InsertarResena(
                        req.SesionId,
                        req.Resena.Servicio.ServicioId,
                        req.Resena.Calificacion,
                        req.Resena.Comentario,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );
                }

                if (errorID == 0)
                {
                    res.mensaje = "Reseña insertada exitosamente";
                    res.resultado = true;
                }
                else
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)(errorID ?? (int)enumErrores.excepcionLogica),
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
