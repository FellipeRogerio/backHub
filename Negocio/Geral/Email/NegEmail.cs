using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Excelencia.Extensions;
using Classe;
using Negocio.Configuracao;
using System.ComponentModel.DataAnnotations;
using Classe.Configuracao;

namespace Negocio
{
    public class NegEmail
    {
        private ClsConfiguracao oDTO = new ClsConfiguracao();


        #region REGRAS

        private void RegraConfiguracaoSMTP()
        {
            try
            {
                if (!oDTO.SmtpEmail.Existe) throw new ValidationException("Dados do SMTP não foram informados.");
                if (!oDTO.SmtpEmail.SmtpUsuario.IsEmail()) throw new ValidationException("Email Usuário do SMTP inválido.");
                if (oDTO.SmtpEmail.SmtpSenha.Length == 0) throw new ValidationException("Senha Usuário do SMTP não informada.");
                if (!oDTO.SmtpEmail.Email.IsEmail()) throw new ValidationException("Email Remetente inválido.");
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "RegraConfiguracaoSMTP - " + ex.Source.ToNull().ToString() };

            }
        }
        private void RegraEmail(ClsEmail email)
        {
            try
            {
                var lista = email.Para?.Split(';').Where(x => x != "").ToList();
                email.CC?.Split(';').Where(x => x != "").ToList().ForEach(x => lista.Add(x));
                email.CCo?.Split(';').Where(x => x != "").ToList().ForEach(x => lista.Add(x));
   


                if (lista.Exists(x => !x.IsEmail())) throw new ValidationException($"Email(s) inválido(s): \n {string.Join(", ", lista.FindAll(x => !x.IsEmail()))}");
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "RegraEmail - " + ex.Source.ToNull().ToString() };

            }

        }
        #endregion

        #region VALIDAÇÃO

        private void ValidarEnvioEmail(ClsEmail email)
        {
            try
            {
                RegraConfiguracaoSMTP();
                RegraEmail(email);
            }

            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ValidarEnvioEmail - " + ex.Source.ToNull().ToString() };

            }

        }
        #endregion

        public bool EnviarEmail(ClsEmail email)
        {

            try
            {
                EnviaEmail(email);

                return true;

            }

            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "EnviarEmail - " + ex.Source.ToNull().ToString() };

            }



        }

        public void EnviaEmail(ClsEmail email)
        {
            try
            {
                email.IsHTML = true;
                oDTO = new NegConfiguracao().Filtrar(1);
                ValidarEnvioEmail(email);


                using (var cli = MontaCredenciais(oDTO.SmtpEmail))
                {
                    using (var msg = MontaMensagem(email, oDTO.SmtpEmail))
                    {

                        cli.Send(msg);
                        msg.Dispose();
                        cli.Dispose();
                    }
                }


            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "EnviaEmail - " + ex.Source.ToNull().ToString() };
            }

        }

        private MailMessage MontaMensagem(ClsEmail email, ClsSmtp config)
        {
            try
            {
                var msg = new MailMessage
                { IsBodyHtml = email.IsHTML, Body = email.CorpoEmail, Subject = email.Assunto, From = new MailAddress(config.Email, email.Titulo) };

                email.Para?.Split(';').Where(x => x != "").ToList().ForEach(x => msg.To.Add(x));
                email.CC?.Split(';').Where(x => x != "").ToList().ForEach(x => msg.CC.Add(x));
                email.CCo?.Split(';').Where(x => x != "").ToList().ForEach(x => msg.Bcc.Add(x));


                return msg;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }

            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "MontaMensagem - " + ex.Source.ToNull().ToString() };
            }
        }
        private SmtpClient MontaCredenciais(ClsSmtp config)
        {
            try
            {
                var cli = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(config.SmtpUsuario.Trim(), config.SmtpSenha.Trim()),
                    Host = config.SmtpServidor.Trim(),
                    Port = config.SmtpPorta.ToInteger(),
                    EnableSsl = config.SSLAtivo
                };

                return cli;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "MontaCredenciais - " + ex.Source.ToNull().ToString() };
            }
        }

    }
}
