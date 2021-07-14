using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{

    public class ClsRetorno
    {
        public bool Existe { get; set; }
        public bool Sucesso { get; set; }
        public dynamic data { get; set; }
        public int CodigoRetorno { get; set; }
        public string DescricaoRetorno { get; set; }

    }
}