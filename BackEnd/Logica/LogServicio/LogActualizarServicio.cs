using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogActualizarServicio
    {
        public ResActualizarServicio actualizarServicio(ReqActualizarServicio req)
        {
            ResActualizarServicio res = new ResActualizarServicio();
            res.error = new List<Error>();

            try
            {
                if (req == null)
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
                string errorDescripcion = "";

                using (var db = new LinQDataContext())
                {
                    db.SP_ActualizarServicio(
                        req.Servicio.ServicioId,
                        req.Servicio.Descripcion,
                        req.Servicio.Precio,
                        req.Servicio.Disponibilidad,
                        ref idReturn,
                        ref errorId,
                        ref errorDescripcion
                    );
                }

                if (errorId == 0)
                {
                    res.resultado = true;
                    res.Mensaje = "Servicio actualizado correctamente.";
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
