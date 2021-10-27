using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            //InserirDadosEmMassa();
            //ConsultaDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            AtualizarDados();
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Set<Cliente>().FirstOrDefault(c => c.Id == 1);
            //var cliente = db.Set<Cliente>().Find(keyValues: 1);

            cliente.Nome = "Cliente Alterado Passo 2";
            //db.Set<Cliente>().Update(cliente); //Se descomentar essa linha o Update sera com todas as colunas
            //Com a linha de cima  ->  UPDATE [Cliente] SET [CEP] = @p0, [Cidade] = @p1, [Estado] = @p2, [Nome] = @p3, [Phone] = @p4
            //Sem a linha de cima  ->  UPDATE[Cliente] SET[Nome] = @p0

            //Com a linha de baixo (por rastreamento)  ->  UPDATE [Cliente] SET [CEP] = @p0, [Cidade] = @p1, [Estado] = @p2, [Nome] = @p3, [Phone] = @p4
            db.Entry(cliente).State = EntityState.Modified;

            db.SaveChanges();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Cenário desconectado
            
            
            var clienteTeste = new Cliente
            {
                Id = 2
            };
            
            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado Passo 4",
                Telefone = "7966669999"
            };
            
            db.Attach(clienteTeste); //Objeto inicializado fora do EF
            db.Entry(clienteTeste).CurrentValues.SetValues(clienteDesconectado);

            //db.Clientes.Update(cliente);
            db.SaveChanges();            
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            //var pedidos = db.Set<Pedido>().ToList(); //Não carrega os Itens

            var pedidos = db.Set<Pedido>()
                .Include(p => p.Itens).ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Set<Cliente>().FirstOrDefault();
            var produto = db.Set<Produto>().FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                 {
                     new PedidoItem
                     {
                         ProdutoId = produto.Id,
                         Desconto = 0,
                         Quantidade = 1,
                         Valor = 10,
                     }
                 }
            };
            db.Set<Pedido>().Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultaDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Cliente where c.Id > 0 select c).ToList(); //Metodo alternativo            
            var consultaPorMetodo = db.Set<Cliente>().Where(c => c.Id > 0).OrderBy(c => c.Id).ToList(); //Mantem objetos em memoria
                                                                                                        //var consultaPorMetodo = db.Cliente.AsNoTracking().Where(c => c.Id > 0).ToList(); //Não mantem objetos em memoria

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                //db.Cliente.Find(cliente.Id); //Objetos ficam memoria e não realiza uma segunda consulta no banco (sem usar o AsNoTracking), apenas o Find faz isso
                db.Set<Cliente>().FirstOrDefault(c => c.Id == cliente.Id); //Sempre faz consulta no banco (independente de usar o AsNoTracking)
            }
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
