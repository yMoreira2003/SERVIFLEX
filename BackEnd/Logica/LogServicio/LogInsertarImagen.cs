using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogInsertarImagen
    {
        public ResInsertarImagen insertarImagen(ReqInsertarImagen req)
        {
            ResInsertarImagen res = new ResInsertarImagen();
            res.resultado = false;
            res.error = new List<Error>();

            try
            {
                if (req == null || req.Imagen == null || req.Imagen.Servicio == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "La imagen o su servicio asociado es nulo."
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    db.SP_InsertarImagen(
                        req.Imagen.Servicio.ServicioId,
                        req.Imagen.RutaImagen,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    );
                }

                if (errorID == 0)
                {
                    res.resultado = true;
                    res.mensaje = errorDescripcion;
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
