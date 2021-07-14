using Excelencia.API.Classe;
using Excelencia.API.Dados;
using Excelencia.BancoDados;
using Excelencia.Extensions;
using Classe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.Geral
{
    public class DadPais : DadPadrao
    {
        private ClsPais oDTO;
        public static readonly string nomeTabela = "Tb_Pais";

        #region CRUD

        public override bool Inserir<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsPais;
                var lista = MontaListaParametros(banco);

                oDTO.Id = banco.ExecutaScript(TScript.Insert, nomeTabela, out DbCommand cmd,  lista.ToArray()).ToString().ToInteger();

                ManterRelacionamentos(ref banco);

                //================= AUDITORIA =================//

                //================= AUDITORIA =================//

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
                oDTO = obj as ClsPais;
                var lista = MontaListaParametros(banco);
                lista.Add(banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true));
                banco.ExecutaScript(TScript.Update, nomeTabela, out DbCommand cmd,  lista.ToArray());

                ManterRelacionamentos(ref banco);

                //================= AUDITORIA =================//

                //================= AUDITORIA =================//
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
                oDTO = obj as ClsPais;

                RemoverRelacionamentos(ref banco);

                var lista = new List<ClsParameter>
                {
                    banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true)
                };
                banco.ExecutaScript(TScript.Delete, nomeTabela, out DbCommand cmd,  lista.ToArray());

                //================= AUDITORIA =================//

                //================= AUDITORIA =================//
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Excluir - " + ex.Source.ToNull().ToString() };
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
                throw new Exception(ex.TrataErro()) { Source = "Manter Relacionamentos - " + ex.Source.ToNull().ToString() };
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
                oBD.RetornaParametro("Status", oDTO.Ativo, false, idTipo: DbType.Boolean),
                oBD.RetornaParametro("Nome", oDTO.Nome, true, idTipo: DbType.String),
                oBD.RetornaParametro("Sigla", oDTO.Sigla, false, idTipo: DbType.String),
                oBD.RetornaParametro("CodIBGE", oDTO.CodIBGE, false, idTipo: DbType.String),

            };

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
                // ================ INÍCIO ==============================
                var sql = "";

                sql = $" SELECT Id FROM Tb_Usuario WHERE IdPais = '{Id}'";
                sql += $" AND Id <> {Id}";

                return banco.LoadQuery(sql).Rows.Count > 0;

                //var dt = banco.LoadQuery(sql);

                //return dt.Rows.Count > 0;
                // ================ FIM ==============================
                // return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Validar Relacionamento - " + ex.Source.ToNull().ToString() };
            }
        }

        public bool Existe(ref ClsBancoDados banco, string strNome, long id = 0)
        {

            try
            {
                var sql = "";
                sql = $" SELECT Id FROM { nomeTabela } WHERE Nome = '{strNome}'";
                sql += $" AND Id <> {id}";

                return banco.LoadQuery(sql).Rows.Count > 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Existe - " + ex.Source.ToNull().ToString() };
            }
        }

        public bool ExisteCodIBGE(ref ClsBancoDados banco, string strCodIBGE, long id = 0)
        {

            try
            {
                var sql = "";
                sql = $" SELECT Id FROM { nomeTabela } WHERE CodIBGE = '{strCodIBGE}'";
                sql += $" AND Id <> {id}";

                return banco.LoadQuery(sql).Rows.Count > 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Existe CodIBGE - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region MONTA OBJETO

        public void Filtra(ref ClsBancoDados banco, ref ClsPais obj)
        {
            try
            {
                string sql = " SELECT * FROM " + nomeTabela + " WHERE Id = " + obj.Id;

                var dt = banco.LoadQuery(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    obj = MontaObjeto(dr);
                    FiltraRelacionamento(ref banco, ref obj);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtra - " + ex.Source.ToNull().ToString() };
            }
        }

        private void FiltraRelacionamento(ref ClsBancoDados banco, ref ClsPais obj)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtra Relacionamento - " + ex.Source.ToNull().ToString() };
            }

        }

        public ClsPais MontaObjeto(DataRow dr)
        {
            try
            {

                var obj = new ClsPais();
                obj.TipoAlteracao = Excelencia.API.Classe.TManter.Alterar;
                obj.Id = dr.GetValue("Id").ToString().ToInteger();
                obj.Ativo = dr.GetValue("Status").ToBoolean();
                obj.Nome = dr.GetValue("Nome").ToString();
                obj.Sigla = dr.GetValue("Sigla").ToString();
                obj.CodIBGE = dr.GetValue("CodIBGE").ToString();


                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Monta Objeto - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region TODOS

        public DataTable Todos(ref ClsBancoDados banco, bool Ativos)
        {
          

           var sql = $"SELECT *, Id Id, Nome Descricao FROM {nomeTabela}";
            sql += " WHERE 1 = 1";


            //if (Ativos) sql += " AND Status = 1";


           var dt = banco.LoadQuery(sql);


            return dt;
        }

        public List<ClsPais> ListaTodos(ref ClsBancoDados banco, bool Ativos)
        {
            var lista = new List<ClsPais>();
            var dt = Todos(ref banco, Ativos);

            foreach (DataRow dr in dt.Rows)
                lista.Add(MontaObjeto(dr));

            return lista;
        }

        #endregion

        #region PESQUISAR

        public DataTable Consultar(ref ClsBancoDados bd, bool soAtivos)
        {
            var dt = new DataTable();
            var sql = "";

            sql = "SELECT ";
            sql += "\n" + $" a.*";
            sql += "\n" + $" FROM {nomeTabela} a ";
            sql += "\n" + $" WHERE 1 = 1";
            if (soAtivos) sql += " AND a.Status = 1";

            dt = bd.LoadQuery(sql);

            return dt;
        }

        #endregion

        #region SUB-CLASSES

        #region PERMISSÃO

        #endregion

        #endregion
    }
}
