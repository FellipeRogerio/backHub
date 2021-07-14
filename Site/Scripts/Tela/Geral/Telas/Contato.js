// LOAD ---------------------------------------------------------------------------------------------------------------
try {


    var nomeFormContato = "formContato";
    var totColunas = 0;
    $(document).ready(function () {
        CarregaTelaContato();

        new PerfectScrollbar('#idPrincipal');


    })
    function CarregaTelaContato() {
        Biblioteca.ControleCampos.HabilitarFormsCampos(nomeFormContato, true);
        CarregaCombosContato();
        AddEventosContato();
        AplicaMascaraContato();
        formataCombosContato();
        CriarTbTelefone();
        CarregaContato();
    }
    $(function () { });
    $(window).on('load', function () { });

    // LOAD ---------------------------------------------------------------------------------------------------------------

    function AplicaMascaraContato() { }
    function formataCombosContato() { }
    function AddEventosContato() {

        $('form[name="' + nomeFormContato + '"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

        $("#cmdAddTelefone").on('click', function () {
            AbrirTelaTelefone(MontaTelefone());
        });

        $("#btnSairContato").on('click', function () {
            SairContato();
        });

        $("#cmdSalvarContato").on('click', function () {

            if ($('form[name="' + nomeFormContato + '"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
                $('#' + nomeFormContato).submit();
                return;
            }
            SalvarContato();

        });
    }

    //CARREGA COMBOS ------------------------------------------------------------------------------------------------------
    function CarregaCombosContato() { }
    //CARREGA COMBOS ------------------------------------------------------------------------------------------------------

    //Funções ------------------------------------------------------------------------------------------------------
    function SairContato() {
        new telaModal().fecharModal('tModal', false);
    }
    function VoltarContato() {
        SairContato();
    }
    //Funções ------------------------------------------------------------------------------------------------------

    //CARREGA DADOS -----------------------------------------------------------------------------------------------
    function CarregaContato() {

        if (model.Editar) {
            Biblioteca.Utils.SetaCheckBox("#chkContatoAtivo", model.Contato.Ativo);

            $("#txtContatoNome").val(model.Contato.Nome);
            $("#txtContatoEmail").val(model.Contato.Email);
            $("#txtContatoDepartamento").val(model.Contato.Departamento);
            $("#txtContatoObs").val(model.Contato.Obs);
            CarregaTelefones(AjustaListagemTelefone(model.Contato.ListaTelefones,true));


        } else {
            Biblioteca.Utils.SetaCheckBox("#chkContatoAtivo", true);


        }


    }

    function CarregaTelefones(lista) {
        var table = $('#tbTelefones').DataTable();
        table.clear().draw();
        table.clear().rows.add(lista).draw();

    }

    //CARREGA DADOS -----------------------------------------------------------------------------------------------

    //SALVAR ------------------------------------------------------------------------------------------------------


    function MontaContato() {

        var obj = {};
        obj.Contato = {};
        obj.ListaTelefones = [];

        obj.IdLinha = $("#txtContatoIdLinha").val().ToInteger();
        obj.ControllerOrigem = $("#txtContatoTipoOrigem").val();
        obj.IdOrigem = $("#txtContatoIdOrigem").val().ToInteger();

        obj.Contato.Id = $("#txtContatoCodigo").val().ToInteger();
        obj.Contato.Ativo = $("#chkContatoAtivo").is(':checked');

        obj.Contato.Nome = $("#txtContatoNome").val();
        obj.Contato.Email = $("#txtContatoEmail").val();
        obj.Contato.Departamento = $("#txtContatoDepartamento").val();
        obj.Contato.Obs = $("#txtContatoObs").val();

        obj.Contato.ListaTelefones = Biblioteca.DataTable.Serializar("#tbTelefones", "4,5,6,7", totColunas);

        if (obj.Contato.Id > 0) obj.Contato.TipoAlteracao = 1;
        return obj;

    }

    function SalvarContato() {

        try {

            var obj = MontaContato();

            var form = new FormData();
            form.append('obj', JSON.stringify(obj));

            var ret = Biblioteca.Chamada.Post(("Geral/GravarContato").RetornaURL(), form);
            if (ret.Sucesso) {

                Biblioteca.Notificacao.Sucesso("", ret.DescricaoRetorno, 1000, null, SairContato, null);
                AtualizaContatos();
            }
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



    function CriarTbTelefone() {

        var Colunas = [
            { "sTitle": "Id", "mData": "txtIdTelefone", "visible": false },
            { "sTitle": "TipoAlteracao", "mData": "txtTipoAlteracao", "visible": false },
            { "sTitle": "Tipo", "mData": "txtTipo", "bSortable": true, "visible": false },
            { "sTitle": "Principal", "mData": "txtPrincipal", "bSortable": true },
            { "sTitle": "Tipo", "mData": "txtDescTipo", "bSortable": true },
            { "sTitle": "Telefone", "mData": "txtTelefone", "bSortable": true },
            { "sTitle": "Ramal", "mData": "txtRamal", "bSortable": true },
            {
                "sTitle": "Ações",
                "data": null,
                render: function (data, type, row, meta) {
                    var botoes = '<div>';
                    botoes += '<button type="button" class="btn-flat btn-info p-0" onClick="EditarTelefone(' + meta.row + ')" title="Editar"><span class="ft-edit-2 font-medium-3 mr-1"></span></button>';
                    botoes += '<button type="button" class="btn-flat btn-danger p-0" onClick="ApagarTelefone(' + meta.row + ')" data-toggle="tooltip" data-placement="top" title="Deletar"><i class="ft-x font-medium-3 mr-1"></i></button>';

                    botoes += '</div>';
                    return botoes;

                }
                , "sWidth": "30px"
            }
        ];

        totColunas = Colunas.length;

        $('#tbTelefones').DataTable({
            "aoColumns": Colunas,
            responsive: true,
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
    function AjustarItemTelefone(item, index, flagFormatar) {
        if (flagFormatar == null) flagFormatar = false;


        if (flagFormatar) {
        }
        item.txtIdTelefone = '<input min="1"  type="number" id ="txtIdTelefone' + index + '" name="Id" value="' + item.Telefone.Id + '"> ';
        item.txtTipoAlteracao = '<input type="number" id ="txtTipoAlteracao' + index + '" name="TipoAlteracao" value="' + item.Telefone.TipoAlteracao + '"> ';
        item.txtTipo = '<input class="semFormatacao" style="width:100%" type="text" id ="txtTipo' + index + '"  name="Tipo.oEnum" value="' + item.Telefone.Tipo.oEnum + '"> ';
        item.txtPrincipal = '<div style="position: relative;"><div class="semFormatacaoSwitch"></div><div class="custom-control custom-switch"><input class="custom-control-input semFormatacao" style="width:100%" type="checkbox" id ="txtPrincipal' + index + '"  name="Principal"';
        item.txtPrincipal += 'value = "' + item.Telefone.Principal.toString() + '" ' + (item.Telefone.Principal ? "checked" : "") +'  ><label class="custom-control-label" for="txtPrincipal' + index + '"></label></div></div> ';
        
        item.txtDescTipo = '<input class="semFormatacao" style="width:100%" type="text" id ="txtDescTipo' + index + '"  name="DescTipo" value="' + item.Telefone.Tipo.Descricao + '"> ';
        item.txtTelefone = '<input class="semFormatacao" style="width:100%" type="text" id ="txtTelefone' + index + '"  name="Telefone" value="' + (item.Telefone.Telefone + "").FormataTelefone() + '"> ';
        item.txtRamal = '<input class="semFormatacao" style="width:100%" type="text" id ="txtRamal' + index + '"  name="Ramal" value="' + item.Telefone.Ramal + '"> ';


        return item;
    }

    function AjustaListagemTelefone(data, flagFormatar) {
        var lista = [];
        
        $.each(data,function (i, item) {
                var obj = {};
                obj.Telefone = data[i];

                var item = AjustarItemTelefone(obj, i, flagFormatar);
                lista.push(item);
            });
        

        return lista;
    }

    function ApagarTelefone(id) {

        Biblioteca.DataTable.LinhaInvisivel("#tbTelefones", id, 1);
    }

    function MontaTelefone() {

        var item = {};
        item.Tipo = {};
        item.Editar = false;
        item.idLinha = -1;
        item.Id = 0;
        item.TipoAlteracao = 0;

        return item;

    }

    function AbrirTelaTelefone(obj) {
        try {
            var titulo = "Adicionar Telefone";

            if (obj.Editar) titulo = "Editar Telefone";
            var modelTelefone = {};
            modelTelefone.Telefone = {};
            modelTelefone.Telefone.Tipo = {};

            modelTelefone.ControllerOrigem = $("#txtContatoTipoOrigem").val();
            modelTelefone.Editar = obj.Editar;
            modelTelefone.IdLinha = obj.IdLinha;
            modelTelefone.Telefone = obj;

            var form = new FormData();
            form.append('model', JSON.stringify(modelTelefone));



            var frame = "<div id='telaTel' class='modal-conteudo'></div>";
            var modal = new telaModal({
                Id: 'tModTel',
                percWidth: 35,
                percHeight: 55,
                minWidth: 360,
                minHeight: 0,
                classModal: 'modal-dialog',
                idFechar: '#btnSairTel',
                Titulo: titulo,
                urlModal: ("Geral/TelefoneIndex").RetornaURL(),
                formUrlModal: form,
                divModal: frame,
                idModal: 'telaTel',
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

    function EditarTelefone(id) {

        var obj = $('#tbTelefones').DataTable().data()[id].Telefone;


        obj.IdLinha = id;
        obj.Editar = true;
        AbrirTelaTelefone(obj);
    }


} catch (e) {
    throw new Error(e.message);
}