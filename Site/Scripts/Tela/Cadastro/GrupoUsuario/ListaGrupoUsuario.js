const NomeControlador = "GrupoUsuario";

$(document).ready(function () {

    Biblioteca.ControleCampos.HabilitarTodosCampos(true);
    ControlarExibicao(false);
    Pesquisar();
    Permissao();
});

function MontaUrlEdicao(id) {
    const urlEdicao = (`${NomeControlador}/Editar`).RetornaURL();
    return urlEdicao + "/" + id;
}

function MontaUrlPermissao(id) {
    const url = ("Login/MontaTabelaMenu").RetornaURL();
    return url + "/" + id;
}


//CHAMA TELA DE EDIÇÃO EM MODO DE CADASTRO ===========================================================

function Permissao() {
    if ($("#txtPermissao").val() == false) {
        $("#cmdAdicionar")[0].style.display = "none";
        $("#cmdSalvarPermissao")[0].style.display = "none";

    }
}

$("#cmdAdicionar").on("click", function () {
    Editar(0);
});

function ControlarExibicao(fExibe) {
    if (fExibe) {
        $("#dPermissoes").show();

    } else {
        $("#dPermissoes").hide();

    }


}



function CarregaPermissoes(id) {

    $("#txtCodGroupUser").val(0);

    if (Biblioteca.DataTable.SelecionarLinha("#tabelaDados", `#txtGrupoUsuarioName${id}`)) {


        $("#divPermissoes").load(MontaUrlPermissao(id), function (response, status, xhr) {
            if (status == "error") {
                Biblioteca.Alertas.Erro("", response);
            } else {
                ControlarExibicao(true);
                $("#txtCodGroupUser").val(id);

            }
        });
    } else {
        ControlarExibicao(false);
        $("#divPermissoes").html("");
    }
}

function Editar(id) {
    try {
        Biblioteca.Utils.AbrirPagina((`${NomeControlador}/Editar`).RetornaURL(), { id });
    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }
}
//====================================================================================================


//CARREGA DADOS -----------------------------------------------------------------------------------------------
function Pesquisar() {

    try {
        const dt = Biblioteca.Chamada.Get((`${NomeControlador}/Consultar`).RetornaURL(), { soAtivos: "false" }, false);
        CriarTabelaGroup(dt);

    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }


}

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
    Biblioteca.Alertas.Question("Confirma a Exclusão", "Deseja excluir o grupo de usuário selecionado?", null, Remover, [id]);

}
//====================================================================================================

//CARREGA DADOS -----------------------------------------------------------------------------------------------


//SALVAR ------------------------------------------------------------------------------------------------------

$("#cmdSalvarPermissao").on("click", function () {
    Salvar();
});

function MontaObjeto() {

    const obj = {};
    obj.ListaPermissoes = [];

    obj.Id = $("#txtCodGroupUser").val();




    obj.ListaPermissoes = MontaPermissoes();


    return obj;

}

function MontaPermissoes() {

    const lista = $.map($(".table.table-striped.table-bordered.tMenu"), function (item, i) { return $(item).dataTable().fnGetData(); });

    lista.map(function (item, i) {

        item.Id = $(item.inpId).val();
        item.TipoAlteracao = $(item.inpTipoAlteracao).val();
        item.Controller = $(item.inpController).val();
        if (!item.Editar) item.Editar = $($(item.inpEditar).children()[0]).val(); //$(`#txtEditar${item.Id}`).val();
        if (!item.Visualiza) item.Visualiza = $($(item.inpVisualizar).children()[0]).val(); // $(`#txtVisualiza${item.Id}`).val();


        return item;
    });

    return lista;
}

function Salvar() {

    try {

        const obj = MontaObjeto();

        const dados = `{obj : ${JSON.stringify(obj)}}`;


        const ret = Biblioteca.Chamada.PostJson((`${NomeControlador}/SalvarPermissoes`).RetornaURL(), dados);
        if (ret.Sucesso)
            Biblioteca.Notificacao.Sucesso("", ret.DescricaoRetorno, 1000, window.location, null, null);
        else {
            if (ret.Validacao)
                Biblioteca.Alertas.Warning("", ret.DescricaoRetorno);
            else
                Biblioteca.Alertas.Erro("Error", ret.DescricaoRetorno + ret.Origem.TrataCaminhoErro());
        }


    } catch (e) {
        Biblioteca.Alertas.Erro("Error", e.message);
    }


}


//SALVAR ------------------------------------------------------------------------------------------------------


//DATATABLE ------------------------------------------------------------------------------------------------------


function CriarTabelaGroup(data) {


    const lista = AjustaListagemGroup(data);

    const Colunas = [
        {
            "sTitle": "Grupo", "mData": "txtGrupoUsuarioName", "bSortable": true
        },
        {
            "sTitle": "Ações",
            "data": null,
            render: function (data, type, row) {
                var botoes = "<div >";

                botoes += `<button type="button" class="btn-flat btn-info p-0" onClick="Editar(${row.Id
                    })" title="Editar"><span class="ft-edit-2 font-medium-3 mr-1"></span></button>`;

                botoes += `<button type="button" class="btn-flat btn-danger p-0" onClick="Excluir(${row.Id
                    })" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="ft-x font-medium-3 mr-1"></i></button>`;
                botoes += "</div>";
                return botoes;

            }
            , "visible": $("#txtPermissao").val()
            , "sWidth": "30px"
            , "className": "dt-body-right"

        }
    ];



    $("#tabelaDados").DataTable({
        "data": lista,
        "aoColumns": Colunas,
        "responsive": true,
        "bLengthChange": false,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
        "sDom": "<'row'<'col-sm-12'tr>>I<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        "bPaginate": true,
        "pageLength": 10,
        "language": {
            "url": Biblioteca.Utils.getURL("Scripts/Componentes/Datatables/language-ptbr.txt")
        }


    });


}



function AjustaListagemGroup(data) {
    const lista = [];

    for (let i = 0; i < data.length; i++) {
        const item = data[i];


        item.txtGrupoUsuarioName = `<a class="textoSistema" href="javascript:CarregaPermissoes(${item.Id})"><spam id = "txtGrupoUsuarioName${item.Id}"> ${item.Descricao}`;
        item.txtGrupoUsuarioName += "</spam></a>";


        lista.push(item);



    }

    return lista;


}


$("#txtsearch").on("keyup", function () {
    const tb = $(".table.table-striped.table-bordered.tMenu").DataTable();
    //tb.draw();
    tb.search(this.value).draw();

});


//DATATABLE ------------------------------------------------------------------------------------------------------
