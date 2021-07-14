using System;
using System.Collections.Generic;
using System.Data;
using Classe;
using Classe.Configuracao;
using Excelencia.API.Classe;
using Excelencia.API.Dados;
using Excelencia.BancoDados;
using Excelencia.Extensions;

namespace Dados.Configuracao
{
    public class DadSmtpEmail : DadPadrao
    {
        private ClsSmtp oDTO;
        public static readonly string nomeTabela = "Tb_ConfiguracaoEmail";


        #region CRUD

        public override bool Inserir<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsSmtp;
                var lista = MontaListaParametros(banco);
                lista.Add(banco.RetornaParametro("IdConfiguracao", oDTO.Id, false, idTipo: DbType.Int32));

                banco.ExecutaScript(TScript.Insert, nomeTabela, out var cmd, lista.ToArray(), true,
                    "Inseriu Configuração Email").ToString().ToInteger();

                ManterRelacionamentos(ref banco);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Inserir - " + ex.Source.ToNull() };
            }
        }

        public override bool Alterar<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsSmtp;
                var lista = MontaListaParametros(banco);
                lista.Add(banco.RetornaParametro("IdConfiguracao", oDTO.Id, false, idTipo: DbType.Int32,
                    doWhere: true));
                banco.ExecutaScript(TScript.Update, nomeTabela, out var cmd, lista.ToArray(), true,
                    "Alterou Configuração Email");

                ManterRelacionamentos(ref banco);


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Alterar - " + ex.Source.ToNull() };
            }
        }

        public override bool Excluir<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsSmtp;

                RemoverRelacionamentos(ref banco);

                var lista = new List<ClsParameter>
                {
                    banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true)
                };
                banco.ExecutaScript(TScript.Delete, nomeTabela, out var cmd, lista.ToArray(), true,
                    "Excluiu Configuração Email");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Excluir - " + ex.Source.ToNull() };
            }
        }


        private bool ManterRelacionamentos(ref ClsBancoDados bd)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Manter Relacionamentos - " + ex.Source.ToNull() };
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
                throw new Exception(ex.TrataErro()) { Source = "RemoverRelacionamentos - " + ex.Source.ToNull() };
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
                    oBD.RetornaParametro("Email", oDTO.Email, true, idTipo: DbType.String),
                    oBD.RetornaParametro("SmtpServidor", oDTO.SmtpServidor, false, idTipo: DbType.String),
                    oBD.RetornaParametro("SmtpPorta", oDTO.SmtpPorta, false, idTipo: DbType.String),
                    oBD.RetornaParametro("SmtpUsuario", oDTO.SmtpUsuario, false, idTipo: DbType.String),
                    oBD.RetornaParametro("SmtpSenha", oDTO.SmtpSenha, false, idTipo: DbType.String),
                    oBD.RetornaParametro("SSLAtivo", oDTO.SSLAtivo, false, idTipo: DbType.Boolean),
                    oBD.RetornaParametro("EmailTeste", oDTO.EmailTeste, false, idTipo: DbType.String)
                };

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "MontaListaParametros - " + ex.Source.ToNull() };
            }
        }

        #endregion

        #region VALIDAÇÕES

        public bool ValidarRelacionamento(ref ClsBancoDados banco, int Id)
        {
            try
            {
                // ================ INÍCIO ==============================
                //var sql = "";

                //sql = $" SELECT* FROM tb_usergroup a";
                //sql = $" LEFT JOIN tb_user b ON a.Id = b.IdUserGroup";
                //sql = $" WHERE a.Id = '{Id}'";
                //sql = $" AND coalesce(b.Id,0)> 0";

                //sql = $" SELECT Id FROM Tb_Usario WHERE IdUserGroup = '{Id}'";
                //sql += $" AND Id <> {Id}";

                //return banco.LoadQuery(sql).Rows.Count > 0;

                //var dt = banco.LoadQuery(sql);

                //return dt.Rows.Count > 0;
                // ================ FIM ==============================
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ValidarRelacionamento - " + ex.Source.ToNull() };
            }
        }

        #endregion

        #region MONTA OBJETO

        public static ClsSmtp Filtra(ref ClsBancoDados banco, int Id, bool fRelacionamento = true)
        {
            try
            {
                var obj = new ClsSmtp();
                var sql = $" SELECT * FROM {nomeTabela} WHERE IdConfiguracao = {Id} ";
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
                throw new Exception(ex.TrataErro()) { Source = "Filtra - " + ex.Source.ToNull() };
            }
        }

        private static void FiltraRelacionamento(ref ClsBancoDados banco, ref ClsSmtp obj)
        {
            try
            {


            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtra Relacionamento - " + ex.Source.ToNull().ToString() };
            }


        }


        public static ClsSmtp MontaObjeto(DataRow dr)
        {
            try
            {
                var obj = new ClsSmtp
                {
                    Existe = true,
                    TipoAlteracao = TManter.Alterar,
                    Id = dr.GetValue("IdConfiguracao").ToString().ToInteger(),
                    Email = dr.GetValue("Email").ToString().Trim(),
                    EmailTeste = dr.GetValue("EmailTeste").ToString().Trim(),
                    SmtpSenha = dr.GetValue("SmtpSenha").ToString().Trim(),
                    SmtpPorta = dr.GetValue("SmtpPorta").ToString().Trim(),
                    SmtpServidor = dr.GetValue("SmtpServidor").ToString().Trim(),
                    SmtpUsuario = dr.GetValue("SmtpUsuario").ToString().Trim(),
                    SSLAtivo = dr.GetValue("SSLAtivo").ToBoolean(),

                };

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "MontaObjeto - " + ex.Source.ToNull() };
            }
        }

        #endregion

        #region TODOS

        #endregion

        #region PESQUISAR

        public DataTable Consultar(ref ClsBancoDados bd)
        {
            try
            {
                var sql = "SELECT ";
                sql += "\n" + " a.*";
                sql += "\n" + $" FROM {nomeTabela} a ";
                sql += "\n" + " WHERE 1 = 1";

                var dt = bd.LoadQuery(sql);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Consultar - " + ex.Source.ToNull() };
            }
        }

        #endregion
    }
}
