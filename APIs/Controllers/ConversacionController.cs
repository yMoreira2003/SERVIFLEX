using APIs.Seguridad;
using BackEnd.Entidades;
using BackEnd.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIs.Controllers
{
    [JwtAuthorize]
    public class ConversacionController : ApiController
    {
        [HttpPost]
        [Route("api/conversacion/insertarConversacion")]
        public ResInsertarConversacion InsertarConversacion(ReqInsertarConversacion req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogInsertarConversacion().insertarConversacion(req);
        }

        [HttpPost]
        [Route("api/conversacion/listarConversacionesPorUsuario")]
        public ResListarConversacionesPorUsuario ListarConversacionesPorUsuario(ReqListarConversacionesPorUsuario req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogListarConversacionesPorUsuario().listarConversacionesPorUsuario(req);
        }

        [HttpPost]
        [Route("api/conversacion/insertarMensaje")]
        public ResInsertarMensaje InsertarMensaje(ReqInsertarMensaje req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogInsertarMensaje().insertarMensaje(req);
        }

        [HttpPost]
        [Route("api/conversacion/eliminarMensaje")]
        public ResEliminarMensaje EliminarMensaje(ReqEliminarMensaje req)
        {
            return new LogEliminarMensaje().eliminarMensaje(req);
        }

        [HttpPost]
        [Route("api/conversacion/listarMensajesPorConversacion")]
        public ResListarMensajesPorConversacion ListarMensajesPorConversacion(ReqListarMensajesPorConversacion req)
        {
            return new LogListarMensajesPorConversacion().listarMensajesPorConversacion(req);
        }

        [HttpPost]
        [Route("api/conversacion/eliminarConversacion")]
        public ResEliminarConversacion EliminarConversacion(ReqEliminarConversacion req)
        {
            return new LogEliminarConversacion().eliminarConversacion(req);
        }
    }
}
