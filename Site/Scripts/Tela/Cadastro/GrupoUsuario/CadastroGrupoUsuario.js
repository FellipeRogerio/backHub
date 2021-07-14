const NomeControlador = "GrupoUsuario";
// LOAD---------------------------------------------------------------------------------------------------------------
$(document).ready(function () {
    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    CarregaCombos();
    AddEventos();
    AplicaMascara();
    formataCombos();
    CarregaDados();
});
// LOAD ---------------------------------------------------------------------------------------------------------------

function AplicaMascara() { }
function formataCombos() { }
function Voltar() {
    Biblioteca.Utils.AbrirPagina((`${NomeControlador}/Index`).RetornaURL());
}
function AddEventos() {
    $('form[name="formGrupoUsuario"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();
    $("#btnSair").on("click", function () {
        Voltar();
    });
    $("#cmdSalvar").on("click", function () {
        if ($('form[name="formGrupoUsuario"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
            $("#formGrupoUsuario").submit();
            return;
        }
        Salvar();
    });

    $("#cbxGPermission").on("change", function () {
        if ($("#cbxGPermission")[0].checked == true) {
            $("#lblOPermission").removeClass("checked");
            $("#cbxOPermission").prop("checked", false);
        }
    });
    $("#cbxOPermission").on("change", function () {
        if ($("#cbxOPermission")[0].checked == true) {
            $("#lblGPermission").removeClass("checked");
            $("#cbxGPermission").prop("checked", false);
        }
    });

}
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------
function CarregaCombos() { }
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------

//Funções ------------------------------------------------------------------------------------------------------


//Funções ------------------------------------------------------------------------------------------------------

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function CarregaDados() {
    if ($("#txtCodigo").val() > 0) {
        $("#txtDescricao").val(model.Descricao);

        Biblioteca.Utils.SetaCheckBox("#cbxGPermission", model.PermissaoTotal);
        Biblioteca.Utils.SetaCheckBox("#cbxOPermission", model.SoVisualiza);
    }
}
//CARREGA DADOS -----------------------------------------------------------------------------------------------

//SALVAR ------------------------------------------------------------------------------------------------------
function MontaObjeto() {
    const obj = {};
    obj.Id = $("#txtCodigo").val();
    obj.Descricao = $("#txtDescricao").val();
    obj.PermissaoTotal = $("#cbxGPermission").is(":checked");
    obj.SoVisualiza = $("#cbxOPermission").is(":checked");
    return obj;
}

//SALVAR ------------------------------------------------------------------------------------------------------
function Salvar() {
    try {
        const obj = `{obj : ${JSON.stringify(MontaObjeto())}}`;
        const ret = Biblioteca.Chamada.PostJson((`${NomeControlador}/Gravar`).RetornaURL(), obj);
        if (ret.Sucesso)
            Biblioteca.Notificacao.Sucesso("", ret.DescricaoRetorno, 1000, null, Voltar, null);
        else {
            if (ret.Validacao)
                Biblioteca.Alertas.Warning("", ret.DescricaoRetorno, null, window.BodyAnterior);
            else
                Biblioteca.Alertas.Erro("Error", ret.DescricaoRetorno + ret.Origem.TrataCaminhoErro(), window.BodyAnterior);
        }
    } catch (e) {
        Biblioteca.Alertas.Erro("Error", e.Message, window.BodyAnterior);
    }
}

//SALVAR ------------------------------------------------------------------------------------------------------