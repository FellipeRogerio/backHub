using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Extensions
{
    public static class Extends
    {
        public static string ResolvePath(this string AbsolutePath)
        {
            var FullPath = AbsolutePath.ToLower();
            var SitePath = HostingEnvironment.MapPath("~").ToLower();
            return FullPath.Replace(SitePath, "~/");
        }


        public static void Add(this List<Site.Models.ClsAnexos> lista, string nomeArquivo, string url)
        {
            var item = new Site.Models.ClsAnexos
            {
                NomeArquivo = nomeArquivo,
                URL = url
            };
            lista.Add(item);

        }

        public static string RetornaCaminho(params string[] caminhoPasta)
        {
            var caminho = Path.Combine(caminhoPasta);

            if (!Directory.Exists(caminho)) Directory.CreateDirectory(caminho);

            return caminho;
        }

        public static void Adicionar(this List<Site.Controllers.PadraoController.ClsMenu> lista, Site.Controllers.PadraoController.ClsMenu item)
        {
            lista.RemoveAll(x => x.IdGrupoUsuario == item.IdGrupoUsuario);
            lista.Add(item);

        }
        public static Site.Controllers.PadraoController.ClsMenu Pesquisar(this List<Site.Controllers.PadraoController.ClsMenu> lista, int IdGrupoUsuario)
        {
            return lista.Find(x => x.IdGrupoUsuario == IdGrupoUsuario);
         

        }

    }

    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Adiciona número de versão à um caminho de conteúdo.
        /// </summary>
        /// <param name="urlHelper">O <see cref="UrlHelper"/> da <see cref="ViewPage"/>.</param>
        /// <param name="caminhoRelativo">O caminho do conteúdo. Exemplo: "~/Scripts/jquery.js"</param>
        /// <returns>
        /// O caminho versionado.
        /// Exemplo: "http://process.lumma.com.br/custom/Lummabpm/Scripts/jquery.js?v=636650181987025930
        /// </returns>
        /// <remarks>
        /// Vítor - 21/06/2018
        /// 
        /// Inspirado em: https://madskristensen.net/blog/cache-busting-in-aspnet/
        /// Mas tirei a parte do URL Rewrite, pois precisaria instalar uma extensão do IIS, e ia dar trabalho pra todos.
        /// </remarks>
        public static string Fingerprint(this UrlHelper urlHelper, string caminhoRelativo)
        {
            if (HttpRuntime.Cache[caminhoRelativo] == null)
            {
                // caminho para o arquivo no disco
                string caminhoAbsoluto = HostingEnvironment.MapPath(caminhoRelativo);

                // versão do arquivo
                DateTime dataArquivo = File.GetLastWriteTime(caminhoAbsoluto);
                long versaoArquivo = dataArquivo.Ticks;

                // caminho com a versão
                string caminhoVersionado = urlHelper.Content($"{caminhoRelativo}?v={versaoArquivo}");
                HttpRuntime.Cache.Insert(caminhoRelativo, caminhoVersionado, new CacheDependency(caminhoAbsoluto));
            }

            return HttpRuntime.Cache[caminhoRelativo] as string;
        }
    }
}