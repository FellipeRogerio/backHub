
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Claims;
using System.Linq;
using Excelencia.Token.JWT;
using System.Configuration;
using Excelencia.Extensions;
using Excelencia.Camada.Integracao;
using Classe;

namespace Negocio
{

    public class ClsPropriedades
    {
        public const string NomeCliente = "tornifuso";
        public string UrlRaiz { get; set; }
        public string ConexaoBD { get; set; }
        public string ConexaoBDImagem { get; set; }
        public string PDFTermoUso { get; set; }
        public ClsTokenConfig TokenConfig { get; set; } = new ClsTokenConfig();

    }
    public class ClsTokenConfig
    {
        public static string Emitente => "05661712000175"; //CNPJ da TECHS
        public TSistema TipoSistema { get; set; }
        public  string TOKENID { get; set; }
        public  int TempoSegundo { get; set; }
        //public string TOKENID { get; set; } = ConfigurationManager.AppSettings["TOKENID"];
        //public int TempoSegundo => ConfigurationManager.AppSettings["DuracaoMinutosTOKEN"].ToInteger() * 60;
        public string GerarTokenAcesso()
        {
            var token = new ClsJwt
            {
                Emitente = Emitente,
                Destinatario = TOKENID,
                TempoSegundo = TempoSegundo,
                IdUsuario = TOKENID,
                Solicitante=
                {
                    TipoSistema= TipoSistema
                },
            };
      

            return token.GeraToken();
        }
        public bool ValidaToken(string Token)
        {

            var token = new ClsJwt
            {
                Emitente = Emitente
            };
            //token.TempoSegundo = TempoSegundo;
            //token.Origem = "0";
            return token.ValidaTOKEN(Token);

        }

    }





}
