using System;
using System.Dynamic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Excelencia.API.Controlador;
using Excelencia.API.Classe;
using Excelencia.Extensions;
using Negocio;
using Classe.Cadastro.Financeiro;

using System.ComponentModel.DataAnnotations;

namespace Site.Controllers
{
    //Descomentar quando fizer a view[Config(Codigo = ClsControler.IdCondicaoPgto, NomeGrupoMenu = ClsMenu.GrupoGeral, iconeCSS = "fas fa-hotel", ActionPrincipal = "Index", NomeController = "CondicaoPgto", NomeAmigavel = "Condição Pgto")]
    public class CondicaoPgtoController : PadraoController
    {
        //public ConfigAttribute RetornaConfig() => (ConfigAttribute)GetType().GetCustomAttributes(typeof(ConfigAttribute), true)[0];
        //private bool userEdita = true;
        //#region TELAS

        //[Config(TipoAcao = TAcao.Visualizar)]
        //public override ActionResult Index()
        //{
        //    userEdita = UsuarioEdita(ClsControler.IdUsuario);
        //    ViewBag.UsuarioEdita = userEdita;
        //    ViewBag.NomeController = RetornaConfig().NomeAmigavel;
        //    return View();
        //}


        ////[HttpPost]
        //[Config(TipoAcao = TAcao.Editar)]
        //public override ActionResult Editar(int Id = 0)
        //{
        //    var neg = new NegCondicaoPgto();
        //    var dto = new ClsCondicaoPgto();
        //    var user = RetornaUsuarioLogado();

        //    if (Id > 0) dto = neg.Filtrar(Id);
        //    ViewBag.NomeController = RetornaConfig().NomeAmigavel;
        //    ViewBag.TelaModal = false;
        //    ViewBag.IdGlobalCondicaoPgto = user.Empresa.Id;
        //    ViewBag.IdUser = user.Id;
        //    ViewBag.NomeUser = user.Nome;

        //    return View(dto);
        //}


        //[Config(TipoAcao = TAcao.Excluir)]
        //public override ActionResult Excluir()
        //{
        //    throw new NotImplementedException();
        //}


        //#endregion

        //#region SALVAR

        //[Config(TipoAcao = TAcao.Editar)]
        //[HttpPost]
        //public string Gravar(ClsCondicaoPgto obj)
        //{
        //    dynamic ret = new ExpandoObject();
        //    ret.Success = false;
        //    ret.Validacao = false;
        //    ret.Message = "";
        //    ret.Origem = "";
        //    try
        //    {
        //        obj = JsonConvert.DeserializeObject<ClsCondicaoPgto>(Request.Form["obj"]);

        //        return Salvar(obj);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        ret.Success = false;
        //        ret.Validacao = true;
        //        ret.Message = ex.TrataErro();
        //        return JsonConvert.SerializeObject(ret);
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Success = false;
        //        ret.Validacao = false;
        //        ret.Message = ex.TrataErro();
        //        ret.Origem = ex.Source;
        //        return JsonConvert.SerializeObject(ret);
        //    }
        //}

        //[Config(TipoAcao = TAcao.Editar)]
        //[HttpPost]
        //protected override string Salvar<T>(T obj)
        //{

        //    dynamic ret = new ExpandoObject();
        //    ret.Success = false;
        //    ret.Validacao = false;
        //    ret.Message = "";
        //    ret.Origem = "";
        //    try
        //    {
        //        var oDTO = (obj as ClsCondicaoPgto);


        //        var neg = new NegCondicaoPgto();
        //        SetaUserNeg(neg);

        //        var erro = neg.Validacao(oDTO);

        //        if (erro != "") throw new Exception(erro);

        //        if (oDTO.Id > 0)
        //            neg.Alterar(ref oDTO);
        //        else
        //            neg.Inserir(ref oDTO);

        //        ret.Success = true;
        //        ret.Message = "Dados salvos com sucesso";

        //        return JsonConvert.SerializeObject(ret);



        //    }
        //    catch (ValidationException ex)
        //    {
        //        ret.Success = false;
        //        ret.Validacao = true;
        //        ret.Message = ex.TrataErro();
        //        return JsonConvert.SerializeObject(ret);
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Success = false;
        //        ret.Validacao = false;
        //        ret.Message = ex.TrataErro();
        //        ret.Origem = ex.Source;
        //        return JsonConvert.SerializeObject(ret);
        //    }
        //}

        //[Config(TipoAcao = TAcao.Excluir)]
        //[HttpPost]
        //public string Excluir(int Codigo)
        //{
        //    dynamic ret = new ExpandoObject();
        //    ret.Success = false;
        //    ret.Validacao = false;
        //    ret.Message = "";
        //    ret.Origem = "";

        //    try
        //    {


        //        var neg = new NegCondicaoPgto();
        //        SetaUserNeg(neg);

        //        neg.Excluir(Codigo);

        //        ret.Success = true;
        //        ret.Message = "Registro excluído com Sucesso";

        //        return JsonConvert.SerializeObject(ret);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        ret.Success = false;
        //        ret.Validacao = true;
        //        ret.Message = ex.TrataErro();
        //        return JsonConvert.SerializeObject(ret);
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Success = false;
        //        ret.Validacao = false;
        //        ret.Message = ex.TrataErro();
        //        ret.Origem = ex.Source;
        //        return JsonConvert.SerializeObject(ret);
        //    }


        //}
        //#endregion

        //public string RetornaParcelas(int IdCondicaoPgto) => JsonConvert.SerializeObject(new NegCondicaoPgto().ListaParcelas(IdCondicaoPgto));


        //#region CONSULTAS
        //public string RetornaTodos(bool soAtivos = false) => JsonConvert.SerializeObject(new NegCondicaoPgto().Todos(soAtivos));
        //public string Consultar(bool soAtivos = false) => JsonConvert.SerializeObject(new NegCondicaoPgto().Consultar(soAtivos));
        //#endregion
    }
}