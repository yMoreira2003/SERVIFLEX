using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogEliminarMensaje
    {
        public ResEliminarMensaje eliminarMensaje(ReqEliminarMensaje req)
        {
            var res = new ResEliminarMensaje
            {
                resultado = false,
                error = new List<Error>()
            };

            try
            {
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    db.SP_EliminarMensaje(
                        req.Mensaje.MensajeId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );
                }

                if (errorID == 0)
                {
                    res.resultado = true;
                    res.mensaje = "Mensaje eliminado correctamente";
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
