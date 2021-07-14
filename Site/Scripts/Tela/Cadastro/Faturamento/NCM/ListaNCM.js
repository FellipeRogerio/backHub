
let NomeControlador = "NCM";
$(document).ready(function () {

    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    Pesquisar();
    Permissao();
})





//CHAMA TELA DE EDIÇÃO EM MODO DE CADASTRO ===========================================================
function Permissao() {
    if ($("#txtPermissao").val() == false) {
        $("#cmdAdicionar")[0].style.display = 'none';
    }
}

$("#cmdAdicionar").on('click', function () {
    Editar(0);
});

function Editar(id) {
    try {

        Biblioteca.Utils.AbrirPagina((`${NomeControlador}/Editar`).RetornaURL(), { id });
    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }
}
//====================================================================================================

//EXCLUSÃO ===========================================================================================

function Remover(id) {

    try {

        var dados = "{Codigo : " + id + "}"
        var ret = Biblioteca.Chamada.Delete((`${NomeControlador}/Excluir`).RetornaURL(), dados);

        if (ret.Sucesso)
            Biblioteca.Alertas.Sucesso("", ret.DescricaoRetorno, null, window.location, null, null);
        else {
            if (ret.Validacao)
                Biblioteca.Alertas.Warning("", ret.DescricaoRetorno);
            else
                Biblioteca.Alertas.Erro("Error", ret.DescricaoRetorno + ret.Origem.TrataCaminhoErro());
        }


    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }
}

function Excluir(id) {
    Biblioteca.Alertas.Question("Confirma a Exclusão", "Deseja excluir NCM selecionado ?", null, Remover, [id]);
}
//====================================================================================================

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function Pesquisar() {

    try {
        var dt = Biblioteca.Chamada.Get((`${NomeControlador}/Consultar`).RetornaURL(), { soAtivos: "false" }, false);
        CriarTabela(dt);

    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }


}
//CARREGA DADOS -----------------------------------------------------------------------------------------------

//DATATABLE ------------------------------------------------------------------------------------------------------


function CriarTabela(data) {


    var lista = AjustaListagem(data);
    
    var Colunas = [
        { "sTitle": "Código", "mData": "txtCodigo", "bSortable": true, "sWidth": "250px" },
        { "sTitle": "Descrição", "mData": "txtDescricao", "bSortable": true, "sWidth": "100%" },
        { "sTitle": "Status", "mData": "DescStatus", "bSortable": true, "sWidth": "50px" },
        {
            "sTitle": "Ações",
            "data": null,
            render: function (data, type, row) {
                var botoes = '<div>';
                botoes += '<button type="button" class="btn-flat btn-info p-0" onClick="Editar(' + row.Id + ')" title="Editar"><span class="ft-edit-2 font-medium-3 mr-1"></span></button>';
                botoes += '<button type="button" class="btn-flat btn-danger p-0" onClick="Excluir(' + row.Id + ')" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="ft-x font-medium-3 mr-1"></i></button>';

                botoes += '</div>';
                return botoes;

            }
            , "visible": $("#txtPermissao").val()
            , "sWidth": "30px"
            , "className": 'dt-body-right'
        }
    ];



    $('#tabelaDados').DataTable({
        "data": lista,
        "aoColumns": Colunas,
        "responsive": true,
        "bLengthChange": false,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
        "sDom": "<'row'<'col-sm-12 col-md-12'tr>>I<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        "bPaginate": true,
        "pageLength": 30,
        "language": {
            "url": ("Scripts/Componentes/Datatables/language-ptbr.txt").RetornaURL()
        },


    });


}


function AjustaListagem(data) {
    var lista = [];

    for (var i = 0; i < data.length; i++) {
        var item = data[i];

        item.DescStatus = "Ativo";

        if (!item.Status) item.DescStatus = "Inativo";
        
        item.txtDescricao = `<spam id = "txtDescricao"> ${Biblioteca.Utils.ToNull(item.Descricao)}</spam>`;
        item.txtCodigo = `<spam id = "txtCodigo"> ${Biblioteca.Mascaras.Formatar(item.Codigo, Biblioteca.Mascaras.RetornaNCM())}</spam>`;
        lista.push(item);

    }

    return lista;


}


$('#search').on('keyup', function () {
    var tb = $('#tabelaDados').DataTable();
    tb.search(this.value).draw();

});


//DATATABLE ------------------------------------------------------------------------------------------------------
