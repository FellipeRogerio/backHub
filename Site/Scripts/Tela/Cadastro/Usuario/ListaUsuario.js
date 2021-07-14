const NomeControlador = "Usuario";

$(document).ready(function () {

    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    Pesquisar();
    Permissao();
})

function MontaUrlEdicao(id) {
    var urlEdicao = (`${NomeControlador}/Editar`).RetornaURL();
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

        Biblioteca.Utils.AbrirPagina((`${NomeControlador}/Editar`).RetornaURL(), { id });
        return;
        var titulo = "Add Usuario";

        if (id > 0) titulo = "Edit Usuario";

        //var cab = "<label id='lblTituloModal' class='cabecalho-modal'>" + titulo+"</label>";
        //var btn = "<button id='btnFecharModal' class='btnFechar btnCloseM'>&times;</button>";

        var frame = "<iframe id='frame' class='modal-conteudo' src='" + MontaUrlEdicao(id) + "' > </iframe >";

        //$('#dModal').html(cab + btn + frame);

        //iniciaModal('iniciarModal');

        var modal = new telaModal({
            Id: 'tModal',
            Width: 50,
            Height: 87,
            classModal: 'modal-dialog',
            idFechar: '#btnFecharModal',
            Titulo: titulo,
            htmlCorpo: frame,
            sairEsc: false,
            sairClick: false,
            refreshAposFechar: true,
        });
        modal.abrirModal();


    } catch (e) {
        Biblioteca.Alertas.exibirModal("", e.message, "E");
    }
}
//====================================================================================================

//EXCLUSÃO ===========================================================================================

function Remover(id) {
    try {
        const dados = `{Codigo : ${id}}`;
        const ret = Biblioteca.Chamada.Delete((`${NomeControlador}/Excluir`).RetornaURL(), dados);
        if (ret.Sucesso) {
            Biblioteca.Alertas.Sucesso("", ret.DescricaoRetorno, null, window.location, null, null);
        } else if (ret.Validacao) {
            Biblioteca.Alertas.Warning("", ret.DescricaoRetorno + ret.Origem);
        } else {
            Biblioteca.Alertas.Erro("", ret.DescricaoRetorno + ret.Origem);
        }
    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }
}

function Excluir(id) {
    Biblioteca.Alertas.Question("Confirma a Exclusão", "Deseja excluir o usuário selecionado?", null, Remover, [id]);

}
//====================================================================================================

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function Pesquisar() {


    $.ajax({
        type: "GET",
        data: { soAtivos: "false" },
        url: (`${NomeControlador}/Consultar`).RetornaURL(),

        async: true,
        beforeSend: function () {
            //Biblioteca.MSG.showMensagem("Buscando", mensagem, "L");
        },
        success: function (ret) {
            ret = JSON.parse(ret);
            CriarTabela(ret);

        },
        error: function (req, status, error) {
            Biblioteca.Alertas.exibirModal("", req.responseText, "E");
        }
    }).responseText;


}
//CARREGA DADOS -----------------------------------------------------------------------------------------------

//DATATABLE ------------------------------------------------------------------------------------------------------


function CriarTabela(data) {


    var lista = AjustaListagem(data);
    var Colunas = [
        { "sTitle": "Usuário", "mData": "txtUsuario", "bSortable": true },
        { "sTitle": "Nome", "mData": "txtNome", "bSortable": true },
        { "sTitle": "Grupo Usuário", "mData": "txtGrupoUsuario", "bSortable": true },
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

        // console.log(item);
        item.txtUsuario = '<spam id = "txtUsuario"> ' + item.Usuario;
        item.txtUsuario += '</spam>';
        item.txtNome = '<spam id = "txtNome"> ' + item.NomeUsuario;
        item.txtNome += '</spam>';
        item.txtGrupoUsuario = '<spam id = "txtGrupoUsuario"> ' + item.GrupoUsuario;
        item.txtGrupoUsuario += '</spam>';
        item.txtEmail = '<spam id = "txtEmail"> ' + item.Email;
        item.txtEmail += '</spam>';


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
