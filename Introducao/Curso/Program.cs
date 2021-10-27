using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            //db.Database.Migrate(); //Não indicado para usar em produção
            var existe = db.Database.GetPendingMigrations().Any();
            if (existe)            
                Console.WriteLine("Existem Migrações pendentes");            

            Console.WriteLine("Hello World!");
        }
    }
}
