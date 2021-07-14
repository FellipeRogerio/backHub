
function SetaTextArea(nomeForm, filterNot) {
    if (filterNot == null) filterNot = "";
    if (nomeForm == null) nomeForm = document;
    if ($(nomeForm).find(".textAreaAutoSize").not(filterNot).length > 0) {
        autosize($($(nomeForm).find(".textAreaAutoSize").not(filterNot)));
    }

}

function SetaCheckbo(nomeForm, filterNot) {
    if (filterNot == null) filterNot = "";
    if (nomeForm == null) nomeForm = document;
    if ($(nomeForm).find(".checkbo").not(filterNot).length > 0) {
        $($(nomeForm).find(".checkbo").not(filterNot)).checkBo();
    }

}
function SetaComboEditavelPesqIni(nomeForm, parentDropdown) {
    if (parentDropdown == null) parentDropdown = document.body;

    if (nomeForm == null) nomeForm = document;
    if ($(nomeForm).find(".ComboEditavelPesqIni").length > 0) {

        $($(nomeForm).find(".ComboEditavelPesqIni")).select2({
            width: '100%'
            , dropdownParent: $(parentDropdown)
            , matcher: function (params, data) {
                params.term = params.term || '';
                if (data.text.toUpperCase().indexOf(params.term.toUpperCase()) == 0) {
                    return data;
                }
                return false;
            }
        });
    }

    $(nomeForm).on('focus', '.select2.select2-container', function (e) {
        // only open on original attempt - close focus event should not fire open
        if (e.originalEvent && $(this).find(".select2-selection--single").length > 0) {
            $(this).siblings('select:enabled').select2('open');
        }
    });
}

function SetaComboEditavel(nomeForm, parentDropdown) {
    if (parentDropdown == null) parentDropdown = document.body;

    if (nomeForm == null) nomeForm = document;
    if ($(nomeForm).find(".ComboEditavel").length > 0) {

        $($(nomeForm).find(".ComboEditavel")).select2({
            width: '100%'
            , dropdownParent: $(parentDropdown)
        });
    }

    $(nomeForm).on('focus', '.select2.select2-container', function (e) {
        // only open on original attempt - close focus event should not fire open
        if (e.originalEvent && $(this).find(".select2-selection--single").length > 0) {
            $(this).siblings('select:enabled').select2('open');
        }
    });
}

function SetaComboOutros(nomeForm, parentDropdown) {
    if (parentDropdown == null) parentDropdown = document.body;
    if (nomeForm == null) nomeForm = document;

    if ($(nomeForm).find(".ComboEditavelOutros").length > 0) {

        var lista = $(nomeForm).find('.ComboEditavelOutros').toArray();

        for (var i = 0; i < lista.length; i++) {
            var nome = lista[i].id.replace("cmb", "");
            var chk = '<span id="span' + nome + '" class="input-group-addon" data-toggle="tooltip' + nome + '" data-placement="bottom" title="For others click here">';
            chk += '<input type="checkbox" class= "chkOut" id="chkSel' + nome + '" data-link= "#' + lista[i].id + '"></span>';
            $(lista[i].parentElement).append(chk);
        }



        $('.chkOut').on('change', function () {
            Biblioteca.Utils.ControleComboOutros(this, this.checked);
        });



        if ($(nomeForm).find(".ComboEditavelOutros").length > 0) {

            $($(nomeForm).find(".ComboEditavelOutros")).select2({
                width: '100%'
                , dropdownParent: $(parentDropdown)
            });
        }

        $(nomeForm).on('focus', '.select2.select2-container', function (e) {
            // only open on original attempt - close focus event should not fire open
            if (e.originalEvent && $(this).find(".select2-selection--single").length > 0) {
                $(this).siblings('select:enabled').select2('open');
            }
        });


    }
}