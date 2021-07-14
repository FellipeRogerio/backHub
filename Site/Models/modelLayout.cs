
using Excelencia.API.ControleUsuario;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Security.Authentication;
using System.Web.Mvc;

namespace Site.Models
{
    public class ModelLayout
    {

        //var pastaIMG = "../../Imagem/Principal";
        //var nomeSite = "Excelencia";
        //var nomeUser = Html.Action("RetornaNomeUsuarioLogado", "Login").ToString();
        //var imgUSer = "";
        //var idEmpresa = "";
        //var logoCliente = "";
        public BaseUsuario Usuario { get; set; } = new BaseUsuario();
        public string LogoCliente { get; set; }
        public string NomeSite { get; set; }
        public string ImgFundoMenu { get; set; }


        public void SetaDados(BaseUsuario oUser)
        {
            Usuario = oUser;
            if (Usuario == null) return;
            LogoCliente = "../../Imagem/Principal/iconeLogo.png";
            NomeSite = "TechsLab";
            ImgFundoMenu = "../../Imagem/Principal/fundo/01.jpg";

            if (Usuario.Foto.Caminho == "")
                Usuario.Foto.Caminho = "../../Content/Tema/imagem/avatar.jpg";
        }
    }
}