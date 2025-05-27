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
    public class ServicioController : ApiController
    {
        [HttpPost]
        [Route("api/servicio/insertar")]
        public ResInsertarServicio InsertarServicio(ReqInsertarServicio req)
        {
            var sesionId = (string)Request.Properties["SesionId"];
            req.SesionId = new Guid(sesionId);

            return new LogInsertarServicio().insertarServicio(req);
        }

        [HttpPost]
        [Route("api/servicio/actualizar")]
        public ResActualizarServicio ActualizarServicio(ReqActualizarServicio req)
        {
            return new LogActualizarServicio().actualizarServicio(req);
        }

        [HttpPost]
        [Route("api/servicio/eliminar")]
        public ResEliminarServicio EliminarServicio(ReqEliminarServicio req)
        {
            return new LogEliminarServicio().eliminarServicio(req);
        }

        [HttpGet]
        [Route("api/servicio/listar")]
        public ResListarServicios ListarServicios()
        {
            return new LogListarServicios().listarServicios();
        }

        [HttpPost]
        [Route("api/servicio/listarPorUsuario")]
        public ResListarServiciosPorUsuarioId ListarServiciosPorUsuario(ReqListarServiciosPorUsuarioId req)
        {
            return new LogListarServiciosPorUsuarioId().listarServicios(req);
        }

        [HttpPost]
        [Route("api/servicio/listarPorCategoria")]
        public ResListarServiciosPorCategoriaId ListarServiciosPorCategoria(ReqListarServiciosPorCategoriaId req)
        {
            return new LogListarServiciosPorCategoriaId().listarServicios(req);
        }

        [HttpPost]
        [Route("api/servicio/listarPorSubCategoria")]
        public ResListarServiciosPorSubCategoriaId ListarServiciosPorSubCategoria(ReqListarServiciosPorSubCategoriaId req)
        {
            return new LogListarServiciosPorSubCategoriaId().listarServicios(req);
        }

        [HttpPost]
        [Route("api/servicio/obtener")]
        public ResObtenerServicio ObtenerServicio(ReqObtenerServicio req)
        {
            return new LogObtenerServicio().obtenerServicio(req);
        }

        [HttpPost]
        [Route("api/servicio/insertarImagen")]
        public ResInsertarImagen InsertarImagen(ReqInsertarImagen req)
        {
            return new LogInsertarImagen().insertarImagen(req);
        }

        [HttpPost]
        [Route("api/servicio/eliminarImagen")]
        public ResEliminarImagen EliminarImagen(ReqEliminarImagen req)
        {
            return new LogEliminarImagen().eliminarImagen(req);
        }

        [HttpPost]
        [Route("api/servicio/listarImagenesPorServicio")]
        public ResListarImagenesPorServicio ListarImagenesPorServicio(ReqListarImagenesPorServicio req)
        {
            return new LogListarImagenesPorServicio().listarImagenes(req);
        }
    }
}
