﻿namespace WorkNetwork.Models
{
    public class Persona
    {
        public int idPersona { get; set; }
        public string nombrePersona { get; set; }
        public string apellidoPersona { get; set; }
        //public string tipoDocumento { get; set; }
        public enum tipoDocumento { dni, LE, }
        public int numeroDocumento { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string correoElectronico { get; set; }
        public string domicilioPersona { get; set; }
        public int idLocalidad { get; set; }
        public virtual Localidad Localidad { get; set; }
        public string Genero { get; set; }
        public int telefono1 { get; set; }
        public int telefono2 { get; set; }
        public int telefono3 { get; set; }
        public string estadoCivil { get; set; }
        public string tituloAcademico { get; set; }
        public int idSubRubro { get; set; }
        public virtual SubRubro SubRubro { get; set; }
        public int  cantidadHijos { get; set; }
    }
}