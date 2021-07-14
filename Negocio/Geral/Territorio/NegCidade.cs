using Excelencia.API.Classe;
using Excelencia.API.Negocio;
using Excelencia.BancoDados;
using Excelencia.Extensions;
using Classe;
using Dados.Geral;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Negocio.Geral
{
    public class NegCidade : NegPadrao
    {
        private readonly DadCidade oDados = new DadCidade();
        private ClsCidade oDTO = new ClsCidade();
        private ClsBancoDados oBD;

        #region CRUD

        protected override bool Cadastrar<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsCidade;
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
                oDTO = obj as ClsCidade;
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
                oDTO = obj as ClsCidade;
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

                oBD.FecharConexao();

                return true;

            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Inserir - " + ex.Source.ToNull().ToString() };
            }
        }

        public override bool Alterar<T>(ref T obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Atualizar(ref oBD, ref obj);
                oBD.FecharConexao();
                return true;

            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Alterar - " + ex.Source.ToNull().ToString() };
            }
        }

        public override bool Excluir(int Codigo)
        {
            try
            {
                var obj = new ClsCidade() { Id = Codigo, TipoAlteracao = TManter.Excluir };
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Remover(ref oBD, ref obj);
                oBD.FecharConexao();


                return true;

            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Excluir - " + ex.Source.ToNull().ToString() };
            }
        }

        public override bool Manter<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = (obj as ClsCidade);

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

        #endregion

        #region VALIDAÇÃO
        /// <summary>
        /// FUNÇÃO PARA INSERIR OS TRATAMENTOS NAS PROPRIEDADES PARA ENVIAR PARA A CAMADA DE DADOS
        /// </summary>
        private void TrataDTO()
        {
            oDTO.Nome = oDTO.Nome.ToNull().ToString();
        }

        /// <summary>
        /// FUNÇÃO PARA INSERIR AS REGRAS DE NEGÓCIO DO OBJETO
        /// </summary>
        private void RegrasDTO()
        {

        }

        public string Validacao(ClsCidade obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                oDTO = obj;
                ValidaDTO(oBD);
                oBD.FecharConexao();

                return "";

            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                return ex.TrataErro();
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
                        throw new ValidationException("A Cidade não pode ser excluída porque está sendo usada em outras partes do sistema.");

                }
                else
                {
                    if (oDTO.Nome.ToNull().ToString().Length > 2)
                    {
                        if (oDados.Existe(ref bd, oDTO.Nome, oDTO.Id))
                            throw new ValidationException("Cidade já existe no sistema.");
                    }
                    else
                        throw new ValidationException("Campo Descrição da Cidade não informado.");

                    if (oDados.ExisteCodIBGE(ref bd, oDTO.CodIBGE, oDTO.Id))
                        throw new ValidationException("Código IBGE já existe no sistema.");
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

        public ClsCidade Filtrar(int Codigo)
        {
            try
            {
                oDTO.Id = Codigo;
                ClsBD.SetaConexao(ref oBD, Audit, false);
                oDados.Filtra(ref oBD, ref oDTO);
                oBD.FecharConexao();
                return oDTO;
            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Filtrar - " + ex.Source.ToNull().ToString() };
            }

        }

        #endregion

        #region FUNÇÃO

        public DataTable Todos(bool Ativos)
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var dt = oDados.Todos(ref oBD, Ativos);

                oBD.FecharConexao();

                return dt;
            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Todos - " + ex.Source.ToNull().ToString() };
            }

        }

        public DataTable TodosPorEstado(bool Ativos, int IdEstado)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var dt = oDados.TodosPorEstado(ref oBD, Ativos, IdEstado);
                oBD.FecharConexao();

                return dt;
            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Todos Por Estado - " + ex.Source.ToNull().ToString() };
            }

        }

        public List<ClsCidade> ListaTodos(bool Ativos)
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var lista = oDados.ListaTodos(ref oBD, Ativos);
                oBD.FecharConexao();
                return lista;
            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Lista Todos - " + ex.Source.ToNull().ToString() };
            }

        }

        public DataTable Consultar(bool soAtivos)
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var dt = oDados.Consultar(ref oBD, soAtivos);
                oBD.FecharConexao();
                return dt;
            }
            catch (ValidationException ex) { try { oBD.FecharConexao(); } catch (Exception) { } throw new ValidationException(ex.TrataErro()); }
            catch (Exception ex)
            {
                try { oBD.FecharConexao(); } catch (Exception) { }
                throw new Exception(ex.TrataErro()) { Source = "Consultar - " + ex.Source.ToNull().ToString() };
            }

        }

        #endregion
    }
}
