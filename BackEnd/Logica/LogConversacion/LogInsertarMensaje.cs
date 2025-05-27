using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogInsertarMensaje
    {
        public ResInsertarMensaje insertarMensaje(ReqInsertarMensaje req)
        {
            var res = new ResInsertarMensaje
            {
                resultado = false,
                error = new List<Error>()
            };

            try
            {
                if (req == null || req.Mensaje == null || req.SesionId == Guid.Empty)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El request, el mensaje o el SesionId son inválidos"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    db.SP_InsertarMensaje(
                        req.SesionId,
                        req.Mensaje.Conversacion.ConversacionId,
                        req.Mensaje.Contenido,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );
                }

                if (errorID == 0)
                {
                    res.resultado = true;
                    res.mensaje = "Mensaje enviado correctamente";
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
