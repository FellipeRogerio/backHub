using Excelencia.API.Dados;
using Excelencia.BancoDados;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Excelencia.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Classe;
using Excelencia.API.ControleUsuario;
using Excelencia.API.Classe;

namespace Dados
{
    public class DadGrupoUsuario : DadPadrao
    {
        private ClsGrupoUsuario oDTO;
        public static string nomeTabela = "Tb_GrupoUsuario";

        #region CRUD

        public override bool Inserir<T>(ref ClsBancoDados banco, T obj)
        {
            try
            {
                oDTO = obj as ClsGrupoUsuario;
                var lista = MontaListaParametros(banco);

                oDTO.Id = banco.ExecutaScript(TScript.Insert, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"Inseriu Grupo de Usuário").ToString().ToInteger();

                ManterRelacionamentos(ref banco);


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
                oDTO = obj as ClsGrupoUsuario;
                var lista = MontaListaParametros(banco);
                lista.Add(banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true));
                banco.ExecutaScript(TScript.Update, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"Alterou Grupo de Usuário");

                ManterRelacionamentos(ref banco);

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
                oDTO = obj as ClsGrupoUsuario;

                RemoverRelacionamentos(ref banco);

                var lista = new List<ClsParameter>
                {
                    banco.RetornaParametro("Id", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true)
                };
                banco.ExecutaScript(TScript.Delete, nomeTabela, out DbCommand cmd, lista.ToArray(), true, $"Excluiu Grupo de Usuário");

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
                new DadUserGroupPermissao(oDTO).Manter(ref bd);
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
                var dadCampus = new DadUserGroupPermissao(oDTO);

                dadCampus.ExcluirGeral(ref bd);

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
                oBD.RetornaParametro("Descricao", oDTO.Descricao, true, idTipo: DbType.String),
                oBD.RetornaParametro("PermissaoGeral", oDTO.PermissaoTotal, false, idTipo: DbType.Boolean),
                oBD.RetornaParametro("SomenteLeitura", oDTO.SoVisualiza, false, idTipo: DbType.Boolean),

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
                var sql = $" SELECT Id FROM {DadUsuario.nomeTabela} WHERE IdGrupoUsuario = {Id}";
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

        public bool Existe(ref ClsBancoDados banco, string strNomeGrupo, long id = 0)
        {

            try
            {
                var sql = "";
                sql = $" SELECT Id FROM { nomeTabela } WHERE Descricao = '{strNomeGrupo}'";
                sql += $" AND Id <> {id}";

                return banco.LoadQuery(sql).Rows.Count > 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Existe - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region MONTA OBJETO

        public void Filtra(ref ClsBancoDados banco, ref ClsGrupoUsuario obj)
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

        private void FiltraRelacionamento(ref ClsBancoDados banco, ref ClsGrupoUsuario obj)
        {
            try
            {
                obj.ListaPermissoes = new DadUserGroupPermissao(obj).RetornaLista(ref banco);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtra Relacionamento - " + ex.Source.ToNull().ToString() };
            }

        }

        public ClsGrupoUsuario MontaObjeto(DataRow dr)
        {
            try
            {

                var obj = new ClsGrupoUsuario
                {
                    Existe = true,
                    TipoAlteracao = TManter.Alterar,
                    Id = dr.GetValue("Id").ToString().ToInteger(),
                    Descricao = dr.GetValue("Descricao").ToString(),
                    PermissaoTotal = dr.GetValue("PermissaoGeral").ToString().ToBoolean(),
                    SoVisualiza = dr.GetValue("SomenteLeitura").ToString().ToBoolean()
                };


                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Monta Objeto - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region TODOS

        public DataTable Todos(ref ClsBancoDados banco)
        {
            try
            {
                var sql = $"SELECT *, Id Id, Descricao Descricao FROM {nomeTabela} ";
                sql += " WHERE 1 = 1";


                var dt = banco.LoadQuery(sql);


                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Todos - " + ex.Source.ToNull().ToString() };
            }
        }

        public List<ClsGrupoUsuario> ListaTodos(ref ClsBancoDados banco)
        {
            try
            {
                var lista = new List<ClsGrupoUsuario>();
                var dt = Todos(ref banco);

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
                sql += "\n" + $" a.*";
                sql += "\n" + $" FROM {nomeTabela} a ";
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

        #region PERMISSÃO

        public class DadUserGroupPermissao
        {
            private readonly ClsGrupoUsuario oDTO;
            public static string nomeTabela = "Tb_GrupoUsuarioPermissao";
            public DadUserGroupPermissao(ClsGrupoUsuario oUser)
            {
                oDTO = oUser;

            }

            #region CRUD
            public bool Manter(ref ClsBancoDados banco)
            {
                try
                {

                    foreach (var item in oDTO.ListaPermissoes)
                    {
                        var parms = new List<ClsParameter>();
                        var fExecuta = false;
                        var script = TScript.Insert;
                        switch (item.TipoAlteracao)
                        {
                            case TManter.Cadastrar:
                                parms.Add(banco.RetornaParametro("IdGrupoUsuario", oDTO.Id, false, idTipo: DbType.Int32));
                                parms.Add(banco.RetornaParametro("IdTela", item.Id, false, idTipo: DbType.Int32));
                                parms.Add(banco.RetornaParametro("PermissaoGeral", item.Editar, false, idTipo: DbType.Boolean));
                                parms.Add(banco.RetornaParametro("SomenteLeitura ", item.Visualiza, false, idTipo: DbType.Boolean));
                                script = TScript.Insert;
                                fExecuta = true;
                                break;
                            case TManter.Alterar:
                                parms.Add(banco.RetornaParametro("PermissaoGeral", item.Editar, false, idTipo: DbType.Boolean));
                                parms.Add(banco.RetornaParametro("SomenteLeitura ", item.Visualiza, false, idTipo: DbType.Boolean));
                                parms.Add(banco.RetornaParametro("IdTela", item.Id, false, idTipo: DbType.Int32, doWhere: true));
                                parms.Add(banco.RetornaParametro("IdGrupoUsuario", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true));
                                script = TScript.Update;
                                fExecuta = true;
                                break;
                            case TManter.Excluir:
                                parms.Add(banco.RetornaParametro("IdTela", item.Id, false, idTipo: DbType.Int32, doWhere: true));
                                parms.Add(banco.RetornaParametro("IdGrupoUsuario", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true));
                                script = TScript.Delete;
                                fExecuta = true;
                                break;
                            default:
                                break;
                        }

                        if (fExecuta)
                        {
                            var Codigo = banco.ExecutaScript(script, nomeTabela, out DbCommand cmd, parms.ToArray(), true, $"Editou Permissão do Grupo de Usuário: {oDTO.Descricao}");
                            if (Codigo.ToNull().ToInteger() > 0) item.Id = Codigo.ToNull().ToInteger();

                        }

                    }


                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.TrataErro()) { Source = "Manter - " + ex.Source.ToNull().ToString() };
                }
            }

            public bool ExcluirGeral(ref ClsBancoDados banco)
            {
                try
                {

                    var parms = new List<ClsParameter>
                    {
                        banco.RetornaParametro("IdGrupoUsuario", oDTO.Id, false, idTipo: DbType.Int32, doWhere: true)
                    };

                    banco.ExecutaScript(TScript.Delete, nomeTabela, out DbCommand cmd, parms.ToArray(), true, $"Excluiu todas as Permissões do Grupo de Usuário");

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.TrataErro()) { Source = "Excluir Geral - " + ex.Source.ToNull().ToString() };
                }
            }

            #endregion

            #region MONTA OBJETO

            public List<BasePermissao> RetornaLista(ref ClsBancoDados banco)
            {
                try
                {
                    var lista = new List<BasePermissao>();

                    string sql = " SELECT * FROM " + nomeTabela + " WHERE IdGrupoUsuario  = " + oDTO.Id;

                    var dt = banco.LoadQuery(sql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lista.Add(MontaObjeto(dr));

                    }

                    return lista;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.TrataErro()) { Source = "Retorna Lista - " + ex.Source.ToNull().ToString() };
                }
            }

            public BasePermissao MontaObjeto(DataRow dr)
            {
                try
                {

                    var obj = new BasePermissao
                    {
                        Existe = true,
                        TipoAlteracao = TManter.Alterar,
                        Id = dr.GetValue("IdTela").ToString().ToInteger(),
                        Editar = dr.GetValue("PermissaoGeral").ToString().ToBoolean(),
                        Visualiza = dr.GetValue("SomenteLeitura").ToString().ToBoolean()
                    };

                    return obj;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.TrataErro()) { Source = "Monta Objeto - " + ex.Source.ToNull().ToString() };
                }
            }

            #endregion

        }


        #endregion

        #endregion
    }
}
