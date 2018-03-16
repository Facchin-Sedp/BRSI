//prende alcune proprietà del server: no Kendo
function GetIseriesLibraries() {
    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/Get_libraries_list",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = (msg.d);

            var t = "<table id='Library' style=\"color:#FFFFFF;font-weight:bold;border:0px;background-color:orange\">";

            var parse = $.parseJSON(data);
            //cicla sull alista degli oggetti restituiti: qui un solo oggetto di due campi
            $.each(parse, function (i, v) {
                t = t + "<tr id='" + v.nome + "'> <td class='lib'> " + v.nome + "</td>" +
                    "<td class='desc'>" + v.Descrizione + "</td></tr>";
            });

            t = t + "</table> ";

            $("#libSystem").html(t);
            dragdrop();


        },
        error: function (msg) {

        }
    });
};

$(function () {
    $("#accordion").accordion();
});

function dragdrop() {
    $(".lib").draggable(
                          {
                              cursor: 'move',
                              opacity: 0.7,
                              revert: true,
                              start: function () {
                                  contents = this.textContent;
                              }
                          }

                          );

    $("#libSelection").droppable(
    {
        over: function () {
            $('#libSelection').css('backgroundColor', '#cedae3');
        },
        out: function () {
            $('#libSelection').css('backgroundColor', 'orange');
        },
        drop: function () {
            //var answer = confirm('Permantly delete this item?');

            $('#libSelection').append("<table style='background-color:gray;width:100%;height:auto'><tr><td>" + contents + "</td></tr></table>");
            $('#libSelection').css('backgroundColor', '#a6bcce');

        }
    }
    );


}

$(document).ready
(
     function () {
         GetIseriesLibraries();

     }

);