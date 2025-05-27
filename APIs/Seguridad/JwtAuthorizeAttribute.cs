using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.IdentityModel.Tokens;

namespace APIs.Seguridad
{
    public class JwtAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly string claveSecreta = "TuClaveSuperSecretaQueDebeSerLarga";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var authHeader = request.Headers.Authorization;

            if (authHeader == null || authHeader.Scheme != "Bearer" || string.IsNullOrEmpty(authHeader.Parameter))
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.Unauthorized, "Token no proporcionado");
                return;
            }

            var token = authHeader.Parameter;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(claveSecreta);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "tuApp",
                    ValidAudience = "tuAppUsuarios",
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                // Se puede guardar el usuario autenticado en el contexto si se desea
                var identity = (ClaimsIdentity)principal.Identity;
                var sesionId = identity.Claims.FirstOrDefault(c => c.Type == "SesionId")?.Value;

                // Guardar el valor en Properties del request si se necesita en el Controller
                actionContext.Request.Properties["SesionId"] = sesionId;
            }
            catch
            {
                actionContext.Response = request.CreateResponse(HttpStatusCode.Unauthorized, "Token inválido o expirado");
            }
        }
    }
}
