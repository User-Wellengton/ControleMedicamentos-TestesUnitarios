

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
        public void NaoDeve_SerNulo_Nome()
        {
            Fornecedor fornecedor = new(null, "99665577", "fornecedor@gmail.com", "Lages", "SC");

            ValidadorFornecedor validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(fornecedor);

            //assert
            Assert.AreEqual("'Nome' não pode ser nulo", resultado.Errors[0].ErrorMessage);
        }

        
        
    }
