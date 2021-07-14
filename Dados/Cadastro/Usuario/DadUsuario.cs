using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Classe;
using Excelencia.API.Dados;
using Excelencia.BancoDados;
using Excelencia.Extensions;
using Excelencia.API.Classe;

namespace Dados
{
    public class DadUsuario : DadPadrao
    {
        private ClsUsuario oDTO;
        public static string nomeTabela = "Tb_Usuario";

        #region CRUD

        public override bool Inserir<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;
                var lista = MontaListaParametros(banco);

                oDTO.Id = banco.ExecutaScript(TScript.Insert, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"Inseriu Usuário").ToString().ToInteger();


                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Inserir - " + ex.Source.ToNull().ToString() };
            }
        }

        public override bool Alterar<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;
                var lista = MontaListaParametros(banco);
                lista.Add(banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true));
                banco.ExecutaScript(TScript.Update, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"Alterou Usuário");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Alterar - " + ex.Source.ToNull().ToString() };
            }
        }

        public override bool Excluir<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;

                RemoverRelacionamentos(ref banco);

                var lista = new List<ClsParameter>
                {
                    banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true)
                };
                banco.ExecutaScript(TScript.Delete, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"Excluiu Usuário");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Excluir - " + ex.Source.ToNull().ToString() };
            }
        }

        private bool RemoverRelacionamentos(ref ClsBancoDados bd)
        {
            try
            {


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Remover Relacionamentos - " + ex.Source.ToNull().ToString() };
            }

        }

        #endregion

        #region MONTA PARÂMETROS

        protected override List<ClsParameter> MontaListaParametros(ClsBancoDados oBD)
        {
            try
            {
                var lista = new List<ClsParameter>
            {
                oBD.RetornaParametro("Usuario", oDTO.Usuario, true, idTipo: DbType.String),
                oBD.RetornaParametro("NomeUsuario", oDTO.Nome, false, idTipo: DbType.String),
                oBD.RetornaParametro("Email", oDTO.Email, false, idTipo: DbType.String),
                oBD.RetornaParametro("Senha", oDTO.Senha, false, idTipo: DbType.String),
                oBD.RetornaParametro("Status", oDTO.Ativo, false, idTipo: DbType.Boolean),
                oBD.RetornaParametro("IdGrupoUsuario", oDTO.GrupoUsuario.Id, false, idTipo: DbType.Int32),
            };

                if (oDTO.Foto.Alterar)
                    lista.Add(oBD.RetornaParametro("Foto", oDTO.Foto.Arquivo, false, idTipo: DbType.Object));



                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Monta Lista Parametros - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region VALIDAÇÕES

        public bool ValidarRelacionamento(ref ClsBancoDados banco, long Id)
        {
            try
            {
                // O CADASTRO DE USUÁRIO AINDA NÃO TEM VÍNCULO COM NENHUMA OUTRA TABELA
                // QUANDO HOUVER É NECESSÁRIO INSERIR A CONSULTA ABAIXO E DESCOMENTAR  O TRECHO ABAIXO:
                // ================ INÍCIO ==============================
                //var sql = ""; 
                //var dt = banco.LoadQuery(sql);

                //return dt.Rows.Count > 0;
                // ================ FIM ==============================
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Validar Relacionamento - " + ex.Source.ToNull().ToString() };
            }
        }

        public bool Existe(ref ClsBancoDados banco, string strUsuario, long id = 0)
        {

            try
            {
                var sql = "";
                sql = $" SELECT Id FROM { nomeTabela } WHERE Usuario = '{strUsuario}'";
                sql += $" AND Id <> {id}";

                return banco.LoadQuery(sql).Rows.Count > 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Existe - " + ex.Source.ToNull().ToString() };
            }
        }

        public bool ValidaEmailExiste(ref ClsBancoDados banco, string email)
        {
            try
            {
                var sql = $"SELECT ID FROM {nomeTabela} WHERE Email = '{email}'";
                return banco.LoadQuery(sql).Rows.Count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ValidaEmailExiste - " + ex.Source.ToNull().ToString() };
            }
        }

        public static long ValidaCodigoUsuario(ref ClsBancoDados banco, string email, string codigo)
        {
            try
            {
                var ret = 0.ToLong();
                var sql = "SELECT Id";
                sql += $" FROM {nomeTabela} ";
                sql += " WHERE 1 = 1";
                sql += $" AND Email = '{email}'";
                sql += $" AND CodigoRecuperacao = '{codigo}'";

                var dt = banco.LoadQuery(sql);

                if (dt.Rows.Count > 0)
                    ret = dt.Rows[0]["Id"].ToString().ToLong();

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ValidaCodigoUsuario - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region FUNÇÕES

        public void RelacionaCodigoAoUsuario(ref ClsBancoDados banco, ClsUsuario user, string codigoRecuperacao)
        {
            try
            {
                var lista = new List<ClsParameter>
                {
                    banco.RetornaParametro("CodigoRecuperacao", codigoRecuperacao, true, idTipo: DbType.String),
                    banco.RetornaParametro("Id", user.Id, false, idTipo: DbType.Int32, doWhere: true)
                };

                banco.ExecutaScript(TScript.Update, nomeTabela, out _, lista.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "RelacionaCodigoAoUsuario - " + ex.Source.ToNull() };
            }
        }

        public bool AtualizaSenha(ref ClsBancoDados banco, long id, string senha)
        {
            try
            {
                var lista = new List<ClsParameter>
                {
                    banco.RetornaParametro("CodigoRecuperacao", null, false, idTipo: DbType.String),
                    banco.RetornaParametro("Senha", senha, false, idTipo: DbType.String),
                    banco.RetornaParametro("Id", id, false, idTipo: DbType.Int64, doWhere: true),
                };
                banco.ExecutaScript(TScript.Update, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"O usuário {id} atualizou a senha.");


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Atualiza Senha - " + ex.Source.ToNull().ToString() };
            }
        }


        #endregion

        #region MONTA OBJETO

        public static ClsUsuario Filtra(ref ClsBancoDados banco, long Id, bool fRelacionamento = true)
        {
            try
            {
                var obj = new ClsUsuario();
                var sql = $" SELECT * FROM {nomeTabela} WHERE Id = {Id} ";
                var dt = banco.LoadQuery(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    obj = MontaObjeto(dr);
                    if (fRelacionamento) FiltraRelacionamento(ref banco, ref obj);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtra - " + ex.Source.ToNull().ToString() };
            }
        }

        public static ClsUsuario FiltrarPorEmail(ref ClsBancoDados banco, string email, bool fRelacionamento = true)
        {
            try
            {
                var obj = new ClsUsuario();
                var sql = $" SELECT * FROM {nomeTabela} WHERE Email = '{email}' ";
                var dt = banco.LoadQuery(sql);
                if (dt.Rows.Count > 1) throw new Exception("Foram encontrados múltiplos usuários com o e-mail cadastrado. Favor contactar o administrador do sistema!");
                foreach (DataRow dr in dt.Rows)
                {
                    obj = MontaObjeto(dr);
                    if (fRelacionamento) FiltraRelacionamento(ref banco, ref obj);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "FiltrarPorEmail - " + ex.Source.ToNull().ToString() };
            }
        }

        private static void FiltraRelacionamento(ref ClsBancoDados banco, ref ClsUsuario obj)
        {
            try
            {


            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtra Relacionamento - " + ex.Source.ToNull().ToString() };
            }


        }

        public static ClsUsuario MontaObjeto(DataRow dr)
        {
            try
            {

                var obj = new ClsUsuario
                {
                    Existe = true,
                    TipoAlteracao = TManter.Alterar,
                    Id = dr.GetValue("Id").ToString().ToInteger(),
                    Usuario = dr.GetValue("Usuario").ToString(),
                    Nome = dr.GetValue("NomeUsuario").ToString(),
                    Email = dr.GetValue("Email").ToString(),
                    Senha = dr.GetValue("Senha").ToString(),
                    Ativo = dr.GetValue("Status").ToString().ToBoolean(),
                    GrupoUsuario = { Id = dr.GetValue("IdGrupoUsuario").ToString().ToInteger() },
                    Foto = { Alterar = true, Arquivo = dr.Field<byte[]>("Foto") }
                };


                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Monta Objeto - " + ex.Source.ToNull().ToString() };
            }
        }

        public ClsUsuario Logar(ref ClsBancoDados banco, string usuario, string senha, long idEmpresa)
        {
            try
            {
                var sql = "";
                var obj = new ClsUsuario();

                sql = $" SELECT * FROM {nomeTabela} a WHERE Usuario = '{usuario}' AND Senha = '{senha}'";
                //sql += $" AND IdEmpresa = '{idEmpresa}'";
                var dt = banco.LoadQuery(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    obj = MontaObjeto(dr);

                }

                return obj;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "(Dados) Logar - " + ex.Source.ToNull().ToString() };
            }

        }
        #endregion

        #region TODOS

        public DataTable Todos(ref ClsBancoDados banco, bool Ativos)
        {
            try
            {

                var sql = "SELECT *,  Usuario Descricao FROM " + nomeTabela + "";
                sql += " WHERE 1 = 1";

                if (Ativos) sql += " AND Status = 1";

                var dt = banco.LoadQuery(sql);


                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Todos - " + ex.Source.ToNull().ToString() };
            }
        }

        public List<ClsUsuario> ListaTodos(ref ClsBancoDados banco, bool Ativos)
        {
            try
            {
                var lista = new List<ClsUsuario>();
                var dt = Todos(ref banco, Ativos);

                foreach (DataRow dr in dt.Rows)
                    lista.Add(MontaObjeto(dr));

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ListaTodos - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region PESQUISAR

        public DataTable Consultar(ref ClsBancoDados bd, bool soAtivos)
        {
            try
            {
                var sql = "SELECT ";
                sql += "\n" + $" a.*, b.Descricao GrupoUsuario";
                sql += "\n" + $" FROM {nomeTabela} a ";
                sql += "\n" + $" JOIN {DadGrupoUsuario.nomeTabela} b ON a.IdGrupoUsuario = b.Id";
                sql += "\n" + $" WHERE 1 = 1";
                if (soAtivos) sql += " AND a.Status = 1";

                var dt = bd.LoadQuery(sql);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Consultar - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region SUB-CLASSES



        #endregion
    }
}
