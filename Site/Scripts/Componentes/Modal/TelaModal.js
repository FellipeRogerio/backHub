//NOVO - Usar Esse
var telaModal = function (options) {
    var vars = {
        Id: '',
        Width: 0,
        Height: 0,
        minWidth: 0,
        minHeight: 0,
        percWidth: 0, //40
        percHeight: 0,//70
        classModal: 'modal-dialog',
        idFechar: 'modal-fechar',
        Titulo: '',
        htmlCorpo: null,
        divModal: null,
        iframeModal: null,
        idModal: null,
        urlModal: null,
        paramUrlModal: null,
        formUrlModal: null,
        elemBlock: null,
        temCabecalho: false,
        sairEsc: false,
        sairClick: false,
        refreshAposFechar: false,
    };

    var root = this;

    this.construct = function (options) {
        $.extend(vars, options);
    };
    var montaModal = function () {
        var html = '';
        html += '<div class="modal-dialog ">';
        html += montaBody();
        html += '</div>';
        return html;
    };

    var montaBody = function () {
        var html = "";
        if (vars.temCabecalho) html += montaCabecalho();
        html += '<div id="dModal' + vars.Id + '" class="modal-content modal-all">';
        if (vars.htmlCorpo != null) html += vars.htmlCorpo;
        if (vars.divModal != null) html += vars.divModal;
        if (vars.iframeModal != null) html += vars.iframeModal;
        html += '</div>';
        return html;
    };
    var montaCabecalho = function () {

        var btn = "<button id='btnFecharModal" + vars.Id + "' class='close modal-fechar' data-dismiss='modal' aria-hidden='true'><span aria-hidden='true' class='modal-fechar'>&times;</span></button>";
        var html = '<div id="mCabecalho' + vars.Id + '" class="modal-cab ">';
        html += '<label id="lblTituloModal' + vars.Id + '" class="cabecalho-modal">' + vars.Titulo + '</label>';
        html += btn;

        html += '</div>';
        return html;
    };

    var carregarTela = function () {
        if (vars.formUrlModal != null) {

            $.ajax({
                type: "POST",
                url: vars.urlModal,
                data: vars.formUrlModal,
                contentType: 'application/json; charset=utf-8',
                cache: false,
                contentType: false,
                processData: false,
                complete: function (data) {
                    if (vars.iframeModal != null) {

                        var iframe = $("#" + vars.idModal)[0].contentWindow.document;
                        iframe.open();
                        iframe.write(data.responseText);
                        iframe.close();
                    } else {
                        $("#" + vars.idModal).html(data.responseText);

                    }

                }
            });
        } else {
            if (vars.paramUrlModal == null) vars.paramUrlModal = {};
            $.ajax({
                type: "POST",
                url: vars.urlModal,
                data: vars.paramUrlModal,
                contentType: 'application/json; charset=utf-8',
                dataType: 'text',
                async: false,
                cache: false,
                complete: function (data) {
                    //var iframe = document.getElementById(vars.idModal);
                    //iframe.contentWindow.document.body.innerHTML = (data);
                    //$("#" + vars.idModal).contents().find("html").append(data);
                    //$("#" + vars.idModal).contents().find("html").remove();
                    if (vars.iframeModal != null) {

                        var iframe = $("#" + vars.idModal)[0].contentWindow.document;
                        iframe.open();
                        iframe.write(data.responseText);
                        iframe.close();
                    } else {
                        $("#" + vars.idModal).html(data.responseText);

                    }

                }
            });
        }

       
    };

    this.bloquear = function (idBlock) {
        //var bloc = $(idBlock).find(".bloqueiaFundo");
        window.parent.$(idBlock).toggleClass("bloqueiaFundo-active");
        $(idBlock).toggleClass('bloqueiaFundo-active');
    };

    this.fecharModal = function (id, refreshAposFechar, idelemBlock) {
        $("#" + id).modal('hide');
        if ($("#" + id).children().length > 0) $("#" + id).children()[0].remove();
        if (idelemBlock != null) root.bloquear("#" + idelemBlock);
        if (refreshAposFechar) parent.location.reload();
    };

    this.abrirModal = function () {
        //if (percLargura == null) percLargura = 40;
        //if (percAltura == null) percAltura = 70;
        var idModal = "#" + vars.Id;
        $(idModal).html(montaModal());
        if (vars.idModal != null) carregarTela();

        var form = $(idModal).find("." + vars.classModal);

        form.css("pointer-events", "all");
        if (vars.percHeight > 0)
            form.css("height", vars.percHeight + "%");
        else
            form.css("height", vars.Height + "px");

        if (vars.percWidth > 0)
            form.css("width", vars.percWidth + "%");
        else
            form.css("width", vars.Width + "px");

        if (vars.minWidth > 0)
            form.css("min-width", vars.minWidth + "px");
        else
            form.css("min-width", "320px");

        if (vars.minHeight> 0)
            form.css("min-height", vars.minHeight + "px");
        
        form.css("max-width", "100%");

        $(idModal).modal({
            backdrop: vars.sairClick,
            keyboard: vars.sairEsc,
            show: true
        });

        if (vars.elemBlock != null) this.bloquear("#" + vars.elemBlock);


        //$(idModal).load(window.location.href + " " + idModal);

        $("#btnFecharModal" + vars.Id).on('click', function () {
            root.fecharModal(vars.Id, vars.refreshAposFechar, vars.elemBlock);
        });

        if (vars.idFechar != '') {

            $(vars.idFechar).on('click', function () {
                root.fecharModal(vars.Id, vars.refreshAposFechar, vars.elemBlock);

            });
        }
    };

    this.construct(options);

}




