using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarImagenesPorServicio
    {
        public ResListarImagenesPorServicio listarImagenes(ReqListarImagenesPorServicio req)
        {
            ResListarImagenesPorServicio res = new ResListarImagenesPorServicio();
            res.resultado = false;
            res.error = new List<Error>();

            try
            {
                if (req == null || req.Servicio == null || req.Servicio.ServicioId <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "Request o ID de servicio no válido"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarImagenesPorServicioResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarImagenesPorServicio(
                        req.Servicio.ServicioId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Imagenes = factoriaListaImagenes(resultado, req.Servicio);
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

        private List<Imagen> factoriaListaImagenes(List<SP_ListarImagenesPorServicioResult> lista, Servicio servicio)
        {
            List<Imagen> imagenes = new List<Imagen>();

            foreach (var item in lista)
            {
                var imagen = new Imagen
                {
                    ImagenId = item.IMAGEN_ID,
                    RutaImagen = item.RUTA_IMAGEN,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    UpdatedAt = item.UPDATED_AT ?? DateTime.MinValue,
                    Servicio = new Servicio
                    {
                        ServicioId = servicio.ServicioId
                    }
                };

                imagenes.Add(imagen);
            }
            return imagenes;
        }
    }
}
