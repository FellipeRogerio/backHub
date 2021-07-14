using System.Collections.Generic;
using Classe.Cadastro.Geral;
using Excelencia.API.Classe.Cadastros.Pessoa;

namespace Classe.Cadastro.Adm
{
    public class ClsCliente : BasePessoa
    {

        public ClsEndereco Endereco { get; set; } = new ClsEndereco();
        public List<ClsContato> ListaContatos { get; set; } = new List<ClsContato>();
    }
}
