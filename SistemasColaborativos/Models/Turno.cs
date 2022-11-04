using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemasColaborativos.Models
{
    public class Turno
    {
        public Turno()
        {

        }

        public Turno(Guid id, DateTime fecha, Paciente paciente, Sucursal sucursal, Medico medico)
        {
            Id = id;
            Fecha = fecha;
            Paciente = paciente;
            Sucursal = sucursal;
            Medico = medico;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Fecha { get; set; }

        public Paciente Paciente { get; set; }
        public Sucursal Sucursal { get; set; }
        public Medico Medico { get; set; }

        [NotMapped]
        public virtual string Notas => $"{Fecha.ToShortDateString()} : {Fecha.ToShortTimeString()} - Paciente {Paciente.Nombre} ({Paciente.ObraSocial.ToString()}) con profesional {Medico.Nombre} - {GetEspecialidadNombre(Medico.Especialidad)}";


        public static string GetEspecialidadNombre(EspecialidadEnum especialidadEnum)
        {
            switch (especialidadEnum)
            {
                case EspecialidadEnum.CirugiaGeneral: return "Cirugía General";
                case EspecialidadEnum.ClinicaMedica: return "Clínica Médica";
                case EspecialidadEnum.DiagnosticoPorImagenes: return "Diagnóstico por Imágenes";
                case EspecialidadEnum.Laboratorio: return "Laboratorio";
                default: return "No indicado";
            }
        }
    }
}