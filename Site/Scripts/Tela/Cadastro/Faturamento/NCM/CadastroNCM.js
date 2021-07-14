// LOAD ---------------------------------------------------------------------------------------------------------------
var tDrop;
var nomeForm = "formNCM";
let NomeControlador = "NCM";
var totColunas = 0;

$(document).ready(function () {
    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    CarregaCombos();
    AddEventos();
    AplicaMascara();
    formataCombos();
    CarregaDados();
})
$(function () {
  


});
$(window).on('load',function () {
  

});

// LOAD ---------------------------------------------------------------------------------------------------------------

function AplicaMascara() {
    Biblioteca.Mascaras.NCM(`#txtCodigoNCM`)
}
function formataCombos() {

}
function AddEventos() {

    $('form[name="' + nomeForm +'"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();
    

    $("#btnSair").on('click', function () {
        Biblioteca.Utils.AbrirPagina((`${NomeControlador}/Index`).RetornaURL());
    });

    $("#cmdSalvar").on('click', function () {

        if ($('form[name="' + nomeForm +'"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
            $('#' + nomeForm ).submit();
            return;
        }
        Salvar();
       
    });

    SetaComboEditavel("#" + nomeForm);

}

//CARREGA COMBOS ------------------------------------------------------------------------------------------------------
function CarregaCombos() {
    
}
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------

//Funções ------------------------------------------------------------------------------------------------------
function Voltar() {

    Biblioteca.Utils.AbrirPagina((`${NomeControlador}/Index`).RetornaURL());
}
//Funções ------------------------------------------------------------------------------------------------------

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function CarregaDados() {

    if ($("#txtCodigo").val() > 0) {
        Biblioteca.Utils.SetaCheckBox("#chkAtivo", model.Ativo);

        $("#txtDescricao").val(model.Descricao);
        $("#txtCodigoNCM").val(model.Codigo);


    } else {
        Biblioteca.Utils.SetaCheckBox("#chkAtivo", true);


    }


}



//CARREGA DADOS -----------------------------------------------------------------------------------------------

//SALVAR ------------------------------------------------------------------------------------------------------


function MontaObjeto() {
    
    var obj = {};

    obj.Id = $("#txtCodigo").val().ToInteger();
    obj.Ativo = $("#chkAtivo").is(':checked');

    obj.Descricao = $("#txtDescricao").val();
    obj.Codigo = $("#txtCodigoNCM").val();

    return obj;

}

function Salvar() {


    try {

        var obj = MontaObjeto();

        var form = new FormData();
        form.append('obj', JSON.stringify(obj));

        var ret = Biblioteca.Chamada.Post((`${NomeControlador}/Gravar`).RetornaURL(), form);
        if (ret.Sucesso)
            Biblioteca.Notificacao.Sucesso("", ret.DescricaoRetorno, 1000, null, Voltar, null);
        else {
            if (ret.Validacao)
                Biblioteca.Alertas.Warning("", ret.DescricaoRetorno);
            else
                Biblioteca.Alertas.Erro("Erro", ret.DescricaoRetorno + ret.Origem.TrataCaminhoErro());
        }


    } catch (e) {
        Biblioteca.Alertas.Erro("Erro", e.message);
    }

}


//SALVAR ------------------------------------------------------------------------------------------------------





