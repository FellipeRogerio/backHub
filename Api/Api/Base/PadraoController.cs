using Api.Models;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Excelencia.Extensions;

namespace Api
{
    public class PadraoController : ApiController
    {
        public static bool ValidaToken(System.Net.Http.HttpRequestMessage request)
        {
            try
            {
                var Token = request?.Headers?.Authorization?.Parameter;

                if (string.IsNullOrEmpty(Token)) throw new ValidationException("TOKEN não informado.");
                var ret = Utilitario.Props.TokenConfig.ValidaToken(Token);

                return ret;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IDX12729"))
                    throw new ValidationException("Token Inválido");
                if (ex.Message.Contains("IDX10503"))
                    throw new ValidationException("Token Inválido");
                throw new ValidationException(ex.Message);
            }

        }
        public ClsRetorno MontaRetorno()
        {

            var ret = new ClsRetorno();
            return ret;

        }
        public void MontaRetornoSucesso(ref ClsRetorno obj, string mensagem, dynamic data, bool existe = false)
        {

            obj.data = data;
            obj.CodigoRetorno = 1;
            obj.Sucesso = true;
            obj.DescricaoRetorno = mensagem;
            obj.Existe = existe;

        }
 

        public static bool ExisteArquivo(string caminho)
        {
            return File.Exists(caminho);
        }
        public bool URLExists(string url)
        {
            bool result = true;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
