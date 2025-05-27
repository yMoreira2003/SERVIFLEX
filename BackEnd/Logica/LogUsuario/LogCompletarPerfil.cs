using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogCompletarPerfil
    {
        public ResCompletarPerfil completarPerfil(ReqCompletarPerfil req)
        {
            ResCompletarPerfil res = new ResCompletarPerfil();
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

                if (req.SesionId == Guid.Empty || req.ProvinciaId <= 0 || req.CantonId <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.datosInvalidos,
                        Message = "Sesión inválida o datos faltantes"
                    });
                    res.resultado = false;
                    return res;
                }

                int? idReturn = 0;
                int? idError = 0;
                string errorDescripcion = "";

                using (var db = new LinQDataContext())
                {
                    db.SP_CompletarPerfil(
                        req.SesionId,
                        req.ProvinciaId,
                        req.CantonId,
                        req.FotoPerfil,
                        req.Telefono,
                        req.Direccion,
                        ref idReturn,
                        ref idError,
                        ref errorDescripcion
                    );
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
