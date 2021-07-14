using Classe.Configuracao;
using Excelencia.BancoDados;
using Excelencia.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Configuracao;

namespace Negocio.Configuracao
{
    public class NegConfiguracao
    {
        private ClsConfiguracao oDTO = new ClsConfiguracao();
        private ClsBancoDados oBD;

        #region FILTRAR

        public ClsConfiguracao Filtrar(int Codigo)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, new Excelencia.API.Classe.BaseAuditoria(), false);

                oDTO = Filtrar(ref oBD, Codigo);

                oBD.FecharConexao();

                return oDTO;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtrar - " + ex.Source.ToNull().ToString() };
            }

        }

        public ClsConfiguracao Filtrar(ref ClsBancoDados bd, int Codigo)
        {
            try
            {

                oDTO.SmtpEmail = DadSmtpEmail.Filtra(ref bd, Codigo);



                return oDTO;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtrar (BD)- " + ex.Source.ToNull().ToString() };
            }


        }
        #endregion
    }
}
