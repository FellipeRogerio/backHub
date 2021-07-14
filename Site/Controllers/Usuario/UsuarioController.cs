using System;
using System.Dynamic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Excelencia.API.Controlador;
using Excelencia.API.Classe;
using Excelencia.Extensions;
using Classe;
using Negocio;
using System.ComponentModel.DataAnnotations;

namespace Site.Controllers
{
    [Config(Codigo = ClsControler.IdUsuario, NomeGrupoMenu = ClsMenu.GrupoUsuario, iconeCSS = "fa fa-user fa-fw", ActionPrincipal = "Index", NomeController = "Usuario", NomeAmigavel = "Usuário")]
    public class UsuarioController : PadraoController
    {
        public ConfigAttribute RetornaConfig() =>(ConfigAttribute)GetType().GetCustomAttributes(typeof(ConfigAttribute), true)[0];
        private bool userEdita = true;
        #region TELAS

        [Config(TipoAcao = TAcao.Visualizar)]
        public override ActionResult Index()
        {
            userEdita = UsuarioEdita(ClsControler.IdUsuario);
            ViewBag.UsuarioEdita = userEdita;
            ViewBag.NomeController = RetornaConfig().NomeAmigavel;
            return View();
        }


        //[HttpPost]
        [Config(TipoAcao = TAcao.Editar)]
        public override ActionResult Editar(int Id)
        {
            var neg = new NegUsuario();
            var dto = new ClsUsuario();
            var user = RetornaUsuarioLogado();

            if (Id > 0) dto = neg.Filtrar(Id);
            ViewBag.NomeController = RetornaConfig().NomeAmigavel;
            ViewBag.TelaModal = false;
            ViewBag.IdGlobalEmpresa = user.Empresa.Id;
            ViewBag.IdUser = user.Id;
            ViewBag.NomeUser = user.Nome;
            //if (dto.Photo != null)
            //    ViewBag.imgSRC = String.Format("data:image/gif;base64,{0}", dto.Photo.Base64);

            return View(dto);
        }


        [Config(TipoAcao = TAcao.Excluir)]
        public override ActionResult Excluir()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region SALVAR

        [Config(TipoAcao = TAcao.Editar)]
        [HttpPost, ValidateInput(false)]
        public string Gravar(ClsUsuario obj)
        {
            var ret = MontaRetorno();

            try
            {
                obj = JsonConvert.DeserializeObject<ClsUsuario>(Request.Form["obj"]);
                var arq = obj.Foto;

                if (arq.Alterar) RetornaArquivo(ref arq, Request.Files);
                //var caminho = SalvarArquivo(Request.Files, "Seguranca");

                return Salvar(obj);
            }
            catch (ValidationException ex)
            {
                MontaRetornoValidacao(ref ret, ex.TrataErro());
                return JsonConvert.SerializeObject(ret);
            }
            catch (Exception ex)
            {
                MontaRetornoErro(ref ret, ex.TrataErro(), ex.Source);
                return JsonConvert.SerializeObject(ret);
            }
        }

        [Config(TipoAcao = TAcao.Editar)]
        [HttpPost]
        protected override string Salvar<T>(T obj)
        {

            var ret = MontaRetorno();


            try
            {
                var oDTO = (obj as ClsUsuario);


                var neg = new NegUsuario();
                SetaUserNeg(neg);

                var erro = neg.Validacao(oDTO);

                if (erro != "") throw new Exception(erro);

                if (oDTO.Id > 0)
                    neg.Alterar(ref oDTO);
                else
                    neg.Inserir(ref oDTO);
                
                MontaRetornoSucesso(ref ret, "Dados salvos com sucesso", null, true);
                return JsonConvert.SerializeObject(ret);



            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Salvar - " + ex.Source.ToNull().ToString() };

            }
        }

        [Config(TipoAcao = TAcao.Excluir)]
        [HttpPost]
        public string Excluir(int Codigo)
        {
            var ret = MontaRetorno();

            try
            {

                var neg = new NegUsuario();
                SetaUserNeg(neg);

                neg.Excluir(Codigo);

                MontaRetornoSucesso(ref ret, "Registro excluído com Sucesso", null, true);

                return JsonConvert.SerializeObject(ret);
            }
            catch (ValidationException ex)
            {
                MontaRetornoValidacao(ref ret, ex.TrataErro());
                return JsonConvert.SerializeObject(ret);
            }
            catch (Exception ex)
            {
                MontaRetornoErro(ref ret, ex.TrataErro(), ex.Source);
                return JsonConvert.SerializeObject(ret);
            }


        }
        #endregion


        public string RetornaTodos(bool soAtivos = false)=> JsonConvert.SerializeObject(new NegUsuario().ListaTodos(soAtivos));

        public string Consultar(bool soAtivos = false)=> JsonConvert.SerializeObject(new NegUsuario().Consultar(soAtivos));


    }
}