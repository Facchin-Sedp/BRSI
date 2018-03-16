// PAGINA CARICATA
$(document).ready(function () {

    $("#").clik(function () {

        putFile();


    });

});


function putFile() {


    var command = { FILE:file};
    var jsonData = JSON.stringify(command);

    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/putFile",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = (msg.d);
            var res = $.parseJSON(data);
            alert(res);



        },
        error: function (request, status, err) {
            overlay_wait.hide();
            if (status == "timeout")
                alert(err);
            else
                alert("put File - Errore generico: " + request.responseText + "<br>" + err.responseText);
        }

    });



}