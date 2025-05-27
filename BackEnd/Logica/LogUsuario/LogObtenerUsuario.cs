using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogObtenerUsuario
    {
        public ResObtenerUsuario obtenerUsuario(ReqObtenerUsuario req)
        {
            ResObtenerUsuario res = new ResObtenerUsuario();
            res.error = new List<Error>();

            try
            {
                if (req == null)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (req.Usuario.UsuarioId <= 0)
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.idFaltante,
                        Message = "Usuario ID inválido"
                    });
                    return res;
                }

                SP_ObtenerUsuarioResult tipoComplejo = null;
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                using (LinQDataContext db = new LinQDataContext())
                {
                    tipoComplejo = db.SP_ObtenerUsuario(
                        req.Usuario.UsuarioId,
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).FirstOrDefault();
                }

                if (errorID == 0 && tipoComplejo != null)
                {
                    res.Usuario = factoriaObtenerUsuario(tipoComplejo);
                    res.resultado = true;
                }
                else
                {
                    res.resultado = false;

                    if (errorID == (int)enumErrores.usuarioNoEncontrado)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.excepcionLogica,
                            Message = errorDescripcion
                        });
                    }
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

        private Usuario factoriaObtenerUsuario(SP_ObtenerUsuarioResult tipoComplejo)
        {
            Usuario usuario = new Usuario();

            usuario.UsuarioId = tipoComplejo.USUARIO_ID;
            usuario.Nombre = tipoComplejo.NOMBRE;
            usuario.Apellido1 = tipoComplejo.APELLIDO1;
            usuario.Apellido2 = tipoComplejo.APELLIDO2;
            usuario.FechaNacimiento = tipoComplejo.FECHA_NACIMIENTO;
            usuario.Correo = tipoComplejo.CORREO;
            usuario.FotoPerfil = tipoComplejo.FOTO_PERFIL;
            usuario.Telefono = tipoComplejo.TELEFONO;
            usuario.Direccion = tipoComplejo.DIRECCION;

            usuario.Provincia = new Provincia
            {
                Nombre = tipoComplejo.PROVINCIA
            };

            usuario.Canton = new Canton
            {
                Nombre = tipoComplejo.CANTON
            };

            return usuario;
        }
    }
}
