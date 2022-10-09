//using MySql.Data.MySqlClient;
using SistemasColaborativos.Models;
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
            //Database.SetInitializer<ColaborativosContext>(new DropCreateDatabaseAlways<ColaborativosContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }
    }
}