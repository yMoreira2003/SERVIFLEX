using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarConversacionesPorUsuario
    {
        public ResListarConversacionesPorUsuario listarConversacionesPorUsuario(ReqListarConversacionesPorUsuario req)
        {
            var res = new ResListarConversacionesPorUsuario
            {
                resultado = false,
                error = new List<Error>(),
                Conversaciones = new List<Conversacion>()
            };

            try
            {
                if (req == null || req.SesionId == Guid.Empty)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "El request es nulo o el SesionId es inválido"
                    });
                    return res;
                }

                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarConversacionesPorUsuarioResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarConversacionesPorUsuario(
                        req.SesionId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Conversaciones = factoriaConversaciones(resultado);
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

        private List<Conversacion> factoriaConversaciones(List<SP_ListarConversacionesPorUsuarioResult> lista)
        {
            var conversaciones = new List<Conversacion>();

            foreach (var item in lista)
            {
                var conversacion = new Conversacion
                {
                    ConversacionId = item.CONVERSACION_ID,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    UpdatedAt = item.UPDATED_AT ?? DateTime.MinValue,
                    Usuario1 = new Usuario
                    {
                        UsuarioId = item.USUARIO1_ID ?? 0,
                        Nombre = item.USUARIO1_NOMBRE,
                        Apellido1 = item.USUARIO1_APELLIDO1,
                        Apellido2 = item.USUARIO1_APELLIDO2
                    },
                    Usuario2 = new Usuario
                    {
                        UsuarioId = item.USUARIO2_ID ?? 0,
                        Nombre = item.USUARIO2_NOMBRE,
                        Apellido1 = item.USUARIO2_APELLIDO1,
                        Apellido2 = item.USUARIO2_APELLIDO2
                    },
                    Servicio = new Servicio
                    {
                        ServicioId = item.SERVICIO_ID ?? 0,
                        Titulo = item.SERVICIO_TITULO
                    }
                };

                conversaciones.Add(conversacion);
            }
            return conversaciones;
        }
    }
}
