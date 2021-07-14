// LOAD ---------------------------------------------------------------------------------------------------------------
try {


    var nomeFormTel = "formTel";

    $(document).ready(function () {
        CarregaTelaTel();

        new PerfectScrollbar('#idPrincipal');


    })
    function CarregaTelaTel() {
        Biblioteca.ControleCampos.HabilitarFormsCampos(nomeFormTel, true);
        CarregaCombosTel();
        AddEventosTel();
        AplicaMascaraTel();
        formataCombosTel();
        CarregaTel();
    }
    $(function () { });
    $(window).on('load', function () { });

    // LOAD ---------------------------------------------------------------------------------------------------------------

    function AplicaMascaraTel() {
        Biblioteca.Mascaras.Telefone("#txtTelTelefone");
    }
    function formataCombosTel() { }
    function AddEventosTel() {

        $('form[name="' + nomeFormTel + '"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

        $("#cmdAddTelefone").on('click', function () {
            AbrirTelaTelefone(MontaTelefone());
        });

        $("#btnSairTel").on('click', function () {
            SairTel();
        });

        $("#cmdSalvarTel").on('click', function () {

            if ($('form[name="' + nomeFormTel + '"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
                $('#' + nomeFormTel).submit();
                return;
            }
            SalvarTel();

        });
        SetaComboEditavel("#" + nomeFormTel);
    }

    //CARREGA COMBOS ------------------------------------------------------------------------------------------------------
    function CarregaCombosTel() {
        Biblioteca.CarregaCombos.TipoTelefone("cmbTelTipo");

    }
    //CARREGA COMBOS ------------------------------------------------------------------------------------------------------

    //Funções ------------------------------------------------------------------------------------------------------
    function SairTel() {
        new telaModal().fecharModal('tModTel', false);
    }
    //Funções ------------------------------------------------------------------------------------------------------

    //CARREGA DADOS -----------------------------------------------------------------------------------------------
    function CarregaTel() {
        if (model.Editar) {
            Biblioteca.Utils.SetaCheckBox("#chkTelAtivo", model.Telefone.Ativo);
            Biblioteca.Utils.SetaCheckBox("#chkTelPrincipal", model.Telefone.Principal);

            $("#cmbTelTipo").findItem(model.Telefone.Tipo.oEnum);
            $("#txtTelTelefone").val(model.Telefone.Telefone);
            $("#txtTelRamal").val(model.Telefone.Ramal);
        } else {
            Biblioteca.Utils.SetaCheckBox("#chkTelAtivo", true);
        }


    }
    
    //CARREGA DADOS -----------------------------------------------------------------------------------------------

    //SALVAR ------------------------------------------------------------------------------------------------------


    function MontaTel() {

        var obj = {};
        obj.Telefone = {};
        obj.Telefone.Tipo = {};

        obj.IdOrigem = $("#txtTelIdOrigem").val().ToInteger();
        obj.ControllerOrigem = $("#txtTelTipoOrigem").val();
        obj.IdLinha = $("#txtTelIdLinha").val().ToInteger();

        obj.Telefone.Id = $("#txtTelCodigo").val().ToInteger();
        obj.Telefone.Ativo = $("#chkTelAtivo").is(':checked');

        obj.Telefone.Principal = $("#chkTelPrincipal").is(':checked');
        obj.Telefone.Tipo.oEnum = $("#cmbTelTipo").GetId();
        obj.Telefone.Tipo.Descricao = $("#cmbTelTipo").GetTexto();
        
        obj.Telefone.Telefone = $("#txtTelTelefone").val();
        obj.Telefone.Ramal = $("#txtTelRamal").val();

        obj.Telefone.TipoAlteracao = 0;
        if (obj.Telefone.Id > 0) obj.Telefone.TipoAlteracao = 1;

        return obj;

    }

    function SalvarTel() {

        try {

            var obj = MontaTel();
            
            var tb = $('#tbTelefones').DataTable();

            if (model.Editar)
                tb.row(obj.idLinha).data(AjustarItemTelefone(obj, obj.IdLinha)).draw();
            else {

                var data = [];
                data.push(AjustarItemTelefone(obj, tb.data().length));

                tb.rows.add(data).draw();

            }

            SairTel();


        } catch (e) {
            Biblioteca.Alertas.Erro("Erro", e.message );
        }
    }


    //SALVAR ------------------------------------------------------------------------------------------------------


    


} catch (e) {
    throw new Error(e.message);
}