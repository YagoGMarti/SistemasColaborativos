using SistemasColaborativos.Models;
using SistemasColaborativos.Models.Transitional;
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

        internal InicioSesion IniciarSesion(string usuario, string clave)
        {
            if (Usuarios.Any(x => x.Nombre == usuario && x.Clave == clave))
                return new InicioSesion() { Usuario = usuario };
            return null;
        }

        internal IEnumerable<Turno> GetTurnos(DateTime fecha)
        {
            var inicioDia = fecha.Date;
            var finDia = fecha.Date.AddDays(1);

            var turnos = Turnos.Where(x => x.Fecha < finDia && x.Fecha > inicioDia)
                    .Include(x => x.Medico)
                    .Include(x => x.Paciente)
                    .Include(x => x.Sucursal)
                    .OrderByDescending(x => x.Fecha)
                    .ToList();

            return turnos;
        }

        internal IEnumerable<SemanaCalendario> GetDiasCalendario(DateTime fechaReferencia)
        {
            var primerDia = new DateTime(fechaReferencia.Year, fechaReferencia.Month, 1);
            var ultimoDia = (primerDia.AddMonths(1)).AddDays(-1);

            var turnos = Turnos.Where(x => x.Fecha < ultimoDia && x.Fecha > primerDia)
                    //.Include(x => x.Medico)
                    //.Include(x => x.Paciente)
                    //.Include(x => x.Sucursal)
                    .ToList();

            SemanaCalendario[] semanas = new SemanaCalendario[6];
            semanas = semanas.Select(x => new SemanaCalendario()).ToArray();

            int diaInicial = (int)fechaReferencia.DayOfWeek;
            for (int dia = 0; dia < 7; dia++)
            {
                if (dia >= (diaInicial))
                    semanas[0].DiasTurnos[dia] = new DiaCalendario(primerDia.AddDays(dia - diaInicial), turnos.Where(x => x.Fecha.Date == primerDia.AddDays(dia - diaInicial).Date));
            }

            for (int sem = 1; sem < semanas.Length; sem++)
                for (int dia = 0; dia < 7; dia++)
                {
                    var dias = (sem * 7 + dia) - diaInicial;
                    if (primerDia.AddDays(dias).Month == primerDia.Month)
                        semanas[sem].DiasTurnos[dia] = new DiaCalendario(primerDia.AddDays(dias), turnos.Where(x => x.Fecha.Date == primerDia.AddDays(dias).Date));
                }

            return semanas.Where(x => x.DiasTurnos.Any(dia => dia != null));
        }

        internal IEnumerable<HoraCalendario> GetOpciones(NuevoTurno nuevoTurno)
        {
            List<HoraCalendario> horarios = new List<HoraCalendario>();
            Sucursal sucursal = GetSucursal(Guid.Parse(nuevoTurno.SucursalId));

            var medicos = Medicos.Where(
                x => x.Especialidad == nuevoTurno.Especialidad
                && x.Sucursal.Id == sucursal.Id)
                    .ToList();

            var primerDia = nuevoTurno.Fecha.AddDays(-(int)nuevoTurno.Fecha.DayOfWeek + 1);
            var ultimoDia = primerDia.AddDays(4);
            var turnos = Turnos.Where(x => x.Fecha < ultimoDia && x.Fecha > nuevoTurno.Fecha
                && x.Sucursal.Id == sucursal.Id)
                    .ToList();
            medicos.ForEach(med => med.Turnos = turnos.Where(x => x.Medico.Id == med.Id));

            var horarioMIN = medicos.Min(x => x.HorarioDesde);
            var horarioMAX = medicos.Max(x => x.HorarioHasta);
            while (horarioMIN < horarioMAX)
            {
                foreach (var med in medicos)
                {

                }
                
                horarioMIN += new TimeSpan(0, 30, 0);
            }

            return horarios;
        }

        public Sucursal GetSucursal(Guid guid) => Sucursales.FirstOrDefault(x => x.Id == guid);
        public IEnumerable<Sucursal> GetSucursales() => Sucursales.Select(x => x);
        public Medico GetMedico(Guid guid) => Medicos.FirstOrDefault(x => x.Id == guid);
        public Paciente GetPaciente(Guid guid) => Pacientes.FirstOrDefault(x => x.Id == guid);
        public Turno GetTurno(Guid guid) => Turnos.FirstOrDefault(x => x.Id == guid);

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Turno> Turnos { get; set; }

        public class SchoolDBInitializer<T> : DropCreateDatabaseAlways<ColaborativosContext>
        {
            protected override void Seed(ColaborativosContext context)
            {
                context.Usuarios.Add(new Usuario(Guid.Parse("00000000-0000-0000-0000-000000000001"), "profe", "profe"));
                context.Sucursales.Add(new Sucursal(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Cerro", "Avenida Recta Martinolli 5151"));
                context.Pacientes.Add(new Paciente(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Juan Perez", ObraSocialEnum.MET));
                context.Pacientes.Add(new Paciente(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Lucas Robles", ObraSocialEnum.MET));
                context.Pacientes.Add(new Paciente(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Martin Luna", ObraSocialEnum.PAMI));
                context.Pacientes.Add(new Paciente(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Marga Lina", ObraSocialEnum.PAMI));
                context.SaveChanges();

                Sucursal sucursal = context.Sucursales.Local.First(x => x.Id == GetGuid("00000000-0000-0000-0000-000000000001"));

                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Clinico 01", "584621", EspecialidadEnum.ClinicaMedica,
                    new TimeSpan(09, 00, 00), new TimeSpan(19, 00, 00), sucursal));
                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Clinico 02", "384623", EspecialidadEnum.ClinicaMedica,
                    new TimeSpan(10, 00, 00), new TimeSpan(17, 00, 00), sucursal));
                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000000011"), "Cirujano 01", "484627", EspecialidadEnum.CirugiaGeneral,
                   new TimeSpan(14, 00, 00), new TimeSpan(17, 00, 00), sucursal));
                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000000101"), "Imagenes 01", "484627", EspecialidadEnum.DiagnosticoPorImagenes,
                   new TimeSpan(07, 00, 00), new TimeSpan(13, 00, 00), sucursal));
                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000000102"), "Imagenes 02", "484627", EspecialidadEnum.DiagnosticoPorImagenes,
                   new TimeSpan(13, 00, 00), new TimeSpan(20, 00, 00), sucursal));
                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000001001"), "Laboratorio 01", "484627", EspecialidadEnum.Laboratorio,
                   new TimeSpan(07, 00, 00), new TimeSpan(14, 00, 00), sucursal));
                context.Medicos.Add(new Medico(Guid.Parse("00000000-0000-0000-0000-000000001002"), "Laboratorio 02", "484627", EspecialidadEnum.Laboratorio,
                   new TimeSpan(06, 00, 00), new TimeSpan(13, 00, 00), sucursal));

                context.SaveChanges();

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0001-0001-000000000001"), new DateTime(2022, 11, 10, 10, 30, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")) // Clinico 01
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0001-0001-000000100001"), new DateTime(2022, 11, 01, 12, 30, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")) // Clinico 01
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0002-0001-000000000002"), new DateTime(2022, 11, 11, 13, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000002")) // Clinico 02
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-1000-0004-000000000003"), new DateTime(2022, 10, 24, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000001001")) // Laboratorio 01
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-1000-0004-000000000004"), new DateTime(2022, 10, 31, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000001001")) // Laboratorio 01
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-1000-0004-000000000005"), new DateTime(2022, 11, 07, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000001002")) // Laboratorio 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-1000-0004-000000000006"), new DateTime(2022, 11, 14, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000001002")) // Laboratorio 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-1000-0004-000000000007"), new DateTime(2022, 11, 21, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000001001")) // Laboratorio 01
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0101-0004-000000000003"), new DateTime(2022, 10, 24, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000101")) // Imagenes 01
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000004"), new DateTime(2022, 10, 31, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000005"), new DateTime(2022, 11, 07, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000006"), new DateTime(2022, 11, 14, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000007"), new DateTime(2022, 11, 21, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000010"), new DateTime(2022, 12, 07, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000011"), new DateTime(2022, 12, 14, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000012"), new DateTime(2022, 12, 21, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));
                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0102-0004-000000000013"), new DateTime(2022, 12, 28, 08, 00, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000004")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000102")) // Imagenes 02
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0001-0003-000000000051"), new DateTime(2022, 11, 04, 10, 30, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000003")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")) // Clinico 01
                    ));

                context.Turnos.Add(new Turno(Guid.Parse("00000000-0000-0001-0003-000000000061"), new DateTime(2022, 11, 04, 10, 30, 00),
                    context.Pacientes.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000003")), sucursal,
                    context.Medicos.Local.First(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000001")) // Clinico 01
                    ));

                context.SaveChanges();

                base.Seed(context);
            }

            private Guid GetGuid(string guid)
            {
                return Guid.Parse(guid);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }

        public System.Data.Entity.DbSet<SistemasColaborativos.Models.Transitional.NuevoTurno> NuevoTurnoes { get; set; }
    }
}