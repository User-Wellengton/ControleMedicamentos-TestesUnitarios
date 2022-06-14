using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        private const string enderecoBanco =
        "Data Source=(localdb)\\MSSQLLocalDB;" +
        "Initial Catalog=ControleMedicamentos;" +
        "Integrated Security=True;" +
        "Pooling=False";

        #region Sql Queries

        private const string sqlInserir =
          @"INSERT INTO [TBMEDICAMENTO]
                (
                    NOME,
                    DESCRICAO,
                    LOTE,
                    VALIDADE,
                    QUANTIDADEDISPONIVEL,
                    FORNECEDOR_ID
                )
                    VALUES
                (
                    @NOME,
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,
                    @FORNECEDOR_ID );
          SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBMEDICAMENTO]	
		        SET
			        NOME = @NOME,
                    DESCRICAO = @DESCRICAO,
                    LOTE = @LOTE,
                    VALIDADE = @VALIDADE,
                    QUANTIDADEDISPONIVEL = @QUANTIDADEDISPONIVEL,
                    FORNECEDOR_ID = @FORNECEDOR_ID
			        

		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"UPDATE FROM [TBMEDICAMENTO]
                SET
                    QUANTIDADEMEDICAMENTO = @QUANTIDADEMEDICAMENTO,
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT
	                ID,
                    NOME,
                    DESCRICAO,
                    LOTE,
                    VALIDADE,
                    QUANTIDADEDISPONIVEL,
                    FORNECEDOR_ID,
                    FORNECEDO.NOME AS FORNECEDOR_NOME,
                    FORNECEDO.TELEFONE,
                    FORNECEDO.EMAIL, 
                    FORNECEDO.CIDADE,
                    FORNECEDO.ESTADO
              FROM 
	                [TBMEDICAMENTO] AS MEDICAMENTO INNER JOIN ON 
                    [TBFORNECEDOR] AS FORNECEDO ON MEDICAMENTO.[FORNECEDOR_ID] = FORNECEDOR.[ID]";

        private const string sqlSelecionarPorId =

            @"SELECT
	                ID,
                    NOME,
                    DESCRICAO,
                    LOTE,
                    VALIDADE,
                    QUANTIDADEDISPONIVEL,
                    FORNECEDOR_ID
                    FORNECEDOR.NOME AS FORNECEDOR_NOME,
                    FORNECEDOR.TELEFONE,
                    FORNECEDOR.EMAIL, 
                    FORNECEDOR.CIDADE,
                    FORNECEDOR.ESTADO
              FROM 
	                TBMEDICAMENTO AS MEDICAMENTO INNER JOIN TBFORNECEDOR AS FORNECEDOR
                    ON MEDICAMENTO.[FORNECEDOR_ID] = FORNECEDOR.[ID]
              WHERE 
	                [ID] = @ID";

        private const string sqlSelecionarRequisicaoTodos =
            @"SELECT                
                REQUISICAO.[ID],
                REQUISICAO.[FUNCIONARIO_ID],
                
                REQUISICAO.[PACIENTE_ID],
                
                FUNCIONARIO.[NOME] AS FUNCIONARIO_NOME,
                FUNCIONARIO.[LOGIN],
                FUNCIONARIO.[SENHA],
                PACIENTE.[NOME] AS PACIENTE_NOME,
                PACIENTE.[CARTAOSUS],
                REQUISICAO.[QUANTIDADEMEDICAMENTO],
                REQUISICAO.[DATA]
            FROM
                TBREQUISICAO AS REQUISICAO INNER JOIN 
                TBPACIENTE AS PACIENTE ON REQUISICAO.[PACIENTE_ID] = PACIENTE.[ID]
                INNER JOIN TBFUNCIONARIO ON
                REQUISICAO.[FUNCIONARIO_ID] = FUNCIONARIO.[ID]";

        private const string sqlSelecionarRequisicaoPorNumero =
        @"SELECT                
                REQUISICAO.[ID],
                REQUISICAO.[FUNCIONARIO_ID],
                
                REQUISICAO.[PACIENTE_ID],
               
                FUNCIONARIO.[NOME] AS FUNCIONARIO_NOME,
                FUNCIONARIO.[LOGIN],
                FUNCIONARIO.[SENHA],
                PACIENTE.[NOME] AS PACIENTE_NOME,
                PACIENTE.[CARTAOSUS],
                REQUISICAO.[QUANTIDADEMEDICAMENTO],
                REQUISICAO.[DATA]
            FROM
                TBREQUISICAO AS REQUISICAO INNER JOIN 
                TBPACIENTE AS PACIENTE ON REQUISICAO.[PACIENTE_ID] = PACIENTE.[ID]
                INNER JOIN TBFUNCIONARIO ON
                REQUISICAO.[FUNCIONARIO_ID] = FUNCIONARIO.[ID]                                             
            WHERE
                REQUISICAO.[MEDICAMENTO_ID] = @MED_ID";

        #endregion

        public ValidationResult Inserir(Medicamento medicamento)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(medicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarMedicamento(medicamento, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            medicamento.Id = Convert.ToInt32(id);
            sqlConnection.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Medicamento medicamento)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(medicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;


            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarMedicamento(medicamento, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return resultadoValidacao;
        }

        public void Excluir(Medicamento medicamento)
        {
            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", medicamento.Id);
            sqlCommand.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", 0);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Medicamento> medicamentos = new List<Medicamento>();
            List<Requisicao> todasRequisicoes = new List<Requisicao>();

            SqlCommand sqlCommandRequisicao = new SqlCommand
            (sqlSelecionarRequisicaoTodos, sqlConnection);

            SqlDataReader sqlDataReaderRequisicao = sqlCommandRequisicao.ExecuteReader();

            while (sqlDataReaderRequisicao.Read())
                todasRequisicoes.Add(ConverterRequisicao(sqlDataReaderRequisicao));
            todasRequisicoes.OrderBy(x => x.Paciente.Id);

            int i = 0;
            while (sqlDataReader.Read())
            {
                Medicamento medicamento = ConverterMedicamento(sqlDataReader);

                List<Requisicao> requisicoes = new List<Requisicao>();

                //Requisicao requisicao = null;

                while (todasRequisicoes[i].Id == medicamento.Id)
                {
                    todasRequisicoes[i].Medicamento = medicamento;
                    requisicoes.Add(todasRequisicoes[i]);
                    i++;
                }

                medicamento.Requisicoes = requisicoes;
                medicamentos.Add(medicamento);
            }
            return medicamentos;
        }

        public Medicamento SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorId, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Medicamento medicamento = null;

            if (sqlDataReader.Read())
            {
                medicamento = ConverterMedicamento(sqlDataReader);
                SqlCommand sqlCommandRequisicao = new SqlCommand
                    (sqlSelecionarRequisicaoPorNumero, sqlConnection);
                sqlCommandRequisicao.Parameters.AddWithValue("MED_ID", medicamento.Id);
                SqlDataReader sqlDataReaderRequisicao = sqlCommandRequisicao.ExecuteReader();
                List<Requisicao> requisicoes = new List<Requisicao>();
                while (sqlDataReaderRequisicao.Read())
                {
                    //Requisicao requisicao = null;
                    Requisicao requisicao = ConverterRequisicao(sqlDataReaderRequisicao);
                    requisicao.Medicamento = medicamento;
                    requisicoes.Add(requisicao);
                }
                medicamento.Requisicoes = requisicoes;
            }
            sqlConnection.Close();

            return medicamento;
        }

        public static Medicamento ConverterMedicamento(SqlDataReader leitorMedicamento)
        {
            int numero = Convert.ToInt32(leitorMedicamento["ID"]);
            string nome = Convert.ToString(leitorMedicamento["NOME"]);
            string descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            string lote = Convert.ToString(leitorMedicamento["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);
            int quantidadeDisponivel = Convert.ToInt32(leitorMedicamento["QUANTIDADEDISPONIVEL"]);

            int fornecedorId = Convert.ToInt32(leitorMedicamento["FORNECEDOR_ID"]);
            string fornecedorNome = Convert.ToString(leitorMedicamento["FORNECEDOR_NOME"]);
            string fornecedorTelefone = Convert.ToString(leitorMedicamento["TELEFONE"]);
            string fornecedorEmail = Convert.ToString(leitorMedicamento["EMAIL"]);
            string fornecedorCidade = Convert.ToString(leitorMedicamento["CIDADE"]);
            string fornecedorEstado = Convert.ToString(leitorMedicamento["ESTADO"]);

            var medicamento = new Medicamento
                (nome, descricao, lote, validade)
            {
                Id = numero,
                QuantidadeDisponivel = quantidadeDisponivel,
                Fornecedor = new Fornecedor
                (fornecedorNome, fornecedorTelefone, fornecedorEmail,
                fornecedorCidade, fornecedorEstado)
                {
                    Id = fornecedorId
                }
            };

            return medicamento;
        }

        public static Requisicao ConverterRequisicao(SqlDataReader leitorRequisicao)
        {
            int numero = Convert.ToInt32(leitorRequisicao["ID"]);

            int funcionarioID = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);

            string funcionarioNome = Convert.ToString(leitorRequisicao["FUNCIONARIO_NOME"]);
            string funcionarioLogin = Convert.ToString(leitorRequisicao["LOGIN"]);
            string funcionarioSenha = Convert.ToString(leitorRequisicao["SENHA"]);

            int pacienteID = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            string pacienteNome = Convert.ToString(leitorRequisicao["PACIENTE_NOME"]);
            string pacienteSUS = Convert.ToString(leitorRequisicao["PACIENTE_SUS"]);

            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);
            int quantidadeMedicamento = Convert.ToInt32
                                            (leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            var requisicao = new Requisicao
            {
                Id = numero,
                Funcionario = new Funcionario
                            (funcionarioNome, funcionarioLogin, funcionarioSenha)
                {
                    Id = funcionarioID
                },

                Paciente = new Paciente(pacienteNome, pacienteSUS)
                {
                    Id = pacienteID
                },
                QtdMedicamento = quantidadeMedicamento,
                Data = data
            };

            return requisicao;
        }

        public static void ConfigurarMedicamento
            (Medicamento medicamento, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", medicamento.Id);
            sqlCommand.Parameters.AddWithValue("NOME", medicamento.Nome);
            sqlCommand.Parameters.AddWithValue("DESCRICAO", medicamento.Descricao);
            sqlCommand.Parameters.AddWithValue("LOTE", medicamento.Lote);
            sqlCommand.Parameters.AddWithValue("VALIDADE", medicamento.Validade);
            sqlCommand.Parameters.AddWithValue
                ("QUANTIDADEDISPONIVEL", medicamento.QuantidadeDisponivel);
            sqlCommand.Parameters.AddWithValue
                ("FORNECEDOR_ID", medicamento.Fornecedor.Id);
        }

        public static void ConfigurarRequisicao
       (Requisicao requisicao, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("ID", requisicao.Id);
            sqlCommand.Parameters.AddWithValue("FUNCIONARIO_ID", requisicao.Funcionario.Id);
            sqlCommand.Parameters.AddWithValue("PACIENTE_ID", requisicao.Paciente.Id);
            sqlCommand.Parameters.AddWithValue("MEDICAMENTO_ID", requisicao.Medicamento.Id);
            sqlCommand.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", requisicao.QtdMedicamento);
            sqlCommand.Parameters.AddWithValue("DATA", requisicao.Data);
        }




    }
}
