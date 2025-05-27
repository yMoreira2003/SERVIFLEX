using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogEliminarServicio
    {
        public ResEliminarServicio eliminarServicio(ReqEliminarServicio req)
        {
            ResEliminarServicio res = new ResEliminarServicio();
            res.error = new List<Error>();

            try
            {
                if (req == null || req.Servicio.ServicioId <= 0)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El ID del servicio es obligatorio."
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    db.SP_EliminarServicio(
                        req.Servicio.ServicioId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );

                    if (errorID == 0 && idReturn > 0)
                    {
                        res.resultado = true;
                        res.Mensaje = "Servicio eliminado exitosamente.";
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
