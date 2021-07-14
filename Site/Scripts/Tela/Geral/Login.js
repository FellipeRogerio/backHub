// LOAD ---------------------------------------------------------------------------------------------------------------

$(document).ready(function () {
    CarregaCombos()
    AddEventos();
})

// LOAD ---------------------------------------------------------------------------------------------------------------

function AplicaMascara() { }

function formataCombos() { }

function EnviaEmail() {
    var outlookApp = new ActiveXObject("Outlook.Application");
    var nameSpace = outlookApp.getNameSpace("MAPI");
    mailFolder = nameSpace.getDefaultFolder(6);
    mailItem = mailFolder.Items.add('IPM.Note.FormA');
    mailItem.Subject = "Me";
    mailItem.To = "desenvolvimento4@excelenciasolucoes.com.br";
    mailItem.HTMLBody = "teste";
    mailItem.display(0);

}
function AddEventos() {

    $("input,select,textarea").not("[type=submit]").jqBootstrapValidation();

    $("#cmdLogar").on('click', function () {

        if ($("input,select,textarea").jqBootstrapValidation("hasErrors")) {
            $('#formUsuario').submit();
            return;
        }
        Logar();

    });

    $("#cmdEmail").on('click', function () {

        EnviaEmail();

    });

}
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------
function CarregaCombos() { }
//CARREGA COMBOS ------------------------------------------------------------------------------------------------------

//Funções ------------------------------------------------------------------------------------------------------
$("#txtUsuario").on('blur', function () {
    //CarregaCombos();
    //CarregaDados();
});
//Funções ------------------------------------------------------------------------------------------------------

//CARREGA DADOS -----------------------------------------------------------------------------------------------
function CarregaDados() {


}
//CARREGA DADOS -----------------------------------------------------------------------------------------------

//SALVAR ------------------------------------------------------------------------------------------------------



function MontaObjeto() {

    var obj = {};
    obj.IdEmpresa = 0
    obj.Usuario = $("#txtUsuario").val();
    obj.Senha = $("#txtSenha").val();
    obj.returnURL = $("#returnURL").val();



    return obj;

}

function Logar() {

    try {


        //========================================================
        // Montando o Objeto
        //========================================================
        var obj = MontaObjeto();



        //Funfou
        $.ajax({
            type: 'POST',
            data: JSON.stringify({ usuario: obj.Usuario, senha: obj.Senha, idEmpresa: obj.IdEmpresa, returnURL: obj.returnURL }),
            url: "Login/Entrar".RetornaURL(),
            contentType: 'application/json; charset=utf-8',
            dataType: 'text',
            success: function (resultado) {
                var ret = jQuery.parseJSON(resultado);
                
                
                if (ret.Sucesso) {

                    Biblioteca.Utils.AbrirPagina(ret?.data?.url.RetornaURL());

                }
                else {
                    Biblioteca.Alertas.Warning("", ret.DescricaoRetorno);

                }


            },
            error: function (req, status, error) {
                Biblioteca.Alertas.Erro("", req.responseText);

            }
        });

    } catch (e) {
        Biblioteca.Alertas.Erro("", e.message);
    }
}


//SALVAR ------------------------------------------------------------------------------------------------------
