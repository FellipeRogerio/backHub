const NomeControlador = "Usuario";
// LOAD ---------------------------------------------------------------------------------------------------------------
var tDrop;
$(document).ready(function () {
    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    CarregaCombos();
    formataCombos();
    //CriarTabela();
    CarregaDados();
    AplicaMascara();
    AddEventos();
})


//$(window).load(function () {
//tDrop = Dropzone.instances[0];

//if (tDrop != null) {

//    if (model.Foto.Base64 != "") {
//        var img = "data: image / gif; base64, " + model.Foto.Base64;
//        Biblioteca.Dropzone.CriaImagem(tDrop, "logo.PNG", 709, img);
//        $("#txtTemImagemNova").val("0");
//    }

//}
//});

// LOAD ---------------------------------------------------------------------------------------------------------------

function AplicaMascara() {
    //var btnExcluir = '<button type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="top" title="Delete Image"><i class="icon-trash"></i></button>';
    //Dropzone.options.tDrop = Biblioteca.Dropzone.ConfiguraDropZone("nameLogo", "idLogo", 30000, 10, 1900, 1900, 1, "image/*", btnExcluir, "txtTemImagemNova");



}
function formataCombos() {

}
function AddEventos() {

    $('form[name="formUsuario"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

    $("#btnSair").on('click', function () {
        Voltar();
    });

    $("#cmdSalvar").on('click', function () {

        if ($('form[name="formUsuario"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
            $('#formUsuario').submit();
            return;
        }
        Salvar();

    });

    SetaComboEditavel("#formUsuario");

}

//CARREGA COMBOS ------------------------------------------------------------------------------------------------------
function CarregaCombos() {

    Biblioteca.CarregaCombos.GrupoUsuario("cmbGrupoUsuario");



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
        $("#txtUsuario").val(model.Usuario);
        $("#txtNome").val(model.Nome);
        $("#txtEmail").val(model.Email);
        $("#txtSenha").val(model.Senha);

        if ($("#txtSenha").val != "") {
            $("#txtConfirmarSenha").val(model.Senha);
        } else {
            $("#txtConfirmarSenha").val(model.Csenha);
        }

        Biblioteca.Utils.SetaCheckBox("#chkAtivo", model.Ativo);
        $("#cmbGrupoUsuario").findItem(model.GrupoUsuario.Id);


    } else {

        Biblioteca.Utils.SetaCheckBox("#chkAtivo", true);


    }


}


//CARREGA DADOS -----------------------------------------------------------------------------------------------

//SALVAR ------------------------------------------------------------------------------------------------------


function MontaObjeto() {

    var obj = {};
    obj.GrupoUsuario = {};
    obj.Foto = {};
    obj.ListaCampus = [];

    obj.Id = $("#txtCodigo").val();
    obj.Ativo = $("#chkAtivo").is(':checked');

    obj.Usuario = $("#txtUsuario").val();
    obj.Nome = $("#txtNome").val();
    obj.Email = $("#txtEmail").val();
    obj.Senha = $("#txtSenha").val();
    obj.Csenha = $("#txtConfirmarSenha").val();
    obj.GrupoUsuario.Id = $("#cmbGrupoUsuario").GetId();
    //obj.Foto.Alterar = $("#txtTemImagemNova").val() == 1;



    return obj;

}

function Salvar() {

    try {


        //========================================================
        // Montando o Objeto
        //========================================================
        var obj = MontaObjeto();


        //========================================================
        // Salvando
        //========================================================

        var form = new FormData();
        //form.append('fileUpload', $("#idLogo")[0].files[0]);
        form.append('obj', JSON.stringify(obj));


        $.ajax({
            type: 'POST',
            data: form,
            url: (`${NomeControlador}/Gravar`).RetornaURL(),
            contentType: 'application/json; charset=utf-8',
            cache: false,
            contentType: false,
            processData: false,
            //dataType: 'text',
            success: function (resultado) {
                var ret = jQuery.parseJSON(resultado);
                var msg = ret.DescricaoRetorno;
                var tMsg = "S";
                var call = null; var callPara = null;

                if (!ret.Sucesso) tMsg = "W";
                if (tMsg == "S") {
                    call = Voltar;
                }
                Biblioteca.Alertas.exibir("", msg, tMsg, (tMsg == "S" ? 1000 : null), null, call, callPara);


            },
            error: function (req, status, error) {
                Biblioteca.Alertas.exibirModal("", req.responseText, "E", null, window.location);


            }
        });

    } catch (e) {
        Biblioteca.Alertas.exibirModal("", e.message, "E", null, window.location);

    }
}


//SALVAR ------------------------------------------------------------------------------------------------------

//DATATABLE ------------------------------------------------------------------------------------------------------




//DATATABLE ------------------------------------------------------------------------------------------------------





