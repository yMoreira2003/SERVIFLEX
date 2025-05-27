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
    public class ResenaController : ApiController
    {
        [HttpPost]
        [Route("api/resena/insertarResena")]
        public ResInsertarResena InsertarResena(ReqInsertarResena req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogInsertarResena().InsertarResena(req);
        }

        [HttpPost]
        [Route("api/resena/eliminarResena")]
        public ResEliminarResena EliminarResena(ReqEliminarResena req)
        {
            return new LogEliminarResena().EliminarResena(req);
        }

        [HttpPost]
        [Route("api/resena/listarResenasPorServicio")]
        public ResListarResenasPorServicio ListarResenasPorServicio(ReqListarResenasPorServicio req)
        {
            return new LogListarResenasPorServicio().ListarResenasPorServicio(req);
        }

        [HttpPost]
        [Route("api/resena/listarResenasPorUsuario")]
        public ResListarResenasPorUsuario ListarResenasPorUsuario(ReqListarResenasPorUsuario req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogListarResenasPorUsuario().ListarResenasPorUsuario(req);
        }
    }
}
