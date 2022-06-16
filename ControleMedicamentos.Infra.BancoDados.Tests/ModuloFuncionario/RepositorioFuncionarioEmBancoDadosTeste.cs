using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTeste
    {
        Funcionario funcionario;

        RepositorioFuncionarioEmBancoDados repositorioFuncionario;

        public RepositorioFuncionarioEmBancoDadosTeste()
        {
            Bd.ComandoSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            funcionario = new("Funcionario1", "Login1", " Senha1");

            repositorioFuncionario = new();


        }

        [TestMethod]
        public void Deve_Inserir_Funcionario()
        {
            repositorioFuncionario.Inserir(funcionario);

            var fornecedorRegistrado = repositorioFuncionario.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(fornecedorRegistrado);

            Assert.AreEqual(funcionario, fornecedorRegistrado);
        }

        [TestMethod]
        public void Deve_Editar_Funcionario()
        {
            repositorioFuncionario.Inserir(funcionario);

            funcionario.Nome = "Tio Home";
            funcionario.Login = "Login2";
            funcionario.Senha = "Senha2";
            

            repositorioFuncionario.Editar(funcionario);

            var fornecedorEditado = repositorioFuncionario.SelecionarPorId(funcionario.Id);


            Assert.IsNotNull(fornecedorEditado);

            Assert.AreEqual(funcionario, fornecedorEditado);
        }

        [TestMethod]
        public void Deve_Excluir_Funcionarior()
        {

            repositorioFuncionario.Inserir(funcionario);

            repositorioFuncionario.Excluir(funcionario);

            var fornecedorEncontrado = repositorioFuncionario.SelecionarPorId(funcionario.Id);

            Assert.IsNull(fornecedorEncontrado);

        }

        [TestMethod]
        public void Deve_Selecionar_Um_Funcionarior()
        {
            repositorioFuncionario.Inserir(funcionario);

            var FornecedorEncontrado = repositorioFuncionario.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(FornecedorEncontrado);

            Assert.AreEqual(funcionario, FornecedorEncontrado);
        }


        [TestMethod]
        public void Deve_Selecionar_Todos_Funcionarior()
        {
            int quantidade = 3;

            for (int i = 0; i < quantidade; i++)
                repositorioFuncionario.Inserir(funcionario);

            var fornecedores = repositorioFuncionario.SelecionarTodos();

            Assert.AreEqual(quantidade, fornecedores.Count);

        }


    }
}
