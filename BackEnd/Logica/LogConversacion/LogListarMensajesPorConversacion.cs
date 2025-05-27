using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarMensajesPorConversacion
    {
        public ResListarMensajesPorConversacion listarMensajesPorConversacion(ReqListarMensajesPorConversacion req)
        {
            var res = new ResListarMensajesPorConversacion
            {
                resultado = false,
                error = new List<Error>(),
                Mensajes = new List<Mensaje>()
            };

            try
            {
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarMensajesPorConversacionResult> resultado;

                using (var db = new LinQDataContext())
                {
                    resultado = db.SP_ListarMensajesPorConversacion(
                        req.Conversacion.ConversacionId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Mensajes = factoriaMensajes(resultado);
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

        private List<Mensaje> factoriaMensajes(List<SP_ListarMensajesPorConversacionResult> lista)
        {
            var mensajes = new List<Mensaje>();

            foreach (var item in lista)
            {
                var mensaje = new Mensaje
                {
                    MensajeId = item.MENSAJE_ID,
                    Contenido = item.CONTENIDO,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    Usuario = new Usuario
                    {
                        UsuarioId = item.USUARIO_ID ?? 0,
                        Nombre = item.NOMBRE_USUARIO?.Split(' ')[0],
                        Apellido1 = item.NOMBRE_USUARIO?.Split(' ').Length > 1 ? item.NOMBRE_USUARIO?.Split(' ')[1] : ""
                    },
                    Conversacion = new Conversacion
                    {
                        ConversacionId = item.CONVERSACION_ID ?? 0
                    }
                };

                mensajes.Add(mensaje);
            }
            return mensajes;
        }
    }
}
