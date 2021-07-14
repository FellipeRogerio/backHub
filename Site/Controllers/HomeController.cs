using System;
using System.IO;
using System.Web.Mvc;
using Excelencia.API.Classe;
using Excelencia.API.Controlador;
using Newtonsoft.Json;
using Site.Models;
using Extensions;
using System.Web.Hosting;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Site.Controllers
{
    
    [Config(Codigo = 0,iconeCSS = "ft-home", NomeController = "Home", NomeAmigavel = "Principal" )]
    public class HomeController : PadraoController
    {
        public ConfigAttribute RetornaConfig() =>(ConfigAttribute)GetType().GetCustomAttributes(typeof(ConfigAttribute), true)[0];
     

        [Config(TipoAcao = TAcao.Visualizar)]
        public override ActionResult Index()
        {
            ViewBag.NomeController = RetornaConfig().NomeAmigavel;

            return View();
        }


    }
}