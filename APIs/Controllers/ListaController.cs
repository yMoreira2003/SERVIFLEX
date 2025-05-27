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
    public class ListaController : ApiController
    {
        [HttpGet]
        [Route("api/provincia/listar")]
        public ResListarProvincias ListarProvincias()
        {
            return new LogListarProvincias().listarProvincias();
        }

        [HttpGet]
        [Route("api/canton/listar")]
        public ResListarCantones ListarCantones()
        {
            return new LogListarCantones().listarCantones();
        }

        [HttpGet]
        [JwtAuthorize]
        [Route("api/categoria/listar")]
        public ResListarCategorias ListarCategorias()
        {
            return new LogListarCategorias().listarCategorias();
        }

        [HttpGet]
        [JwtAuthorize]
        [Route("api/subcategoria/listar")]
        public ResListarSubCategorias ListarSubCategorias()
        {
            return new LogListarSubCategorias().listarSubCategorias();
        }
    }
}
