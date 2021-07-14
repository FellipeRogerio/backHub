// LOAD ---------------------------------------------------------------------------------------------------------------
var tDrop;
var nomeForm = "formConta";
var totColunas = 0;

$(document).ready(function () {
    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    CarregaCombos();
    AddEventos();
    AplicaMascara();
    formataCombos();
    CriarTabela();
    CarregaDados();
})
$(function () {
  


});
$(window).on('load',function () {
  

});

// LOAD ---------------------------------------------------------------------------------------------------------------

function AplicaMascara() {
    Biblioteca.Mascaras.Monetario("#txtSaldoInicial");
    Biblioteca.Mascaras.CNPJCPF("#txtCNPJCPFFavorecido");
    Biblioteca.Mascaras.CEP("#txtCEP");

}
function formataCombos() {

}
function AddEventos() {

    $('form[name="' + nomeForm +'"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

    $("#cmbEstado").on('change', function () {
        Biblioteca.CarregaCombos.CidadePorEstado("cmbCidade", $(this).GetId());
    });

    $("#btnSair").on('click', function () {
        Biblioteca.Utils.AbrirPagina(("Conta/Index").RetornaURL());
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

    Biblioteca.CarregaCombos.EstadoPorPais("cmbEstado","0");
    Biblioteca.CarregaCombos.TipoConta("cmbTConta");


}
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------

//Funções ------------------------------------------------------------------------------------------------------
function Voltar() {

    Biblioteca.Utils.AbrirPagina(("Conta/Index").RetornaURL());
}
//Funções ------------------------------------------------------------------------------------------------------

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function CarregaDados() {

    if ($("#txtCodigo").val() > 0) {
        Biblioteca.Utils.SetaCheckBox("#chkAtivo", model.Ativo);

        $("#txtDescricao").val(model.Descricao);
        $("#txtNumBanco").val(model.NumBanco);
        $("#txtFavorecido").val(model.Favorecido);
        $("#txtCNPJCPFFavorecido").val(model.CNPJCPFFavorecido);
        $("#txtNomeAgencia").val(model.NomeAgencia);
        $("#txtNumAgencia").val(model.NumAgencia);
        $("#txtDigitoAgencia").val(model.DigitoAgencia);
        $("#txtNumConta").val(model.NumConta);
        $("#txtDigitoConta").val(model.DigitoConta);
        $("#cmbTConta").findItem(model.Tipo.oEnum);
        
        $("#txtDataAbertura").val(Biblioteca.Data.FormatarDataDoServidor(model.DataAbertura));
        $("#txtSaldoInicial").val(model.SaldoInicial.FormataDecimal(2));
        $("#txtDataInicioSaldo").val(Biblioteca.Data.FormatarDataDoServidor(model.DataInicioSaldo));
        $("#txtDataEncerramento").val(Biblioteca.Data.FormatarDataDoServidor(model.DataEncerramento));
        $("#txtObs").val(model.Obs);


        $("#txtEndereco").val(model.Endereco.Endereco);
        $("#txtNumero").val(model.Endereco.Numero);
        $("#txtComplemento").val(model.Endereco.Complemento);
        $("#cmbEstado").findItem(model.Endereco.Cidade.Estado.Id);
        $("#cmbEstado").change();
        $("#cmbCidade").findItem(model.Endereco.Cidade.Id);
        $("#txtBairro").val(model.Endereco.Bairro);
        $("#txtCEP").val(model.Endereco.Cep);
        
        CarregaContatos(AjustaListagemContato(model.ListaContatos));


    } else {
        Biblioteca.Utils.SetaCheckBox("#chkAtivo", true);


    }


}

function AtualizaContatos() {
    try {
        var dt = Biblioteca.Chamada.Get(('Conta/RetornaContatos').RetornaURL(), { IdConta: $("#txtCodigo").val().ToInteger() }, false);
        CarregaContatos(AjustaListagemContato(dt));

    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);

    }

}
function CarregaContatos(lista) {
    var table = $('#tbContatos').DataTable();
    table.clear().draw();
    table.clear().rows.add(lista).draw();
}

//CARREGA DADOS -----------------------------------------------------------------------------------------------

//SALVAR ------------------------------------------------------------------------------------------------------


function MontaObjeto() {
    
    var obj = {};
    obj.Tipo = {};
    obj.Endereco = {};
    obj.ListaContatos = [];
    obj.Endereco.Cidade = {};
    obj.Endereco.Cidade.Estado = {};

    obj.Id = $("#txtCodigo").val().ToInteger();
    obj.Ativo = $("#chkAtivo").is(':checked');
    
    obj.Descricao = $("#txtDescricao").val();
    obj.NumBanco = $("#txtNumBanco").val();
    obj.Favorecido = $("#txtFavorecido").val();
    obj.CNPJCPFFavorecido = $("#txtCNPJCPFFavorecido").val();
    obj.NomeAgencia = $("#txtNomeAgencia").val();
    obj.NumAgencia = $("#txtNumAgencia").val();
    obj.DigitoAgencia = $("#txtDigitoAgencia").val();
    obj.NumConta = $("#txtNumConta").val();
    obj.DigitoConta = $("#txtDigitoConta").val();
    obj.Tipo.oEnum = $("#cmbTConta").GetId();
    obj.DataAbertura = $("#txtDataAbertura").val().DataParaServidor();
    obj.SaldoInicial = $("#txtSaldoInicial").val().ToDecimal(2);
    obj.DataInicioSaldo = $("#txtDataInicioSaldo").val().DataParaServidor();
    obj.DataEncerramento = $("#txtDataEncerramento").val().DataParaServidor();
    obj.Obs = $("#txtObs").val();

    obj.Endereco.Endereco = $("#txtEndereco").val();
    obj.Endereco.Numero = $("#txtNumero").val();
    obj.Endereco.Complemento = $("#txtComplemento").val();
    obj.Endereco.Cidade.Estado.Id = $("#cmbEstado").GetId();
    obj.Endereco.Cidade.Id = $("#cmbCidade").GetId();
    obj.Endereco.Bairro = $("#txtBairro").val();
    obj.Endereco.Cep = $("#txtCEP").val();


    if (obj.Id> 0) obj.ListaContatos = Biblioteca.DataTable.Serializar("#tbContatos", "2,3,4,5,6,7", totColunas);

    return obj;

}

function Salvar() {


    try {

        var obj = MontaObjeto();

        var form = new FormData();
        form.append('obj', JSON.stringify(obj));

        var ret = Biblioteca.Chamada.Post(("Conta/Gravar").RetornaURL(), form);
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

//DATATABLE ------------------------------------------------------------------------------------------------------

function CriarTabela() {

    if ($("#txtCodigo").val().ToInteger() == 0) {
        $("#linhaContato").hide();
        return;

    }
    var Colunas = [
        { "sTitle": "Id", "mData": "txtIdContato", "visible": false },
        { "sTitle": "TipoAlteracao", "mData": "txtTipoAlteracao", "visible": false },
        { "sTitle": "Nome", "mData": "txtNome", "bSortable": true},
        { "sTitle": "Email", "mData": "txtEmail", "bSortable": true },
        { "sTitle": "Departamento", "mData": "txtDepartamento", "bSortable": true },
        { "sTitle": "Telefones", "mData": "txtTel", "bSortable": true },
        { "sTitle": "Obs", "mData": "txtObs", "bSortable": true },
        {
            "sTitle": "Actions",
            "data": null,
            render: function (data, type, row, meta) {
                var botoes = '<div>';
                botoes += '<button type="button" class="btn-flat btn-info p-0" onClick="EditarContato(' + meta.row + ')" title="Editar"><span class="ft-edit-2 font-medium-3 mr-1"></span></button>';
                botoes += '<button type="button" class="btn-flat btn-danger p-0" onClick="ApagarContato(' + meta.row + ')" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="ft-x font-medium-3 mr-1"></i></button>';

                botoes += '</div>';
                return botoes;

            }
            , "sWidth": "30px"
        }
    ];

    totColunas = Colunas.length;

    $('#tbContatos').DataTable({
        "aoColumns": Colunas,
        "responsive": true,
        "bLengthChange": false,
        "bFilter": true,
        "dom": 'lrtip',
        "bSort": true,
        "bInfo": false,
        "bPaginate": true,
        "pageLength": 2,
        "language": {
            "url": ("Scripts/Componentes/Datatables/language-ptbr.txt").RetornaURL()
        },

    });


}
function AjustarItemContato(item, index, flagFormatar) {
    if (flagFormatar == null) flagFormatar = false;


    if (flagFormatar) { }

    var lTelefones = "";
    lTelefones = $.map(item.ListaTelefones, function (x) {
        return x.Telefone.FormataTelefone();
    }).join("\n");



    item.txtIdContato = '<input min="1"  type="number" id ="txtIdContato' + index + '" name="Id" value="' + item.Id + '"> ';
    item.txtTipoAlteracao = '<input type="number" id ="txtTipoAlteracao' + index + '" name="TipoAlteracao" value="' + item.TipoAlteracao + '"> ';
    item.txtNome = '<input class="semFormatacao" style="width:100%" type="text" id ="txtNome' + index + '"  name="Nome" value="' + item.Nome + '"> ';
    item.txtEmail = '<input class="semFormatacao" style="width:100%" type="text" id ="txtEmail' + index + '"  name="Email" value="' + item.Email + '"> ';
    item.txtDepartamento = '<input class="semFormatacao" style="width:100%" type="text" id ="txtDepartamento' + index + '"  name="Departamento" value="' + item.Departamento + '"> ';
    //item.txtTel = '<input class="semFormatacao" style="width:100%" type="text" id ="txtTel' + index + '"  name="Telefones" value="' + lTelefones + '"> ';
    //item.txtObs = '<input class="semFormatacao" style="width:100%" type="text" id ="txtObs' + index + '"  name="Obs" value="' + item.Obs + '"> ';

    item.txtTel = '<textarea class="semFormatacaoTextArea" readonly="readonly" rows="3" style="width:100%" type="text" id ="txtTel' + index + '"  name="Telefones">' + lTelefones + '</textarea> ';
    item.txtObs = '<textarea class="semFormatacaoTextArea" readonly="readonly" rows="3" style="width:100%" type="text" id ="txtObs' + index + '"  name="Obs">' + item.Obs + '</textarea> ';

    return item;
}

function AjustaListagemContato(data, flagFormatar) {
    var lista = [];

    for (var i = 0; i < data.length; i++) {
        var item = AjustarItemContato(data[i], i, flagFormatar);
        lista.push(item);
    }

    return lista;
}

function ApagarContato(id) {

    Biblioteca.DataTable.LinhaInvisivel("#tbContatos", id, 1);
}

function MontaContatos() {

    var item = {};
    item.ListaTelefones = [];

    item.Editar = false;
    item.idLinha = -1;
    item.Id = 0;
    item.TipoAlteracao = 0;

    return item;

}

function AbrirTelaContato(obj) {
    try {
        var titulo = "Adicionar Contato";

        if (obj.Editar) titulo = "Editar Contato";
        var modelContato = {};
        modelContato.Contato = {};

        modelContato.IdOrigem = $("#txtCodigo").val().ToInteger();
        modelContato.ControllerOrigem = 2; //Conta
        modelContato.Editar = obj.Editar;
        modelContato.IdLinha = obj.IdLinha;
        modelContato.Contato = obj;

        var form = new FormData();
        form.append('model', JSON.stringify(modelContato));

       

        var frame = "<div id='telaContato' class='modal-conteudo'></div>";
        var modal = new telaModal({
            Id: 'tModal',
            percWidth: 50,
            percHeight: 90,
            minWidth: 360,
            minHeight: 0,
            classModal: 'modal-dialog',
            idFechar: '#btnSairContato',
            Titulo: titulo,
            urlModal: ("Geral/ContatoIndex").RetornaURL(),
            formUrlModal: form,
            divModal: frame,
            idModal: 'telaContato',
            sairEsc: false,
            sairClick: false,
            refreshAposFechar: false,
            elemBlock: 'blocktModal'
        });
        modal.abrirModal();

    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);

    }
}

function EditarContato(id) {

    var obj = $('#tbContatos').DataTable().data()[id];
    obj.txtIdContato = null;
    obj.txtTipoAlteracao = null;
    obj.txtNome = null;
    obj.txtEmail = null;
    obj.txtDepartamento = null;
    obj.txtTel = null;
    obj.txtObs = null;

    obj.IdLinha = id;
    obj.Editar = true;
    AbrirTelaContato(obj);
}

$("#cmdAddContato").on('click', function () {
    AbrirTelaContato(MontaContatos());
});

//DATATABLE ------------------------------------------------------------------------------------------------------






