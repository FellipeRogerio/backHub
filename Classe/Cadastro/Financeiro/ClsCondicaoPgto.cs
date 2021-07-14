using Excelencia.API.Classe.Cadastros.Financeiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classe.Cadastro.Financeiro
{
    public class ClsCondicaoPgto : BaseCondicaoPgto
    {


        public List<ClsParcelas> Parcelas { get; set; } = new List<ClsParcelas>();

        #region SUB CLASSES

        public class ClsParcelas : BaseCondicaoPgtoParcela
        {
        }

        #endregion
    }


}
