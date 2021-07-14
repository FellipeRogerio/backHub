using Excelencia.Extensions;
using Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SetarConfigs();

        }
        private string RecuperaProp(string nome)
        {
            try
            {

                return ConfigurationManager.AppSettings[nome];
            }
            catch (Exception ex)
            {

                throw new Exception($"Propriedade: {nome}" + ex.Message);
            }
        }
        private void SetarConfigs()
        {
            try
            {


                var props = new ClsPropriedades
                {
                    UrlRaiz = RecuperaProp("UrlRaiz"),
                    ConexaoBD = RecuperaProp("ConexaoBD"),
                    ConexaoBDImagem = RecuperaProp("ConexaoBDImagem"),
                    PDFTermoUso = RecuperaProp("PDFTermoUso"),
                    TokenConfig = new ClsTokenConfig
                    {
                        TempoSegundo = Convert.ToInt32(RecuperaProp("TokenDuracaoSegundos"))
                    }
                };

                //props.TokenConfig.Emitente = RecuperaProp("TokenIssuer");



                Utilitario.SetarProps(props);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

    }
}
