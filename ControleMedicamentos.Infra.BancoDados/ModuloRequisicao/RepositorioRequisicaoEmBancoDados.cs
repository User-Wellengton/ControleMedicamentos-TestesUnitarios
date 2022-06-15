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
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private const string enderecoBanco =
       "Data Source=(localdb)\\MSSQLLocalDB;" +
       "Initial Catalog=ControleMedicamentos;" +
       "Integrated Security=True;" +
       "Pooling=False";

        #region SQL Queries

        private const string sqlInserir =
           @"INSERT INTO [TBREQUISICAO] 
                (
                    
                    FUNCIONARIO_ID,
                    PACIENTE_ID,
                    MEDICAMENTO_ID,
                    QUANTIDADEMEDICAMENTO,
                    DATA
	            )
	            VALUES
                (
                   
                    @FUNCIONARIO_ID,   
                    @PACIENTE_ID,
                    @MEDICAMENTO_ID,
                    @QUANTIDADEMEDICAMENTO,
                    @DATA
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBREQUISICAO]	
		        SET
			        FUNCIONARIO_ID = @FUNCIONARIO_ID,
                    PACIENTE_ID = @PACIENTE_ID,
                    MEDICAMENTO_ID = @MEDICAMENTO_ID,
                    QUANTIDADEMEDICAMENTO = @QUANTIDADEMEDICAMENTO,
			        DATA = @DATA
                WHERE
			        ID = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBREQUISICAO]                                
		        WHERE
			        ID = @ID;";


        private const string sqlSelecionarTodos =
          @"SELECT
	                ID,
                    NOME,
                    TELEFONE,
                    EMAIL,
                    CIDADE,
                    ESTADO,

                    PACIENTE.NOME AS PACIENTE_NOME,
                    PACIENTE.CARTAOSUS AS PACIENTE_CARTAOSUS,

                    FUNCIONARIO.NOME AS FUNCIONARIO_NOME,
                    FUNCIONARIO.LOGIN AS FUNCIONARIO_LOGIN,
                    FUNCIONARIO.SENHA AS FUNCIONARIO_SENHA,
                    
                    MEDICAMENTO.ID AS MEDICAMENTO_ID,
                    MEDICAMENTO.NOME AS MEDICAMENTO.NOME,
                    MEDICAMENTO.DESCRICAO AS MEDICAMENTO.DESCRICAO,
                    MEDICAMENTO.LOTE AS MEDICAMENTO.LOTE,
                    MEDICAMENTO.VALIDADE AS MEDICAMENTO.VALIDADE,
              FROM 
	                TBREQUISICAO AS REQUISICAO INNER JOIN
                    TBPACIENTE AS PACIENTE ON 
                    REQUISICAO.PACIENTE_ID = PACIENTE.ID INNER JOIN
                    TBFUNCIONARIO AS FUNCIONARIO ON REQUISICAO.FUNCIONARIO_ID = FUNCIONARIO.ID
                    INNER JOIN TBMEDICAMENTO AS MEDICAMENTO ON
                    REQUISICAO.ID = MEDICAMENTO.ID
    
              WHERE 
	                [ID] = @ID";



        private const string sqlSelecionarPorId =
           @"SELECT
	                ID,
                    NOME,
                    TELEFONE,
                    EMAIL,
                    CIDADE,
                    ESTADO,

                    PACIENTE.NOME AS P.NOME,
                    PACIENTE.CARTAOSUS AS P.CARTAOSUS,

                    FUNCIONARIO.NOME AS F.NOME,
                    FUNCIONARIO.LOGIN AS F.LOGIN,
                    FUNCIONARIO.SENHA AS F.SENHA,

                    MEDICAMENTO.NOME AS M.NOME,
                    MEDICAMENTO.DESCRICAO AS M.DESCRICAO,
                    MEDICAMENTO.LOTE AS M.LOTE,
                    MEDICAMENTO.VALIDADE AS M.VALIDADE,
                                        
              FROM 
	                TBREQUISICAO AS REQUISICAO INNER JOIN
                    TBPACIENTE AS PACIENTE ON 
                    REQUISICAO.PACIENTE_ID = PACIENTE.ID INNER JOIN
                    TBFUNCIONARIO AS FUNCIONARIO ON REQUISICAO.FUNCIONARIO_ID = FUNCIONARIO.ID
                    INNER JOIN TBMEDICAMENTO AS MEDICAMENTO ON
                    REQUISICAO.ID = MEDICAMENTO.ID
    
              WHERE 
	                ID = @ID";


        #endregion

        public ValidationResult Inserir(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;


            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlInserir, sqlConnection);

            ConfigurarRequisicao(requisicao, sqlCommand);

            sqlConnection.Open();
            var id = sqlCommand.ExecuteScalar();
            requisicao.Id = Convert.ToInt32(id);
            sqlConnection.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlEditar, sqlConnection);

            ConfigurarRequisicao(requisicao, sqlCommand);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return resultadoValidacao;

        }

        public void Excluir(Requisicao requisicao)
        {
            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlExcluir, sqlConnection);

            sqlCommand.Parameters.AddWithValue("ID", requisicao.Id);
            sqlCommand.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", 0);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);
            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarTodos, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (sqlDataReader.Read())
            {
                Requisicao requisicao = ConverterRequisicao(sqlDataReader);

                requisicoes.Add(requisicao);
            }
            return requisicoes;
        }

        public Requisicao SelecionarPorNumero(int numero)
        {
            SqlConnection sqlConnection = new SqlConnection(enderecoBanco);

            SqlCommand sqlCommand = new SqlCommand(sqlSelecionarPorId, sqlConnection);
            sqlCommand.Parameters.AddWithValue("ID", numero);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            Requisicao requisicao = null;

            if (sqlDataReader.Read())
                requisicao = ConverterRequisicao(sqlDataReader);

            sqlConnection.Close();

            return requisicao;
        }

        public static Requisicao ConverterRequisicao(SqlDataReader leitorRequisicao)
        {
            int numero = Convert.ToInt32(leitorRequisicao["ID"]);

            int funcionarioId = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);
            string funcionarioNome = Convert.ToString(leitorRequisicao["FUNCIONARIO_NOME"]);
            string funcionarioLogin = Convert.ToString(leitorRequisicao["FUNCIONARIO_LOGIN"]);
            string funcionarioSenha = Convert.ToString(leitorRequisicao["FUNCIONARIO_SENHA"]);

            Funcionario funcionario = new Funcionario
                (funcionarioNome, funcionarioLogin, funcionarioSenha)
            {
                Id = funcionarioId
            };

            int pacienteId = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            string pacienteNome = Convert.ToString(leitorRequisicao["PACIENTE_NOME"]);
            string pacienteCARTAOSUS = Convert.ToString(leitorRequisicao["PACIENTE_CARTAOSUS"]);

            Paciente paciente = new Paciente(pacienteNome, pacienteCARTAOSUS)
            {
                Id = pacienteId
            };

            int medicamentoId = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_ID"]);
            string medicamentoNome = Convert.ToString(leitorRequisicao["MEDICAMENTO_NOME"]);
            string medicamentoDescricao = Convert.ToString(leitorRequisicao["MEDICAMENTO_DESCRICAO"]);
            string medicamentoLote = Convert.ToString(leitorRequisicao["MEDICAMENTO_LOTE"]);
            DateTime medicamentoValidade = Convert.ToDateTime(leitorRequisicao["MEDICAMENTO_VALIDADE"]);
            int medicamentoQtdDisponivel = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_QUANTIDADEDISPONIVEL"]);
            int medicamentoFornecedorId = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_FORNECEDOR_ID"]);
            string medicamentoFornecedorNome = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_NOME"]);
            string medicamentoFornecedorTelefone = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_TELEFONE"]);
            string medicamentoFornecedorEmail = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_EMAIL"]);
            string medicamentoFornecedorCidade = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_CIDADE"]);
            string medicamentoFornecedorEstado = Convert.ToString(leitorRequisicao["MEDICAMENTO_FORNECEDOR_ESTADO"]);

            Fornecedor fornecedor = new Fornecedor
                (medicamentoFornecedorNome, medicamentoFornecedorTelefone,
                medicamentoFornecedorEmail, medicamentoFornecedorCidade,
                medicamentoFornecedorEstado)
            {
                Id = medicamentoFornecedorId
            };

            List<Requisicao> requisicoes = new List<Requisicao>();

            Medicamento medicamento = new Medicamento
                (medicamentoNome, medicamentoDescricao, medicamentoLote, medicamentoValidade)
            {
                Id = medicamentoId,
                QuantidadeDisponivel = medicamentoQtdDisponivel,
                Fornecedor = fornecedor,
                Requisicoes = requisicoes
            };

            int qntMedicamento = Convert.ToInt32(leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            var requisicao = new Requisicao
            {
                Id = numero,
                Medicamento = medicamento,
                Paciente = paciente,
                QtdMedicamento = qntMedicamento,
                Data = data,
                Funcionario = funcionario
            };

            return requisicao;
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
