using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excelencia.API.Classe;
using Excelencia.Extensions;
using Classe.Cadastro.Geral;
using Extensions;
using Classe;

namespace Site.Models
{
   

    public class ClsAnexos 
    {
        public string NomeArquivo { get; set; }
        public string URL { get; set; }

    }
   
    public class ModelEmail
    {
        public bool Ini { get; set; }
        public ClsEmail Email { get; set; } = new ClsEmail();
        
        public List<ClsAnexos> DefaultAnexos { get; set; } = new List<ClsAnexos>();

        [AllowHtml]
        public string BodyEmail { get; set; }

        public List<ClsAnexos> SalvarAnexosEmail(string pasta, List<ClsArquivo> arquivos)
        {
            var lista = new List<ClsAnexos>();

            arquivos.ForEach(x =>
            {
                var item = new ClsAnexos();
                if (x.Base64 != "")
                {
                    var caminho = Extends.RetornaCaminho(pasta, x.NomeArquivo);
                    x.Arquivo.ToSalvaArquivo(caminho);
                    item.NomeArquivo = x.NomeArquivo;
                    item.URL = caminho;
                }
                else
                {
                    item.NomeArquivo = x.NomeArquivo;
                    item.URL = x.Caminho;
                }
                lista.Add(item);

            });


            return lista;

        }
    }

 
}