using Classe.Cadastro.Geral;
using Excelencia.API.Classe;
using Excelencia.API.Classe.Cadastros.Pessoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classe.Cadastro.Adm
{
    public class ClsEmpresa : BasePessoa
    {
        #region CONSTRUTORES

        public ClsEmpresa()
        {
            Logo = new ClsArquivo();
            Endereco = new ClsEndereco();
            ListaTelefones = new List<ClsContato.ClsTelefone>();
            ListaRedeSocial = new List<ClsContato.ClsRedeSocial>();
        }

        #endregion
        public BaseEnum<TRegimeTributario> RegimeTributario { get; set; } = new BaseEnum<TRegimeTributario>();
        public bool Matriz { get; set; }
        public string CNAE { get; set; }
        public string Site { get; set; }
        public ClsArquivo Logo { get; set; }
        public ClsEndereco Endereco { get; set; }
        public List<ClsContato.ClsTelefone> ListaTelefones { get; set; }
        public List<ClsContato.ClsRedeSocial> ListaRedeSocial { get; set; }
    }
}
