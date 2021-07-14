using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Excelencia.API.Controlador;
using Excelencia.API.ControleUsuario;
using Excelencia.Extensions;
using Negocio;
using Web;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using Extensions;
using System.ComponentModel.DataAnnotations;
using Excelencia.API.Classe;

namespace Site.Controllers
{

    [AllowAnonymous]
    [Config(NomeAmigavel = "Login")]
    public class LoginController : PadraoController
    {
        [AllowAnonymous]
        public ActionResult Login(dynamic model, string returnUrl)
        {
            ViewBag.ReturnURl = returnUrl;

            return View(model);
        }


        [AllowAnonymous]
        [HttpPost]
        public string Entrar(string usuario, string senha, int idEmpresa, string returnUrl)
        {

            var ret = MontaRetorno();

            try
            {
                var url = returnUrl;

                var neg = new NegUsuario();
                NovoObjNeg(neg, 0, usuario);

                //  var validar = neg.ValidarLogin(usuario, senha, idCampus);

                // if (validar.Length > 0) throw new Exception(validar);

                var dto = neg.Logar(usuario, senha, idEmpresa);
     


                dto.GrupoUsuario.ListaPermissoes.ForEach(x =>
                    x.Controller = MvcApplication.LGeralPermissao.Find(w => w.Codigo == x.Id)?.NomeController ??
                                   x.Controller);


                PermissaoFiltro.Logar(dto, Response, 1440);


                //limparPastaTemp(dto.Id);

                CriaMenu(dto);

                //ret.IdUser = dto.Id;
                //ret.nomeUser = dto.Usuario;
                //ret.IdEmpresa = dto.Empresa.Id;

                PreencheModelLayout(dto);


                if (!(Url.IsLocalUrl(url) && url.Length > 1 && url.StartsWith("/") && !url.StartsWith("//") && !url.StartsWith("/\\")))
                {
                    url = "Home/Index";
                }
                MontaRetornoSucesso(ref ret, "Usuário logado", new { url }, true);

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

        [AllowAnonymous]
        public ActionResult Negado()
        {
            ViewBag.Title = "Acesso Negado";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Sair()
        {
            try
            {
                return DeslogarUsuario();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro());
            }
        }

        [AllowAnonymous]
        public ActionResult RecuperarSenha() => View();

        [AllowAnonymous]
        public ActionResult RecuperarSenhaCodigo(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [AllowAnonymous]
        public ActionResult RecuperarSenhaNova(string email, string codigo)
        {

            var neg = new NegUsuario();
            var user = neg.FiltrarPorEmail(email);
            ViewBag.Email = email;
            ViewBag.NomeUsuario = user.Nome;
            return View();
        }

        [AllowAnonymous]
        public string EnviarRecuperacaoSenha(string email)
        {
          
            var ret = MontaRetorno();

            try
            {
                var neg = new NegUsuario();
                neg.ValidaEmail(email);
                var user = neg.FiltrarPorEmail(email);
                NovoObjNeg(neg, 0, user.Nome);

                neg.EnviaEmailRecuperacao(email);
           
                MontaRetornoSucesso(ref ret, "Usuário encontrado", new { Email = email, }, true);

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

        [AllowAnonymous]
        public string ValidaCodigoRecuperacao(string email, string codigoVerificacao)
        {
            var ret = MontaRetorno();


            try
            {
                var neg = new NegUsuario();
                var idUser = neg.ValidaCodigo(email, codigoVerificacao);

             
                MontaRetornoSucesso(ref ret, "Usuário encontrado", new { existe = true, Id = idUser, }, idUser > 0);

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

        [AllowAnonymous]
        public string CadastrarNovaSenha(long IdUser, string novaSenha)
        {
            var ret = MontaRetorno();

            try
            {
                var neg = new NegUsuario();
                neg.AtualizarSenha(IdUser, novaSenha);
              
                MontaRetornoSucesso(ref ret, "Senha Atualizada", null);

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

        [AllowAnonymous]
        public void limparPastaTemp(int IdUsuario)
        {
            try
            {
                var temp = new DirectoryInfo(Server.MapPath("~/Relcert"));


                var files = temp.GetFiles().ToList()
                    .FindAll(x => x.Name.ToUpper().Contains(string.Concat("US", IdUsuario, "US")));

                foreach (var item in files)
                {
                    item.Delete();
                }
            }
            catch (Exception ex)
            {
                var erro = ex.TrataErro();
            }
        }

        [AllowAnonymous]
        public string RetornaNomeUsuarioLogado()
        {
            var ret = "";

            try
            {
                ret = RetornaUsuarioLogado().Usuario;
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro());
            }

            


            return ret;
        }

        [AllowAnonymous]
        public long RetornaIdEmpresaUsuarioLogado()
        {
            var ret = (long)0;

            try
            {
                ret = RetornaCampusUsuarioLogado();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro());
            }


            return ret;
        }

        [AllowAnonymous]
        public string RetornaFotoUsuarioLogado()
        {
            var ret = "";

            try
            {
                ret = RetornaUsuarioLogado().Foto.Base64;
                if (ret != "") ret = "data: image / gif; base64, " + ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro());
            }


            return ret;
        }

        public ClsMenu RetornaMenu()
        {
            var user = new BaseUsuario();
            try
            {
                user = ((UsuarioPrincipal)User).Dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro());
            }


            return GeraMenu(user.GrupoUsuario, false);
        }

        private ClsMenu GeraMenu(BaseGrupoUsuario oGrupoUser, bool VerificarAcesso = true)
        {
            var oMenu = new ClsMenu();
            var lTelas = new List<ConfigAttribute>();

            if (VerificarAcesso)
                lTelas = MvcApplication.LGeralPermissao.FindAll(x => PermissaoFiltro.VerificarAcesso(oGrupoUser, x));
            else
            {
                MvcApplication.LGeralPermissao.ForEach(x => lTelas.Add((ConfigAttribute)x.Clone()));
                lTelas.ForEach(x => PermissaoFiltro.SetaPermissao(ref x, oGrupoUser));

                if (oGrupoUser.PermissaoTotal)
                    lTelas.ForEach(x =>
                    {
                        x.Permissao.Editar = true;
                        x.Permissao.Visualiza = false;
                        x.Permissao.TipoAlteracao = TManter.Alterar;
                    });

                if (oGrupoUser.SoVisualiza)
                    lTelas.ForEach(x =>
                    {
                        x.Permissao.Visualiza = true;
                        x.Permissao.Editar = false;
                        x.Permissao.TipoAlteracao = TManter.Alterar;
                    });
            }


            foreach (var item in oMenu.ListaGrupos)
            {
                var lista = lTelas.FindAll(x => x.NomeGrupoMenu == item.Descricao);
                item.InsereTela(lista);

                foreach (var itemM in item.GrupoTelas)
                {
                    lista = lTelas.FindAll(x => x.NomeGrupoMenu == itemM.Descricao);
                    itemM.InsereTela(lista);
                }
            }

            return oMenu;
        }

        private void CriaMenu(BaseUsuario oUser)
        {
            Menu = GeraMenu(oUser.GrupoUsuario);
        }


        [AllowAnonymous]
        public ActionResult MontaMenu(string tela)
        {
            if (Menu == null) Menu = new ClsMenu();
            Menu.TelaAtual = tela;
            return PartialView(Menu);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult MontaTabelaMenu(int Id)
        {
            var obj = new NegGrupoUsuario().Filtrar(Id);
            return PartialView(GeraMenu(obj, false));
        }
    }
}