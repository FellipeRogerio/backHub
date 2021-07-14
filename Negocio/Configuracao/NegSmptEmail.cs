using System;
using System.ComponentModel.DataAnnotations;
using Excelencia.API.Negocio;
using Excelencia.Extensions;
using Excelencia.BancoDados;
using Dados.Configuracao;
using Classe;
using Excelencia.API.Classe;
using System.Data;

namespace Negocio.Configuracao
{
    public class NegSmptEmail : NegPadrao
    {
        private readonly DadSmtpEmail oDados = new DadSmtpEmail();
        private ClsSmtp oDTO = new ClsSmtp();
        private ClsBancoDados oBD;


        #region CRUD

        protected override bool Cadastrar<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsSmtp;
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
                oDTO = obj as ClsSmtp;
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
                oDTO = obj as ClsSmtp;
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
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
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
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
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
                var obj = new ClsSmtp() { Id = Codigo, TipoAlteracao = TManter.Excluir };
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Remover(ref oBD, ref obj);


                return true;

            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
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
                oDTO = (obj as ClsSmtp);

                switch (oDTO.TipoAlteracao)
                {
                    case TManter.Cadastrar:
                        return Cadastrar(ref bd, ref oDTO);

                    case TManter.Alterar:
                        return Atualizar(ref bd, ref oDTO);

                    case TManter.Excluir:
                        return Remover(ref bd, ref oDTO);

                    default:
                        return true;
                }




            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Manter - " + ex.Source.ToNull().ToString() };
            }
        }


        #endregion

        #region VALIDAÇÃO

        private void TrataDTO()
        {



        }

        private void CryptografarProps()
        {
            try
            {






            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "CryptografarProps - " + ex.Source.ToNull().ToString() };
            }



        }

        public string Validacao(ClsSmtp obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                oDTO = obj;
                ValidaDTO(oBD);

                return "";

            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                return "validação: " + ex.TrataErro();
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
                TrataDTO();



                return true;

            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Valida DTO - " + ex.Source.ToNull().ToString() };
            }

        }

        #endregion

        #region FILTRAR

        public ClsSmtp Filtrar(int Codigo)
        {
            try
            {
                oDTO.Id = Codigo;
                ClsBD.SetaConexao(ref oBD, Audit, false);


                oDTO = DadSmtpEmail.Filtra(ref oBD, Codigo);
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

        public DataTable Consultar()
        {

            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, false);
                var dt = oDados.Consultar(ref oBD);
                oBD.FecharConexao();
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
