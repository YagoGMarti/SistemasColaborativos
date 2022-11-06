using SistemasColaborativos.Models;
using SistemasColaborativos.Transitional;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Business
{
    public class ColaborativosContext : DbContext
    {
        public ColaborativosContext() : base("Colaborativos")
        {
            Database.SetInitializer<ColaborativosContext>(new SchoolDBInitializer<ColaborativosContext>());
        }

        internal void GuardarEvento(Evento evento)
        {
            Eventos.Add(evento);
            this.SaveChanges();
        }

        internal void EditarEvento(Evento evento)
        {
            var nuevoEvento = Eventos.Find(evento.ID);
            nuevoEvento.Fecha = evento.Fecha;
            nuevoEvento.Titulo = evento.Titulo;
            nuevoEvento.Objetivo = evento.Objetivo;
            this.SaveChanges();
        }


        internal IEnumerable<SemanaCalendario> GetDiasCalendario(DateTime fechaReferencia)
        {
            var primerDia = new DateTime(fechaReferencia.Year, fechaReferencia.Month, 1);
            var ultimoDia = (primerDia.AddMonths(1)).AddDays(-1);

            var eventos = Eventos.Where(x => x.Fecha < ultimoDia && x.Fecha > primerDia)
                    .ToList();

            SemanaCalendario[] semanas = new SemanaCalendario[6];
            semanas = semanas.Select(x => new SemanaCalendario()).ToArray();

            int diaInicial = (int)fechaReferencia.DayOfWeek;
            for (int dia = 0; dia < 7; dia++)
            {
                if (dia >= (diaInicial))
                    semanas[0].DiasEventos[dia] = new DiaCalendario(primerDia.AddDays(dia - diaInicial), eventos.Where(x => x.Fecha.Date == primerDia.AddDays(dia - diaInicial).Date));
            }

            for (int sem = 1; sem < semanas.Length; sem++)
                for (int dia = 0; dia < 7; dia++)
                {
                    var dias = (sem * 7 + dia) - diaInicial;
                    if (primerDia.AddDays(dias).Month == primerDia.Month)
                        semanas[sem].DiasEventos[dia] = new DiaCalendario(primerDia.AddDays(dias), eventos.Where(x => x.Fecha.Date == primerDia.AddDays(dias).Date));
                }

            return semanas.Where(x => x.DiasEventos.Any(dia => dia != null));
        }

        internal void BorrarEvento(Guid eventoID)
        {
            var evento = GetEvento(eventoID);
            Eventos.Remove(evento);
            SaveChanges();
        }

        internal IEnumerable<Evento> GetEventos(DateTime fecha)
        {
            var inicioDia = fecha.Date;
            var finDia = inicioDia.AddDays(1).AddSeconds(-1);

            var eventos = Eventos.Where(x => x.Fecha <= finDia && x.Fecha >= inicioDia).AsEnumerable();
            return eventos;
        }

        internal Evento GetEvento(Guid eventoID) => Eventos.FirstOrDefault(x => x.ID == eventoID);
        internal IEnumerable<Evento> GetEventos() => Eventos.AsEnumerable();

        public DbSet<Evento> Eventos { get; set; }

        public class SchoolDBInitializer<T> : CreateDatabaseIfNotExists<ColaborativosContext>
        {
            protected override void Seed(ColaborativosContext context)
            {
                context.Eventos.Add(new Evento(new DateTime(2022, 08, 21), "Actividad 1 [API1]",
                    "Analizar y comparar sitios web a través de software pertinente que permita realizar un análisis FODA sobre las páginas seleccionadas."));

                context.Eventos.Add(new Evento(new DateTime(2022, 09, 11), "Actividad 2 [API2]",
                    "Diseñar y desarrollar los módulos de una aplicación móvil compartiendo el proyecto realizado en un almacenamiento gratuito en la nube."));

                context.Eventos.Add(new Evento(new DateTime(2022, 10, 16), "Actividad 3 [API3]",
                    "Diseñar y programar el SharePoint Framework para el caso presentado como situación problemática del lado del cliente."));

                context.Eventos.Add(new Evento(new DateTime(2022, 11, 13), "Actividad 4 [API4]",
                    "Diseñar y programar un calendario para el proyecto del caso presentado como situación problemática, empleando una herramienta de software para tal fin."));

                context.SaveChanges();

                base.Seed(context);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }
    }
}