using Classe;
using Dados;
using Excelencia.API.Classe;
using Excelencia.API.ControleUsuario;
using Excelencia.API.Negocio;
using Excelencia.BancoDados;
using Excelencia.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Negocio
{
    public class NegUsuario : NegPadrao
    {
        private readonly DadUsuario oDados = new DadUsuario();
        private ClsUsuario oDTO = new ClsUsuario();
        private ClsBancoDados oBD;

        #region CRUD

        protected override bool Cadastrar<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;
                if (!ValidaDTO(bd)) return false;

                return oDados.Inserir(ref bd, obj);
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Cadastrar - " + ex.Source.ToNull() };
            }
        }

        protected override bool Atualizar<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;
                if (!ValidaDTO(bd)) return false;
                return oDados.Alterar(ref bd, obj);
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Atualizar - " + ex.Source.ToNull() };
            }
        }

        protected override bool Remover<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;
                if (!ValidaDTO(bd)) return false;
                return oDados.Excluir(ref bd, obj);
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Remover - " + ex.Source.ToNull() };
            }
        }

        #endregion

        #region ALTERAÇÃO DE DADOS

        public override bool Inserir<T>(ref T obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Cadastrar(ref oBD, ref obj);


                return true;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Inserir - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public override bool Alterar<T>(ref T obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Atualizar(ref oBD, ref obj);
                return true;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Alterar - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public override bool Excluir(int Codigo)
        {
            try
            {
                var obj = new ClsUsuario { Id = Codigo, TipoAlteracao = TManter.Excluir };
                ClsBD.SetaConexao(ref oBD, Audit, true);
                Remover(ref oBD, ref obj);


                return true;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Excluir - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();
            }
        }

        public override bool Manter<T>(ref ClsBancoDados bd, ref T obj)
        {
            try
            {
                oDTO = obj as ClsUsuario;

                switch (oDTO.TipoAlteracao)
                {
                    case TManter.Cadastrar:
                        return Cadastrar(ref bd, ref oDTO);

                    case TManter.Alterar:
                        return Atualizar(ref bd, ref oDTO);

                    case TManter.Excluir:
                        return Remover(ref bd, ref oDTO);

                    default:
                        return true;
                }
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Manter - " + ex.Source.ToNull() };
            }
        }

        #endregion

        #region VALIDAÇÃO

        /// <summary>
        ///     FUNÇÃO PARA INSERIR OS TRATAMENTOS NAS PROPRIEDADES PARA ENVIAR PARA A CAMADA DE DADOS
        /// </summary>
        private void TrataDTO()
        {
            try
            {
                oDTO.Nome = oDTO.Nome.ToNull().ToString();
                oDTO.Usuario = oDTO.Usuario.ToNull().ToString();
                oDTO.Senha = CryptografarSenha(oDTO.Senha.ToNull().ToString());
                oDTO.Csenha = CryptografarSenha(oDTO.Csenha.ToNull().ToString());
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "TrataDTO - " + ex.Source.ToNull() };
            }
        }


        private string CryptografarSenha(string Senha)
        {
            try
            {
                return Utilitario.Cryptografar(Senha.ToNull().ToString());
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "CryptografarSenha - " + ex.Source.ToNull() };
            }
        }


        /// <summary>
        ///     FUNÇÃO PARA INSERIR AS REGRAS DE NEGÓCIO DO OBJETO
        /// </summary>
        private void RegrasDTO()
        {
            try
            {

            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "RegrasDTO - " + ex.Source.ToNull() };
            }
        }

        public string ValidarLogin(string Usuario, string senha, int idEmpresa)
        {
            try
            {
                if (Usuario.ToNull().ToString().Length == 0)
                    throw new ValidationException("Usuário não informado.");
                if (senha.ToNull().ToString().Length == 0)
                    throw new ValidationException("Senha não informada.");
                //if (idEmpresa == 0)
                //throw new ValidationException("Empresa não informada.");

                return "";
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ValidarLogin - " + ex.Source.ToNull() };
            }
        }

        public string Validacao(ClsUsuario obj)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);
                oDTO = obj;
                ValidaDTO(oBD);

                return "";
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Validacao - " + ex.Source.ToNull() };

            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        private bool ValidaDTO(ClsBancoDados bd)
        {
            try
            {
                if (oDTO.Validado) return oDTO.Validado;
                TrataDTO();

                if (oDTO.TipoAlteracao == TManter.Excluir)
                {
                    if (oDados.ValidarRelacionamento(ref bd, oDTO.Id))
                        throw new ValidationException(
                            "O usuário não pode ser excluído porque está sendo usado em outras partes do sistema.");
                }
                else
                {
                    if (oDTO.Usuario.ToNull().ToString().Length >= 2)
                    {
                        if (oDados.Existe(ref bd, oDTO.Usuario, oDTO.Id))
                            throw new ValidationException("Usuário já existe no sistema.");
                    }
                    else if (oDTO.Nome.ToNull().ToString().Length <= 2)
                        throw new ValidationException("O campo Nome deve conter pelo menos 2 caracteres.");

                    if (oDTO.Email.ToNull().ToString().Length <= 2)
                        throw new ValidationException("O campo E-mail deve conter pelo menos 2 caracteres.");

                    if (!oDTO.Email.IsEmail())
                        throw new ValidationException("Email inválido.");

                    if (oDTO.Senha.ToNull().ToString().Length <= 6)
                        throw new ValidationException("O campo da senha deve conter pelo menos 6 caracteres.");

                    if (oDTO.Senha.ToNull().ToString() != oDTO.Csenha.ToNull().ToString())
                        throw new ValidationException("Senhas diferentes.");
                }

                oDTO.Validado = true;
                return oDTO.Validado;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Valida DTO - " + ex.Source.ToNull() };
            }
        }



        #endregion

        #region FILTRAR

        public ClsUsuario Filtrar(long id)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);

                oDTO = DadUsuario.Filtra(ref oBD, id);
                oDTO.Senha = Utilitario.DeCryptografar(oDTO.Senha);
   

                return oDTO;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Filtrar - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public ClsUsuario FiltrarPorEmail(string email)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);
                FiltrarPorEmail(ref oBD, email);

                return oDTO;
            }
            catch (ValidationException ex)
            {

                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {


                throw new Exception(ex.TrataErro()) { Source = "FiltrarPorEmail - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public ClsUsuario FiltrarPorEmail(ref ClsBancoDados bd, string email)
        {
            try
            {
                oDTO = DadUsuario.FiltrarPorEmail(ref bd, email);
                if (oDTO.Existe)
                    oDTO.Senha = Utilitario.DeCryptografar(oDTO.Senha);

                return oDTO;
            }
            catch (ValidationException ex)
            {

                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {


                throw new Exception(ex.TrataErro()) { Source = "FiltrarPorEmail - " + ex.Source.ToNull() };
            }
          
        }


        public ClsUsuario Logar(string usuario, string senha, long idEmpresa)
        {
            try
            {
                if (usuario == "") throw new ValidationException("Informe o Usuário.");
                if (senha == "") throw new ValidationException("Informe a Senha.");
                //if (idEmpresa.ToString() == "") throw new Exception("Empresa não selecionada.");
                ClsBD.SetaConexao(ref oBD, Audit, true);

                oDTO = oDados.Logar(ref oBD, usuario, Utilitario.Cryptografar(senha), idEmpresa);
                oDTO.Empresa.Id = idEmpresa;

                if (oDTO.Id == 0)
                    throw new ValidationException("Não foi encontrado nenhum usuário com os dados digitados.");

                var dadGrupoUser = new DadGrupoUsuario();
                var oGrupUser = new ClsGrupoUsuario { Id = oDTO.GrupoUsuario.Id };
                dadGrupoUser.Filtra(ref oBD, ref oGrupUser);

                oGrupUser.ListaPermissoes.Add(new BasePermissao { Controller = "Home", Visualiza = true });

                oDTO.GrupoUsuario = oGrupUser;

                oDTO.TokenAcesso = oDTO.Id + "|" + DateTime.Now.ToString("ddMMyyHmmss");

                oBD.Auditoria.CodUsuario = oDTO.Id.ToInteger();
                oBD.RegistraAuditoria(TScript.Outros, "", $"O usuário: {usuario}. Logou no Sistema");
                return oDTO;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Logar - " + ex.Source.ToNull() };
            }
            finally
            {

                if(oBD !=null)oBD.FecharConexao();

            }
        }

        public void DesLogar()
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);


                oBD.RegistraAuditoria(TScript.Outros, "", $"O usuário: {Audit.NomeUsuario}. Saiu do Sistema");
     
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "DesLogar - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        #endregion

        #region FUNÇÃO

        public DataTable Todos(bool Ativos)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);
                var dt = oDados.Todos(ref oBD, Ativos);

                return dt;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Todos - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public List<ClsUsuario> ListaTodos(bool Ativos)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);
                var lista = oDados.ListaTodos(ref oBD, Ativos);

                return lista;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Lista Todos - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public DataTable Consultar(bool soAtivos)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);
                var dt = oDados.Consultar(ref oBD, soAtivos);
                //var dtAux = new DadGrupoUsuario().Todos(ref oBD, Audit, false);
                //DadGeral.InsereColumDescricao(ref dt, "IdGrupoUsuario", "GrupoUsuario", dtAux, "Descricao");

                return dt;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "Consultar - " + ex.Source.ToNull() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        #endregion

        #region RECUPERAÇÃO DE SENHA
        public void ValidaEmail(string email)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);
               if(!oDados.ValidaEmailExiste(ref oBD, email))
                throw new ValidationException("O e-mail digitado não existe!");
            
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.TrataErro()) { Source = "ValidaEmail - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public long ValidaCodigo(string email, string codigo)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                var IdUser = DadUsuario.ValidaCodigoUsuario(ref oBD, email, codigo);

                if (IdUser == 0) throw new ValidationException("Código inválido");

                return IdUser;
            }
            catch (ValidationException ex)
            {

                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "ValidaCodigo - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public void AtualizarSenha(long Id, string novaSenha)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit);



                oDados.AtualizaSenha(ref oBD, Id, CryptografarSenha(novaSenha));
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.TrataErro()) { Source = "AtualizarSenha - " + ex.Source.ToNull().ToString() };

            }
            finally
            {
                oBD.FecharConexao();

            }
        }

        public bool EnviaEmailRecuperacao(string email)
        {
            try
            {
                ClsBD.SetaConexao(ref oBD, Audit, true);
                var usuario = FiltrarPorEmail(ref oBD, email);

                if (!usuario.Existe)
                    throw new ValidationException("Email informado não esta vinculado a nenhum usuário no sistema.");

                var codigoRandomico = new Random().Next(0, 99999).ToString("00000");

                oDados.RelacionaCodigoAoUsuario(ref oBD, usuario, codigoRandomico);


                var objEmail = new ClsEmail
                {
                    Assunto = "Código de Recuperação de Senha",
                    Para = email,
                    Titulo = usuario.Empresa.NomeFantasia,
                    IsHTML = true,
                    CorpoEmail = $"<h1>Olá, {usuario.Nome}</h1>" +
                                 "<h2>Segue o código para recuperação da sua senha:</h2>" +
                                 $"<h2><span style='color: red'><strong>{codigoRandomico}</strong></span></h2>" +
                                 "<p>Insira-o na tela de recuperação de senha e atualize sua senha para poder voltar a acessar o sistema.</p>" +
                                 "<br /><br />" +
                                 $"<p><strong>Departamento de TI - {usuario.Empresa.NomeFantasia }</strong></p>" +
                                 "<p>Esse e-mail foi enviado automaticamente, não sendo necessário responder.</p>"
                };

                var negEmail = new NegEmail();
                negEmail.EnviaEmail(objEmail);

                return true;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.TrataErro());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.TrataErro()) { Source = "EnviaEmailRecuperacao - " + ex.Source.ToNull().ToString() };
            }
            finally
            {
                oBD.FecharConexao();

            }
        }
        #endregion
    }
}