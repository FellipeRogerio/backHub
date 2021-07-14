using System;
using System.Collections.Generic;
using Excelencia.API.Negocio;
using Excelencia.API.Classe;
using Excelencia.Extensions;
using Excelencia.BancoDados;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Classe;
using Dados;

namespace Negocio
{
    public class NegGrupoUsuario : NegPadrao
    {
        private readonly DadGrupoUsuario oDados = new DadGrupoUsuario();
        private ClsGrupoUsuario oDTO = new ClsGrupoUsuario();
        private ClsBancoDados oBD;

        #region CRUD

        protected override bool Cadastrar<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsGrupoUsuario;
                if (!ValidaDTO(bd)) return false;

                return oDados.Inserir(ref oBD, obj);
            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Cadastrar - " + ex.Source.ToNull().ToString() };
            }
        }

        protected override bool Atualizar<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsGrupoUsuario;
                if (!ValidaDTO(bd)) return false;
                return oDados.Alterar(ref oBD, obj);
            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Atualizar - " + ex.Source.ToNull().ToString() };
            }
        }

        protected override bool Remover<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsGrupoUsuario;
                if (!ValidaDTO(bd)) return false;
                return oDados.Excluir(ref bd, obj);
            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Remover - " + ex.Source.ToNull().ToString() };
            }
        }

        #endregion

        #region ALTERAÇÃO DE DADOS

        public override bool Inserir<T>(ref T obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Cadastrar(ref oBD, ref obj);

                return true;

            }
            catch (ValidationException ex) {  throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Inserir - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public override bool Alterar<T>(ref T obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Atualizar(ref oBD, ref obj);
                return true;

            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Alterar - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public override bool Excluir(int Codigo)
        {
            try
            {
                var obj = new ClsGrupoUsuario() { Id = Codigo, TipoAlteracao = TManter.Excluir };
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Remover(ref oBD, ref obj);
              
                return true;

            }
            catch (ValidationException ex) {throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Excluir - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public override bool Manter<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = (obj as ClsGrupoUsuario);

                switch (oDTO.TipoAlteracao)
                {
                    case TManter.Cadastrar:
                        return Cadastrar(ref oBD, ref oDTO);

                    case TManter.Alterar:
                        return Atualizar(ref oBD, ref oDTO);

                    case TManter.Excluir:
                        return Remover(ref oBD, ref oDTO);

                    default:
                        return true;
                }

            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Manter - " + ex.Source.ToNull().ToString() };
            }
        }

        public bool SalvarPermissoes(ref ClsGrupoUsuario obj)
        {
            try
            {
                if (obj.Id == 0) throw new ValidationException("Grupo de Usuário não informado");
                var oDad = new DadGrupoUsuario.DadUserGroupPermissao(obj);

                ClsBD.SetaConexao(ref oBD, Audit, true);
                oDad.Manter(ref oBD);

                return true;

            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Salvar Permissoes - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }
        #endregion

        #region VALIDAÇÃO

        private void TrataDTO()
        {
            try
            {
                oDTO.Descricao = oDTO.Descricao.ToNull().ToString();
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "TrataDTO - " + ex.Source.ToNull() };
            }
            
        }

        public string Validacao(ClsGrupoUsuario obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                oDTO = obj;
                ValidaDTO(oBD);

                return "";

            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Validacao - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }

        }

        private bool ValidaDTO(ClsBancoDados bd)
        {
            try
            {
                if (oDTO.Validado) return oDTO.Validado;
                TrataDTO();

                if (oDTO.TipoAlteracao == TManter.Excluir)
                {
                    if (oDados.ValidarRelacionamento(ref bd, oDTO.Id))
                        throw new ValidationException("O Grupo de usuário não pode ser excluído porque está sendo usado em outras partes do sistema.");

                }
                else
                {
                    if (oDTO.Descricao.ToNull().ToString().Length >= 2)
                    {
                        if (oDados.Existe(ref bd, oDTO.Descricao, oDTO.Id))
                            throw new ValidationException("Grupo de usuário já existe no sistema.");
                    }
                    else
                        throw new ValidationException("Campo Descrição do Grupo de Usuário precisa ter 2 ou mais caractéres.");

                }

                oDTO.Validado = true;
                return oDTO.Validado;

            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Valida DTO - " + ex.Source.ToNull().ToString() };
            }

        }

        #endregion

        #region FILTRAR

        public ClsGrupoUsuario Filtrar(int Codigo)
        {
            try
            {
                oDTO.Id = Codigo;
                ClsBD.SetaConexao(ref oBD, Audit, false);


                oDados.Filtra(ref oBD, ref oDTO);
    
                return oDTO;
            }
            catch (ValidationException ex) { throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtrar - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }

        }

        #endregion

        #region FUNÇÃO

        public DataTable Todos()
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var dt = oDados.Todos(ref oBD);

                return dt;
            }
            catch (ValidationException ex) {  throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Todos - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }

        }
        public List<ClsGrupoUsuario> ListaTodos()
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var lista = oDados.ListaTodos(ref oBD);

                return lista;
            }
            catch (ValidationException ex) {  throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Lista Todos - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }

        }


        public DataTable Consultar(bool soAtivos)
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var dt = oDados.Consultar(ref oBD, soAtivos);

                return dt;
            }
            catch (ValidationException ex) {  throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Consultar - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        #endregion
    }
}
