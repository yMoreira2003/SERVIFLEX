using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogInsertarUsuario
    {
        public ResInsertarUsuario insertar(ReqInsertarUsuario req)
        {
            ResInsertarUsuario res = new ResInsertarUsuario();
            res.error = new List<Error>();

            try
            {
                #region Validaciones
                if (req == null || req.Usuario == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)enumErrores.requestNulo,
                        Message = "Request nulo o Usuario nulo"
                    });
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(req.Usuario.Nombre))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.nombreFaltante,
                            Message = "Nombre es requerido"
                        });
                    }

                    if (string.IsNullOrWhiteSpace(req.Usuario.Apellido1))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.apellidoFaltante,
                            Message = "Primer apellido es requerido"
                        });
                    }

                    if (string.IsNullOrWhiteSpace(req.Usuario.Apellido2))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.apellidoFaltante,
                            Message = "Segundo apellido es requerido"
                        });
                    }

                    if (req.Usuario.FechaNacimiento == default(DateTime))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.fechaNacimientoInvalida,
                            Message = "Fecha de nacimiento inválida"
                        });
                    }
                    else if (!EsMayorDeEdad(req.Usuario.FechaNacimiento))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.usuarioMenorEdad,
                            Message = "Debes tener al menos 18 años"
                        });
                    }

                    if (string.IsNullOrWhiteSpace(req.Usuario.Correo))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.correoFaltante,
                            Message = "Correo es requerido"
                        });
                    }
                    else if (!EsCorreoValido(req.Usuario.Correo))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.correoIncorrecto,
                            Message = "Correo no válido"
                        });
                    }

                    if (string.IsNullOrWhiteSpace(req.Usuario.Contrasena))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.passwordFaltante,
                            Message = "Password es requerido"
                        });
                    }
                    else if (!EsPasswordSeguro(req.Usuario.Contrasena))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.passwordMuyDebil,
                            Message = "Password muy débil"
                        });
                    }
                }
                #endregion

                if (res.error.Any())
                {
                    res.resultado = false;
                }
                else
                {
                    int? idBD = 0;
                    int? errorIdBD = 0;
                    string errorMsgBD = "";
                    string llave = Guid.NewGuid().ToString("N");
                    string passHash = hashearPassword(req.Usuario.Contrasena, llave);
                    int pinVerificacion = GenerarPin(6);

                    using (LinQDataContext linq = new LinQDataContext())
                    {
                        linq.SP_InsertarUsuario(
                            req.Usuario.Nombre,
                            req.Usuario.Apellido1,
                            req.Usuario.Apellido2,
                            req.Usuario.FechaNacimiento,
                            req.Usuario.Correo,
                            llave,
                            passHash,
                            pinVerificacion,
                            ref idBD,
                            ref errorIdBD,
                            ref errorMsgBD
                        );
                    }

                    if (idBD > 0)
                    {
                        bool correoEnviado = HelperMailcs.EnviarCorreoVerificacion(req.Usuario.Correo, req.Usuario.Nombre, pinVerificacion);
                        res.resultado = correoEnviado;

                        if (!correoEnviado)
                        {
                            res.error.Add(new Error
                            {
                                ErrorCode = (int)enumErrores.excepcionLogica,
                                Message = "Error al enviar correo de verificación"
                            });
                        }
                    }
                    else
                    {
                        res.resultado = false;
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)enumErrores.excepcionBaseDatos,
                            Message = errorMsgBD
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
        #region helpers
        public bool EsCorreoValido(string correo)
        {
            // Verifica que el correo no sea nulo o vacío.
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            // Patrón simple para validar correo electrónico.
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(correo, patron);
        }

        public bool EsPasswordSeguro(string password)
        {
            // Verifica que el password no sea nulo o vacío.
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Patrón que valida el password según los criterios mencionados.
            string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            return Regex.IsMatch(password, patron);
        }

        private int GenerarPin(int digitos)
        {
            if (digitos != 6)
                throw new ArgumentException("Solo se permite generar PIN de 6 dígitos por ahora.");

            Random rnd = new Random();
            return rnd.Next(100000, 1000000); // 100000 inclusive, 1000000 exclusivo
        }

        private string hashearPassword(string passwordUsuario, string key)
        {
            return this.GetMD5Hash(passwordUsuario + key); //INVESTIGAR!!!
        }

        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.Unicode.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convertir los bytes de hash a una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private bool EsMayorDeEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

            return edad >= 18;
        }
        #endregion
    }
}
