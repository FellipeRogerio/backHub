using Negocio;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classe;
using System.ComponentModel.DataAnnotations;

namespace Api.Cadastro.Usuario
{
    public class GrupoUsuarioController : PadraoController
    {
        private readonly NegGrupoUsuario neg = new NegGrupoUsuario();

        #region INSERT/ALTER/DELETE

        // POST api/<controller>
        [Route("~/api/GrupoUsuario")]
        public IHttpActionResult Post([FromBody] dynamic value)
        {
            var ret = MontaRetorno();
            try
            {
                var texto = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                ClsGrupoUsuario obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ClsGrupoUsuario>(texto);
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
        [Route("~/api/GrupoUsuario")]
        public IHttpActionResult Put([FromBody] dynamic value)
        {
            var ret = MontaRetorno();
            try
            {
                var texto = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                ClsGrupoUsuario obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ClsGrupoUsuario>(texto);
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
        [Route("~/api/GrupoUsuario/{id}")]
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

        [Route("~/api/GrupoUsuario")]
        public IHttpActionResult Get()
        {

            var ret = MontaRetorno();
            try
            {

                var data = neg.Todos();
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
        [Route("~/api/GrupoUsuario/{id}")]
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

        
    }
}
