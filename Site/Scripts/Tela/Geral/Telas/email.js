// LOAD ---------------------------------------------------------------------------------------------------------------

let nomeForm = 'formEmail';
let tDrop;
let quill;

$(document).ready(function () {
    Dropzone.autoDiscover = false;

    AddEventos();
    formatarCampos();
})


// LOAD ---------------------------------------------------------------------------------------------------------------

function ValidarEmail($el, value, callback) {
    var regex = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    value.split(";").map(x => {
        if ($.trim(x) == "") return false;

        callback({
            value: value,
            valid: regex.test($.trim(x)),
            message: `Email informado (${x}) não é válido`
        });
    });
}
function AddEventos() {
    $('form[name="' + nomeForm + '"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();


    $("#cmdSalvar").on('click', function () {

        if ($('form[name="' + nomeForm + '"]').find("input,select,textarea").jqBootstrapValidation("hasErrors")) {
            $('#' + nomeForm).submit();
            return;
        }
        Salvar();

    });

    $("#cmdLimpar").on('click', function () {

        $('form[name="' + nomeForm + '"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation("destroy");

        Limpar();
        $('form[name="' + nomeForm + '"]').find("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

    });

    $("#cmdExcluir").on('click', function () {


        Fechar();

    });


}

function formatarCampos() {
    tDrop = "";
    var btnExcluir = '<button type="button" class="btn-flat btn-danger p-0" data-toggle="tooltip" data-placement="top" title="Remover Imagem"><i class="ft-x font-medium-3 mr-1" style="cursor: pointer;"></i></button>';
    var option = Biblioteca.Dropzone.ConfiguraDropZone("anexos", "idAnexos"
        , 30000, 10, 50, 50, 10, null, btnExcluir, "txtTemImagemNova", 'http://localhost:23095/geral/upload');
    tDrop = new Dropzone("#tAnexos", option);

    let toolbarOptions = [
        [{ 'font': [] }],
        [{ 'color': [] }, { 'background': [] }],
        [{ 'align': [] }],
        [{ 'header': 1 }, { 'header': 2 }],
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],

        // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown


        // dropdown with defaults from theme


        ['link', 'image', 'formula']                                         // remove formatting button

        , ['clean']                                         // remove formatting button
    ];
    let options = {
        debug: 'info',
        modules: {
            toolbar: toolbarOptions
        },
        placeholder: '',
        readOnly: false,
        theme: 'snow'
    };
    quill = new Quill('#editor', options);

    if (tDrop != null) {

        var anexos = JSON.parse($('#txtAnexos').val());

        anexos.map(x => {
            fetch(x.URL, { mode: "cors" })
                .then(res => res.blob())
                .then(blob => {
                    const file = new File([blob], x.NomeArquivo);
                    tDrop.files.push(file);
                    tDrop.emit("addedfile", file);
                    tDrop.emit("thumbnail", file, x.URL);

                    tDrop.emit("complete", file);
                    tDrop._updateMaxFilesReachedClass();
                });
        });

        var body = $('#txtBodyIni').val();

        quill.clipboard.dangerouslyPasteHTML(0, ` ${body}`);


    }

    //$("#txtPara").val('desenvolvimento4@excelenciasolucoes.com.br');
    //$("#txtAssunto").val('Teste');
}

function Limpar() {


    var inputs = ':input[type= "email"], :input[type = "text"]';
    Biblioteca.ControleCampos.LimparCampos(inputs);
    quill.setContents([]);
    tDrop.removeAllFiles();


}

function Fechar() {

    window.close();


}

//SALVAR ------------------------------------------------------------------------------------------------------


function MontaObjeto() {

    var obj = {};
    obj.Email = {};



    obj.Email.Titulo = $("#txtAssunto").val();
    obj.Email.Para = $("#txtPara").val();
    obj.Email.CC = $("#txtCC").val();
    //obj.CCo = $("#txtCodigo").val();
    obj.Email.Assunto = $("#txtAssunto").val();



    return obj;

}


function Salvar() {

    try {

        var obj = MontaObjeto();

        var form = new FormData();
        tDrop.files.map(x => {
            form.append('fileUpload', x);

        });
        form.append('bodyHTML', quill.root.innerHTML);
        form.append('obj', JSON.stringify(obj));

        var ret = Biblioteca.Chamada.Post(("Geral/EnviarEmail").RetornaURL(), form);
        if (ret.Sucesso) {
            Biblioteca.Alertas.Sucesso("", ret.DescricaoRetorno, 1000, null, Fechar, null);

        }
        else {
            if (ret.Validacao)
                Biblioteca.Alertas.Warning("", ret.DescricaoRetorno);
            else
                Biblioteca.Alertas.Erro("Error", ret.DescricaoRetorno + ret.Origem.TrataCaminhoErro());
        }


    } catch (e) {
        var msg = e.message;
        if (ret?.Origem) msg += ret.Origem.TrataCaminhoErro();

        Biblioteca.Alertas.Erro("Error", msg);
    }
}


    //SALVAR ------------------------------------------------------------------------------------------------------






