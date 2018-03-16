﻿//le chiama quando ha finito di caricare la pagina

var intervaljob;// handle degli intervalli
var intervalftplist;
var intervalCharts;

var ajaxCPU, ajaxASP, ajaxJOBS, ajaxUSERS;





// PAGINA CARICATA
$(document).ready(function ()
{
    Grid_FTP_List();
    Grid_JOBSEDD();// jobedd

    $("#modal-RICERCA").modal("hide");

    $("#modal-RICERCA-ERRORILIB").modal("hide");

    $("#btnMsgQError").click(function (event) {

        event.preventDefault();

    });

    $("#btnMsgQAll").click(function (event) {

        event.preventDefault();

    });
   
 

    $("#datetimepicker").kendoDateTimePicker({
        format: "dd/MM/yyyy HH:mm:ss",
        change: onChangeDTPicker,
        footer: "Oggi - #=kendo.toString(data, 'd') #",
        value: setDate()// setto come data NOW
    });
    // tooltips
    $("#upMsgq").kendoTooltip({
        content: "UP",
        position: "right"
    });

    $("#MainContent_ImgStatFtp").kendoTooltip({
        content: "Nessun Errore rilevato nella destinazione remota",
        position: "top"
    });
    $("#MainContent_ImgStatJobs").kendoTooltip({
        content: "Nessun Errore rilevato nel server iSeries",
        position: "top"
    });

      



    $("#ChartUsers").click(function () { usersdetails(); });
    // provo a draggare
    $('.console').kendoDraggable({
        axis: "x",
        hint: Hint,
        dragstart: DragStart,
        drag: Drag,
        dragend: DragEnd
    });


    $("#btnRefresh").click(function (event) {
        
        Grid_FTP_List();

    });

    $("#btnSearch").click(function (event) {
        event.preventDefault();
        $("#tableSearch").html("");// ripulisco
        $("#modal-RICERCA").modal("show");
        clearInterval(intervalCharts);
        clearInterval(intervaljob);

    });

    $("#btnSearchItems").click(function (event) {
        event.preventDefault();
        var testo = $("#txtRicerca").val();
        $("#tableSearch").html("");// ripulisco
        var txtLibreria = $("#txtLibRicerca").val();
        var txtTipo = $("#txtTipoRicerca").val();
        if (testo.trim() == "" && txtLibreria.trim()=="" && txtTipo.trim() == "" )
            alert("Valorizzare almeno una ricerca!");
        else
            search_Items(testo);

    });

    $("#btnSearch-IFS").click(function (event) {
        event.preventDefault();
        $("#tableSearch-IFS").html("");// ripulisco
        $("#modal-RICERCA-IFS").modal("show");
        clearInterval(intervalCharts);
        clearInterval(intervaljob);

    });
    $("#btnSearchItems-IFS").click(function (event) {
        event.preventDefault();
        var testo = $("#txtRicerca-IFS").val();
        $("#tableSearch").html("");// ripulisco
        var txtLibreria = $("#txtObjRicerca-IFS").val();
        var txtTipo = $("#txtTipoRicerca-IFS").val();
        if (testo.trim() == "" && txtLibreria.trim() == "" && txtTipo.trim() == "")
            alert("Valorizzare almeno una ricerca!");
        else
            search_Items_IFS(testo);

    });

    $("#btnSearch-QDLS").click(function (event) {
        event.preventDefault();
        $("#tableSearch-QDLS").html("");// ripulisco
        $("#modal-RICERCA-QDLS").modal("show");
        clearInterval(intervalCharts);
        clearInterval(intervaljob);

    });
   
    $("#btnSearchItems-QDLS").click(function (event) {
        event.preventDefault();
           
        $("#tableSearch").html("");// ripulisco 
        search_Items_QDLS( );

    });

    $("#btnSearchClose").click(function () {

        intervalCharts = setInterval(function () {

            if (ajaxCPU != undefined) {
                if (ajaxCPU.readyState == 4)
                    requestCPU();
            }
            else
                requestCPU();//cpu

            if (ajaxASP != undefined) {
                if (ajaxASP.readyState == 4)
                    requestASP();//asp
            }
            else
                requestASP();//asp



            if (ajaxJOBS != undefined) {
                if (ajaxJOBS.readyState == 4)
                    requestJobs();
            }
            else
                requestJobs();//cpu


            if (ajaxUSERS != undefined) {
                if (ajaxUSERS.readyState == 4)
                    requestUsers();//users
            }
            else
                requestUsers();//users



        }
            , 20000);
        intervaljob = setInterval(Grid_JOBSEDD, 60000); //richiama la funzione ogni tot msec

    });


    GetIseriesProperties();// nome iseries e SN
    ChartCPU();//cpu
    ChartASP();//asp
    ChartJobs();// jobs
    ChartUsers();// users 





    intervalCharts = setInterval(function ()
    {
                 
        if (ajaxCPU != undefined) {
            if (ajaxCPU.readyState == 4)
                requestCPU();
            }
        else
            requestCPU();//cpu

        if (ajaxASP != undefined) {
            if (ajaxASP.readyState == 4)
                requestASP();//asp
        }
        else
            requestASP();//asp



        if (ajaxJOBS != undefined) {
            if (ajaxJOBS.readyState == 4)
                requestJobs();
        }
        else
            requestJobs();//cpu


        if (ajaxUSERS != undefined) {
            if (ajaxUSERS.readyState == 4)
                requestUsers();//users
        }
        else
            requestUsers();//users



    }
    , 20000);
   

    intervaljob = setInterval(Grid_JOBSEDD, 60000); //richiama la funzione ogni tot msec


 

    //// coda messaggi
    var datatimeNow = $("#datetimepicker").data("kendoDateTimePicker").value();
    var dataora = kendo.toString(datatimeNow, 'dd/MM/yyyy HH:mm:ss').toString();
    setTimeout(Grid_MsgCoda(dataora), 30000);



});





//prende alcune proprietà del server: no Kendo
function GetIseriesProperties() {
    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/GetServerProperties",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = (msg.d);
         
            var t = "<table border='0' style=\"color:#FFFFFF;font-weight:bold\"><tr> ";

            var parse = $.parseJSON(data);
            //oggetti restituiti: qui un solo oggetto di due campi
            t += "<td>Server iSeries:" + parse.Name + "</td>";
            t += "<td>SN:" + parse.Serial + "</td>";
            t += "</tr> </table> ";

            $("#iSeriesValueDIV").html(t);
            $('#iseriesdata').fadeIn('slow');
        },
        error: function (msg) {

        }
    });
};

function GetContentFTPFile(file) {
    // formatto l'editor di testo
    $("#editor").css("color", "gray");
    $("#editor").css("font-size", "14px");
    $("#editor").css("font-weight", "bold");
    $("#editor").css("background-color", "white");
    $("#box").css("background-color", "white");
    $("#box").css("color", "gray");
    $("#titoloOverlay").css("color", "gray");

    file = file.replace(".ibm", ".inf").replace(".zip", ".inf").replace(".ZIP", ".inf")

    $("#editor").html("<hr style='color:gray'><br><center>Attendere...</center>");
    TitoloOverlay("File Content: " + file);
    OpenOverlay();// apre l'overlay con il testo del JOBLOG

    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/ReadFtpFile2",
        data: "{'file':'" + file + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = (msg.d);
            if (data.indexOf("Error") != 0) {
                var t = "<table class='CSS_Table_SAVF'> <tr>" +
                    "<td> <strong>Data</strong></td> <td> " +
                    "<strong>Libreria</strong></td> <td> " +
                    "<strong>Oggetto</strong></td> <td> " +
                    "<strong>Tipo</strong></td> <td> " +
                    "<strong>Attributo</strong></td> <td> " +
                    "<strong>Testo</strong></td>" +
                    "</tr> ";

                //alert(data.toString());
                if (data.toString().indexOf('Errore') >= 0) //se c'è scritto errore nel msg
                    $("#editor").html("<hr style='color:gray'><br><center><font style='color:red'>" + data.toString() + "</font></center>");
                else {
                    var parse = $.parseJSON(data);

                    //cicla sull alista degli oggetti restituiti: qui un solo oggetto di due campi
                    $.each(parse, function (i, v) {
                        t = t + "<tr><td>" + v.Campo7 + "</td>" +
                            "<td> " + v.Campo5 + "</td>" +
                            "<td> " + v.Campo8 + "</td>" +
                            "<td> " + v.Campo9 + "</td>" +
                            "<td> " + v.Campo10 + "</td>" +
                            "<td> " + v.Campo13 + "</td>" +
                            "</tr>";
                    });

                    t = t + "</tr></table> ";

                    $("#editor").html("<hr style='color:gray'><br>" + t);
                }
            }
 
        },
        error: function (msg) {

        }
    });
};


//lettura user iseries
function GetIseriesUsers() {
        $.ajax({
            type: "POST",
            url: "IseriesWebServices.asmx/GetUserList",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = (msg.d);
                var t = "<table border='0'> <tr>" +
                    "<td> <strong>Name</strong></td> " +
                    "<td> <strong>Descrizione</strong></td> " +
                    "</tr>";

                var parse = $.parseJSON(data);
                $.each(parse, function (i, v) {

                    t = t + "<tr><td> " + v.Name + "</td>"+"<td> " + v.Descrizione + "</td><td> " + v.lastlogon + "</td></tr>";

                });

                t = t + "   </table> ";

                // $("#jsonDiv").html(t); 
            },
            error: function (msg) {

            }
        });
};

// lettura joblog
function ReadJobLog(par) {
    // formatto l'editor di testo
    $("#editor").css("color", "green");
    $("#editor").css("font-size", "14px");
    $("#editor").css("font-weight", "bold");
    $("#editor").css("background-color", "black");
    $("#box").css("background-color", "black");
    $("#box").css("color", "green");
    $("#titoloOverlay").css("color", "green");

    $("#editor").html("<hr style='color:green'><br><center>Attendere</center>");

    TitoloOverlay("Job Log Details");
    OpenOverlay();// apre l'overlay con il testo del JOBLOG
    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/JOBLOG",
        data: "{'par':'" + par + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = (msg.d);
            t = data;
            $("#editor").html("<hr style='color:green'><br>" + t);
        },
        error: function (msg) {
            //alert("Error");
        }
    });
};

//chiamata per ottnere valore CPU
var chartCPU;
var mediaCPU = 0;

function requestCPU() {
   ajaxCPU= $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/GetServerPerfRealTime',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        //point è un argomento generico di ritorno. point.d è il valore restituito che in questo caso è unico (CPU%)
        success: function (point) {
            var series = chartCPU.series[0], shift = series.data.length > 20; // shift se la serie è più di 20

            chartCPU.series[0].name = "iSeries";
            chartCPU.series[0].addPoint([(new Date()).getTime(), point.d], true, shift);

            if (chartCPU.series[0].points.length > 1)
                //mediaCPU = (chartCPU.series[0].points[chartCPU.series[0].points.length - 1].y + point.d) / 2;
                mediaCPU = (mediaCPU + point.d) / 2;
            else
                mediaCPU = point.d; //primo giro
            try
            {
                chartCPU.series[1].addPoint([(new Date()).getTime(), mediaCPU], true, shift);
            }
            catch (err) { }
 


        },
        error: function (response) {
            // alert(response.responseText);
        },
        cache: false
    });
}
function ChartCPU()
{
    chartCPU = new Highcharts.Chart
        (
            {
                chart: {
                    renderTo: 'chartCPU',// questo significa che il chart sarà renderizzato sul div ChartCPU
                    defaultSeriesType: 'line',
                    events: {
                        load: requestCPU // funzione che richiama il WEB SERVICE
                    }
                },
                title: {
                    text: 'iSeries CPU %'
                },
                xAxis: {
                    type: 'datetime'
                },
                yAxis: {
                    title: {
                        text: 'CPU %' 
                    }
                },
                series: [{
                    name: 'CPU %',
                    data: []
                },
                {
                    type: 'spline',
                    name: 'media',
                    data: []
                }]
            }
        );
}

var chartJobs;
function requestJobs() {
    $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/GetJobsSummary',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        //point è un argomento generico di ritorno. point.d è il valore restituito
        success: function (point) {
            
            var data = $.parseJSON(point.d);// parsifica il json e legge i valori come array
        
            chartJobs.series[0].setData([data[0].Number]);// data[0].Number;          
            chartJobs.series[1].setData([data[1].Number]);//;
            chartJobs.series[2].setData([data[2].Number]);//;
            chartJobs.series[3].setData([data[3].Number]);//;
            chartJobs.series[4].setData([data[4].Number]);//;
            // chiamata ogni n msecondi
            setTimeout(requestJobs, 60000);
        },
        error: function (response) {
            //alert(response.responseText);
        },
        cache: false
    });
}
function ChartJobs()
{   
    chartJobs = new Highcharts.Chart(
    {
            chart: {
                renderTo: 'ChartJobs',
                type: 'column',
                events: {
                    load: requestJobs // funzione che richiama il WEB SERVICE
                }
            },
            title: {
                text: 'Jobs Summary'
            },
            subtitle: {
                text: 'iseries jobs'
            },
            xAxis: {
                categories: ['Jobs']
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'number'
                }
            },
            legend: {
                layout: 'vertical',
                backgroundColor: '#FFFFFF',
                align: 'left',
                verticalAlign: 'top',
                x: 50,
                y: 10,
                floating: true,
                shadow: true
            },
            tooltip: {
                formatter: function () {
                    return  this.series.name +' ' +
                        this.x + ': ' + this.y;
                }
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: 'Active',
                data: []
            }, {
                name: 'Total',
                data: []           
            },
            {
                name: 'Batch',
                data: []
            },
            {
                name: 'MSGW Batch',
                data: []
            },
            {
                name: 'Active Thread',
                data: []
            }
            ]
         
    });

};

var chartASP;
function requestASP() {
   ajaxASP = $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/GetASPPercentage',
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (point) {
            var data = point.d;

            chartASP.series[0].name = "iSeries"; 
            chartASP.series[0].setData([data]);   
           
 
        },
        error: function (response) {

           // alert("Err: "+ response.responseText);

        },
        cache: false
    });
}
function ChartASP() {
     
    chartASP = new Highcharts.Chart(
        {
            chart: {
                renderTo: 'ChartASP',
                type: 'column',
                events: {
                    load: requestASP // funzione che richiama il WEB SERVICE
                }
            },
            title: {
                text: '% ASP'
            },
            subtitle: {
                text: 'occupazione Disco'
            },
            xAxis: {
                categories: ['ASP']
            },
            yAxis: {
                min: 0,
                title: {
                    text: '%'
                }
            },
            legend: {
                /*layout: 'vertical',
                backgroundColor: '#FFFFFF',
                align: 'left',
                verticalAlign: 'top',
                x: 50,
                y: 10,
                floating: true,
                shadow: true*/
            },
            tooltip: {
                formatter: function () {
                    return this.series.name + ' ' +
                        this.x + ': ' + this.y;
                }
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{ name: 'ASP%', data: [] }]

        });
    }
 
var chartUsers;
function requestUsers() {
    $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/GetUsersSignedOn',
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (point) {
            var data = point.d;

            chartUsers.series[0].name = "iSeries";
            chartUsers.series[0].setData([data]);

            // chiamata ogni n secondi
            setTimeout(requestUsers, 60000);
        },
        error: function (response) {

            //alert("Err: "+response.responseText);

        },
        cache: false
    });
}
function ChartUsers() {

    chartUsers = new Highcharts.Chart(
        {
            chart: {
                renderTo: 'ChartUsers',
                type: 'column',
                events: {
                    load: requestUsers // funzione che richiama il WEB SERVICE
                }
            },
            title: {
                text: 'Users'
            },
            subtitle: {
                text: 'n° users connessi'
            },
            xAxis: {
                categories: ['Users']
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'n°'
                }
            },
            legend: {
                /*layout: 'vertical',
                backgroundColor: '#FFFFFF',
                align: 'left',
                verticalAlign: 'top',
                x: 50,
                y: 10,
                floating: true,
                shadow: true*/
            },
            tooltip: {
                formatter: function () {
                    return this.series.name + ' ' +
                        this.x + ': ' + this.y;
                }
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                },
                series: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                usersdetails();// visualizzo chi è connesso
                            }
                        }
                    }
                }
            },
            
            series: [{ name: 'Users', data: [] }]

        });
}

function Grid_FTPList_old() {
    Data = new kendo.data.DataSource
    (
        {
            type: "json",
            transport: {
                read: {
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "IseriesWebServices.asmx/FTP_LISTFILE",
                    datatype: "json"
                }
            },
            //pageSize: 300,   
            schema:
            {
                data: "d",
                total: "total",
                model:
                {

                    fields:
                        {
                            Name: { type: "string" },
                            CreateTime: { type: "string" },
                            CreateTimeFrom:{ type: "string" },
                            TransferTime: { type: "string" },
                            Owner: { type: "string" },
                            Group: { type: "string" },
                            Flags: { type: "string" },
                            IsDirectory: { type: "boolean" },
                            Size: { type: "string" }
                        }
                }
            }
        }
    );

    $("#GridFTPList").kendoGrid({
        dataSource: Data,            
        columns: [
            { field: "Name", title: "Nome", width: 85 },
            { field: "CreateTime", title: "CreateTime", width: 35 },
            //{ field: "CreateTimeFrom", title: "CreateTimeFrom", width: 25 },
            { field: "TransferTime", title: "TransferTime", width: 30 },
            { field: "Size", title: "Size Kb", width: 20 },
            
        ],
        height: 640,
        change: onClick,
        dataBound: 
            function(e){
                 Ftphavefile(e);// se non ho file rosso altrimenti verde
            },
        selectable: "multiple cell",
       // pageSize:10,
        scrollable: true,
        //pageable: {
        //    refresh: true,
        //    pageSizes: true
        //},
        sortable: {
            mode: "multiple"
        }
    });

    // lettura contentuto file FTP sulla selezione della riga della grid
    function onClick(arg) {

        clearInterval(intervalftplist);
        clearInterval(intervaljob);

        var selected = $.map(this.select(), function (item) {              
            return $(item).text();
        });

        //ReadFile(selected);// lettura file old
      
        var fileinf = (selected[0].toString().replace(".ibm", ".inf")).replace(".zip", ".inf").replace(".ZIP", ".inf");// così leggo sempre l'inf
        if (fileinf.indexOf('Errore') == -1)// se non c'è scritto errore
        {
            GetContentFTPFile(fileinf);
            kendoConsole.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");
        }
        else {
            scrollwinUp();
            $("#message").html('<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>'+
		                        '<strong>Errore:</strong> Il file selezionato non esiste');
            $("#errorMessage").fadeIn('slow');
            setTimeout(hideError, 2000);
        }
        
           
    }

    function hideError()
    {
        $("#errorMessage").fadeOut('slow');
    }

    // funzione  controllo righe -  se ci sono righe significa che FTP funziona
    function Ftphavefile(e) {
        var grid = e.sender;


        var data = grid.dataSource.data();

        var foundError=false;
        if (data.length == 0) {
            $("#MainContent_ImgStatFtp").attr('src', "Images/red_16.png");          
        }
        else {
           
            for (var i = 0; i <= data.length-1; i++)
            {
                var dataItem = data[i];
                var tr = $("#GridFTPList").find("[data-uid='" + dataItem.uid + "']");
                if (data[i].Name.toString().indexOf("Errore") != -1) {                 
                    foundError = true;
                    tr.css("color", "red");
                }
                else {
                   
                    tr.css("color","green");
                    
                     
                }
                   
            }

            if(!foundError)
                $("#MainContent_ImgStatFtp").attr('src', "Images/green_16.png");
            else
                $("#MainContent_ImgStatFtp").attr('src', "Images/red_16.png");
        }
    }

}


function Grid_FTP_List() {

    var row_template = "<tr style='color:green' id='row#SEP##NAME#'><td>#NAME#</td><td>#CREATETIME#</td><td>#SIZE#</td></tr>";
    var t = "";
    $("#table-FTP_File tbody").html('');
    $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/FTP_LISTFILE',
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (res) {
            var data = res.d;

            var rows =  data;
            $.each(rows, function (index, f) {


                t = row_template.replace(/#NAME#/g,f.Name).replace("#CREATETIME#",f.CreateTime).replace("#SIZE#",f.Size + " Kb");

                $("#table-FTP_File tbody").append(t);
            });
            $("#table-FTP_File tbody [id^=row#SEP#]").hover(function () { $(this).css({ 'cursor': 'pointer' }); })// cambio il puntatore quando passo sopra

            $("#table-FTP_File tbody [id^=row#SEP#]").click(function () {
                var fileinf = $(this)[0].id.split("#SEP#")[1];
                GetContentFTPFile(fileinf);


            });
     
        },
        error: function (response) {

            //alert("Err: "+response.responseText);

        },
        cache: false
    });



}
// grid dei jobs
function Grid_JOBSEDD() {


    var row_template = "<tr style='color:green' >"
        + "<td>#SUBSYSTEM#</td> <td>#NAME#</td> <td>#NUMBER#</td>"
        + "<td>#STATUS#</td> <td>#ACTIVESTATUS#</td> <td>#USER#</td>"
        + "</tr> ";


    var t = "";
    $("#table-JOBEDD tbody").html('');
    $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/JOBLIST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (res) {
            var data = res.d;

            var rows = data;
            $.each(rows, function (index, f) {


                t = row_template
                    .replace("#SUBSYSTEM#", f.Subsystem).replace("#NUMBER#", f.Number).replace("#STATUS#", f.Status )
                    .replace("#ACTIVESTATUS#", f.ActiveStatus).replace("#USER#", f.User).replace(/#NAME#/g, f.Name);

                $("#table-JOBEDD tbody").append(t);
            });
            

        },
        error: function (response) {

            //alert("Err: "+response.responseText);

        },
        cache: false
    });


}
 

// lettura CODA MESSAGGI passando la data del datetimepicker
function Grid_MsgCoda(datax,severity)
{
    if (datax == undefined)
        datax = "01/01/1970 00:00:00";// data old

    if (severity == undefined) severity = "0";// maggiore uguale a zero

    var command = { datax: datax, Severity: severity };
    var jsonData = JSON.stringify(command);// converto in JSON e invio

    // lettura dati da sorgente
    Data = new kendo.data.DataSource(
        {
            type: 'json',             
            transport:
            {
                read: {
                    type: 'POST',                       
                    url: 'IseriesWebServices.asmx/GetMsgCoda',
                    datatype: 'json',                        
                    contentType: 'application/json; charset=UTF-8',
                    cache: false,
                    data: { datax: "" + datax + "", Severity: "" + severity + "" }
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                },
               // pageSize: 10
            },
            schema:
            {
                data: "d",
                model:
                {

                    fields:
                        {
                            Msgid: { type: "string" },
                            datatime: { type: "string" },
                            Testo: { type: "string" },
                            severity: { type: "int" },
                            Help: {type:"string"}
                        }
                }
            },
            error: function (e) {
               // alert("Err: "+e.responseText);
            }
          
        }
        );

    

        $("#GridMsgCoda").kendoGrid({
            dataSource: Data, 
                    
            columns: [
                { field: "datatime", title: "Data", width: 40 },
                { field: "severity", title: "severity", width: 20 },
                { field: "Msgid", title: "ID", width: 40},
                { field: "Testo", title: "Messaggio", width: 200 },
                { field: "Help", title: "Dettagli", width: 250 }
            ],
            height: 250,
            dataBound: function (e)
            {
                haveMessage(e);
            },
            scrollable: true,
            //pageable: {
            //    refresh: true,
            //    pageSizes: true
            //},
            sortable: {
                mode: "multiple"
            }
        });
    

   

    // funzione  controllo righe al databound 
    function haveMessage(e) {

        // al caricamento dei dati
        var grid = e.sender;
        var data = grid.dataSource.data();// contiene tutte le righe è un array di righe

        // modifico se trovo dati con immagine rossa o verde
        if (data.length > 0)
        {

            var foundError = false;

            for (var i = 0; i <= data.length - 1; i++) {
                var dataItem = data[i];
                var tr = $("#GridMsgCoda").find("[data-uid='" + dataItem.uid + "']");
                if (parseInt(data[i].severity)>0) {
                    foundError = true;
                    tr.css("color", "red");
                }
                else {

                    tr.css("color", "green");


                }

            }
            if(foundError)
                $("#MainContent_ImgStatCoda").attr('src', "Images/red_16.png");
            else
                $("#MainContent_ImgStatCoda").attr('src', "Images/green_16.png");
        }
        else
            $("#MainContent_ImgStatCoda").attr('src', "Images/green_16.png");



    }



}
 
// creazione data NOW
function setDate()
{
    var currentTime = new Date();
    var month = currentTime.getMonth();
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var dataOggi = new Date(year,month,day)

    return dataOggi;

}

 // selezione data per filtrare i messaggi della coda
function onChangeDTPicker() {
    kendoConsole.log("Change :: " + kendo.toString(this.value(), 'dd/MM/yyyy HH:mm:ss'));

    Grid_MsgCoda(kendo.toString(this.value(), 'dd/MM/yyyy HH:mm:ss'));
}


// OVERLAY window 
function chiudiOverlay()
{
    intervaljob = setInterval(Grid_JOBSEDD, 60000); //richiama la funzione ogni tot msec
    //intervalftplist = setInterval(Grid_FTPList, 60000); //richiama la funzione ogni tot msec

    $('#overlay').fadeOut('fast');
    $('#box').hide();
}
// APRI
function OpenOverlay()
{
    $('#overlay').fadeIn('fast');
    $('#box').fadeIn('slow');
}

// TITOLO
function TitoloOverlay(titolo)
{
    document.getElementById('titoloOverlay').innerHTML = titolo;
}



////// serve per posizionaci in un punto della pagina in modo fluido
/*

 = Is Equal
!= Is Not Equal
^= Starts With
$= Ends With
*= Contains

Tutti gli a con haref che contengono il cancelletto

*/
$('a[href*=#]').click(function () {
    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
  && location.hostname == this.hostname) {
        var $target = $(this.hash);
        $target = $target.length && $target || $('[name=' + this.hash.slice(1) + ']');
        if ($target.length) {
            var targetOffset = $target.offset().top;
            $('html,body').animate({ scrollTop: targetOffset }, 1000);
            return false;
        }
    }
});

function scrollwinUp()// scrolla la pagina quando chiamato verso il name specificato
{
    // topPage si trova nel primo div del master e quindi scrolla fino in alto
    $('html,body').animate({
        scrollTop: $("#topPage").offset().top
    }, 800);
     
}





// funzioni per drag
function Hint(element) {
    return element.clone();
}

function DragStart() {
    kendoConsole.log("dragstart");
}

function Drag() {
    kendoConsole.log("draging");
}

function DragEnd(event) {
    kendoConsole.log("dragend");
    kendoConsole.log(event.x.location);
    $('.console').css('left', "'"+event.x.location+"'");
}
 
// 17.06.2013 funzione per visualizzare gli user connessi
function usersdetails() {
    // formatto l'editor di testo
    $("#editor").css("color", "green");
    $("#editor").css("font-size", "14px");
    $("#editor").css("font-weight", "bold");
    $("#editor").css("background-color", "black");
    $("#box").css("background-color", "black");
    $("#box").css("color", "green");
    $("#titoloOverlay").css("color", "green");

    $("#editor").html("<hr style='color:green'><br><center>Attendere</center>");
    TitoloOverlay("User Connessi");

    $.ajax({
        url: "IseriesWebServices.asmx/GetInteractiveUsers",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: {},
        dataType: "json",
        success: function (json) {

            var data = (json.d);
           


            var t = "<table border='0' style='color:green;font-size:9px;width:100%;'>";

          

            for (i = 0; i < data.length; i++)

                t = t + "<tr><td>" + data[i].UserName + "</td><td style='width:auto'>"
                    + data[i].LogonDateTime + "</td><td>"
                    + data[i].ActiveStatus + "</td><td>"
                    + data[i].JobUser + "</td></tr>";

            t = t + "</table>";

           
            $("#editor").html("<hr style='color:green'><br>" + t);
            OpenOverlay();
        },
        error: function (x, json) {

            if (x.status = 200)
                alert("Err: "+x.statusText);


        },
        cache: false
    });
}


 
 


function get_Message_by_Severity(severity)
{
    
    //// coda messaggi
    var datatimeNow = $("#datetimepicker").data("kendoDateTimePicker").value();
    var dataora = kendo.toString(datatimeNow, 'dd/MM/yyyy HH:mm:ss').toString();
    setTimeout(Grid_MsgCoda(dataora, severity), 30000);// solo errori oppure tutti
}


function search_Items(testo) {
    $("#imgWait").show();

    testo = testo.toUpperCase();// tutta maiuscola la ricerca per portarlo a CASE INSENSITIVE
    var txtLibreria = $("#txtLibRicerca").val().toUpperCase().trim();
    var txtTipo = $("#txtTipoRicerca").val().toUpperCase().trim();


    var command = { txtSearch: testo.trim(), txtLibreria: txtLibreria, txtTipo: txtTipo };

    var jsonData = JSON.stringify(command);// converto in JSON e invio

    var TEMPLATE_TABELLA = "<thead><tr><th>FileName</th><th>Object Name</th><th>Object Type</th><th>Library Name</th> <th>SaveTime</th></tr></thead><tbody>#RIGHE#</tbody>";
    var TEMPLATE_RIGA = "<tr><td>#NOMEFA#</td><td>#SROMNM#</td><td>#SROTYP#</td><td>#SROLIB#</td><td>#SROSVT#</td></tr>";

    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/search_Items",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var t_riga = "";
            var data = (msg.d);
            var rows = $.parseJSON(data);

            $.each(rows, function (key, value) {
                var NOMEFA =  value.NOMEFA;
                var SROMNM =   value.SROMNM  ;
                var SROTYP =   value.SROTYP ;
                var SROLIB =  value.SROLIB  ;
                var SROSVT =   value.SROSVT  ;
                var SROTYP_X = SROTYP;
                var SROLIB_X = SROLIB;

                if (NOMEFA.toString().toUpperCase().indexOf(testo) > -1 && testo!="") {
                    NOMEFA = highlight_Text(NOMEFA, NOMEFA.toString().toUpperCase().indexOf(testo), testo.length);
                }
                if (SROMNM.toString().indexOf(testo) > -1 && testo != "") {
                    SROMNM = highlight_Text(SROMNM, SROMNM.toString().toUpperCase().indexOf(testo), testo.length);
                }

                if (SROTYP.toString().indexOf(testo) > -1 && testo != "") {
                    SROTYP_X = highlight_Text(SROTYP, SROTYP.toString().toUpperCase().indexOf(testo), testo.length);
                }
                else if (txtTipo != "")
                {
                    SROTYP_X = highlight_Text(SROTYP, SROTYP.toString().toUpperCase().indexOf(txtTipo), txtTipo.length);
                     
                }

                if (SROLIB.toString().indexOf(testo) > -1 && testo != "") {
                    SROLIB_X = highlight_Text(SROLIB, SROLIB.toString().toUpperCase().indexOf(testo), testo.length);
                }
                else if(txtLibreria!=""){
                    SROLIB_X = highlight_Text(SROLIB, SROLIB.toString().toUpperCase().indexOf(txtLibreria), txtLibreria.length);
                }

                if (SROSVT.toString().indexOf(testo) > -1 && testo != "")
                    SROSVT = highlight_Text(SROSVT, SROSVT.toString().toUpperCase().indexOf(testo), testo.length);


                t_riga += TEMPLATE_RIGA.replace("#NOMEFA#", NOMEFA).replace("#SROMNM#", SROMNM)
                        .replace("#SROTYP#", SROTYP_X).replace("#SROLIB#", SROLIB_X).replace("#SROSVT#", SROSVT);
          



            });

            var tabella = TEMPLATE_TABELLA.replace("#RIGHE#", t_riga);
            $("#tableSearch").html(tabella);
            $("#imgWait").hide();
        },
        error: function (ex) {
            $("#imgWait").hide();
        }

    });
}


function search_Items_IFS(testo) {
    $("#imgWait-IFS").show();

    testo = testo.toUpperCase();// tutta maiuscola la ricerca per portarlo a CASE INSENSITIVE
    var txtOggetto = $("#txtObjRicerca-IFS").val().toUpperCase().trim();
    var txtTipo = $("#txtTipoRicerca-IFS").val().toUpperCase().trim();


    var command = { txtSearch: testo.trim(), txtOggetto: txtOggetto, txtTipo: txtTipo };

    var jsonData = JSON.stringify(command);// converto in JSON e invio

    var TEMPLATE_TABELLA = "<thead><tr><th>FileName</th><th>Dir</th><th>Object Name</th><th>Object Type</th><th>SaveTime</th></tr></thead><tbody>#RIGHE#</tbody>";
    var TEMPLATE_RIGA = "<tr><td>#NOMEFA#</td><td>#SRDIR#</td><td>#SROBJ#</td><td>#SRTIP#</td><td>#SRDAT#</td></tr>";

    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/search_Items_IFS",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var t_riga = "";
            var data = (msg.d);
            var rows = $.parseJSON(data);

            $.each(rows, function (key, value) {
                var NOMEFA = value.NOMEFA;
                var SRDIR = value.SRDIR;
                var SRTIP = value.SRTIP;
                var SROBJ = value.SROBJ;
                var SRDAT = value.SRDAT + value.SRTIM;

                var SRTIP_X = SRTIP;
                var SROBJ_X = SROBJ;

                if (NOMEFA.toString().toUpperCase().indexOf(testo) > -1 && testo != "") {
                    NOMEFA = highlight_Text(NOMEFA, NOMEFA.toString().toUpperCase().indexOf(testo), testo.length);
                }
                if (SRDIR.toString().indexOf(testo) > -1 && testo != "") {
                    SRDIR = highlight_Text(SRDIR, SRDIR.toString().toUpperCase().indexOf(testo), testo.length);
                }

                if (SRTIP.toString().indexOf(testo) > -1 && testo != "") {
                    SRTIP_X = highlight_Text(SRTIP, SRTIP.toString().toUpperCase().indexOf(testo), testo.length);
                }
                else if (txtTipo != "") {
                    SRTIP_X = highlight_Text(SRTIP, SRTIP.toString().toUpperCase().indexOf(txtTipo), txtTipo.length);

                }

                if (SROBJ.toString().indexOf(testo) > -1 && testo != "") {
                    SROBJ_X = highlight_Text(SROBJ, SROBJ.toString().toUpperCase().indexOf(testo), testo.length);
                }
                else if (txtOggetto != "") {
                    SROBJ_X = highlight_Text(SROBJ, SROBJ.toString().toUpperCase().indexOf(txtOggetto), txtOggetto.length);
                }

 
                t_riga += TEMPLATE_RIGA.replace("#NOMEFA#", NOMEFA).replace("#SRDIR#", SRDIR)
                    .replace("#SROBJ#", SROBJ_X).replace("#SRTIP#", SRTIP_X ).replace("#SRDAT#", SRDAT);




            });

            var tabella = TEMPLATE_TABELLA.replace("#RIGHE#", t_riga);
            $("#tableSearch-IFS").html(tabella);
            $("#imgWait-IFS").hide();
        },
        error: function (ex) {
            $("#imgWait-IFS").hide();
        }

    });
}




function search_Items_QDLS( ) {
    $("#imgWait-QDLS").show();

     
    var txtOggetto = $("#txtObjRicerca-QDLS").val().toUpperCase().trim();
    var Tipo = $("#comboTipo").val();


    var command = { txtSearch: "", txtOggetto: txtOggetto, Tipo: Tipo };

    var jsonData = JSON.stringify(command);// converto in JSON e invio

    var TEMPLATE_TABELLA = "<thead><tr><th>FileName</th><th>DOC Name</th><th>Path</th><th>Description</th><th>Type</th><th>DateTime</th></tr></thead><tbody>#RIGHE#</tbody>";
    var TEMPLATE_RIGA = "<tr><td>#NOMEFA#</td><td>#SDLDOC#</td><td>#SDLPTH#</td><td>#SDLDSC#</td><td>#SDLTYP#</td><td>#SDLDAT#</td></tr>";

    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/search_Items_QDLS",
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var t_riga = "";
            var data = (msg.d);
            var rows = $.parseJSON(data);

            $.each(rows, function (key, value) {
                //NOMEFA,SDLDOC,SDLPTH,SDLDSC,SDLDAT,SDLTIM 
                var NOMEFA = value.NOMEFA;
                var SDLDOC = value.SDLDOC;
                var SDLPTH = value.SDLPTH;
                var SDLDSC = value.SDLDSC;
                var SDLTYP = value.SDLTYP;
                var SDLDAT = value.SDLDAT +" " + value.SDLTIM;

                if (SDLTYP.trim() == "1")
                    SDLTYP = "Folder"
                else
                    SDLTYP = "Document";

               
                var SDLDOC_X = SDLDOC;

        

                t_riga += TEMPLATE_RIGA.replace("#NOMEFA#", NOMEFA).replace("#SDLDOC#", SDLDOC).replace("#SDLPTH#", SDLPTH)
                    .replace("#SDLDOC#", SDLDOC_X).replace("#SDLDSC#", SDLDSC).replace("#SDLTYP#", SDLTYP).replace("#SDLDAT#", SDLDAT);




            });

            var tabella = TEMPLATE_TABELLA.replace("#RIGHE#", t_riga);
            $("#tableSearch-QDLS").html(tabella);
            $("#imgWait-QDLS").hide();
        },
        error: function (ex) {
            $("#imgWait-QDLS").hide();
        }

    });
}





function highlight_Text (testo, start, end) {
    var str = testo;
    
    str = str.substr(0, start) +
        '<label style="background-color:yellow">' +
        str.substr(start, end) +
        '</label>' +
        str.substr(start + end);

        return str;
}

// estraggo il nome del file di log dal messaggio per ERROR LIB
function get_FileName_ErroreLib(messaggio)
{ 

    var regEx = /.*libreria\s(.*)\ssono.*/ig;
    var data = messaggio;
    var match = regEx.exec(data);
    console.log(match[1]);

    return match[1];
    /*while (match !== null) {
        console.log(match[1]);
        match = regEx.exec(data);
    }*/

}

function get_FileName_IFSLog(messaggio) {

    var regEx = /^.*allegato "(.*)"$/ig;
    var data = messaggio;
    var match = regEx.exec(data);
    console.log(match[1]);

    return match[1];
    /*while (match !== null) {
        console.log(match[1]);
        match = regEx.exec(data);
    }*/

}