using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // NOME DO BUNDLE NÃO PODE SER O MESMO NOME DE UMA PASTA
            bundles.UseCdn = true;



            //.Include("~/Content/Fontes/css/feather.css", new CssRewriteUrlTransform())

            bundles.Add(new StyleBundle("~/Content/Fontes/feather/css")
                .Include("~/Content/Fontes/feather/css/feather.css", new CssRewriteUrlTransform())
               );

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Fontes/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())

                .Include(
                "~/Content/Utilitario.css",
                "~/Content/bootstrap/bootstrap.css"
                )

                );




            bundles.Add(new StyleBundle("~/Content/temaBaseCSS")
                .Include(
            "~/Content/Tema/app.css"
        ));

            bundles.Add(new StyleBundle("~/Content/ComponentesCSS")
                .Include(
            "~/Content/Componentes/scrollbar/perfect-scrollbar.css",
            "~/Content/Componentes/alertas/toastr.css",
            "~/Content/Componentes/alertas/sweetalert2.min.css",
            "~/Content/Componentes/Combos/select2.css",
            "~/Content/Componentes/Datepicker/bootstrap-datepicker3.css"
        ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/basico/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/basico/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/basico/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/temaBaseJS").Include(
                "~/Scripts/Componentes/scrollbar/perfect-scrollbar.min.js",
                "~/Scripts/Componentes/screenfull/screenfull.min.js",
                "~/Scripts/Tema/app-sidebar.js",
                "~/Scripts/Tema/customizer.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/Componentes").Include(
                "~/Scripts/Componentes/DateTime/moment-with-locales.js",
                "~/Scripts/Componentes/alertas/toastr.min.js",
                "~/Scripts/Componentes/alertas/sweetalert2.min.js",
                "~/Scripts/Componentes/Combo/select2.min.js",
                "~/Scripts/Componentes/Mask/inputmask/inputmask.js",
                "~/Scripts/Componentes/Mask/inputmask/jquery.inputmask.js",
                "~/Scripts/Componentes/Mask/inputmask/inputmask.numeric.extensions.js",
                "~/Scripts/Componentes/DatePicker/bootstrap-datepicker.js",
                "~/Scripts/Componentes/Validation/jqBootstrapValidation.js",
                "~/Scripts/Componentes/TextArea/autosize.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/Utils").Include(
                         "~/Scripts/Tela/Geral/Extensao.js"
                         , "~/Scripts/Tela/Geral/Utilitario.js"
                         , "~/Scripts/Tela/Geral/Componentes.js"
                         ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
