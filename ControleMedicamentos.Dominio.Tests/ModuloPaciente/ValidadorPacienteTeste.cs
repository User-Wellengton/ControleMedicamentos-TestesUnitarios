using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ControleMedicamentos.Dominio.Tests.ModuloPaciente
{
    [TestClass]

    public  class ValidadorPacienteTeste
    {
        [TestMethod]
        public void Nome_nao_Pode_Ser_Nulo()
        {
            //arrange
            Paciente paciente = new(null, "321654987");

            ValidadorPaciente validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(paciente);

            //assert
            Assert.AreEqual("Campo 'Nome' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
           
        }

        [TestMethod]
        public void Nome_nao_Pode_Ser_Vazio()
        {
            //arrange
            Paciente paciente = new("", "321654987");

            ValidadorPaciente validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(paciente);

            //assert
            Assert.AreEqual("Campo 'Nome' Não pode ser vazio", resultado.Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void Cartao_nao_Pode_Ser_Nulo()
        {
            //arrange
            Paciente paciente = new("Paciente1", null);

            ValidadorPaciente validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(paciente);

            //assert
            Assert.AreEqual("Campo 'CartaoSUS' Não pode ser nulo", resultado.Errors[0].ErrorMessage);
            
        }

        [TestMethod]
        public void Cartao_nao_Pode_Ser_Vazio()
        {
            //arrange
            Paciente paciente = new("Paciente1", "");

            ValidadorPaciente validacao = new();

            //action
            ValidationResult resultado = validacao.Validate(paciente);

            //assert
            Assert.AreEqual("Campo 'CartaoSUS' Não pode ser vazio", resultado.Errors[0].ErrorMessage);

        }


    }
    
}
