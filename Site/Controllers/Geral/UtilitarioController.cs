
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using Excelencia.API.Classe;
using Excelencia.API.Classe.Cadastros.Financeiro;
using Excelencia.Extensions;
using Negocio;
using Negocio.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Site.Controllers
{
    public class UtilitarioController : Controller
    {
        public string Criptografar(string texto) => Utilitario.Cryptografar(texto);

        public string RetornaTodosPais() => JsonConvert.SerializeObject(new NegPais().Todos(true));

        public string RetornaEstadoPorPais(int IdPais)=> JsonConvert.SerializeObject(new NegEstado().TodosPorPais(true, IdPais));

        public string RetornaCidadePorEstado(int IdEstado) => JsonConvert.SerializeObject(new NegCidade().TodosPorEstado(true, IdEstado));

        public DataTable RetornaDTEnum(int NumEnum)
        {
            var DT = new DataTable();
            switch (NumEnum)
            {
                case 1:
                    DT = new TPessoa().ToDataTable();
                    break;

                case 2:
                    DT = new TRegimeTributario().ToDataTable();
                    break;
                case 3:
                    DT = new TTelefone().ToDataTable();
                    break;
                case 4:
                    DT = new TConta().ToDataTable();
                    break;
                case 5:
                    //DT = new ClsContaReceber.TEmitente().ToDataTable();
                    break;
                default:
                    break;
            }
            DT.Columns["KEY"].ColumnName = "Id";
            DT.Columns["DisplayDescription"].ColumnName = "Descricao";
            return DT;
        }

        public string MontaDTEnum(int NumEnum) =>JsonConvert.SerializeObject(RetornaDTEnum(NumEnum));

    }
}