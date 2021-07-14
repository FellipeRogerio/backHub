using Excelencia.API.Classe;

using Excelencia.API.Controlador;
using Excelencia.API.ControleUsuario;
using Excelencia.API.Negocio;
using Excelencia.Extensions;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Web;

namespace Site.Controllers
{
    [Authorize]
    [PermissaoFiltro]
    public class PadraoController : ExcelenciaController
    {
        #region RETORNO

        public ClsRetorno MontaRetorno()
        {

            var ret = new ClsRetorno();
            return ret;

        }
        public void MontaRetornoSucesso(ref ClsRetorno obj, string mensagem, dynamic data, bool existe = false)
        {

            obj.data = data;
            obj.CodigoRetorno = 1;
            obj.Sucesso = true;
            obj.DescricaoRetorno = mensagem;
            obj.Existe = existe;

        }
        public void MontaRetornoValidacao(ref ClsRetorno obj, string mensagem)
        {

            obj.data = null;
            obj.CodigoRetorno = 2;
            obj.Sucesso = false;
            obj.Validacao = true;
            obj.DescricaoRetorno = mensagem;
            obj.Existe = false;

        }
        public void MontaRetornoErro(ref ClsRetorno obj, string mensagem, string origem)
        {

            obj.data = null;
            obj.CodigoRetorno = 3;
            obj.Sucesso = false;
            obj.Validacao = false;
            obj.DescricaoRetorno = mensagem;
            obj.Origem = origem;
            obj.Existe = false;

        }
        #endregion




        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var acao = filterContext.RouteData.Values["action"].ToString();
            var anonimo = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).ToList();
            var obj = new Models.ModelLayout();

            if (anonimo.Count() == 0)
            {
                if (controller != "Login")
                {
                    var usuario = RetornaUsuarioLogado();
                    if (usuario == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"controller", "Login"}, {"action", "Login"}
                        });
                        return;
                    }

                    if (usuario?.Id == 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"controller", "Login"}, {"action", "Login"}
                        });
                        return;
                    }

                    obj.SetaDados(usuario);
                }
            }

            ViewBag.layoutModel = obj;

        }
        public void PreencheModelLayout(BaseUsuario obj)
        {
            MvcApplication.DadosGlobais.SetaDados(obj);
        }
        public void SetaUserNeg(NegPadrao neg)
        {
            var user = RetornaUsuarioLogado();

            NovoObjNeg(neg, user.Id, user.Nome);
        }
        public static void NovoObjNeg(NegPadrao neg, long IdUser, string NomeUser)
        {
            neg.Audit.CodUsuario = IdUser.ToInteger();
            neg.Audit.NomeUsuario = NomeUser;
            neg.Audit.Computador = System.Net.Dns.GetHostName();
        }
  

        public PadraoController()
        {

        }
        public override ActionResult Index()
        {
            throw new NotImplementedException();

        }
        public override ActionResult Editar(int Id)
        {
            throw new NotImplementedException();

        }
        protected override string Salvar<T>(T obj)
        {
            throw new NotImplementedException();

        }
        public override ActionResult Excluir()
        {
            throw new NotImplementedException();

        }


        private BaseUsuario RetornaUsuario()
        {
            try
            {
                if (User is UsuarioPrincipal)
                {
                    return ((UsuarioPrincipal)User).Dados;
                }

                RedirecionarLogin();
                return new BaseUsuario();
            }
            catch (AuthenticationException ex)
            {
                throw new AuthenticationException(ex.Message);
            }
            catch (Exception )
            {
                return new BaseUsuario();
            };
        }
        public bool UsuarioEdita(int IdController)
        {

            var user = RetornaUsuarioLogado();
            if (user.Id == 0) return false;

            if (user.GrupoUsuario.PermissaoTotal) return true;

            if (user.GrupoUsuario.SoVisualiza) return false;

            var permissao = user.GrupoUsuario.ListaPermissoes.Find(x => x.Id == IdController);
            return permissao.Editar;

        }

        public ActionResult RedirecionarLogin()
        {
            PermissaoFiltro.Deslogar(Request.RequestContext.HttpContext);
            //PreencheModelLayout(null);
            return RedirectToAction("Login", "Login");

        }

        public ActionResult DeslogarUsuario()
        {


            try
            {

                PermissaoFiltro.Deslogar(Request.RequestContext.HttpContext);

                var neg = new Negocio.NegUsuario();
                var user = RetornaUsuarioLogado();

                NovoObjNeg(neg, user.Id, user.Usuario);
                neg.DesLogar();

                return RedirecionarLogin();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public BaseUsuario RetornaUsuarioLogado() => RetornaUsuario();

        public long RetornaCampusUsuarioLogado() => RetornaUsuario().Empresa.Id;

        #region CONTROLE ARQUIVO

        public static string RetornaPastaServidorTemp()
        {
            var caminho = HostingEnvironment.MapPath("~/Temp") ?? throw new Exception("Erro ao gerar caminho do anexo.");

            return caminho;
        }
        public static string RetornaPastaServidorTempAnexos(string nomeUser)
        {
            var pastaTemp = RetornaPastaServidorTemp();

            var caminho = RetornaCaminho(pastaTemp, "Email", "Anexos", nomeUser
                , DateTime.Now.ToString("ddMMyy"));

            return caminho;
        }
        public static string RetornaCaminho(params string[] caminhoPasta)
        {
            var caminho = Path.Combine(caminhoPasta);

            if (!Directory.Exists(caminho)) Directory.CreateDirectory(caminho);

            return caminho;
        }
        private System.Web.HttpPostedFileBase RetornaFile(System.Web.HttpFileCollectionBase Arquivos)
        {
            var oFile = Arquivos;
            if (oFile.Count > 0)
            {
                var arq = oFile[0];

                if (arq.ContentLength > 0)
                    return arq;

            }
            return null;

        }

        public byte[] RetornaArquivoByte(System.Web.HttpFileCollectionBase Arquivos)
        {
            byte[] ret;

            var oFile = RetornaFile(Arquivos);
            if (oFile != null)
            {

                using (var binaryReader = new BinaryReader(oFile.InputStream))
                    ret = binaryReader.ReadBytes(oFile.ContentLength);

                return ret;
            }

            return null;
        }

        public void RetornaArquivo(ref ClsArquivo arquivo, System.Web.HttpFileCollectionBase Arquivos)
        {

            var oFile = RetornaFile(Arquivos);
            if (oFile != null)
            {

                using (var binaryReader = new BinaryReader(oFile.InputStream))
                {
                    arquivo.Arquivo = binaryReader.ReadBytes(oFile.ContentLength);
                    arquivo.NomeArquivo = oFile.FileName;
                    arquivo.TipoArquivo = oFile.ContentType;
                }

            }

        }

        public List<ClsArquivo> RetornaArquivos( System.Web.HttpFileCollectionBase Arquivos, string PastaASalvar)
        {
            var lista = new List<ClsArquivo>();
            
            var uploadPath = PastaASalvar;
            if (Arquivos.Count > 0)
            {
                for (int i = 0; i < Arquivos.Count; i++)
                {
                    var file = Arquivos[i];
                    var item = new ClsArquivo();
                    if (file.ContentLength > 0)
                    {
                        item.NomeArquivo = file.FileName;
                        item.TipoArquivo = file.ContentType;

                        item.Caminho = Path.Combine(@uploadPath, Path.GetFileName(item.NomeArquivo));
                        file.SaveAs(item.Caminho);

                        lista.Add(item);
                    }

                }
               

            }

            return lista;
         

        }


        public List<ClsArquivo> RetornaArquivos(HttpFileCollectionBase arquivos, List<ClsArquivo> anexos)
        {
            var lista = new List<ClsArquivo>();
            for (var i = 0; i < arquivos.Count; i++)
            {
                var arquivo = arquivos[i];
                var anexoController = anexos[i];
                var anexo = new ClsArquivo();
                if (arquivo != null)
                    using (var leitor = new BinaryReader(arquivo.InputStream))
                    {
                        anexo.NomeArquivo = arquivo.FileName;
                        anexo.Arquivo = leitor.ReadBytes(arquivo.ContentLength);
                        anexo.TipoArquivo = arquivo.ContentType;
                        anexo.Id = anexoController.Id;
                        anexo.TipoAlteracao = anexoController.TipoAlteracao;
                    }

                lista.Add(anexo);
            }

            return lista;
        }

        public string SalvarArquivo(System.Web.HttpFileCollectionBase Arquivos, string PastaASalvar)
        {
            try
            {


                var caminho = "";
                var uploadPath = Server.MapPath($"~/{PastaASalvar}");
                var oFile = RetornaFile(Arquivos);

                if (oFile != null)
                {
                    caminho += Path.Combine(@uploadPath, Path.GetFileName(oFile.FileName));
                    oFile.SaveAs(caminho);

                }



                return caminho;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.TrataErro());
            }

        }

        #endregion

        #region MENU

        public static ClsMenu Menu { get; set; }

        public interface IMenu
        {
            long Id { get; set; }
            string Descricao { get; set; }
            string iconeCSS { get; set; }

        }
        public abstract class ClsBaseMenu : IMenu
        {
            public long Id { get; set; }
            public string Descricao { get; set; }
            public string iconeCSS { get; set; }

        }

        public class ClsControler
        {
            public const int IdPxdrao = 0;
            public const int IdGrupoUsuario = 1;
            public const int IdUsuario = 2;
            public const int IdEmpresa = 3;
            public const int IdCliente = 4;
            public const int IdFornecedor = 5;
            public const int IdConta = 6;
            public const int IdCondicaoPgto = 7;
            public const int IdFormaPgto = 8;
            public const int IdTipoDoc = 9;
            public const int IdContaReceber = 10;
            public const int IdContaPagar = 11;
            public const int IdBaixaContaReceber = 12;
            public const int IdBaixaContaPagar = 13;
            public const int IdNCM = 14;
            public const int IdProspect = 15;


        }

        public class ClsMenu
        {
            #region INSERIR CONSTANT DOS GRUPOS DO MENU DA TELA PRINCIPAL

            public const string GrupoPrincipal = "Principal";
            public const string GrupoUsuario = "Usuário";
            public const string GrupoGeral = "Geral";
            public const string GrupoFinanceiro = "Financeiro";
            #endregion


            public long IdGrupoUsuario { get; set; }
            public string TelaAtual { get; set; }
            public List<ClsGrupoMenu> ListaGrupos { get; set; }





            public ClsMenu()
            {
                ListaGrupos = new List<ClsGrupoMenu>();
                ConstroiGrupoMenu();

            }

            private void ConstroiGrupoMenu()
            {
                var oGrupo = new ClsGrupoMenu
                {
                    Id = 1,
                    Descricao = GrupoPrincipal,
                    Principal = true,
                    iconeCSS = "fa fa-flask",

                };
                ConstroiGrupoTela(ref oGrupo);
                ListaGrupos.Add(oGrupo);

                oGrupo = new ClsGrupoMenu
                {
                    Id = 2,
                    Descricao = GrupoGeral,
                    iconeCSS = "ft-settings"
                };
                ConstroiGrupoTela(ref oGrupo);
                ListaGrupos.Add(oGrupo);

                oGrupo = new ClsGrupoMenu
                {
                    Id = 3,
                    Descricao = GrupoUsuario,
                    iconeCSS = "ft-users"
                };
                ConstroiGrupoTela(ref oGrupo);
                ListaGrupos.Add(oGrupo);

                oGrupo = new ClsGrupoMenu
                {
                    Id = 4,
                    Descricao = GrupoFinanceiro,
                    iconeCSS = "ft-users"
                };
                ConstroiGrupoTela(ref oGrupo);
                ListaGrupos.Add(oGrupo);

            }

            private void ConstroiGrupoTela(ref ClsGrupoMenu obj)
            {
                //if (obj.Descricao == GrupoMinisterio)
                //{
                //    var oGrupo = new ClsGrupoTela
                //    {
                //        Id = 1,
                //        Descricao = GrupoEspiritual,
                //        iconeCSS = "glyphicon glyphicon-tags"
                //    };
                //    obj.GrupoTelas.Add(oGrupo);

                //    oGrupo = new ClsGrupoTela
                //    {
                //        Id = 1,
                //        Descricao = GrupoGeral,
                //        iconeCSS = "glyphicon glyphicon-globe"
                //    };
                //    obj.GrupoTelas.Add(oGrupo);
                //}




            }

            public class ClsGrupoMenu : ClsBaseMenu
            {
                public bool Principal { get; set; }
                public List<ClsTelas> Telas { get; set; }
                public List<ClsGrupoTela> GrupoTelas { get; set; }

                public ClsGrupoMenu()
                {

                    Telas = new List<ClsTelas>();
                    GrupoTelas = new List<ClsGrupoTela>();

                }

                public void InsereTela(List<ConfigAttribute> listaTelas)
                {
                    foreach (var item in listaTelas)
                    {

                        var obj = new ClsMenu.ClsTelas
                        {
                            Descricao = item.NomeAmigavel,
                            Controller = item.NomeController,
                            iconeCSS = item.iconeCSS,
                            NomeActionPrincipal = item.ActionPrincipal,
                            Editar = item.Permissao.Editar,
                            Excluir = item.Permissao.Excluir,
                            Visualiza = item.Permissao.Visualiza,
                            TipoAlteracao = item.Permissao.TipoAlteracao,
                            Id = item.Codigo
                        };

                        Telas.Add(obj);


                    }
                }
            }

            public class ClsGrupoTela : ClsBaseMenu
            {
                public List<ClsTelas> Telas { get; set; }


                public ClsGrupoTela()
                {
                    Telas = new List<ClsTelas>();
                }

                public void InsereTela(List<ConfigAttribute> listaTelas)
                {
                    foreach (var item in listaTelas)
                    {


                        var obj = new ClsMenu.ClsTelas
                        {
                            Descricao = item.NomeAmigavel,
                            Controller = item.NomeController,
                            iconeCSS = item.iconeCSS,
                            NomeActionPrincipal = item.ActionPrincipal,
                            Editar = item.Permissao.Editar,
                            Excluir = item.Permissao.Excluir,
                            Visualiza = item.Permissao.Visualiza,
                            TipoAlteracao = item.Permissao.TipoAlteracao,
                            Id = item.Codigo
                        };

                        Telas.Add(obj);


                    }
                }

            }

            public class ClsTelas : BasePermissao, IMenu
            {
                public string Descricao { get; set; }
                public string iconeCSS { get; set; }
                public bool Acessa { get; set; }
            }

        }
        #endregion
    }
}
