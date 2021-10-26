using Microsoft.EntityFrameworkCore;
using System;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();
            
            db.Database.Migrate(); //Não indicado para usar em produção

            Console.WriteLine("Hello World!");
        }
    }
}
