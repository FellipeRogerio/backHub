using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Excelencia.API.Controlador;
using Negocio;
using Site.Models;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static public List<ConfigAttribute> LGeralPermissao;

        static public ModelLayout DadosGlobais { get; set; } = new ModelLayout();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SetarConfigs();
            LGeralPermissao = new List<ConfigAttribute>();
            try
            {
                LGeralPermissao = Utils.RetornaController<MvcApplication>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
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

        public void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
            //Response.Clear();
            //var httpException = exception;
            //if (httpException as HttpException != null)
            //{
            //    //RouteData routeData = new RouteData();
            //    //routeData.Values.Add("Login", "Login");
            //}
            //else if (httpException as AuthenticationException != null)
            //{
            //    Response.Redirect("/Login/Login");
            //    Server.ClearError();
            //}
            
        }

    }
}
