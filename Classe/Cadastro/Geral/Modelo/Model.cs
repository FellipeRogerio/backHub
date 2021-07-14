using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classe.Cadastro.Geral
{
    public abstract class Modelo
    {
        public TCadastro ControllerOrigem { get; set; }
        public int IdOrigem { get; set; }
        public int IdLinha { get; set; }
        public bool Editar { get; set; }
    }
}
