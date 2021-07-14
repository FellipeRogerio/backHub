using Excelencia.API.Classe;
using Excelencia.Camada.Integracao;
using Excelencia.API.Classe.Cadastros.Geral;

namespace Classe
{
        #region BASE

        public class ClsBase : ClsPadrao
        {
            public new long Id { get; set; }
            public string IP { get; set; }
            public TSistema Origem { get; set; }

        }
        public class ClsBaseIntegracao
        {
            public string IP { get; set; }
            public TSistema Origem { get; set; }

        }

        #endregion

        #region GEOGRÁFICO

        public class ClsEndereco : BaseEndereco
        {
            public ClsCidade Cidade { get; set; } = new ClsCidade();
        }

        public class ClsPais : BasePais
        {
        }


        public class ClsEstado : BaseEstado
        {
        public ClsPais Pais { get; set; } = new ClsPais();


    }


    public class ClsCidade : BaseCidade
        {

            public ClsEstado Estado { get; set; } = new ClsEstado();
        }


        #endregion

}
