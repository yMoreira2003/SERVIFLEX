using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogLoginUsuario
    {
        public ResLoginUsuario loginUsuario(ReqLoginUsuario req)
        {
            var res = new ResLoginUsuario
            {
                error = new List<Error>()
            };

            try
            {
                if (req == null || req.Usuario == null)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El request no puede ser nulo"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorId = 0;
                string errorDescripcion = string.Empty;
                Guid? sesionId = null;

                using (var db = new LinQDataContext())
                {
                    db.SP_LOGIN_USUARIO(
                        req.Usuario.Correo,
                        req.Usuario.Contrasena,
                        ref sesionId,
                        ref idReturn,
                        ref errorId,
                        ref errorDescripcion
                    );

                    if (errorId == 0 && sesionId != null && sesionId != Guid.Empty)
                    {
                        string tokenJwt = TokenHelper.GenerarToken(sesionId.Value);

                        res.resultado = true;
                        res.sesion = new Sesion
                        {
                            SesionId = sesionId.Value,
                            Activo = true,
                            FechaCreacion = DateTime.Now,
                            FechaCierre = null,
                            Token = tokenJwt
                        };
                    }
                    else
                    {
                        res.resultado = false;
                        res.error.Add(new Error
                        {
                            ErrorCode = errorId ?? (int)enumErrores.excepcionLogica,
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
