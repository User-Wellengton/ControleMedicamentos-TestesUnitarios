using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]

    public class RepositorioPacienteBancoDadosTeste
    {
        Paciente paciente;
       

        RepositorioPacienteEmBancoDados repositorioPaciente;

        public RepositorioPacienteBancoDadosTeste()
        {
           Bd.ComandoSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");

            paciente = new("Paciente1", "987654321");
            

            repositorioPaciente = new();
        }

        [TestMethod]
        public void Deve_Inserir_paciente()
        {
            repositorioPaciente.Inserir(paciente);

            var pacienteRegistrado = repositorioPaciente.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteRegistrado);

            Assert.AreEqual(paciente, pacienteRegistrado);
        }

        [TestMethod]
        public void Deve_Editar_paciente()
        {
            repositorioPaciente.Inserir(paciente);

            paciente.Nome = "Tio Home";
            paciente.CartaoSUS = "456132789";

            repositorioPaciente.Editar(paciente);

            var pacienteEditado = repositorioPaciente.SelecionarPorId(paciente.Id);
            

            Assert.IsNotNull(pacienteEditado);

            Assert.AreEqual(paciente, pacienteEditado);
        }

        [TestMethod]
        public void Deve_Excluir_paciente()
        {

            repositorioPaciente.Inserir(paciente);

            repositorioPaciente.Excluir(paciente);

            var pacienteEncontrado = repositorioPaciente.SelecionarPorId(paciente.Id);

            Assert.IsNull(pacienteEncontrado);

        }

        [TestMethod]
        public void Deve_Selecionar_Um_paciente()
        {
            repositorioPaciente.Inserir(paciente);

            var pacienteEncontrado = repositorioPaciente.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteEncontrado);

            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_Selecionar_Todos_paciente()
        {
            int quantidade = 3;

            for (int i = 0; i < quantidade; i++)
                repositorioPaciente.Inserir(paciente);

            var pacientes = repositorioPaciente.SelecionarTodos();

            Assert.AreEqual(quantidade, pacientes.Count);

        }




    }






    
}
