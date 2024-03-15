using Microsoft.AspNetCore.Mvc;
using PrimeraPracticaNetCoreZapatillas.Models;
using PrimeraPracticaNetCoreZapatillas.Repositories;

namespace PrimeraPracticaNetCoreZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;

        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> Details(int? posicion,int idzapatilla)
        {

            if (posicion == null)
            {
                posicion = 1;
            }
            ModelPaginacionZapatillas model = await
                this.repo.GetImagenesZapatillaAsync
                (posicion.Value, idzapatilla);
            //Zapatilla zapatilla =
            //    await this.repo.FindDepartamentosAsync(idzapatilla);
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idzapatilla);
            ViewData["ZAPATILLASELECCIONADA"] = zapatilla;
            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["ZAPATILLA"] = idzapatilla;
            int siguiente = posicion.Value + 1;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > model.NumeroRegistros)
            {
                //EFECTO OPTICO
                siguiente = model.NumeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.NumeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            

            return View(model.Zapatilla);
        }

        


        public async Task<IActionResult> _ImagenesZapatillaPartial(int? posicion, int idzapatilla)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ModelPaginacionZapatillas model =
                await this.repo.GetImagenesZapatillaAsync
                (posicion.Value, idzapatilla);
            int numeroRegistros = model.NumeroRegistros;
            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ÚLTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["ZAPATILLASELECCIONADA"] = model.Zapatilla;
            ViewData["POSICION"] = posicion;
            return PartialView("_ImagenesZapatillaPartial", model.ImagenZap);
        }

        public async Task<IActionResult> NuevasImagenes()
        {

            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }
    }
}
