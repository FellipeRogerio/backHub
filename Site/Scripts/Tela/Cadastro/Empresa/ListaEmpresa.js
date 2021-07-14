$(document).ready(function () {

    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    Pesquisar();
    Permissao();
})

function MontaUrlEdicao(id) {
    var urlEdicao = ("Empresa/Editar").RetornaURL();
    return urlEdicao + "/" + id;
}


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

        Biblioteca.Utils.AbrirPagina(("Empresa/Editar").RetornaURL(), { id });
    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }
}
//====================================================================================================

//EXCLUSÃO ===========================================================================================

function Remover(id) {

    try {
        
        var dados = "{Codigo : " + id + "}"
        var ret = Biblioteca.Chamada.Delete(("Empresa/Excluir").RetornaURL(), dados);

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
    Biblioteca.Alertas.Question("Confirma a Exclusão", "Deseja excluir Empresa selecionada ?", null, Remover, [id]);

}
//====================================================================================================

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function Pesquisar() {
    try {
        var dt = Biblioteca.Chamada.Get(('Empresa/Consultar').RetornaURL(), { soAtivos: "false" }, false);
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
        { "sTitle": "Razão Social", "mData": "txtRazaoSocial", "bSortable": true },
        { "sTitle": "Nome Fantasia", "mData": "txtNomeFantasia", "bSortable": true },
        { "sTitle": "CNPJ", "mData": "txCNPJ", "bSortable": true },
        //{ "sTitle": "Campus", "mData": "txtCampus", "bSortable": true },
        { "sTitle": "Status", "mData": "Status", "bSortable": true, "sWidth": "50px" },
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
        "pageLength": 10,
        "language": {
            "url": ("Scripts/Componentes/Datatables/language-ptbr.txt").RetornaURL()
        },


    });


}


function AjustaListagem(data) {
    var lista = [];

    for (var i = 0; i < data.length; i++) {
        var item = data[i];

        if (item.Status == true) {
            item.Status = "Ativo";

        } else {
            item.Status = "Inativo";

        }

        item.txtRazaoSocial = '<spam id = "txtRazaoSocial"> ' + item.RazaoSocial;
        item.txtRazaoSocial += '</spam>';
        item.txtNomeFantasia = '<spam id = "txtNomeFantasia"> ' + item.NomeFantasia;
        item.txtNomeFantasia += '</spam>';
        item.txCNPJ = '<spam id = "txCNPJ"> ' + item.CNPJ;
        item.txCNPJ += '</spam>';
        
        lista.push(item);



    }

    return lista;


}


$('#search').on('keyup', function () {
    var tb = $('#tabelaDados').DataTable();
    //tb.draw();
    tb.search(this.value).draw();

});


//DATATABLE ------------------------------------------------------------------------------------------------------
