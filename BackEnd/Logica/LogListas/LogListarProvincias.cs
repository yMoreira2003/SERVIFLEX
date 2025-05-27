using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarProvincias
    {
        public ResListarProvincias listarProvincias()
        {
            ResListarProvincias res = new ResListarProvincias();
            res.resultado = false;
            res.error = new List<Error>();

            try
            {
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarProvinciasResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarProvincias(
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Provincias = factoriaListaProvincias(resultado);
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

        private List<Provincia> factoriaListaProvincias(List<SP_ListarProvinciasResult> lista)
        {
            List<Provincia> provincias = new List<Provincia>();

            foreach (var item in lista)
            {
                Provincia provincia = new Provincia
                {
                    ProvinciaId = item.PROVINCIA_ID,
                    Nombre = item.NOMBRE,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    UpdatedAt = item.UPDATED_AT ?? DateTime.MinValue
                };

                provincias.Add(provincia);
            }

            return provincias;
        }
    }
}
