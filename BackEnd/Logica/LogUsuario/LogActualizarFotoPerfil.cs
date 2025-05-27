using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogActualizarFotoPerfil
    {
        public ResActualizarFotoPerfil actualizarFotoPerfil(ReqActualizarFotoPerfil req)
        {
            ResActualizarFotoPerfil res = new ResActualizarFotoPerfil();
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

                if (req.SesionId == Guid.Empty || string.IsNullOrWhiteSpace(req.FotoPerfil))
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.datosInvalidos,
                        Message = "Datos faltantes o inválidos"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorId = 0;
                string errorDescripcion = "";

                using (var db = new LinQDataContext())
                {
                    db.SP_ActualizarFotoPerfil(
                        req.SesionId,
                        req.FotoPerfil,
                        ref idReturn,
                        ref errorId,
                        ref errorDescripcion
                    );
                }

                if (errorId == 0)
                {
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
    }
}
