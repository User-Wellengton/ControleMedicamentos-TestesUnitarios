using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamentos.Dominio.Tests.ModuloRequisicao
{
    [TestClass]
    public class ValidadorRequisicaoTeste
    {
        [TestMethod]
        public void Medicamento_nao_Pode_Ser_Nulo()
        {
            Requisicao requisicao = new();
            requisicao.Medicamento = null;

            ValidadorRequisicao validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(requisicao);

            //assert
            Assert.AreEqual("Campo 'Medicamento' não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Paciente_nao_Pode_Ser_Nulo()
        {
            Requisicao requisicao = new();
            requisicao.Medicamento = ExemploMedicamento();
            requisicao.Paciente = null;

            ValidadorRequisicao validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(requisicao);

            //assert
            Assert.AreEqual("Campo 'Paciente' não pode ser nulo ", resultado.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Funcionario_nao_Pode_Ser_Nulo()
        {
            Requisicao requisicao = new();
            requisicao.Medicamento = ExemploMedicamento();
            requisicao.Paciente = ExemploPaciente();
            requisicao.Funcionario = null;

            ValidadorRequisicao validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(requisicao);

            //assert
            Assert.AreEqual("Campo 'Funcionario' não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void QtdMedicamento_nao_Pode_Ser_Nulo()
        {
            Requisicao requisicao = new();
            requisicao.Medicamento = ExemploMedicamento();
            requisicao.Paciente = ExemploPaciente();
            requisicao.Funcionario = ExemploFuncioinario();
            requisicao.QtdMedicamento = 0;

            ValidadorRequisicao validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(requisicao);

            //assert
            Assert.AreEqual("Campo 'Quantidade de Medicamento' não pode ser vazia ", resultado.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Data_Invalida()
        {
            Requisicao requisicao = new();
            requisicao.Medicamento = ExemploMedicamento();
            requisicao.Paciente = ExemploPaciente();
            requisicao.Funcionario = ExemploFuncioinario();
            requisicao.QtdMedicamento = 10;
            requisicao.Data = DateTime.MinValue;

            ValidadorRequisicao validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(requisicao);

            //assert
            Assert.AreEqual("Campo 'Data' incorreto", resultado.Errors[0].ErrorMessage);
        }





        private Medicamento ExemploMedicamento()
        {
            return new Medicamento("Medicamento1", "Decricao1", "Lote1", DateTime.Now)
            {
                Fornecedor = new Fornecedor("Fornecedor1", "99885544", "Fornecedor@gmail.com", "Lages", "SC")
                {

                },

                QuantidadeDisponivel = 15,
                Id = 20

            };

        }


        private Paciente ExemploPaciente()
        {
            return new Paciente("Paciente1", "321654987654")
            {
                Id = 1
            };
        }

        private Funcionario ExemploFuncioinario()
        {
            return new Funcionario("Funcionario1", "Login1", "Senha1")
            {

            };
        }

    }
}
