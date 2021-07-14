String.prototype.ReplaceAll = function (find, newText) { return this.replace(new RegExp("[" + find + "]", "g"), newText); };
String.prototype.RetornaURL = function () {
    return root + "/" + this;
};
String.prototype.TrataCaminhoErro = function () {
    return "<br/><br/><sub> Way: " + this + "<\sub>";
};

String.prototype.ToNull = function () { return (this == null) ? "" : this; };

String.prototype.FormataTelefone = function () {
    return Inputmask.format(this, { mask: Biblioteca.Mascaras.RetornaTelefone() });
};
String.prototype.DataParaServidor = function () {
    var ret = moment(this, 'L').format('Y/MM/DD');
    if (ret == "Invalid date") ret = null;
    return ret;
};

String.prototype.ToDecimal = function (casasDecimais) {
    var valor = this;

    var sepador = ".";
    var milhar = ",";
    var idioma = window.navigator.userLanguage || window.navigator.language;
    if (idioma == "pt-BR") {
        sepador = ",";
        milhar = ".";
    }

    valor = valor.ReplaceAll(milhar, "");
    valor = valor.ReplaceAll(sepador, ".");
    if ($.isNumeric(casasDecimais)) valor = Number(valor).toFixed(casasDecimais);
    return parseFloat($.isNumeric(valor) ? valor : "0");

};
String.prototype.ToInteger = function () { return parseInt($.isNumeric(this) ? this : 0); };

Number.prototype.RetInteger = function () { return (this).toString().ToInteger(); };
Number.prototype.FormataDecimal = function (casasDecimais) {
    var sepador = ".";
    var idioma = window.navigator.userLanguage || window.navigator.language;
    if (idioma == "pt-BR") sepador = ",";

    var ret = this.toFixed(casasDecimais).toString();
    ret = ret.ReplaceAll(",", "");
    ret = ret.ReplaceAll(".", sepador);
    return ret;
};

Number.prototype.PadLeft = function (len, caracter) { return (this).toString().padStart(len, caracter); };
Number.prototype.PadRight = function (len, caracter) { return (this).toString().padEnd(len, caracter); };

$.fn.extend({
    findItem: function (valor) {
        $('#' + this[0].id + ' option[value="' + valor + '"]').prop('selected', true).trigger('change');

    }
    , findDesc: function (valor) {

        $('#' + this[0].id + ' option[text="' + valor + '"]').prop('selected', true).trigger('change');
    }
    , SetaOutro: function (valor) {
        if (valor == null) return;
        if (valor == "") return;

        item = new Option(valor, -1, true, true);
        $(this).append(item).trigger('change');

    }
    , GetIdDesc: function (valor) {
        var combo = this[0];
        var ret = "0";
        for (i = 0; i < combo.length; i++) {
            if (combo[i].text.toUpperCase() == (valor + "").toUpperCase()) {
                ret = combo[i].value;
                break;
            }
        }
        return (ret == undefined ? "0" : ret);
    }
    , GetId: function (valorPadrao) {
        valorPadrao == null ? "0" : valorPadrao;
        var ret = "0";
        ret = this.find('option:selected').val();
        return (ret == undefined ? valorPadrao : ret);
    }
    , GetTexto: function () {
        var ret = "";
        ret = this.find('option:selected').text();
        return (ret == undefined ? "" : ret);
    }
    , GetDataAux: function () {
        var ret = "";
        ret = this.find('option:selected').attr("data-Aux");
        return (ret == undefined ? "" : ret);
    }

    , getDate: function () {
        var dateFormat = "mm/dd/yyyy";
        var dateLocalidade = "en-US";
        try {
            return new Date(Date.parse(this.value, dateFormat)).toLocaleDateString(dateLocalidade);
        } catch (error) {
            return null;
        }
    }

    , GetChecked: function () {
        return $(this).is(':checked');
    }

});