using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
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

            //InserirDados();
            InserirDadosEmMassa();
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Caneta Azul",
                CodigoBarras = "11111111111",
                Valor = 20m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "David Oliveira",
                CEP = "99999000",
                Cidade = "Maringá",
                Estado = "PR",
                Telefone = "48999999999"
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Cliente 1",
                    CEP = "99999000",
                    Cidade = "Maringá",
                    Estado = "PR",
                    Telefone = "48999999999"
                },
                new Cliente
                {
                    Nome = "Cliente 2",
                    CEP = "99999000",
                    Cidade = "Maringá",
                    Estado = "PR",
                    Telefone = "48999999999"
                }
        };

            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente);

            //db.AddRange(listaClientes);
            db.Set<Cliente>().AddRange(listaClientes);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total Registro(s) em massa: {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            //db.Produto.Add(produto); //*Metodo Fixo
            db.Set<Produto>().Add(produto);
            //**Metodo dinamico (n precisa propriedade no ApplicationContext)
            //db.Entry(produto).State = EntityState.Added; //Metodo informado o estado da entidade a ser persistida
            //db.Add(produto); //Metodo direto do ApplicationContext

            var registros = db.SaveChanges(); //Persiste todos os objetos rastreados e persiste no banco de dados

            Console.WriteLine($"Total Registro(s): {registros}");
        }

    }
}
