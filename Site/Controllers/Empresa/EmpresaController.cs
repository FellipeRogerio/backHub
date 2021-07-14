using System;
using System.Dynamic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Excelencia.API.Controlador;
using Excelencia.API.Classe;
using Excelencia.Extensions;
using Classe;
using Negocio;
using Classe.Cadastro.Adm;
using System.ComponentModel.DataAnnotations;
using Excelencia.API.Negocio;

namespace Site.Controllers
{
    //[Config(Codigo = ClsControler.IdEmpresa, NomeGrupoMenu = ClsMenu.GrupoGeral, iconeCSS = "fas fa-hotel", ActionPrincipal = "List", NomeController = "Empresa", NomeAmigavel = "Empresa")]
    public class EmpresaController : PadraoController
    {
        //public ConfigAttribute RetornaConfig() => (ConfigAttribute)GetType().GetCustomAttributes(typeof(ConfigAttribute), true)[0];
        //private bool userEdita = true;
        //#region TELAS

        //[Config(TipoAcao = TAcao.Visualizar)]
        //[ActionName("List")]
        //public override ActionResult Index()
        //{
        //    userEdita = UsuarioEdita(RetornaConfig().Codigo);
        //    return RedirectToAction("Index", new { edit = userEdita, type = RetornaConfig().NomeAmigavel });
        //}

        //[Authorize]
        //public ActionResult Index(bool edit, string type)
        //{
        //    ViewBag.UsuarioEdita = edit;
        //    ViewBag.NomeController = type;
        //    return View();
        //}
        ////[HttpPost]
        //[Config(TipoAcao = TAcao.Editar)]
        //public override ActionResult Editar(int Id=0)
        //{
        //    var neg = new NegEmpresa();
        //    var dto = new ClsEmpresa();
        //    var user = RetornaUsuarioLogado();

        //    if (Id > 0) dto = neg.Filtrar(Id);
        //    ViewBag.NomeController = RetornaConfig().NomeAmigavel;
        //    ViewBag.TelaModal = false;
        //    ViewBag.IdGlobalEmpresa = user.Empresa.Id;
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
        //public string Gravar(ClsEmpresa obj)
        //{
        //    var ret = MontaRetorno();
        //    try
        //    {
        //        obj = JsonConvert.DeserializeObject<ClsEmpresa>(Request.Form["obj"]);
        //        var arq = obj.Logo;

        //        if (arq.Alterar) RetornaArquivo(ref arq, Request.Files);
        //        //var caminho = SalvarArquivo(Request.Files, "Seguranca");

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
        //    var ret = MontaRetorno();
            
        //    try
        //    {
        //        var oDTO = (obj as ClsEmpresa);


        //        var neg = new NegEmpresa();
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
        //    var ret = MontaRetorno();

        //    try
        //    {


        //        var neg = new NegEmpresa();
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


        //#region CONSULTAS
        //public string RetornaTodos(bool soAtivos = false)=> JsonConvert.SerializeObject(new NegEmpresa().Todos(soAtivos));
        //public string Consultar(bool soAtivos = false) => JsonConvert.SerializeObject(new NegEmpresa().Consultar(soAtivos));
        //#endregion
    }
}