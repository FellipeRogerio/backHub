using Excelencia.API.Classe;
using Excelencia.API.Controlador;
using Excelencia.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web;
using Extensions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Site.Models;

using Classe.Cadastro.Geral;
//using Negocio.Cadastro;
//using Negocio.Geral;

namespace Site.Controllers.Geral
{
    [Authorize]
    public class GeralController : PadraoController
    {
        #region CONTATO
        
        public ActionResult ContatoIndex()
        {
            var dto = JsonConvert.DeserializeObject<ModelContato>(Request.Form["model"]);

            return View(dto);
        }

        #region SALVAR
    
        #endregion

        #endregion
        
        public ActionResult TelefoneIndex()
        {
            var dto = JsonConvert.DeserializeObject<ModelTelefone>(Request.Form["model"]);

            return View(dto);
        }



    }
}