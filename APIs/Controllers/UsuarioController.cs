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
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("api/usuario/insertar")]
        public ResInsertarUsuario InsertarUsuario(ReqInsertarUsuario req)
        {
            return new LogInsertarUsuario().insertar(req);
        }

        [HttpPost]
        [Route("api/usuario/obtener")]
        public ResObtenerUsuario ObtenerUsuario(ReqObtenerUsuario req)
        {
            return new LogObtenerUsuario().obtenerUsuario(req);
        }

        [HttpPost]
        [Route("api/usuario/verificar")]
        public ResVerificacion verificarUsuario(ReqVerificacion req)
        {
            return new LogVerificacion().verificarUsuario(req);
        }

        [HttpPost]
        [JwtAuthorize]
        [Route("api/usuario/completarPerfil")]
        public ResCompletarPerfil CompletarPerfil(ReqCompletarPerfil req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogCompletarPerfil().completarPerfil(req);
        }

        [HttpPost]
        [JwtAuthorize]
        [Route("api/usuario/actualizar")]
        public ResActualizarDatosUsuario ActualizarDatosUsuario(ReqActualizarDatosUsuario req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogActualizarDatosUsuario().actualizarDatosUsuario(req);
        }

        [HttpPost]
        [JwtAuthorize]
        [Route("api/usuario/actualizarFotoPerfil")]
        public ResActualizarFotoPerfil ActualizarFotoPerfil(ReqActualizarFotoPerfil req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogActualizarFotoPerfil().actualizarFotoPerfil(req);
        }

        [HttpPost]
        [Route("api/usuario/login")]
        public ResLoginUsuario Login(ReqLoginUsuario req)
        {
            return new LogLoginUsuario().loginUsuario(req);
        }
    }
}
