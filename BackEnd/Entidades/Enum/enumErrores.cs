﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public enum enumErrores
    {
        excepcionBaseDatos = -2,
        excepcionLogica = -1,
        requestNulo = 1,
        nombreFaltante = 2,
        apellidoFaltante = 3,
        correoFaltante = 4,
        passwordFaltante = 5,
        correoIncorrecto = 6,
        passwordMuyDebil = 7,
        idFaltante = 8,
        sesionCerrada = 9,
        verificacionFallida = 10,
        verificacionExpirada = 11,
        usuarioFaltante = 12,
        temaFaltante = 13,
        tituloFaltante = 14,
        mensajeFaltante = 15,
        idPublicacionFaltante = 16,
        correoEnviadoFallido = 17,
        usuarioYaExiste = 18,
        usuarioNoEncontrado = 19,
        parametrosInvalidos = 20,
        servicioNoEncontrado = 21,
        datosInvalidos = 22,
        usuarioMenorEdad = 107,
        fechaNacimientoInvalida = 108,
    }
}
