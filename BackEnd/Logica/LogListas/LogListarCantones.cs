using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarCantones
    {
        public ResListarCantones listarCantones()
        {
            ResListarCantones res = new ResListarCantones();
            res.resultado = false;
            res.error = new List<Error>();

            try
            {
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarCantonesResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarCantones(
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Cantones = factoriaListaCantones(resultado);
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

        private List<Canton> factoriaListaCantones(List<SP_ListarCantonesResult> lista)
        {
            List<Canton> cantones = new List<Canton>();

            foreach (var item in lista)
            {
                Canton canton = new Canton
                {
                    CantonId = item.CANTON_ID,
                    Nombre = item.NOMBRE,
                    Provincia = new Provincia
                    {
                        Nombre = item.PROVINCIA
                    }
                };
                cantones.Add(canton);
            }
            return cantones;
        }
    }
}
