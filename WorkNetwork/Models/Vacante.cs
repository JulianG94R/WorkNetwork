﻿namespace WorkNetwork.Models

{
    public class Vacante
    {
        [Key]
        public int VacanteID { get; set; }
        public int EmpresaID { get; set; }
        public virtual Empresa? Empresa { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? ExperienciaRequerida { get; set; }
        //Agregar la relacion con rubro
        public int LocalidadID { get; set; }
        public DateTime FechaDeFinalizacion { get; set; }
        public string? Idiomas { get; set; }
        public byte[] Imagen { get; set; }
        [NotMapped]
        public string? ImagenString { get; set; }
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
   
}
