using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogVerificacion
    {
        public ResVerificacion verificarUsuario(ReqVerificacion req)
        {
            ResVerificacion res = new ResVerificacion();
            res.error = new List<Error>();

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    res.resultado = false;
                    return res;
                }

                if (req.SesionId == Guid.Empty || req.Verificacion <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.idFaltante,
                        Message = "Sesión inválida o código de verificación inválido"
                    });
                    res.resultado = false;
                    return res;
                }

                int? idReturn = 0;
                int? idError = 0;
                string errorDescripcion = "";

                using (var db = new LinQDataContext())
                {
                    db.SP_Verificacion(req.SesionId, req.Verificacion, ref idReturn, ref idError, ref errorDescripcion);
                }

                if (idError == 0)
                {
                    res.resultado = true;
                }
                else
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.verificacionFallida,
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
