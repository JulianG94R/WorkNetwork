﻿namespace WorkNetwork.Models

{
    public class Vacante
    {
        [Key]
        public int VacanteID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? ExperienciaRequerida { get; set; }
        public int RubroID { get; set; }
        public int LocalidadID { get; set; }
        public DateTime FechaDeFinalizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Idiomas { get; set; }
        public byte[]? Imagen { get; set; }
        public string? TipoImagen{ get; set; }
        public string? Estado { get; set; }
        public bool Eliminado { get; set; }
        public DisponibilidadHoraria DisponibilidadHoraria{ get; set; }

        public tipoModalidad tipoModalidad{ get; set; }
        //public virtual ICollection<PersonaVacante>? PersonaVacante { get; set; }
    }

    public enum DisponibilidadHoraria
    {
        fulltime, partime
    }
    public enum tipoModalidad
    {
        presencial, semipresencial, remoto
    }

    public class VacanteMostrar
    {
        public int VacanteID { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? ExperienciaRequerida { get; set; }
        public int EmpresaID { get; set; }
        public string EmpresaNombre { get; set; }
        public int RubroID { get; set; }
        public string RubroNombre {get; set;}
        public int LocalidadID { get; set; }
        public string LocalidadNombre {get; set;}
        public int ProvinciaID { get; set; }
        public string ProvinciaNombre { get; set; }
        public int PaisID { get; set; }
        public string PaisNombre { get; set; }
        public DateTime FechaDeFinalizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string? Idiomas { get; set; }
        public string? Estado { get; set; }
        public bool Eliminado { get; set; }
        public string ImagenVacante {get; set;}
        public string? TipoImagen{ get; set; }
        public DisponibilidadHoraria DisponibilidadHoraria{ get; set; }

        public tipoModalidad tipoModalidad{ get; set; }
    }
   
}
