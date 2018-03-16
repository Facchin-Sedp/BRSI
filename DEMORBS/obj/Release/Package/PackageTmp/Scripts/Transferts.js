// PAGINA CARICATA
$(document).ready(function () {
    getFTPList();
    $("#btnTxFile").click(function (event) {
        event.preventDefault();
        putFile();


    });

});



function getFTPList() {

    var url = "IseriesWebServices.asmx/FTP_LISTFILE";

    $.ajax({
        type: "POST",
        url: url,
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var rows = (msg.d);
             
            $.each(rows, function (index, value) {

                alert(value.Name + "---" + value.Size);



            });



        },
        error: function (request, status, err) {
            overlay_wait.hide();
            if (status === "timeout")
                alert(err);
            else
                alert("put File - Errore generico: " + request.responseText + "<br>" + err.responseText);
        }

    });


}

function putFile() {
    var file = $("#txtNomeFile").val();

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
            if (status === "timeout")
                alert(err);
            else
                alert("put File - Errore generico: " + request.responseText + "<br>" + err.responseText);
        }

    });



}