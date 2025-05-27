using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarResenasPorUsuario
    {
        public ResListarResenasPorUsuario ListarResenasPorUsuario(ReqListarResenasPorUsuario req)
        {
            var res = new ResListarResenasPorUsuario
            {
                resultado = false,
                error = new List<Error>(),
                Resenas = new List<Resena>()
            };

            try
            {
                if (req == null || req.SesionId == Guid.Empty)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El request o la sesión es inválida"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarResenasPorUsuarioResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarResenasPorUsuario(
                        req.SesionId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Resenas = FactoriaResenas(resultado);
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

        private List<Resena> FactoriaResenas(List<SP_ListarResenasPorUsuarioResult> lista)
        {
            var resenas = new List<Resena>();

            foreach (var item in lista)
            {
                var resena = new Resena
                {
                    ResenaId = item.RESENA_ID,
                    Servicio = new Servicio
                    {
                        ServicioId = item.SERVICIO_ID ?? 0,
                        Titulo = item.SERVICIO_TITULO
                    },
                    Calificacion = item.CALIFICACION,
                    Comentario = item.COMENTARIO,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    UpdatedAt = item.UPDATED_AT ?? DateTime.MinValue
                };

                resenas.Add(resena);
            }
            return resenas;
        }
    }
}
