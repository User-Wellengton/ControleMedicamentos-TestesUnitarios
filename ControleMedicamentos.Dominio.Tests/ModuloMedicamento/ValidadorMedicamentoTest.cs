using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class ValidadorMedicamentoTest
    {
        [TestMethod]
        public void Nome_nao_Pode_Ser_Nulo()
        {
            Medicamento medicamento = new(null, "Descri��o1", "Lote1", DateTime.Now);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Nome' n�o pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Nome_nao_Pode_Ser_Vazio()
        {
            Medicamento medicamento = new("", "Descri��o1", "Lote1", DateTime.Now);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Nome' n�o pode ser vazio", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Descricao_nao_Pode_Ser_Nulo()
        {
            Medicamento medicamento = new("Medicamento1", null, "Lote1", DateTime.Now);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Descricao' n�o pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Descricao_nao_Pode_Ser_Vazio()
        {
            Medicamento medicamento = new("Medicamento1", "", "Lote1", DateTime.Now);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Descricao' n�o pode ser vazio", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Lote_nao_Pode_Ser_Nulo()
        {
            Medicamento medicamento = new("Medicamento1", "Descri��o1", null, DateTime.Now);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Lote' n�o pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Lote_nao_Pode_Ser_Vazio()
        {
            Medicamento medicamento = new("Medicamento1", "Descri��o1", "", DateTime.Now);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Lote' n�o pode ser vazio", resultado.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Validade_Incorreta()
        {
            Medicamento medicamento = new("Medicamento1", "Descri��o1", "Lote1", DateTime.MinValue);

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Validade' incorreto", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Fornecedor_nao_Pode_Ser_Nulo()
        {
            Medicamento medicamento = new("Medicamento1", "Descri��o1", "Lote1" , DateTime.Now);
            medicamento.QuantidadeDisponivel = 15;
            medicamento.Fornecedor = null;

            ValidadorMedicamento validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(medicamento);

            //assert
            Assert.AreEqual("Campo 'Fornecedor' n�o pode ser nulo", resultado.Errors[0].ErrorMessage);
        }
    }



}
