using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarResenasPorServicio
    {
        public ResListarResenasPorServicio ListarResenasPorServicio(ReqListarResenasPorServicio req)
        {
            var res = new ResListarResenasPorServicio
            {
                resultado = false,
                error = new List<Error>(),
                Resenas = new List<Resena>()
            };

            try
            {
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarResenasPorServicioResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarResenasPorServicio(
                        req.Servicio.ServicioId,
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

        private List<Resena> FactoriaResenas(List<SP_ListarResenasPorServicioResult> lista)
        {
            var resenas = new List<Resena>();

            foreach (var item in lista)
            {
                var resena = new Resena
                {
                    ResenaId = item.RESENA_ID,
                    Usuario = new Usuario
                    {
                        UsuarioId = item.USUARIO_ID ?? 0,
                        Nombre = item.NOMBRE_USUARIO
                    },
                    Calificacion = item.CALIFICACION,
                    Comentario = item.COMENTARIO,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    UpdatedAt = item.CREATED_AT ?? DateTime.MinValue
                };

                resenas.Add(resena);
            }
            return resenas;
        }
    }
}
