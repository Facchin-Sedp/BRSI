﻿$(document).ready(function () {

    
    reload2();

    
});


function Save_schedule_data( id_btn) {

    var libreria = id_btn.replace("BtnSaveSched", "");
    var timeSched1 = document.getElementById("timepicker1" + libreria).value;
    var tipo1 = document.getElementById("type1" + libreria);

    $('#wait' + escapeStr(libreria)).fadeIn('fast');

    if ($(tipo1).hasClass('T'))
        tipo1 = "T";
    else
        tipo1 = "I";
    
    var sched2 = document.getElementById("sched2" + libreria);
    if ($(sched2).hasClass('true'))
    {
        var timeSched2 = document.getElementById("timepicker2" + libreria).value;
        var tipo2 = document.getElementById("type2" + libreria);

        if ($(tipo2).hasClass('T'))
            tipo2 = "T";
        else
            tipo2 = "I";
    }
    else
    {
        var timeSched2 = "";
        var tipo2 = "";
    }

    var sched3 = document.getElementById("sched3" + libreria);
    if ($(sched3).hasClass('true')) {
        var timeSched3 = document.getElementById("timepicker3" + libreria).value;
        var tipo3 = document.getElementById("type3" + libreria);

        if ($(tipo3).hasClass('T'))
            tipo3 = "T";
        else
            tipo3 = "I";
    } else {
        var timeSched3 = "";
        var tipo3 = "";
    }
    var sched4 = document.getElementById("sched4" + libreria);
    if ($(sched4).hasClass('true')) {
        var timeSched4 = document.getElementById("timepicker4" + libreria).value;
        var tipo4 = document.getElementById("type4" + libreria);

        if ($(tipo4).hasClass('T'))
            tipo4 = "T";
        else
            tipo4 = "I";
    } else {
        var timeSched4 = "";
        var tipo4 = "";
    }


    var sched5 = document.getElementById("sched5" + libreria);
    if ($(sched5).hasClass('true')) {
        var timeSched5 = document.getElementById("timepicker5" + libreria).value;
        var tipo5 = document.getElementById("type5" + libreria);

        if ($(tipo5).hasClass('T'))
            tipo5 = "T";
        else
            tipo5 = "I";
    } else {
        var timeSched5 = "";
        var tipo5 = "";
    }
    var sched6 = document.getElementById("sched6" + libreria);
    if ($(sched6).hasClass('true')) {
        var timeSched6 = document.getElementById("timepicker6" + libreria).value;
        var tipo6 = document.getElementById("type6" + libreria);

        if ($(tipo6).hasClass('T'))
            tipo6 = "T";
        else
            tipo6 = "I";
    } else {
        var timeSched6 = "";
        var tipo6 = "";
    }


    var sched7 = document.getElementById("sched7" + libreria);
    if ($(sched7).hasClass('true')) {
        var timeSched7 = document.getElementById("timepicker7" + libreria).value;
        var tipo7 = document.getElementById("type7" + libreria);

        if ($(tipo7).hasClass('T'))
            tipo7 = "T";
        else
            tipo7 = "I";
    } else {
        var timeSched7 = "";
        var tipo7 = "";
    }

    var sched8 = document.getElementById("sched8" + libreria);
    if ($(sched8).hasClass('true')) {
        var timeSched8 = document.getElementById("timepicker8" + libreria).value;
        var tipo8 = document.getElementById("type8" + libreria);

        if ($(tipo8).hasClass('T'))
            tipo8 = "T";
        else
            tipo8 = "I";
    } else {
        var timeSched8 = "";
        var tipo8 = "";
    }

    var sched9 = document.getElementById("sched9" + libreria);
    if ($(sched9).hasClass('true')) {
        var timeSched9 = document.getElementById("timepicker9" + libreria).value;
        var tipo9 = document.getElementById("type9" + libreria);

        if ($(tipo9).hasClass('T'))
            tipo9 = "T";
        else
            tipo9 = "I";
    } else {
        var timeSched9 = "";
        var tipo9 = "";
    }


    sched = {
        Libreria: libreria,
        timeSched1: timeSched1, tipo1: tipo1,
        timeSched2: timeSched2, tipo2: tipo2,
        timeSched3: timeSched3, tipo3: tipo3,
        timeSched4: timeSched4, tipo4: tipo4,
        timeSched5: timeSched5, tipo5: tipo5,
        timeSched6: timeSched6, tipo6: tipo6,
        timeSched7: timeSched7, tipo7: tipo7,
        timeSched8: timeSched8, tipo8: tipo8,
        timeSched9: timeSched9, tipo9: tipo9
    };

    var jsonSched = JSON.stringify(sched);// converto in JSON e invio

    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/Save_Sched_lib",
        data: "{'JsonSched':'" + jsonSched + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#wait' + escapeStr(libreria)).hide(0);

            // messaggio di fine salvataggio
            var tmsg = $("#EventMessage");
            tmsg.html('<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>' +
                                       '<strong>INFO:</strong> Parametri Schedulazione salvati correttamente');
            var msg = $("#message");
            msg.fadeIn('slow');
            setTimeout(hideMessage, 20000);
        },
        error: function (msg) {
            alert(msg.statusText);
        }
    });
}

// serve per evitare i problemi con i char speciali nei componenti web
function escapeStr(str) {
    if (str)
        return str.replace(/([ #;?%&,.+*~\':"!^$[\]()=>|\/@])/g, '\\$1')
    else
        return str;
}

function hideMessage() {
    $("#EventMessage").fadeOut('slow');
}

// funzione veloce di caricamento 
// carica gli effetti solo sull'accordion aperto
function reload2()
{
 

    $("#librerie").accordion({
        collapsible: true,
        active: false,
        change: function (event, ui)
        {
  
            var CurrentIdx = $("#librerie h3").index($("#librerie h3.ui-state-active"));
            var nomelib;
            if (CurrentIdx > -1) {
                nomelib = $(this).find("H3")[CurrentIdx].id;
                show_part(nomelib);
            }
 

        }// activate function
 
    });
     
    
}
 
// mostra le parti ono off a seconda dei dati contenuti nel DB
    function show_part(nomelib)
    {
        nomelibNoEscape = nomelib;
        nomelib = escapeStr(nomelib);
       
            // progress salvataggio
            $('#wait' + nomelib).hide(0);

            for (i = 1; i <= 9; i++) // i 9 calendari e tot par
            {
                $('#timepicker' + i + nomelib).timepicker({
                    showNowButton: true,
                    showDeselectButton: true,
                    defaultTime: '',  // removes the highlighted time for when the input is empty.
                    showCloseButton: true
                });
 

                if (i > 1)// per tutti gli switch maggiori del primo perchè questo è il default
                {
                    var time = document.getElementById("Time" + i + nomelibNoEscape);

                    if ($(time).parent().parent().hasClass("true"))// parto dallo stato invisibile
                    {
                        var tp = escapeStr(time.id);// id switch cliccato
                        tp = tp.replace("Time", "timepicker");
                        $("#" + tp).fadeIn("slow");
                        tp = tp.replace("timepicker", "type");
                        $("#" + tp).fadeIn("slow");
                        $(time).parent().parent().addClass('true').removeClass('false');
                    }
                }

                // questo controlla lo stato dell switch e fa apparire il timepicker e il totale parziale
                // per ogni oggetto imposto il click
                $('#Time' + i + nomelib).click(function () {

                    var tp = escapeStr(this.id.toString());// id switch cliccato

                    if (!$(this).parent().parent().hasClass('disabled')) {
                        if ($(this).parent().parent().hasClass('true'))// se true metto false
                        {
                            tp = tp.replace("Time", "timepicker");
                            $("#" + tp).fadeOut("slow");
                            tp = tp.replace("timepicker", "type");
                            $("#" + tp).fadeOut("slow");
                            $(this).parent().parent().addClass('false').removeClass('true');
                        }
                        else// se false metto true VISIBILE
                        {
                            tp = tp.replace("Time", "timepicker");
                            $("#" + tp).fadeIn("slow");
                            tp = tp.replace("timepicker", "type");
                            $("#" + tp).fadeIn("slow");
                            $(this).parent().parent().addClass('true').removeClass('false');
                        }
                    }

                });

                // cambia la classe dello switch tipo per fare tot-inc
               // imposto il click nello switch 
                $('#Type' + i + nomelib).click(function () {

                    if (!$(this).parent().parent().hasClass('disabled')) {
                        if ($(this).parent().parent().hasClass('T'))// se true metto false
                        {
                            $(this).parent().parent().addClass('I').removeClass('T');
                        }
                        else// se false metto true
                        {
                            $(this).parent().parent().addClass('T').removeClass('I');
                        }
                    }

                });
            }// end for

            $(".btnSaveSched").button();
            $(".btnSaveSched").click(
                function () {
                    var id_btn = this.id.toString();
                    Save_schedule_data(id_btn);
                });

    }// activate function

    
function showWaitpage()
{
    $('#Waitpages').fadeIn('fast');
}


/////////////////////////////////////////////////////////////















// lentissima funzione di caricamento
/*function reload()// ad ogni update panel devo fare il reload dello stile altrimenti restano senza jquery
{


    $("#librerie").accordion();
    $(".btnSaveSched").button();
    $(".btnSaveSched").click(
        function () {
            var id_btn = this.id.toString();
            Save_schedule_data(id_btn);
        }


        );

    // questo controlla lo stato dell switch e fa apparire il timepicker e il totale parziale
    $('.bool-slider .inset .control').click(function () {

        var tp = escapeStr(this.id.toString());// id switch cliccato

        if (!$(this).parent().parent().hasClass('disabled')) {
            if ($(this).parent().parent().hasClass('true'))// se true metto false
            {
                tp = tp.replace("Time", "timepicker");
                $("#" + tp).fadeOut("slow");
                tp = tp.replace("timepicker", "type");
                $("#" + tp).fadeOut("slow");
                $(this).parent().parent().addClass('false').removeClass('true');
            }
            else// se false metto true VISIBILE
            {
                tp = tp.replace("Time", "timepicker");
                $("#" + tp).fadeIn("slow");
                tp = tp.replace("timepicker", "type");
                $("#" + tp).fadeIn("slow");
                $(this).parent().parent().addClass('true').removeClass('false');
            }
        }

    });

    // cambia la classe dello switch tipo per fare tot-par
    $('.type-slider .inset .control').click(function () {

        if (!$(this).parent().parent().hasClass('disabled')) {
            if ($(this).parent().parent().hasClass('T'))// se true metto false
            {
                $(this).parent().parent().addClass('I').removeClass('T');
            }
            else// se false metto true
            {
                $(this).parent().parent().addClass('T').removeClass('I');
            }
        }

    });

    // a tutti i componenti con stile xx aggiungo il timepicker
    $('.ui-timepicker').timepicker({
        showNowButton: true,
        showDeselectButton: true,
        defaultTime: '',  // removes the highlighted time for when the input is empty.
        showCloseButton: true
    });
    // faccio sparire tutti i timepicker e lo switch tot-par se no hanno valori salvati in tabella
    var arrayTime = $('div[id^="Time"]');

    for (i = 1; i < arrayTime.length; i++) {

        var time = arrayTime[i];
        if ($(time).parent().parent().hasClass("false")) {
            var tp = escapeStr(time.id.toString());// id switch cliccato
            tp = tp.replace("Time", "timepicker");
            $("#" + tp).hide(0);
            tp = tp.replace("timepicker", "type");
            $("#" + tp).hide(0);
            $(time).parent().parent().addClass('false').removeClass('true');

        }
    }


    //$(".ui-timepicker").hide(0);
    //$('div[id^="type"]').hide(0);

    // progress salvataggio
    $('div[id^="wait"]').hide(0);

    // rendo visibili solo i primi perchè sono i default
    //$('input[id^="timepicker1"]').fadeIn("fast");
    //$('div[id^="type1"]').fadeIn("fast");


}*/