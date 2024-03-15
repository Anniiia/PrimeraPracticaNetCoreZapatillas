using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrimeraPracticaNetCoreZapatillas.Data;
using PrimeraPracticaNetCoreZapatillas.Models;
using System.Data;

#region PROCEEDIMIENTOS ALMACENADOS
//create procedure SP_ZAPATILLAS_OUT
//(@posicion int, @idzapatilla int
//, @registros int out)
//as
//select @registros = count(idproducto) from IMAGENESZAPASPRACTICA
//where idproducto=@idzapatilla
//select idimagen, idproducto, imagen from 
//    (select cast(
//    ROW_NUMBER() OVER (ORDER BY idimagen) as int) AS POSICION
//   , idimagen, idproducto, imagen
//   from IMAGENESZAPASPRACTICA
//    where idproducto=@idzapatilla) as QUERY
//    where QUERY.POSICION >= @posicion
//go

//declare @registros int 
//set @registros = 0
//exec SP_ZAPATILLAS_OUT 1,1, @registros out

#endregion

namespace PrimeraPracticaNetCoreZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task<Zapatilla> FindZapatillaAsync(int idzapatilla)
        {
            return await this.context.Zapatillas
                .FirstOrDefaultAsync(x => x.IdProducto == idzapatilla);
        }

        public async Task<ModelPaginacionZapatillas> GetImagenesZapatillaAsync(int posicion, int idzapatilla)
        {
            string sql = "SP_ZAPATILLAS_OUT @posicion, @idzapatilla, "
                + " @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamZapatilla =
                new SqlParameter("@idzapatilla", idzapatilla);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Imagenes.FromSqlRaw
                (sql, pamPosicion, pamZapatilla, pamRegistros);
            var datos = await consulta.ToListAsync();
            Imagen imagen = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new ModelPaginacionZapatillas
            {
                NumeroRegistros = registros,
                ImagenZap = imagen
                
            };
        }

    }
}
