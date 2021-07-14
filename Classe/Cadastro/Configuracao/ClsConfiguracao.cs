using Excelencia.API.Classe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classe.Configuracao
{
    public class ClsConfiguracao : ClsPadrao
    {
        public ClsSmtp SmtpEmail { get; set; } = new ClsSmtp();
    }
}
