using Excelencia.API.Classe.Cadastros.Financeiro;
using Classe.Cadastro.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classe.Cadastro.Financeiro
{
    public class ClsConta:BaseConta
    {
        
        public string Descricao { get; set; }
        public ClsEndereco Endereco { get; set; } = new ClsEndereco();
        public List<ClsContato> ListaContatos { get; set; } = new List<ClsContato>();
    }
}
