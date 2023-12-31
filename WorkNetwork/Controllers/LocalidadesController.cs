﻿using WorkNetwork.Models;

namespace WorkNetwork.Controllers
{
    [Authorize]
    public class LocalidadesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LocalidadesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public IActionResult Index()
        {
            //Empresa actual
            var paises = _context.Pais.Where(x => x.Eliminado == false).ToList();
            paises.Add(new Pais { PaisID = 0, NombrePais = "[SELECCIONE UN PAIS]" });
            ViewBag.PaisID = new SelectList(paises.OrderBy(e => e.NombrePais), "PaisID", "NombrePais");

            List<Provincia> provincias = new List<Provincia>();
            provincias.Add(new Provincia { ProvinciaID = 0, NombreProvincia = "[SELECCIONE UN PAIS]" });
            ViewBag.ProvinciaID = new SelectList(provincias.OrderBy(x => x.NombreProvincia), "ProvinciaID", "NombreProvincia");
            return View(_context.Localidad.ToList());

        }

        public JsonResult TablaLocalidades()
        {
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            EmpresaUsuario empresaUsuarioActual = new EmpresaUsuario();
            // BuscarEmpresaActual(usuarioActual, empresaUsuarioActual);

            var localidades = _context.Localidad.Include(p => p.Provincia.Pais).ToList();

            List<LocalidadMostrar> localidadesMostrar = localidades
                .Select(localidad => new LocalidadMostrar
                {
                    LocalidadID = localidad.LocalidadID,
                    NombreLocalidad = localidad.NombreLocalidad,
                    CP = localidad.CP,
                    ProvinciaID = localidad.ProvinciaID,
                    PaisID = localidad.Provincia.PaisID,
                    NombreProvincia = localidad.Provincia.NombreProvincia,
                    NombrePais = localidad.Provincia.Pais.NombrePais,
                    Eliminado = localidad.Eliminado

                })
                .OrderBy(l => l.NombreProvincia)
                .ThenBy(l => l.NombreLocalidad)
                .ToList();

            return Json(localidadesMostrar);
        }

    
    //}  public JsonResult TablaLocalidades()
    //{
    //    var usuarioActual = _userManager.GetUserId(HttpContext.User);
    //    EmpresaUsuario empresaUsuarioActual = new EmpresaUsuario();
    //    // BuscarEmpresaActual(usuarioActual, empresaUsuarioActual);

    //    var localidades = _context.Localidad.Include(p => p.Provincia.Pais).ToList();

    //    List<LocalidadMostrar> localidadesMostrar = new List<LocalidadMostrar>();
    //    foreach(var localidad in localidades)
    //    {
    //        var localidadMostrar = new LocalidadMostrar
    //        {
    //            LocalidadID = localidad.LocalidadID,
    //            NombreLocalidad = localidad.NombreLocalidad,
    //            CP = localidad.CP,
    //            ProvinciaID = localidad.ProvinciaID,
    //            PaisID = localidad.Provincia.PaisID,
    //            NombreProvincia = localidad.Provincia.NombreProvincia,
    //            NombrePais = localidad.Provincia.Pais.NombrePais,
    //            Eliminado = localidad.Eliminado

    //        };
    //        localidadesMostrar.Add(localidadMostrar);
    //    }

    //    return Json(localidadesMostrar);

    //}

    public JsonResult ComboLocalidades(int id)
            {
                var localidades = _context.Localidad
                    .Where(p => p.ProvinciaID == id && p.Eliminado == false)
                    .ToList();

                return Json(new SelectList(localidades, "LocalidadID", "NombreLocalidad"));
            }

            public JsonResult GuardarLocalidad(int IdLocalidad, string NombreLocalidad, int CP, int ProvinciaID)
            {
                int resultado = 0;

                //Si es 0 es correcto
                //Si es 1 descripcion vacia
                //Si es 2 campo existente
                if (!string.IsNullOrEmpty(NombreLocalidad))
                {
                    NombreLocalidad = NombreLocalidad.ToUpper();
                    //Pregunta si el Codigo Postal es único 
                    if (_context.Localidad.Any(e => e.CP == CP && e.LocalidadID != IdLocalidad))
                    {
                        resultado = 3; //Si ya existe                    
                    }
                    else
                    {
                        if (IdLocalidad is 0)
                        {
                            if (_context.Localidad.Any(e => e.NombreLocalidad == NombreLocalidad && e.ProvinciaID == ProvinciaID))
                            {
                                resultado = 2;
                            }
                            else
                            {
                                var localidad = new Localidad
                                {
                                    NombreLocalidad = NombreLocalidad,
                                    CP = CP,
                                    ProvinciaID = ProvinciaID,

                                };
                                _context.Add(localidad);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            if (_context.Localidad.Any(e => e.NombreLocalidad == NombreLocalidad && e.ProvinciaID == ProvinciaID && e.LocalidadID != IdLocalidad))
                            {
                                resultado = 2;
                            }
                            else
                            {
                                var localidad = _context.Localidad.Single(e => e.LocalidadID == IdLocalidad);
                                localidad.NombreLocalidad = NombreLocalidad;
                                localidad.CP = CP;
                                localidad.ProvinciaID = ProvinciaID;
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                return Json(resultado);
            }

            public JsonResult BuscarLocalidad(int LocalidadID)
            {
                var localidad = _context.Localidad.Include(p => p.Provincia.Pais).FirstOrDefault(m => m.LocalidadID == LocalidadID);

                var localidadVer = new LocalidadMostrar
                {
                    LocalidadID = localidad.LocalidadID,
                    NombreLocalidad = localidad.NombreLocalidad,
                    CP = localidad.CP,
                    NombreProvincia = localidad.Provincia.NombreProvincia,
                    NombrePais = localidad.Provincia.Pais.NombrePais,
                    PaisID = localidad.Provincia.PaisID,
                    ProvinciaID = localidad.Provincia.ProvinciaID,
                };

                return Json(localidadVer);
                //return Json(localidad);
            }


            public JsonResult EliminarLocalidad(int LocalidadID, int Elimina)
            {
                int resultado = 0;

                var localidad = _context.Localidad.Find(LocalidadID);
                if (localidad is not null)
                {
                    if (Elimina is 0)
                    {
                        //Se marca como NO ELIMINADO
                        localidad.Eliminado = false;
                        _context.SaveChanges();
                    }
                    else
                    {
                        //NO SE PUEDE ELIMINAR LOCALIDAD SI TIENE USUARIOS O VACANTES CON DICHA LOCALIDAD
                        var tieneEmpresas = _context.Empresa.Any(e => e.LocalidadID == LocalidadID && e.Eliminado == false);
                        var tieneVacantes = _context.Vacante.Any(v => v.LocalidadID == LocalidadID && v.Eliminado == false);
                        var tienePersonas = _context.Persona.Any(p => p.LocalidadID == LocalidadID && p.Eliminado == false);

                        if (!tieneEmpresas && !tieneVacantes && !tienePersonas)
                        {
                            //SI NO SE ASOCIA A NINGUNO, SE ELIMINA (DESACTIVA POR AHORA)
                            localidad.Eliminado = true;
                            _context.SaveChanges();

                        }
                        else
                        {
                            //SI HAY ASOCIACIONES, SALTA EL ALERT
                            resultado = 1;
                        }
                    }
                }

                return Json(resultado);

            }
        }
        //   private bool LocalidadExists(int id)
        //   {
        //return _context.Localidad.Any(e => e.VacanteID == id);
        //   }
    }
