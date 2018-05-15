using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QC = System.Data.SqlClient;
using DT = System.Data;

namespace Projeto_Final.B.D
{
    
    public class Conexao
    {
        string StringConnection = "Server=tcp:maratonabot.database.windows.net,1433;Initial Catalog=maraton_final;Persist Security Info=False;User ID=Administrador;Password=H0sp1t4l;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public void Conectar()
        {
            var conexao = new QC.SqlConnection(StringConnection);
            conexao.Open();
        }
        public void Desconectar()
        {
            var conexao = new QC.SqlConnection(StringConnection);
            conexao.Close();
        }
        public string ConsultaEspecialidade()
        {
            string resultado = "";
            var conexao = new QC.SqlConnection(StringConnection);
            conexao.Open();
            var command = new QC.SqlCommand();
            command.Connection = conexao;
            command.CommandType = DT.CommandType.Text;
            command.CommandText = @"select especialidade from Medico";

            QC.SqlDataReader reader = command.ExecuteReader();
           

            while (reader.Read())
            {
                 resultado += "\n" + "* *" + reader.GetString(0) + "*\n";
            }
            return resultado;
        }

        public string ConsultaMedico()
        {
            string resultado = "";
            var conexao = new QC.SqlConnection(StringConnection);
            conexao.Open();
            var command = new QC.SqlCommand();
            command.Connection = conexao;
            command.CommandType = DT.CommandType.Text;
            command.CommandText = @"select nome_medico, especialidade from Medico";

            QC.SqlDataReader reader = command.ExecuteReader();

            int contatador = 1;
            while (reader.Read())
            {

                resultado += "\n" + contatador + " *" + reader.GetString(0) + " - " + reader.GetString(1) + "*\n";
                contatador++;
            }
            return resultado;
        }

        public List<string> Nome_Medico()
        {
            var conexao = new QC.SqlConnection(StringConnection);
            conexao.Open();
            var command = new QC.SqlCommand();
            command.Connection = conexao;
            command.CommandType = DT.CommandType.Text;
            command.CommandText = @"select Id, nome_medico from Medico";
            QC.SqlDataReader reader = command.ExecuteReader();
            List<string> nomes = new List<string>();
            while (reader.Read())
            {
                nomes.Add(reader.GetInt32(0).ToString()+ " - " + reader.GetString(1));
            }
            return nomes;
        }
    }

    
}