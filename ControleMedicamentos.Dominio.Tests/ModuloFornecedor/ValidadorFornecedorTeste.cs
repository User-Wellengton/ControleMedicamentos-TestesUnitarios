

using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamentos.Dominio.Tests.ModuloFornecedor
{
    [TestClass]
    public class ValidadorFornecedorTeste
    {
        [TestMethod]
        public void Nome_nao_Pode_Ser_Nulo()
        {
            Fornecedor fornecedor = new(null, "99665577", "fornecedor@gmail.com", "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Nome' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Nome_nao_Pode_Ser_Vazio()
        {
            Fornecedor fornecedor = new("", "99665577", "fornecedor@gmail.com", "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Nome' Não pode ser vazio", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Telefone_nao_Pode_Ser_Nulo()
        {
            Fornecedor fornecedor = new("Fornecedor1", null , "fornecedor@gmail.com", "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Telefone' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Telefone_nao_Pode_Ser_Vazio()
        {
            Fornecedor fornecedor = new("Fornecedor1", "", "fornecedor@gmail.com", "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Telefone' Não pode ser vazio", resultado.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Email_nao_Pode_Ser_Nulo()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99665588", null, "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Email' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Email_nao_Pode_Ser_Vazio()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99885566", "", "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Email' Não pode ser vazio", resultado.Errors[0].ErrorMessage);
        }



        [TestMethod]
        public void Email_Formato()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99665577", " ", "Lages", "SC");

            ValidadorFornecedor valfor = new();

            //action
            ValidationResult resultado = valfor.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Email' Formato incorreto", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Cidade_nao_Pode_Ser_Nulo()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99886655", "fornecedor@gmail.com", null, "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Cidade' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Cidade_nao_Pode_Ser_Vazio()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99886655", "fornecedor@gmail.com","", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Cidade' Não pode ser vazio", resultado.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Estado_nao_Pode_Ser_Nulo()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99886655", "fornecedor@gmail.com", "Lages", null);

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Estado' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Estado_nao_Pode_Ser_Vazio()
        {
            Fornecedor fornecedor = new("Fornecedor1", "99886655", "fornecedor@gmail.com", "Lages", "");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("Campo 'Estado' Não pode ser vazio", resultado.Errors[0].ErrorMessage);
        }

    }
}