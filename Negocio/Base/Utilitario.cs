using Dados;
using Excelencia.API.Classe;
using Excelencia.API.Funcoes;
using Excelencia.BancoDados;
using Excelencia.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
//using Despachante.Negocio.Base;

namespace Negocio
{

    public class Utilitario
    {
        #region BASE




        public static void SetarProps(ClsPropriedades obj)
        {
            Props = obj;

        }
        public static ClsPropriedades Props { get; set; }


        public static string RetornaConexao() => Props.ConexaoBD;

        public static string RetornaConexaoImagem() => Props.ConexaoBDImagem;
        private static CryptProvider RetornaTipoCrypt() => CryptProvider.DES;
        private static string RetornaChaveCrypt() => $"TECHLABs@{ClsPropriedades.NomeCliente}";

        public static string Cryptografar(string texto) =>
            Criptografia.Crypt(texto, RetornaChaveCrypt(), RetornaTipoCrypt());

        public static string DeCryptografar(string texto) =>
            Criptografia.DeCryptar(texto, RetornaChaveCrypt(), RetornaTipoCrypt());

        public static string Cryptografar(string texto, string Chave) =>
            Criptografia.Crypt(texto, RetornaChaveCrypt() + "@" + Chave, RetornaTipoCrypt());

        public static string DeCryptografar(string texto, string Chave) =>
            Criptografia.DeCryptar(texto, RetornaChaveCrypt() + "@" + Chave, RetornaTipoCrypt());
        #endregion

        #region FUNÇÕES

        public static T CopiarObj<T>(T obj)
        {
            try
            {

                var jsonCopy = JsonConvert.SerializeObject(obj);

                return JsonConvert.DeserializeObject<T>(jsonCopy);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
  


    }

    public class ClsBD
    {
        public static void SetaConexao(ref ClsBancoDados obj, BaseAuditoria oAudit, bool ComBegin = false)
        {
            if (obj == null)
                obj = new ClsBancoDados(Utilitario.RetornaConexao(), ComTransaction: ComBegin);
            else
            if (obj.StatusConexao() != System.Data.ConnectionState.Open)
                obj = new ClsBancoDados(Utilitario.RetornaConexao(), ComTransaction: ComBegin);

            obj.Auditoria.CodUsuario = oAudit.CodUsuario;
            obj.Auditoria.NomeUsuario = oAudit.NomeUsuario;
            obj.Auditoria.Computador = oAudit.Computador;
            obj.Auditoria.NomeTabela = "Tb_Auditoria";

        }

        public static void SetaConexaoImagem(ref ClsBancoDados obj, BaseAuditoria oAudit, bool ComBegin = false)
        {
            if (obj == null)
                obj = new ClsBancoDados(Utilitario.RetornaConexaoImagem(), ComTransaction: ComBegin);
            else
            if (obj.StatusConexao() != System.Data.ConnectionState.Open)
                obj = new ClsBancoDados(Utilitario.RetornaConexaoImagem(), ComTransaction: ComBegin);

            obj.Auditoria.CodUsuario = oAudit.CodUsuario;
            obj.Auditoria.NomeUsuario = oAudit.NomeUsuario;
            obj.Auditoria.Computador = oAudit.Computador;
            obj.Auditoria.NomeTabela = "Tb_Auditoria";

        }
    }

}
