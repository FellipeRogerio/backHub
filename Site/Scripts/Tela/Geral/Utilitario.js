var Biblioteca = {};
var Despachante = {};

//var root = "";
Biblioteca.Chamada = (function () {
    const api = {};
    api.Get = function (url, obj, assincrono, retornaJSON, onLoanding) {
        if (retornaJSON == null) retornaJSON = true;
        if (assincrono == null) assincrono = true;

        var retorno;
        $.ajax({
            type: "GET",
            data: obj,
            url: url,
            async: assincrono,
            beforeSend: function () {
                if (onLoanding) onLoanding(true);
            }
        }).done(function (ret) {
            if (retornaJSON)
                retorno = JSON.parse(ret);
            else
                retorno = ret;
            if (onLoanding) onLoanding(false);

        }).fail(function (req, status, error) {
            if (onLoanding) onLoanding(false);
            throw new Error(req.responseText);
        });

        return retorno;

    };

    api.Post = function (url, form, retornaJSON) {
        if (retornaJSON == null) retornaJSON = true;
        //if (assincrono == null) assincrono = true;
        //

        var retorno;
        $.ajax({
            type: "POST",
            data: form,
            url: url,
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {

            }
        }).done(function (ret) {
            if (retornaJSON)
                retorno = JSON.parse(ret);
            else
                retorno = ret;

        }).fail(function (req, status, error) {
            throw new Error(req.responseText);
        });

        return retorno;

    };

    api.PostJson = function (url, obj) {
        //if (assincrono == null) assincrono = true;
        //

        var retorno;
        $.ajax({
            type: "POST",
            data: obj,
            url: url,
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            processData: false,
            beforeSend: function () {

            }
        }).done(function (ret) {
            retorno = JSON.parse(ret);
        }).fail(function (req, status, error) {
            throw new Error(req.responseText);
        });

        return retorno;

    };

    api.PostNewAba = function (url, form) {
        //if (assincrono == null) assincrono = true;
        //

        var retorno;
        $.ajax({
            type: "POST",
            data: form,
            url: url,
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            processData: false,
            beforeSend: function () {

            }
        }).done(function (ret) {
            const w = window.open(ret);
            w.focus();
        }).fail(function (req, status, error) {
            throw new Error(req.responseText);
        });

        return retorno;

    };

    api.PostNewURL = function (url, obj) {
        try {
            //var form = new FormData();
            //form.append('fileUpload', $("#idLogo")[0].files[0]);
            //form.append('obj', JSON.stringify(obj));

            //Define o formulário
            const myForm = document.createElement("form");
            myForm.action = url;
            myForm.method = "post";

            const input = document.createElement("input");
            input.type = "text";
            input.value = JSON.stringify(obj);
            input.name = "obj";
            myForm.appendChild(input);
            //Adiciona o form ao corpo do documento
            document.body.appendChild(myForm);
            //Envia o formulário
            myForm.submit();
        } catch (e) {
            throw new Error(e.mensage);
        }
    };


    api.Delete = function (url, obj) {
        //if (assincrono == null) assincrono = true;
        //

        var retorno;
        $.ajax({
            type: "POST",
            data: obj,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: false,
            beforeSend: function () {

            }
        }).done(function (ret) {
            retorno = JSON.parse(ret);
        }).fail(function (req, status, error) {
            throw new Error(req.responseText);
        });

        return retorno;

    };

    api.GeraRetornoGenerico = function () {
        const ret = {
            Origem: "",
            Message: "",
            Validacao: false,
            Success: false
        };

        return ret;
    };

    return api;
})();

Biblioteca.Utils = (function () {
    const api = {};

    api.getURL = function (URL_Relative) {
        return root + "/" + URL_Relative;
    };


    api.AbrirPagina = function (url, obj) {
        try {


            //Define o formulário
            const myForm = document.createElement("form");
            myForm.action = url;
            myForm.method = "post";

            for (let key in obj) {
                const input = document.createElement("input");
                input.type = "text";
                input.value = obj[key];
                input.name = key;
                myForm.appendChild(input);
            }
            //Adiciona o form ao corpo do documento
            document.body.appendChild(myForm);
            //Envia o formulário
            myForm.submit();
        } catch (e) {
            throw new Error(e.mensage);
        }
    };


    api.SetaRadioButton = function (idElemento, idElementoLabel, flag) {
        api.SetaCheckBox(idElemento);

        if (flag)
            $(idElementoLabel).addClass("checked");
        else
            $(idElementoLabel).removeClass("checked");

    };

    api.SetaCheckBox = function (idElemento, fChecked, noChange) {
        var check = fChecked;

        const flag = $(idElemento).attr("checked");

        if (fChecked == null) check = (flag == null);
        if (noChange == null) noChange = false;


        if (check) {
            $(idElemento).attr("checked", "checked");
        } else if (flag) {

            $(idElemento).removeAttr("checked");
        }

        if (!noChange) $(idElemento).change();

        api.SetaValCheckBox(idElemento);

    };

    api.CheckBox = function (idElemento, fChecked, noChange) {
        var check = fChecked;

        const flag = $(idElemento).attr("checked");

        if (fChecked == null) check = (flag == null);
        if (noChange == null) noChange = false;

        $(idElemento).prop("checked", check);


        if (!noChange) $(idElemento).change();

        $(idElemento).val(check);

    };

    api.SetaValCheckBox = function (idElemento) {

        const flag = $(idElemento).attr("checked");

        if (flag) {
            $(idElemento).val("true");

        } else {
            $(idElemento).val("false");

        }


    };

    api.trataJson = function (txt) {
        var ret = "";
        if (ret != "")
            ret = jQuery.parseJSON(txt.replace(/\\u003c/g, "&lt;").replace(/\\u003e/g, "&gt;"));
        return ret;


    };

    api.ToNull = function (txt) {
        return $.isEmptyObject(txt) ? "" : txt;
    };

    api.ControleComboOutros = function (item, flag) {

        const cmb = $(item).attr("data-link");
        $(cmb).val(null).empty().trigger("change");

        if (flag) {
            $(cmb).select2({
                placeholder: "Digite e clique Enter",
                maximumSelectionLength: 1,
                multiple: true,
                tags: true,
                allowClear: true,
                createTag: function (params) {
                    const obj = $.trim(params.term);
                    if (obj == "") return null;
                    return {
                        id: 0,
                        text: obj,
                        newTag: true
                    };
                }
            });
        }
        else {

            $(cmb).select2({ placeholder: "Select", tags: false, multiple: false, width: "100%" });
        }


    };
    api.ControlaTooltip = function (item, fExibe) {
        if (fExibe) {
            $(`[data-toggle=tooltip${item}]`).tooltip("show");
            setTimeout(function () {
                $(`[data-toggle=tooltip${item}]`).tooltip("hide");
            }, 3000);
        } else
            $(`[data-toggle=tooltip${item}]`).tooltip("hide");
    };
    api.ExibeComboOutros = function (elCombo, fExibe) {
        const idElem = elCombo.replace("cmb", "");
        if (fExibe) {
            $(`#span${idElem}`).css("display", "table-cell");
            Biblioteca.Utils.ControlaTooltip(idElem, true);

        } else
            $(`#span${idElem}`).css("display", "none");
    };

    return api;


})();

Biblioteca.Data = (function () {
    const api = {};

    api.IsDate = function (valor, formato) {
        if (formato == null) formato = "L";
        return moment(valor, "L", true).isValid();
    };

    api.PrimeiroDiaMes = function (data) {
        if (Biblioteca.Data.IsDate(data))
            return moment(moment(data, "l").format("YYYY-MM-") + "01");
        else
            return null;
    };

    api.DateAdd = function (valor, qtd, tPeriodo) {
        if (tPeriodo == null) tPeriodo = "d";
        if (qtd == null) qtd = 0;
        if (valor == null)
            return "";
        else
            return Biblioteca.Data.FormatarData(moment(valor, "L").add(qtd, tPeriodo));
    };

    api.FormatarData = function (valor, formato) {
        if (formato == null) formato = "L";
        if (valor == null) valor = "";
        const ret = moment(valor).format(formato);

        if (Biblioteca.Data.IsDate(ret, formato))
            return ret;
        else
            return "";
    };

    api.FormatarDataDoServidor = function (valor, formato) {
        if (formato == null) formato = "L";
        if (valor == null) valor = "";
        const ret = moment(valor).format(formato);

        if (Biblioteca.Data.IsDate(ret, formato))
            return ret;
        else
            return "";
    };

    api.DataParaServidor = function (valor) {
        var ret = moment(valor, "L").format("Y/MM/DD");
        if (ret == "Invalid date") ret = null;
        return ret;
    };
    api.getDate = function (idElemento) {
        var date;
        const dateFormat = "mm/dd/yyyy";
        const dateLocalidade = "en-US";
        try {
            date = new Date(Date.parse(idElemento.value, dateFormat)).toLocaleDateString(dateLocalidade);
        } catch (error) {
            date = null;
        }
        return date;
    };


    return api;

})();

Biblioteca.Mascaras = (function () {
    const api = {};
    api.RetornaCNPJ = function () { return "99.999.999/9999-99"; };
    api.RetornaCPF = function () { return "999.999.999-99"; };
    api.RetornaCEP = function () { return "99999-999"; };
    api.RetornaTelefone = function () { return "(999) 9999-9999[9]"; };
    api.CNPJ = function (elemento) {
        $(elemento).inputmask(Biblioteca.Mascaras.RetornaCNPJ());
        $(elemento).attr("placeholder", "00.000.000/0000-00");
    };
    api.CPF = function (elemento) {
        $(elemento).inputmask(Biblioteca.Mascaras.RetornaCPF());
        $(elemento).attr("placeholder", "000.000.000-00");
    };
    api.CNPJCPF = function (elemento) {
        $(elemento).inputmask({
            mask: [Biblioteca.Mascaras.RetornaCPF(), Biblioteca.Mascaras.RetornaCNPJ()],
            keepStatic: true
        });
    };
    api.CEP = function (elemento) {
        $(elemento).inputmask(Biblioteca.Mascaras.RetornaCEP());
        $(elemento).attr("placeholder", "00000-00");
    };
    api.Telefone = function (elemento) {
        $(elemento).inputmask(Biblioteca.Mascaras.RetornaTelefone());
        $(elemento).attr("placeholder", "(000) 0000-0000");
    };

    api.Monetario = function (elemento, casasDecimais) {
        if (casasDecimais == null) casasDecimais = 2;
        $(elemento).inputmask({ alias: "decimal", unmaskAsNumber: true, inputType: "number", positionCaretOnClick: "select", digits: casasDecimais });
    };

    api.Inteiro = function (elemento, limiteInferior, limiteSuperior) {
        if (limiteInferior == null) limiteInferior = Number.MIN_SAFE_INTEGER;
        if (limiteSuperior == null) limiteSuperior = Number.MAX_SAFE_INTEGER;
        $(elemento).inputmask("numeric", { min: limiteInferior, max: limiteSuperior });
    };

    api.Placa = function (elemento) {
        $(elemento).inputmask("aaa-9*99");
    }

    api.FixaValores = function (elemento, casasDecimais, formata) {
        if (casasDecimais == null) casasDecimais = 2;
        if (formata == null) formata = false;
        let value = elemento.value;
        if (value != null) { value = value.replace(".", "").replace(",", "."); }
        if (!isNaN(value * 1)) {
            const valor = formata ?
                (value * 1).toLocaleString(undefined, { minimumFractionDigits: casasDecimais, maximumFractionDigits: casasDecimais }) :
                (value * 1).toLocaleString(undefined, { minimumFractionDigits: casasDecimais, maximumFractionDigits: casasDecimais }).replace(".", "");
            $(elemento).val(valor);
        }
    };
    return api;

})();

Biblioteca.MSG = (function () {
    const api = {};

    api.hideMensagem = function (tempo) {
        if (!tempo) tempo = "slow";
        $("#msgModal").modal("hide");
        $("#msgModal").remove();


    };

    api.showMensagem = function (titulo, texto, tipoMensagem, botaoSim, evento) {
        if (!texto) texto = "<div class='spinner'><div class='double-bounce1'></div><div class='double-bounce2'></div></div>Carregando...";
        if (!titulo) titulo = "";
        if (!botaoSim) botaoSim = "";
        if (!evento) evento = "";
        if (!tipoMensagem) tipoMensagem = "";
        this.hideMensagem();


        var div = "";
        div += "<div class='modal ";
        if (tipoMensagem != "L") div += "fade";
        div += "' id='msgModal' tabindex=' - 1' role='dialog' aria-labelledby='msgModalLabel' data-backdrop='static' aria-hidden='true'>";
        div += "<div class='modal-dialog' role='document'>";
        div += "<div class='modal-content'>";
        div += "<div class='modal-header ";
        if (tipoMensagem == "L") div += " bg-primary white";
        if (tipoMensagem == "E") div += " bg-danger  white";
        if (tipoMensagem == "W") div += " bg-warning white";
        if (tipoMensagem == "S") div += " bg-success white";
        div += "'>";
        div += `<h4 class='modal-title' id='msgModalLabel'>${titulo}</h4>`;
        div += "<button class='close' type='button' data-dismiss='modal' aria-label='Close'>";
        div += "<span aria-hidden='true'>&times;</span>";
        div += "</button>";
        div += "</div>";
        div += `<div class='modal-body'>${texto}</div>`;
        div += "<div class='modal-footer'> <div class='tooltip-demo'>";
        if ((tipoMensagem != "L") && (tipoMensagem != "Q")) {
            div += "<button class='btn ";
            if (tipoMensagem == "L") div += " btn-outline-primary";
            if (tipoMensagem == "E") div += " btn-outline-danger ";
            if (tipoMensagem == "W") div += " btn-outline-warning";
            if (tipoMensagem == "S") div += " btn-outline-success";
            div += " btn-circle btn-lg' type='button' data-dismiss='modal' data-toggle='tooltip' data-placement='bottom' title='Ok'";
            if (evento != "") div += `onClick='${evento}'`;
            div += "> <i class='fa fa-check'></i></button > ";
        }
        else if (tipoMensagem == "Q") {
            div += "<button class='btn btn-success btn-circle btn-lg' type='button' data-dismiss='modal' data-toggle='tooltip' data-placement='bottom' title='Ok'";
            if (evento != "") div += `onClick='${evento}'`;
            div += "> <i class='fa fa-check'></i></button > ";

            div += "<button class='btn btn-danger btn-circle btn-lg' type='button' data-dismiss='modal' data-toggle='tooltip' data-placement='bottom' title='Cancell'";

            div += "> <i class='fa fa-close'></i></button > ";
        }

        if (botaoSim != "") div += botaoSim;

        div += "</div>";
        div += "</div>";
        div += "</div>";
        div += "</div>";
        div += "</div>";




        $(div).appendTo($("body"));
        $("#msgModal").modal("show");
    };

    return api;


})();

Biblioteca.Notificacao = (function () {
    const api = {};

    api.removerAll = function () {
        toastr.clear();
    };

    api.Erro = function (titulo, mensagem, duracao) {
        Biblioteca.Notificacao.exibir(titulo, mensagem, "E", duracao, null, null, null);
    };
    api.Warning = function (titulo, mensagem) {
        Biblioteca.Notificacao.exibir(titulo, mensagem, "W", null, null, null, null);
    };
    api.Sucesso = function (titulo, mensagem, duracao, pageReload, callFuncao, callParametros) {
        Biblioteca.Notificacao.exibir(titulo, mensagem, "S", duracao, pageReload, callFuncao, callParametros);
    };
    api.Info = function (titulo, mensagem, duracao) {
        Biblioteca.Notificacao.exibir(titulo, mensagem, "I", duracao, null, null, null);
    };

    api.exibir = function (titulo, mensagem, tipoAlerta, duracao, pageReload, callFuncao, callParametros) {
        //if (duracao == null) duracao = 1000;
        if (pageReload != null) {
            toastr.options.onHidden = function () {
                pageReload.reload();
            };
        }
        if (callFuncao != null) {
            toastr.options.onHidden = function () {
                callFuncao(callParametros);
            };
        }

        if (tipoAlerta == "S") {
            toastr.success(mensagem, titulo, {
                timeOut: duracao
            });
        } else if (tipoAlerta == "I") {
            toastr.info(mensagem, titulo, {
                timeOut: duracao
            });

        } else if (tipoAlerta == "W") {
            toastr.warning(mensagem, titulo, {
                timeOut: duracao
            });

        } else if (tipoAlerta == "E") {
            toastr.error(mensagem, titulo, {
                timeOut: duracao
            });

        }
    };
    return api;

})();

Biblioteca.Alertas = (function () {
    const api = {};

    api.Erro = function (titulo, mensagem, raiz) {
        Biblioteca.Alertas.exibir(titulo, mensagem, "E", null, null, null, null, raiz);
    };
    api.Warning = function (titulo, mensagem, duracao, raiz) {
        Biblioteca.Alertas.exibir(titulo, mensagem, "W", duracao, null, null, null, raiz);
    };
    api.Sucesso = function (titulo, mensagem, duracao, pageReload, callFuncao, callParametros, raiz, carregando, mensagemOnConfirm) {
        Biblioteca.Alertas.exibir(titulo, mensagem, "S", duracao, pageReload, callFuncao, callParametros, raiz, carregando, mensagemOnConfirm);
    };
    api.Info = function (titulo, mensagem, duracao, raiz, carregando) {
        Biblioteca.Alertas.exibir(titulo, mensagem, "I", duracao, null, null, null, raiz, carregando);
    };
    api.Question = function (titulo, mensagem, duracao, callFuncao, callParametros, raiz) {
        Biblioteca.Alertas.exibir(titulo, mensagem, "Q", duracao, null, callFuncao, callParametros, raiz);
    };

    api.exibir = function (titulo, mensagem, tipoAlerta, duracao, pageReload, callFuncao, callParametros, raiz, carregando, mensagemOnConfirm) {
        var width = "";
        if (raiz == null) {
            raiz = "body";
            width = (tipoAlerta == "E" ? null : "32%");
        }

        var tipo = "";
        if (tipoAlerta == "S") tipo = "success";
        if (tipoAlerta == "I") tipo = "info";
        if (tipoAlerta == "W") tipo = "warning";
        if (tipoAlerta == "E") tipo = "error";

        if (tipoAlerta == "Q") {
            tipo = "question";
            Swal.fire({
                target: raiz,
                title: titulo,
                text: mensagem,
                type: tipo,
                timer: duracao,
                allowOutsideClick: false,
                showCancelButton: true,
                confirmButtonColor: "#0CC27E",
                cancelButtonColor: "#FF586B",
                confirmButtonText: "Sim",
                cancelButtonText: "Não",
                confirmButtonClass: "btn btn-success btn-raised mr-5",
                cancelButtonClass: "btn btn-danger btn-raised",
                customClass: {
                    container: "swal-Alerta"
                }
            }).then((result) => {
                if (result.value && !mensagemOnConfirm) {
                    callFuncao(callParametros);
                } else if (mensagemOnConfirm && result.isConfirmed) {
                    callFuncao(callParametros);
                }

            });

        } else {

            Swal.fire({
                target: raiz,
                title: titulo,
                html: mensagem,
                type: tipo,
                timer: duracao,
                allowOutsideClick: false,
                customClass: {
                    container: "swal-Alerta"
                },
                width: width,
                onClose: function () {
                    if (pageReload != null) pageReload.reload();
                    if (callFuncao != null) callFuncao(callParametros);
                }, onOpen: () => {
                    if (carregando) Swal.showLoading();
                }
            });
        }

    };

    return api;


})();

Biblioteca.Geral = (function () {
    const api = {};

    function Chamada(type, url, param, asyncrono) {
        if (asyncrono == null) asyncrono = false;

        return $.ajax({
            type: type,
            url: root + url,
            data: param,
            async: asyncrono
        }
        );
    }

    api.Enum = function (idEnum, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Utilitario/MontaDTEnum", { NumEnum: idEnum }, assincrono);
    };

    api.Empresa = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Empresa/fixaTodos", { soAtivos: ativos }, assincrono);
    };

    api.Cliente = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Cliente/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.Fornecedor = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Fornecedor/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.EmpresaPorUsuario = function (NomeUsuario, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Empresa/RetornaTodosPorUsuario", { Usuario: NomeUsuario }, assincrono);
    };

    api.Usuario = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Usuario/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.Pais = function (assincrono) {
        return Biblioteca.Chamada.Get(root + "/Utilitario/RetornaTodosPais", {}, assincrono);
    };

    api.EstadoPorPais = function (idPais, comSiglas, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Utilitario/RetornaEstadoPorPais", { IdPais: idPais, comSiglas: comSiglas }, assincrono);
    };

    api.CidadePorEstado = function (IdEstado, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Utilitario/RetornaCidadePorEstado", { IdEstado: IdEstado }, assincrono);

    };

    api.GrupoUsuario = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/GrupoUsuario/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.FormaPagamento = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/FormaPgto/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.TipoDocumento = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/TipoDocumento/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.CondicaoPagamento = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/CondicaoPgto/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.Conta = function (ativos, assincrono) {
        return Biblioteca.Chamada.Get(root + "/Conta/RetornaTodos", { soAtivos: ativos }, assincrono);
    };

    api.DownloadPDF = function (caminho) {
        const lnk = root + "/Geral/DownloadPDF";

        window.open(`${lnk}?PATH=${caminho}`);


    };
    return api;

})();

Biblioteca.ControleCampos = (function () {
    const api = {};
    api.AjustarMenu = function () {
        esconderMenu = !esconderMenu;
        if (esconderMenu) {
            $("body").addClass("sidenav - toggled");

        } else {
            $("body").removeClass("sidenav - toggled");

        }
    };
    api.HabilitarTodosCampos = function (flag) {
        $("input, select").prop("disabled", !flag);
    };
    api.HabilitarCampo = function (nomeCampo, flag) {
        $(nomeCampo).prop("disabled", !flag);
    };
    api.LimparCampos = function (nomeCampo) {
        $(nomeCampo).val("");

    };
    api.HabilitarFormsCampos = function (nomeForm, flag) {
        $(`#${nomeForm} :input`).prop("disabled", !flag);
    };
    return api;

})();

Biblioteca.CarregaCombos = (function () {
    const api = {};

    api.MontaItens = function (data, aux, valueItem) {
        const lista = [];
        lista.push('<option value="0">Selecione</option>');

        lista.push($.map(data, function (item) {
            var sItem = `<option value=${item.Id}`;
            if (aux != null) sItem += ` data-aux= "${item[aux]}" `;
            if (valueItem != null)
                if (item.Id == valueItem) sItem += "selected";
            sItem += `>${item.Descricao}</option>`;
            return sItem;
        }));
        return lista;
    };

    api.PreencheCombo = function (combo, data, aux, opcaoTodos, valorPadraoSelecione, valorInicial) {
        //  combo = $("#" + combo);
        combo.empty();
        valorPadraoSelecione = valorPadraoSelecione ? valorPadraoSelecione : 0;
        combo.append(`<option value="${valorPadraoSelecione}">Selecione</option>`);
        if (opcaoTodos) combo.append('<option value="-1">Todos</option>');

        $.each(data,
            function (i, item) {
                var sItem = `<option value="${item.Id}"`;
                if (aux != null) sItem += ` data-aux= "${item[aux]}" `;
                if (valorInicial != null)
                    if (item.Id == valorInicial) sItem += 'selected';
                sItem += `>`;

                sItem += `${item.Descricao}</option>`;
                combo.append(sItem);
            });

    };

    api.Empresa = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Empresa(true, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.Cliente = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Cliente(true, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.Fornecedor = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Fornecedor(true, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.EmpresaPorUsuario = function (combo, NomeUsuario, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();
        if (NomeUsuario != "") {
            const ret = Biblioteca.Geral.EmpresaPorUsuario(NomeUsuario, assincrono);
            Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
        }
    };

    api.Pais = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Pais(true, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret, "ControlStateCity");
    };

    api.EstadoPorPais = function (combo, pais, comSigla, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.EstadoPorPais(pais, comSigla, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret, null, true);
    };

    api.CidadePorEstado = function (combo, estado, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();
        if (estado != "") {
            const ret = Biblioteca.Geral.CidadePorEstado(estado, assincrono);
            Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
        }
    };

    api.Usuario = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Usuario(true, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.GrupoUsuario = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.GrupoUsuario(true, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.RegimeTributario = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(2, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.Ambiente = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(3, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };



    api.StatusAprovacaoGerencia = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(10, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret, null, false, -1);
    };

    api.EtapaCompra = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(11, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret, null, false, -1);
    };
    api.StatusCompra = function (combo, assincrono, soCancelado, valorInicial) {
        if (assincrono == null) assincrono = false;
        if (valorInicial == null) valorInicial = -1;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(12, assincrono);
        let lista = ret;
        if (soCancelado)
            lista = ret.filter(x => [3, 4, 5].includes(x.Id))

        Biblioteca.CarregaCombos.PreencheCombo(combo, lista, null, false, -1, valorInicial);
    };


    api.TipoEmail = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(8, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret);
    };

    api.CamposDisponiveis = function (combo, assincrono) {
        if (assincrono == null) assincrono = false;
        combo = $(`#${combo}`);
        combo.empty();

        const ret = Biblioteca.Geral.Enum(9, assincrono);
        Biblioteca.CarregaCombos.PreencheCombo(combo, ret, "ShortNameAttribute");
    };

    return api;

})();

Biblioteca.Table = (function () {
    const api = {};

    api.RemoveRow = function (idElemento, rowIndex) {
        const tb = $(`#${idElemento}`)[0];
        tb.deleteRow(rowIndex);
    };
    api.InsereRow = function (idElemento, qtdCols, cell) {
        const tb = $(`#${idElemento}`)[0];

        const nRow = tb.insertRow(tb.rows.length);
        // Faz um loop para criar as colunas
        for (let j = 0; j < qtdCols; j++) {
            // Insere uma coluna na nova linha 
            newCell = nRow.insertCell(j);
            // Insere um conteúdo na coluna
            newCell.innerHTML = cell[j];
        }
    };
    return api;
})();

Biblioteca.DataTable = (function () {
    const api = {};

    api.AtualizaDados = function (name, idRow, column, value) {
        const array = column.split(".");
        if (array.length == 1)
            $(name).DataTable().data()[idRow][column] = value;
        else
            $(name).DataTable().data()[idRow][array[0]][array[1]] = value;

    };

    api.RemoveRow = function (idElemento, rowIndex) {
        const tb = $(`#${idElemento}`).DataTable();
        tb.row(rowIndex).delete();
    };

    api.InsereRow = function (idElemento, data) {
        const tb = $(`#${idElemento}`).DataTable();

        tb.rows.add(data).draw();


    };

    api.EsconderLinha = function (idElemento, id) {

        const table = $(idElemento).DataTable();


        $.fn.dataTable.ext.search.push(
            function (settings, data, index) {
                if (settings.sTableId == idElemento.replace("#", ""))
                    return index != id;

                return true;
            }
        );
        table.draw();
    };

    api.ExibeLinhas = function (idElemento) {

        const table = $(idElemento).DataTable();
        $.fn.dataTable.ext.search.pop();
        table.draw();

    };

    function TrataValor(valor) {
        if (Biblioteca.Data.IsDate(valor)) valor = Biblioteca.Data.DataParaServidor(valor);
        if (!$.isNumeric(valor.toString()) && valor.toString().ToDecimal() > 0) valor = valor.toString().ToDecimal();
        return valor;
    }
    function RetornaItem(item) {
        const obj = {};
        const lista = item["name"].split(".");

        if (lista.length > 1) {
            const novo = { name: item["name"].replace(lista[0] + ".", ""), value: item["value"] };
            obj[lista[0]] = {};
            obj[lista[0]] = RetornaItem(novo);
        } else {
            obj[item["name"]] = TrataValor(item["value"]);
        }

        return obj;

    }



    api.Serializar = function (idElemento, idsColumnAtivas, totalCols) {
        var lista = [];
        api.ControlarTodasColunas(idElemento, totalCols, true);
        const data = $(idElemento).DataTable().$("input, select, textarea, checkbox").serializeArray();
        api.ControlarTodasColunas(idElemento, totalCols, false);
        api.ExibirColuna(idElemento, idsColumnAtivas, true);
        var primeiroItem = "";
        var item = {};
        $.each(data, function () {

            const obj = RetornaItem(this);
            if (primeiroItem == this.name) {
                lista.push(item);
                item = {};
            }

            for (let key in obj) {
                if (item[key]) {
                    $.extend(item[key], TrataValor(obj[key]));
                } else {
                    if (obj.hasOwnProperty(key)) {
                        item[key] = TrataValor(obj[key]);
                    }
                }
            }

            if (primeiroItem == "") primeiroItem = this.name;

        });
        if (data.length > 0) lista.push(item);

        return lista;
    };

    api.ExibirColuna = function (idElemento, idsColumn, flag) {

        const lista = idsColumn.toString().split(",");
        const table = $(idElemento).DataTable();

        for (let i = 0; i < lista.length; i++) {
            const column = table.column(lista[i]);

            column.visible(flag);
        }
        table.columns.adjust().draw(false);
        //table.draw();

    };

    api.ControlarTodasColunas = function (idElemento, totalCols, flag) {

        const table = $(idElemento).DataTable();

        for (let i = 0; i < totalCols; i++) {
            const column = table.column(i);

            column.visible(flag);
        }
        table.columns.adjust().draw(false);
        //table.draw();

    };

    api.InserirCampo = function (tipo, style, nomeClass, idElemento, nomeElemento, valor, outrasProp) {
        var oElem = `<${tipo}`;
        if (idElemento != null) oElem += ` id="${idElemento}"`;
        if (nomeElemento != null) oElem += ` name="${nomeElemento}"`;
        if (style != null) oElem += ` style="${style}"`;
        if (nomeClass != null) oElem += ` class="${nomeClass}"`;
        if (outrasProp != null) oElem += ` ${outrasProp}`;

        if (tipo == "select") {
            oElem += ">";
            if (valor != null) oElem += valor;
            oElem += `</${tipo}>`;

        } else {
            if (valor != null) oElem += ` value = "${valor}"`;
            oElem += ">";

        }

        return oElem;

    };

    api.LinhaInvisivel = function (idElemento, id, idColuna) {

        const table = $(idElemento).DataTable();

        table.rows().data()[id].TipoAlteracao = 2;
        Biblioteca.DataTable.ExibirColuna(idElemento, idColuna, true);
        $(idElemento + " #txtTipoAlteracao" + id).val("2");
        Biblioteca.DataTable.EsconderLinha(idElemento, id);
        Biblioteca.DataTable.ExibirColuna(idElemento, idColuna, false);

    };


    api.SelecionarLinha = function (idTable, idElemento, removeSelecao) {

        const table = $(idTable).DataTable();
        if (removeSelecao == null) removeSelecao = true;

        const elem = $(idElemento).closest("tr");
        var flag = true;

        if (removeSelecao && elem.hasClass("trSelected")) {
            flag = false;
            elem.removeClass("trSelected");
        }
        if (flag && !elem.hasClass("trSelected")) {

            table.$("tr.trSelected").removeClass("trSelected");
            elem.addClass("trSelected");
        }

        return flag;

    };

    api.CancelarLinha = function (idTable, idElemento, removeSelecao) {

        const table = $(idTable).DataTable();
        if (removeSelecao == null) removeSelecao = true;

        const elem = $(idElemento).closest("tr");
        var flag = true;

        if (removeSelecao && elem.hasClass("trCancel")) {
            flag = false;
            elem.removeClass("trCancel");
        }
        if (flag && !elem.hasClass("trCancel")) {

            table.$("tr.trCancel").removeClass("trCancel");
            elem.addClass("trCancel");
        }

        return flag;

    };

    api.ConciliarLinha = function (idTable, idElemento, removeSelecao) {

        const table = $(idTable).DataTable();
        if (removeSelecao == null) removeSelecao = true;

        const elem = $(idElemento).closest("tr");
        var flag = true;

        if (removeSelecao && elem.hasClass("trConciliado")) {
            flag = false;
            elem.removeClass("trConciliado");
        }
        if (flag && !elem.hasClass("trConciliado")) {

            table.$("tr.trConciliado").removeClass("trConciliado");
            elem.addClass("trConciliado");
        }

        return flag;

    };

    api.MarcarLinhaExcluida = function (idTabela, idElemento, idLinha, indiceLinha) {

        $(`#${idTabela}`).DataTable().rows().every(function () {
            if (this.index() === idElemento) {
                this.data()[idLinha] = 2; // Marca como excluída.
            }
        });

        Biblioteca.DataTable.ExibirColuna(`#${idTabela}`, indiceLinha, true);
        Biblioteca.DataTable.EsconderLinha(`#${idTabela}`, idElemento);
        Biblioteca.DataTable.ExibirColuna(`#${idTabela}`, indiceLinha, false);
    };


    return api;
})();

Biblioteca.Dropzone = (function () {
    const api = {};

    api.CriaImagem = function (dropZone, nome, tamanho, url) {
        const tDrop = dropZone;

        const mockFile = {
            name: nome, size: tamanho, url: url, type: "image/png", accepted: true, status: "added"
        };
        if (tDrop.files.length < tDrop.options.maxFiles) {

            tDrop.files.push(mockFile);
            tDrop.emit("addedfile", mockFile);
            tDrop.emit("thumbnail", mockFile, url);

            tDrop.emit("complete", mockFile);
            tDrop._updateMaxFilesReachedClass();
        }

        return mockFile;
    };
    api.CriaArquivo = function (dropZone, url, nome) {
        var tDrop = dropZone;

        fetch(url, { mode: "cors" })
            .then(res => res.blob())
            .then(blob => {
                const file = new File([blob], nome);
                tDrop.files.push(file);
                tDrop.emit("addedfile", file);
                tDrop.emit("thumbnail", file, caminho);

                tDrop.emit("complete", file);
                tDrop._updateMaxFilesReachedClass();
            });

    };

    api.ConfiguraDropZone = function (nomeInputArq, idInputArq, timeout, maxFileZiseMiniatura, widthMiniatura, heightMiniatura,
        qtdMaxArq, tiposArq, htmlBtnRemove, idComponenteExisteNewIMG, strURL) {

        if (!strURL) strURL = "#";

        const dropzoneOptions = {
            url: strURL
            , method: "post"
            , withCredentials: false
            , uploadMultiple: false
            , timeout: timeout/*30000*/
            , maxThumbnailFilesize: maxFileZiseMiniatura/*10*/
            , thumbnailWidth: widthMiniatura/* 1090*/
            , thumbnailHeight: heightMiniatura/*1900*/

            , dictDefaultMessage: ""
            , clickable: true
            , maxFiles: qtdMaxArq
            , paramName: nomeInputArq/*"nameLogo"*/
            , paramId: idInputArq/*"idLogo"*/
            , acceptedFiles: tiposArq/*"image/*"*/
            , autoProcessQueue: true
            , init: function () {

                var tDrop = this;

                tDrop.on("maxfilesexceeded", function () {
                    while (tDrop.files.length > tDrop.options.maxFiles) {
                        tDrop.removeFile(tDrop.files[tDrop.options.maxFiles]);
                    }
                });


                tDrop.hiddenFileInput.setAttribute("name", tDrop.options.paramName);
                tDrop.hiddenFileInput.setAttribute("id", tDrop.options.paramId);

                tDrop.on("complete", function (file, xhr, formData) {
                    if (idComponenteExisteNewIMG != null)
                        $(`#${idComponenteExisteNewIMG}`).val("1");
                });

                tDrop.on("addedfile", function (file) {

                    // Capture the Dropzone instance as closure.
                    if (tDrop.hiddenFileInput.files.length > 0) {

                        if (idComponenteExisteNewIMG != null)
                            $(`#${idComponenteExisteNewIMG}`).val("1");

                    }

                    if (htmlBtnRemove != null) {

                        // Create the remove button
                        //var removeButton = Dropzone.createElement('<div class="btnRemove"><button type="button" class="btn btn-danger"><i class="glyphicon glyphicon-trash"></i></button><button type="button" class="btn btn-danger">Baixar</button></div>');
                        const elementoAcoes = Dropzone.createElement(`<div class="acoesDropZone">${htmlBtnRemove}</div>`);

                        // Listen to the click event
                        elementoAcoes.childNodes[0].addEventListener("click", function (e) {
                            // Make sure the button click doesn't submit the form:
                            e.preventDefault();
                            e.stopPropagation();

                            // Remove the file preview.
                            tDrop.removeFile(file);
                            if (idComponenteExisteNewIMG != null)
                                $(`#${idComponenteExisteNewIMG}`).val("1");
                            // If you want to the delete the file on the server as well,
                            // you can do the AJAX request here.
                        });

                        // Add the button to the file preview element.
                        file.previewElement.appendChild(elementoAcoes);
                    }

                });
            }
        };


        return dropzoneOptions;

    };


    return api;

})();

Biblioteca.Email = (function () {
    const api = {};
    api.NewObj = () => {
        var obj = {};
        obj.Email = {};
        obj.Email.Para = "";
        obj.Email.CC = "";
        obj.Email.Assunto = "";
        obj.Email.Titulo = "";
        obj.Email.CorpoEmail = "";
        obj.DefaultAnexos = [];

        return obj;


    };

    api.NewAnexo = () => {
        var obj = {};
        obj.URL = "";
        obj.NomeArquivo = "";

        return obj;


    };
    api.Enviar = (oEmail) => {
        try {



            Biblioteca.Chamada.PostNewURL(("Geral/Email").RetornaURL(), oEmail);



        } catch (e) {
            throw new Error(e.mensage);
        }
    };
    return api;
})();

