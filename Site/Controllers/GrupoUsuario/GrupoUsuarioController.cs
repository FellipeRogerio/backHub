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
    [Config(Codigo = ClsControler.IdGrupoUsuario, NomeGrupoMenu = ClsMenu.GrupoUsuario, iconeCSS = "fas fa-users fa-fw", ActionPrincipal = "Index", NomeController = "GrupoUsuario", NomeAmigavel = "Grupo de Usuário")]
    public class GrupoUsuarioController : PadraoController
    {
        public ConfigAttribute RetornaConfig() =>(ConfigAttribute)GetType().GetCustomAttributes(typeof(ConfigAttribute), true)[0];
        private bool userEdita = true;

        #region TELAS

        [Config(TipoAcao = TAcao.Visualizar)]
        public override ActionResult Index()
        {
            userEdita = UsuarioEdita(ClsControler.IdGrupoUsuario);
            ViewBag.UsuarioEdita = userEdita;
            ViewBag.NomeController = RetornaConfig().NomeAmigavel;

            return View();
        }


        //[HttpPost]
        [Config(TipoAcao = TAcao.Editar)]
        public override ActionResult Editar(int Id)
        {
            var neg = new NegGrupoUsuario();
            var dto = new ClsGrupoUsuario();
            var user = RetornaUsuarioLogado();

            if (Id > 0) dto = neg.Filtrar(Id);
            ViewBag.TelaModal = true;
            ViewBag.IdGlobalEmpresa = user.Empresa.Id;
            ViewBag.IdUser = user.Id;
            ViewBag.NomeUser = user.Nome;
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
        [HttpPost]
        public string Gravar(ClsGrupoUsuario obj) => Salvar(obj);

        [Config(TipoAcao = TAcao.Editar)]
        [HttpPost]
        public string SalvarPermissoes(ClsGrupoUsuario obj)
        {

            var ret = MontaRetorno();

            try
            {
                var oDTO = obj;


                var neg = new NegGrupoUsuario();
                SetaUserNeg(neg);


                neg.SalvarPermissoes(ref oDTO);


                MontaRetornoSucesso(ref ret, "Dados salvos com sucesso", null, true);

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

        [Config(TipoAcao = TAcao.Editar)]
        [HttpPost]
        protected override string Salvar<T>(T obj)
        {

            var ret = MontaRetorno();


            try
            {
                var oDTO = (obj as ClsGrupoUsuario);


                var neg = new NegGrupoUsuario();
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
                var neg = new NegGrupoUsuario();

                neg.Excluir(Codigo);
                SetaUserNeg(neg);

                MontaRetornoSucesso(ref ret, "Registro excluído com sucesso", null, true);

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


        public string RetornaTodos()
        {
            var dt = new NegGrupoUsuario().ListaTodos();


            return JsonConvert.SerializeObject(dt);

        }

        public string Consultar(bool soAtivos = false)
        {
            var dt = new NegGrupoUsuario().Consultar(soAtivos);


            return JsonConvert.SerializeObject(dt);

        }
    }
}