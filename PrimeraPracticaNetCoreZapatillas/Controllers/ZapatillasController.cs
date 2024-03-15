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

        public async Task<IActionResult> Details(int idzapatilla)
        {
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idzapatilla);

            return View(zapatilla);
        }

        public async Task<IActionResult> _EmpleadosDepartamentoPartial(int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ModelPaginacionDepartamentosEmpleados model =
                await this.repo.GetEmpleadoDepartamentoAsync
                (posicion.Value, iddepartamento);
            int numeroRegistros = model.NumeroRegistrosEmpleados;
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
            ViewData["DEPART"] = model.Departamento;
            ViewData["POSICION"] = posicion;
            return PartialView("_EmpleadosDepartamentoPartial", model.Empleado);
        }

        public async Task<IActionResult> EmpleadosDepartamentoOut
        (int? posicion, int iddepartamento)
        {
            if (posicion == null)
            {
                //POSICION PARA EL EMPLEADO
                posicion = 1;
            }
            ModelPaginacionDepartamentosEmpleados model = await
                this.repo.GetEmpleadoDepartamentoAsync
                (posicion.Value, iddepartamento);
            Departamento departamento =
                await this.repo.FindDepartamentosAsync(iddepartamento);
            ViewData["DEPARTAMENTOSELECCIONADO"] = departamento;
            ViewData["REGISTROS"] = model.NumeroRegistrosEmpleados;
            ViewData["DEPARTAMENTO"] = iddepartamento;
            int siguiente = posicion.Value + 1;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > model.NumeroRegistrosEmpleados)
            {
                //EFECTO OPTICO
                siguiente = model.NumeroRegistrosEmpleados;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.NumeroRegistrosEmpleados;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return View(model.Empleado);
        }
    }
}
