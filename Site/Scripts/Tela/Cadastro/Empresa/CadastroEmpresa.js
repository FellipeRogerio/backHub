// LOAD ---------------------------------------------------------------------------------------------------------------
var tDrop;
var nomeForm = "formEmpresa";

$(document).ready(function () {
    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    CarregaCombos();
    AddEventos();
    AplicaMascara();
    formataCombos();
    //CriarTabela();
    CarregaDados();
})
$(function () {
    var btnExcluir = '<button type="button" class="btn-flat btn-danger p-0" data-toggle="tooltip" data-placement="top" title="Remover Imagem"><i class="ft-x font-medium-3 mr-1"></i></button>';
    var option = Biblioteca.Dropzone.ConfiguraDropZone("nameLogo", "idLogo", 30000, 10, 1900, 1900, 1, "image/*", btnExcluir, "txtTemImagemNova");

    tDrop = new Dropzone("#tDrop", option);

    if (tDrop != null) {

        if (model.Logo.Base64 != "") {
            var img = "data: image / gif; base64, " + model.Logo.Base64;
            Biblioteca.Dropzone.CriaImagem(tDrop, "logo.PNG", 709, img);
            $("#txtTemImagemNova").val("0");
        }

    }


});
$(window).on('load',function () {
  

});

// LOAD ---------------------------------------------------------------------------------------------------------------

function AplicaMascara() {
    Biblioteca.Mascaras.CEP("#txtCEP");
    Biblioteca.Mascaras.CNPJ("#txtCNPJ");

}
function formataCombos() {

}
function AddEventos() {

    $('form[name="' + nomeForm +'"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

    $("#cmbEstado").on('change', function () {
        Biblioteca.CarregaCombos.CidadePorEstado("cmbCidade", $(this).GetId());
    });

    $("#btnSair").on('click', function () {
        Biblioteca.Utils.AbrirPagina(("Empresa/Index").RetornaURL());
    });

    $("#cmdSalvar").on('click', function () {

        if ($('form[name="' + nomeForm +'"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
            $('#' + nomeForm).submit();
            return;
        }
        Salvar();
       
    });
    SetaComboEditavel("#" + nomeForm);

}

//CARREGA COMBOS ------------------------------------------------------------------------------------------------------
function CarregaCombos() {

    Biblioteca.CarregaCombos.EstadoPorPais("cmbEstado","0");
    Biblioteca.CarregaCombos.RegimeTributario("cmbRegimeTrib");


}
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------

//Funções ------------------------------------------------------------------------------------------------------
function Voltar() {

    Biblioteca.Utils.AbrirPagina(("Empresa/Index").RetornaURL());
}
//Funções ------------------------------------------------------------------------------------------------------

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function CarregaDados() {

    if ($("#txtCodigo").val() > 0) {
        Biblioteca.Utils.SetaCheckBox("#chkAtivo", model.Ativo);
        Biblioteca.Utils.SetaCheckBox("#chkMatriz", model.Matriz);

        $("#txtRazaoSocial").val(model.RazaoSocial);
        $("#txtNomeFantasia").val(model.NomeFantasia);
        $("#txtCNPJ").val(model.CNPJ);
        $("#txtIE").val(model.IE);
        $("#txtIM").val(model.IM);
        $("#txtCNAE").val(model.CNAE);
        $("#txtEndereco").val(model.Endereco.Endereco);
        $("#txtNumero").val(model.Endereco.Numero);
        $("#txtComplemento").val(model.Endereco.Complemento);
        $("#cmbEstado").findItem(model.Endereco.Cidade.Estado.Id);
        $("#cmbEstado").change();
        $("#cmbCidade").findItem(model.Endereco.Cidade.Id);
        $("#txtBairro").val(model.Endereco.Bairro);
        $("#txtCEP").val(model.Endereco.Cep);
        $("#txtSite").val(model.Site);
        $("#cmbRegimeTrib").findItem(model.RegimeTributario.oEnum);


        
        //CarregaCampus(AjustaListagem(model.ListaCampus));


    } else {
        //$("#chkAtivo").prop('checked', true);
        Biblioteca.Utils.SetaCheckBox("#chkAtivo", true);


    }


}

function CarregaEmpresas(lista) {
    var table = $('#tabelaEmpresas').DataTable();
    table.clear().draw();
    table.clear().rows.add(lista).draw();

}

//CARREGA DADOS -----------------------------------------------------------------------------------------------

//SALVAR ------------------------------------------------------------------------------------------------------


function MontaObjeto() {

    var obj = {};
    obj.Endereco = {};
    obj.Endereco.Cidade = {};
    obj.Endereco.Cidade.Estado = {};
    obj.Logo = {};
    obj.RegimeTributario = {};

    obj.Id = $("#txtCodigo").val();
    obj.Ativo = $("#chkAtivo").is(':checked');

    obj.Matriz = $("#chkMatriz").is(':checked');
    obj.RazaoSocial = $("#txtRazaoSocial").val();
    obj.NomeFantasia = $("#txtNomeFantasia").val();
    obj.CNPJ = $("#txtCNPJ").val();
    obj.IE = $("#txtIE").val();
    obj.IM = $("#txtIM").val();
    obj.CNAE = $("#txtCNAE").val();
    obj.Endereco.Endereco = $("#txtEndereco").val();
    obj.Endereco.Numero = $("#txtNumero").val();
    obj.Endereco.Complemento = $("#txtComplemento").val();
    obj.Endereco.Cidade.Estado.Id = $("#cmbEstado").GetId();
    obj.Endereco.Cidade.Id = $("#cmbCidade").GetId();
    obj.Endereco.Bairro = $("#txtBairro").val();
    obj.Endereco.Cep = $("#txtCEP").val();
    obj.Site = $("#txtSite").val();
    obj.RegimeTributario.oEnum = $("#cmbRegimeTrib").GetId();

    obj.Logo.Alterar = $("#txtTemImagemNova").val() == 1;

    //var totColunas = 4;

    //obj.ListaCampus = Biblioteca.DataTable.Serializar("#tabelaCampus", "2,3", totColunas);

    return obj;

}

function Salvar() {

    try {

        var obj = MontaObjeto();
        
        var form = new FormData();
        form.append('fileUpload', $("#idLogo")[0].files[0]);
        form.append('obj', JSON.stringify(obj));

        var ret = Biblioteca.Chamada.Post(("Empresa/Gravar").RetornaURL(), form);
        if (ret.Sucesso)
            Biblioteca.Notificacao.Sucesso("", ret.DescricaoRetorno, 1000, null, Voltar, null);
        else {
            if (ret.Validacao)
                Biblioteca.Alertas.Warning("", ret.DescricaoRetorno);
            else
                Biblioteca.Alertas.Erro("Error", ret.DescricaoRetorno + ret.Origem.TrataCaminhoErro());
        }


    } catch (e) {
        Biblioteca.Alertas.Erro("Error", e.Message + ret.Origem.TrataCaminhoErro());
    }
}


//SALVAR ------------------------------------------------------------------------------------------------------



