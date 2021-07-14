using Excelencia.API.Classe.Cadastros.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classe.Cadastro.Geral
{
    public class ClsContato : BaseContato
    {

        #region CONSTRUTORES

        public ClsContato()
        {
            ListaTelefones = new List<ClsTelefone>();
        }

        #endregion

        public List<ClsTelefone> ListaTelefones { get; set; }

        #region SUB CLASSES

        public class ClsTelefone : BaseContatoTel
        {
        }

        public class ClsRedeSocial : BaseContatoRedeSocial
        {
        }

        #endregion
    }

    public class ModelContato : Modelo
    {
        public ClsContato Contato { get; set; } = new ClsContato();

    }
    public class ModelTelefone : Modelo
    {
        public ClsContato.ClsTelefone Telefone { get; set; } = new ClsContato.ClsTelefone();

    }
}
