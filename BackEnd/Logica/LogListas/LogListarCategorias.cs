using AccesoDatos;
using BackEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logica
{
    public class LogListarCategorias
    {
        public ResListarCategorias listarCategorias()
        {
            ResListarCategorias res = new ResListarCategorias();
            res.resultado = false;
            res.error = new List<Error>();

            try
            {
                int? idReturn = 0;
                int? errorID = 0;
                string errorDescripcion = "";

                List<SP_ListarCategoriasResult> resultado;

                using (LinQDataContext db = new LinQDataContext())
                {
                    resultado = db.SP_ListarCategorias(
                        ref idReturn,
                        ref errorID,
                        ref errorDescripcion
                    ).ToList();
                }

                if (errorID == 0 && resultado != null)
                {
                    res.Categorias = factoriaListaCategorias(resultado);
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

        private List<Categoria> factoriaListaCategorias(List<SP_ListarCategoriasResult> lista)
        {
            List<Categoria> categorias = new List<Categoria>();

            foreach (var item in lista)
            {
                categorias.Add(new Categoria
                {
                    CategoriaId = item.CATEGORIA_ID,
                    Nombre = item.NOMBRE,
                    CreatedAt = item.CREATED_AT ?? DateTime.MinValue,
                    UpdatedAt = item.UPDATED_AT ?? DateTime.MinValue
                });
            }
            return categorias;
        }
    }
}
