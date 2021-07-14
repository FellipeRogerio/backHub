using Negocio;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classe;
using System.ComponentModel.DataAnnotations;

namespace Api.Cadastro.Usuario
{
    public class UsuarioController : PadraoController
    {
        private readonly NegUsuario neg = new NegUsuario();

        #region INSERT/ALTER/DELETE

        // POST api/<controller>
        [Route("~/api/Usuario")]
        public IHttpActionResult Post([FromBody] dynamic value)
        {
            var ret = MontaRetorno();
            try
            {
                var texto = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                ClsUsuario obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ClsUsuario>(texto);
                obj.TipoAlteracao = Excelencia.API.Classe.TManter.Cadastrar;
                var flag = neg.Inserir(ref obj);


                MontaRetornoSucesso(ref ret, "Usuário Cadastrado com sucesso", null, true);


                return Ok(ret);
            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }


        }

        // PUT api/<controller>/5
        [Route("~/api/Usuario")]
        public IHttpActionResult Put([FromBody] dynamic value)
        {
            var ret = MontaRetorno();
            try
            {
                var texto = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                ClsUsuario obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ClsUsuario>(texto);
                obj.TipoAlteracao = Excelencia.API.Classe.TManter.Alterar;
                var flag = neg.Alterar(ref obj);

                MontaRetornoSucesso(ref ret, "Usuário Alterado com sucesso", null, true);


                return Ok(ret);
            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }

        // DELETE api/<controller>/5
        [Route("~/api/Usuario/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var ret = MontaRetorno();
            try
            {

                var flag = neg.Excluir(id);


                MontaRetornoSucesso(ref ret, "Usuário Excluído com sucesso", null, true);

                return Ok(ret);
            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        #endregion

        #region CONSULTA

        [Route("~/api/Usuario")]
        public IHttpActionResult Get()
        {

            var ret = MontaRetorno();
            try
            {
                var data = neg.Todos(true);
                MontaRetornoSucesso(ref ret, "Consulta Realizada com sucesso", data, data.Rows.Count >0);
                return Ok(ret);

            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }

        [Route("~/api/Usuario/{usuario}/{senha}/{idEmpresa}")]
        public IHttpActionResult Get(string usuario, string senha, long idEmpresa)
        {
            var ret = MontaRetorno();

            try
            {
                //ValidaToken(Token);
                var data = neg.Logar(usuario, senha, idEmpresa);

                MontaRetornoSucesso(ref ret, "Usuário Existe no sistema", data, data.Existe);

                return Ok(ret);

            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }

        [Route("~/api/Usuario/{id}")]
        public IHttpActionResult Get(int id)
        {
            var ret = MontaRetorno();
            try
            {
                //ValidaToken(Token);
                var obj = neg.Filtrar(id);

                if (obj.Id == 0) throw new ValidationException("Usuário não encontrado");


                MontaRetornoSucesso(ref ret, "Usuário encontrado", obj, obj.Existe);

                return Ok(ret);

            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }


        #endregion

        #region RECUPERAÇÃO SENHA

        [HttpGet]
        [Route("~/api/Usuario/validaCodigoSenha/{email}/{codigo}")]
        public IHttpActionResult ValidaCodigoSenha(string email, string codigo)
        {
            var ret = MontaRetorno();
            try
            {
                //ValidaToken(Token);
                var idUser = neg.ValidaCodigo(email, codigo);


                MontaRetornoSucesso(ref ret, "Usuário encontrado", new { existe = true, Id = idUser, }, idUser >0);

                return Ok(ret);

            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        [Route("~/api/Usuario/RecuperaSenha")]
        [HttpPost]
        public IHttpActionResult RecuperaSenha([FromBody] dynamic value)
        {
            var ret = MontaRetorno();
            try
            {

                var caminhoLogo = $"{Utilitario.Props.UrlRaiz}/img/logo.jpg";


                var email = value.email.ToString();
                neg.EnviaEmailRecuperacao(email);



                MontaRetornoSucesso(ref ret, "Email enviado com sucesso", null,true);

                return Ok(ret);
            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }


        }

        [Route("~/api/Usuario/AlteraSenha")]
        [HttpPut]
        public IHttpActionResult AlteraSenha([FromBody] dynamic value)
        {
            var ret = MontaRetorno();
            try
            {


                var Id = Convert.ToInt32(value.Id.ToString() as string);
                var novaSenha = value.novaSenha.ToString();
                neg.AtualizarSenha(Id, novaSenha);

                MontaRetornoSucesso(ref ret, "Senha alterada com sucesso", null, true);


                return Ok(ret);
            }
            catch (ValidationException ex)
            {
                var err = new HttpError("Validação") { ["CodigoRetorno"] = 2, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));

            }
            catch (Exception ex)
            {

                var err = new HttpError("Erro") { ["CodigoRetorno"] = 3, ["DescricaoRetorno"] = ex.Message };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }


        }
        #endregion
    }
}
