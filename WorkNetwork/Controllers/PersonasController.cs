﻿//using System.Runtime.Intrinsics.Arm;
//using WorkNetwork.Migrations;

using WorkNetwork.Models;

namespace WorkNetwork.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public PersonasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // BUSCO EL USUARIO ACTUAL
            var usuarioActual = _userManager.GetUserId(HttpContext.User);

            var rolUsuario = _context.UserRoles.Where(u => u.UserId == usuarioActual).FirstOrDefault();
            //EN BASE A ESE ID BUSCAMOS EN LA TABLA DE RELACION USUARRIO-ROL QUE REGISTRO TIENE
            var rolNombre = _context.Roles.Where(u => u.Id == rolUsuario.RoleId).Select(r => r.Name).FirstOrDefault();
            //EN BASE AL USUARIO BUSCO EN LA TABLA PARA VER SI ESTA RELACIONADO A ALGUAN PERSONA. 
            if (rolNombre is "Usuario")
            {

                var personaUsuario = (from p in _context.PersonaUsuarios where p.UsuarioID == usuarioActual select p).Count();
                if (personaUsuario == 0)
                {
                    return RedirectToAction("NewPerson", "Personas");
                }
            }
            return View();
        }

        public JsonResult PerfilInfo()
        {
            //BUSCO L USUARIO ACTUAL
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            //BUSCO LA RELACION ENTRE PERSONA USUARIO
            var personaUsuario = _context.PersonaUsuarios.Where(u => u.UsuarioID == usuarioActual).FirstOrDefault();
            //SEGUN EL ID DE LA PERSONA OBTENGO TODA LA COLUMNA
            var persona = _context.Persona.Where(u => u.PersonaID == personaUsuario.PersonaID).FirstOrDefault();
            return Json(persona);
        }

        public IActionResult PerfilUser(int? id)
        {
            var personaMostrar = new PersonaMostrar();
            if (id == null)
            {
                //BUSCO L USUARIO ACTUAL
                var usuarioActual = _userManager.GetUserId(HttpContext.User);
                if (usuarioActual == null)
                {  return RedirectToAction("Index","Home"); }
                //BUSCO LA RELACION ENTRE PERSONA USUARIO
                var personaUsuario = _context.PersonaUsuarios.Where(u => u.UsuarioID == usuarioActual).FirstOrDefault();
                if(personaUsuario == null)
                {
                    return RedirectToAction("NewPerson", "Personas");
                }
                //SEGUN EL ID DE LA PERSONA OBTENGO TODA LA COLUMNA
                var persona = _context.Persona.Where(u => u.PersonaID == personaUsuario.PersonaID).FirstOrDefault();
                var correo = _context.Users.Where(u => u.Id == personaUsuario.UsuarioID).Select(c => c.Email).Single();
                var localidadNombre = _context.Localidad.Where(u => u.LocalidadID == persona.LocalidadID).Select(l => l.NombreLocalidad).Single();
                personaMostrar.PersonaID = persona.PersonaID;
                personaMostrar.NombrePersona = persona.NombrePersona;
                personaMostrar.ApellidoPersona = persona.ApellidoPersona;
                personaMostrar.Telefono1 = persona.Telefono1;
                personaMostrar.NumeroDocumento = persona.NumeroDocumento;
                personaMostrar.FechaNacimiento = persona.FechaNacimiento;
                //personaMostrar.FechaNacimientoString = persona.FechaNacimiento.ToString("dd/MM/yyyy");
                personaMostrar.DomicilioPersona = persona.DomicilioPersona;
                personaMostrar.Instagram = persona.Instagram;
                personaMostrar.Linkedin = persona.Linkedin;
                personaMostrar.Twitter = persona.Twitter;
                personaMostrar.LocalidadNombre = localidadNombre;
                personaMostrar.Correo = correo;

                var paises = _context.Pais.Where(x => x.Eliminado == false).ToList();
                paises.Add(new Pais { PaisID = 0, NombrePais = "[SELECCIONE UN PAIS]" });
                ViewBag.PaisID = new SelectList(paises.OrderBy(e => e.NombrePais), "PaisID", "NombrePais");

                var provincias = _context.Provincia.Where(x => x.Eliminado == false).ToList();
                provincias.Add(new Provincia { ProvinciaID = 0, NombreProvincia = "[SELECCIONE UN PAIS]" });
                ViewBag.ProvinciaID = new SelectList(provincias.OrderBy(x => x.NombreProvincia), "ProvinciaID", "NombreProvincia");

                var localidad = _context.Localidad.Where(x => x.Eliminado == false).ToList();
                localidad.Add(new Localidad { LocalidadID = 0, NombreLocalidad = "[SELECCIONE UN PAIS]" });
                ViewBag.LocalidadID = new SelectList(localidad.OrderBy(x => x.NombreLocalidad), "LocalidadID", "NombreLocalidad");

                if (persona.Imagen != null)
                {
                    personaMostrar.ImagenPersona = persona.Imagen;
                    personaMostrar.TipoImagen = persona.TipoImagen;
                    personaMostrar.Imagen = Convert.ToBase64String(persona.Imagen);
                }
                if (persona.Curriculum != null)
                {
                    personaMostrar.Curriculum = persona.Curriculum;
                    personaMostrar.TipoCV = persona.TipoCV;
                    personaMostrar.CurriculumString = Convert.ToBase64String(persona.Curriculum);
                }

                personaMostrar.Eliminado = persona.Eliminado;
            }
            else
            {
                //Agregar redes, telefono, localidad y correo
                ViewBag.VistaLectura = 2;
                var personaUsuario = _context.PersonaUsuarios.Where(u => u.PersonaID == id).FirstOrDefault();
                var persona = _context.Persona.Where(u => u.PersonaID == id).FirstOrDefault();
                var correo = _context.Users.Where(c => c.Id == personaUsuario.UsuarioID).Select(c => c.Email).Single();
                var localidadNombre = _context.Localidad.Where(u => u.LocalidadID == persona.LocalidadID).Select(l => l.NombreLocalidad).Single();

                personaMostrar.PersonaID = persona.PersonaID;
                personaMostrar.NombrePersona = persona.NombrePersona;
                personaMostrar.ApellidoPersona = persona.ApellidoPersona;
                personaMostrar.NumeroDocumento = persona.NumeroDocumento;
                personaMostrar.Telefono1 = persona.Telefono1;
                personaMostrar.FechaNacimiento = persona.FechaNacimiento;
                personaMostrar.DomicilioPersona = persona.DomicilioPersona;
                personaMostrar.Instagram = persona.Instagram;
                personaMostrar.Linkedin = persona.Linkedin;
                personaMostrar.Twitter = persona.Twitter;
                personaMostrar.LocalidadNombre = localidadNombre;
                personaMostrar.Correo = correo;



                var paises = _context.Pais.ToList();
                paises.Add(new Pais { PaisID = 0, NombrePais = "[SELECCIONE UN PAIS]" });
                ViewBag.PaisID = new SelectList(paises.OrderBy(e => e.NombrePais), "PaisID", "NombrePais");

                var provincias = _context.Provincia.ToList();
                provincias.Add(new Provincia { ProvinciaID = 0, NombreProvincia = "[SELECCIONE UN PAIS]" });
                ViewBag.ProvinciaID = new SelectList(provincias.OrderBy(x => x.NombreProvincia), "ProvinciaID", "NombreProvincia");

                var localidad = _context.Localidad.ToList();
                localidad.Add(new Localidad { LocalidadID = 0, NombreLocalidad = "[SELECCIONE UN PAIS]" });
                ViewBag.LocalidadID = new SelectList(localidad.OrderBy(x => x.NombreLocalidad), "LocalidadID", "NombreLocalidad");
                if (persona.Imagen != null)
                {
                    personaMostrar.ImagenPersona = persona.Imagen;
                    personaMostrar.TipoImagen = persona.TipoImagen;
                    personaMostrar.Imagen = Convert.ToBase64String(persona.Imagen);
                }
                personaMostrar.Eliminado = persona.Eliminado;
            }

            ViewData["persona"] = personaMostrar;
            return View();
        }

        public IActionResult NewPerson()
        {
            var paises = _context.Pais.Where(x=>x.Eliminado == false).ToList();
            paises.Add(new Pais { PaisID = 0, NombrePais = "[SELECCIONE UN PAIS]" });
            ViewBag.PaisID = new SelectList(paises.OrderBy(e => e.NombrePais), "PaisID", "NombrePais");

            var provincias = _context.Provincia.Where(x => x.Eliminado == false).ToList();
            provincias.Add(new Provincia { ProvinciaID = 0, NombreProvincia = "[SELECCIONE UNA PROVINCIA]" });
            ViewBag.ProvinciaID = new SelectList(provincias.OrderBy(x => x.NombreProvincia), "ProvinciaID", "NombreProvincia");

            var localidad = _context.Localidad.Where(x => x.Eliminado == false).ToList();
            localidad.Add(new Localidad { LocalidadID = 0, NombreLocalidad = "[SELECCIONE UN LOCALIDAD]" });
            ViewBag.LocalidadID = new SelectList(localidad.OrderBy(x => x.NombreLocalidad), "LocalidadID", "NombreLocalidad");

            return View();
        }
        public JsonResult TablaPersonas()
        {
            var personas = _context.Persona.ToList();
            return Json(personas);
        }

        //--------------------PARAMETROS DEL GUARDAR PERSONA ---------------------------
        //Metodo para limpiar el numero telefonico
        static string ClearNumber(string numero) => new string((numero ?? "").Where(c => c == '+' || char.IsNumber(c)).ToArray());
        public JsonResult GuardarPersona(int IdPersona, string nombrePersona, string apellidoPersona, int numeroDocumento, DateTime fechaNacimiento, int LocalidadID, string domicilio, int nro, string telefono1Persona, string instagram, string twitter, string linkedin, int generoID, IFormFile curriculPersona, IFormFile personaFoto)
        {
            bool resultado = false;
            string msjError = null;

            var existePersona = _context.Persona.Where(p => p.NumeroDocumento == numeroDocumento).Count();
            if (existePersona == 0)
            {

                byte[] cv = null;
                string tipoCV = null;
                byte[] img = null;
                string tipoImg = null;
                if (personaFoto != null)
                {
                    if (personaFoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            personaFoto.CopyTo(ms);
                            img = ms.ToArray();
                            tipoImg = personaFoto.ContentType;
                        }
                    }
                }
                if (curriculPersona != null)
                {
                    if (curriculPersona.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            curriculPersona.CopyTo(ms);
                            cv = ms.ToArray();
                            tipoCV = curriculPersona.ContentType;
                        }
                    }
                }

                var generoEnum = Genero.Masculino;

                if (generoID is 1)
                {
                    generoEnum = Genero.Femenino;
                }
                if (generoID is 2)
                {
                    generoEnum = Genero.Otro;
                }

                var telefono1Clean = ClearNumber(telefono1Persona);
                var persona = new Persona
                {
                    NombrePersona = nombrePersona,
                    ApellidoPersona = apellidoPersona,
                    NumeroDocumento = numeroDocumento,
                    FechaNacimiento = fechaNacimiento,
                    LocalidadID = LocalidadID,
                    DomicilioPersona = domicilio,
                    Telefono1 = telefono1Clean,
                    Instagram = instagram,
                    Twitter = twitter,
                    Linkedin = linkedin,
                    Genero = generoEnum,
                    Curriculum = cv,
                    TipoCV = tipoCV,
                    TipoImagen = tipoImg,
                    Imagen = img
                };
                _context.Add(persona);
                _context.SaveChanges();

                var usuarioActual = _userManager.GetUserId(HttpContext.User);
                var nuevaPersonaUsuario = new PersonaUsuario
                {
                    UsuarioID = usuarioActual,
                    PersonaID = persona.PersonaID
                };
                _context.Add(nuevaPersonaUsuario);
                _context.SaveChanges();

                resultado = true;
            }
            else
            {
                msjError = "El DNI ya está siendo usado por otra persona.";
                resultado = false;
            }

            return Json(new { resultado, msjError });
        }

        public JsonResult EditarPersona(int IdPersona, string nombrePersona, string apellidoPersona,int numeroDocumento,int LocalidadID, string domicilio, string telefono1Persona, string correoPersona)
        {
            bool resultado = false;
            string msjError = string.Empty;
            //validamos que no haya otra persona con el mismo DNI
            var existePersona = _context.Persona
                .Where(p => p.NumeroDocumento == numeroDocumento && p.PersonaID != IdPersona)
                .Count();

            if (existePersona == 0)
            {

                var telefono1Clean = ClearNumber(telefono1Persona);
                //string domicilioCompleto = domicilio + " " + nro;
                var persona = _context.Persona
                    .FirstOrDefault(p => p.PersonaID == IdPersona);

                var usuarioActual = _userManager.GetUserId(HttpContext.User);
                var personaUsuario = _context.PersonaUsuarios
                    .Where(u => u.UsuarioID == usuarioActual).FirstOrDefault();

                var user = _context.Users
                    .Where(u => u.Id == personaUsuario.UsuarioID).Single();

                //para actualizar el correo, primero comparamos que no haya ya otro registrado
                //luego se actualizan los datos y la persona se puede loguear con el nuevo mail
                var existeUsuario = _context.Users
                    .Where(u => u.Email == correoPersona && u.Id != personaUsuario.UsuarioID).Count();

                if (existeUsuario == 0)
                {
                    persona.NombrePersona = nombrePersona;
                    persona.ApellidoPersona = apellidoPersona;
                    persona.LocalidadID = LocalidadID;
                    persona.DomicilioPersona = domicilio;
                    persona.NumeroDocumento = numeroDocumento;
                    user.Email = correoPersona;
                    user.UserName = correoPersona;
                    user.NormalizedEmail = correoPersona.ToUpper();
                    user.NormalizedUserName = correoPersona.ToUpper();
                    persona.Telefono1 = telefono1Clean;

                    _context.Update(persona);
                    _context.Update(user);
                    _context.SaveChanges();

                    resultado = true;
                }
                else
                {
                    msjError = "El correo ya está registrado";
                }

            }
            else
            {
                msjError = "Ya existe una persona con el mismo DNI";
            }

            return Json (new{ resultado, msjError });
        }

        public JsonResult BuscarPersona(int PersonaID)
        {
            var persona = _context.Persona.Include(l => l.Localidad).Single(p => p.PersonaID == PersonaID);
            var provincia = _context.Provincia.Where(p => p.ProvinciaID == persona.Localidad.ProvinciaID).FirstOrDefault();
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            var personaUsuario = _context.PersonaUsuarios.Where(u => u.UsuarioID == usuarioActual).FirstOrDefault();
            var correo = _context.Users.Where(u => u.Id == personaUsuario.UsuarioID).Select(c => c.Email).Single();

            var personaVer = new PersonaMostrar
            {
                PersonaID = persona.PersonaID,
                NombrePersona = persona.NombrePersona,
                ApellidoPersona = persona.ApellidoPersona,
                Telefono1 = persona.Telefono1,
                NumeroDocumento = persona.NumeroDocumento,
                FechaNacimiento = persona.FechaNacimiento,
                FechaNacimientoString = persona.FechaNacimiento.ToString("dd/MM/yyyy"),
                PaisID = provincia.PaisID,
                ProvinciaID = provincia.ProvinciaID,
                LocalidadID = persona.LocalidadID,
                DomicilioPersona = persona.DomicilioPersona,
                Instagram = persona.Instagram,
                Linkedin = persona.Linkedin,
                Twitter = persona.Twitter,
                Genero = persona.Genero,
                Correo = correo,
                
            };

            return Json(personaVer);
        }

        public IActionResult VerEmpresa(int? id)
        {
            
            //Busca al usuario antes de cerrar sesion
            var usuarioActual = _userManager.GetUserId(HttpContext.User);
            if (usuarioActual == null)
            { return RedirectToAction("Index", "Home"); }


            var empresaUsuario = _context.EmpresaUsuarios
                .Where(u => u.EmpresaID == id)
                .FirstOrDefault();

            var empresa = _context.Empresa
                .Where(u => u.EmpresaID == id)
                .FirstOrDefault();

            var localidadNombre = _context.Localidad
                .Where(u => u.LocalidadID == empresa.LocalidadID)
                .Select(l => l.NombreLocalidad)
                .FirstOrDefault();

            var correo = _context.Users
                .Where(c => c.Id == empresaUsuario.UsuarioID)
                .Select(c => c.Email)
                .Single();

            var rubroNombre = _context.Rubro
                .Where(r => r.RubroID == empresa.RubroID)
                .Select(r => r.NombreRubro)
                .Single();

            var empresaMostrar = new EmpresaMostrar();
            {
                empresaMostrar.EmpresaID = empresa.EmpresaID;
                empresaMostrar.RazonSocial = empresa.RazonSocial;
                empresaMostrar.Descripcion = empresa.Descripcion;
                empresaMostrar.CUIT = empresa.CUIT;
                empresaMostrar.Domicilio = empresa.Domicilio;
                empresaMostrar.Localidad = localidadNombre;
                empresaMostrar.Telefono1 = empresa.Telefono1;
                empresaMostrar.Instagram = empresa.Instagram;
                empresaMostrar.Twitter = empresa.Twitter;
                empresaMostrar.Linkedin = empresa.Linkedin;
                empresaMostrar.Domicilio = empresa.Domicilio;
                empresaMostrar.Rubro = rubroNombre;
                empresaMostrar.Correo = correo;
            }
                if (empresa.Imagen != null)
                {
                    empresaMostrar.ImagenEmpresa = empresa.Imagen;
                    empresaMostrar.TipoImagen = empresa.TipoImagen;
                    empresaMostrar.Imagen = Convert.ToBase64String(empresa.Imagen);
                }
                empresaMostrar.Eliminado = empresa.Eliminado;

                ViewData["empresa"] = empresaMostrar;

            return View();
        }
    }

}









