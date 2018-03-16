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
            t = t + "<td>Server iSeries:" + parse.Name + "</td>";
            t = t + "<td>SN:" + parse.Serial + "</td>";
            t = t + "</tr> </table> ";

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
           
            var t = "<table class='CSS_Table_SAVF'> <tr>" +
                "<td> <strong>Data</strong></td> <td> " +
                "<strong>Libreria</strong></td> <td> " +
                "<strong>Oggetto</strong></td> <td> " +
                "<strong>Tipo</strong></td> <td> " +
                "<strong>Attributo</strong></td> <td> " +
                "<strong>Testo</strong></td>"+                
                "</tr> ";

            //alert(data.toString());
            if (data.toString().indexOf('Errore') >= 0) //se c'è scritto errore nel msg
                $("#editor").html("<hr style='color:gray'><br><center><font style='color:red'>" + data.toString() + "</font></center>");
            else
            {
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
var mediaCPU=0;
function requestCPU() {
    $.ajax({
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

            // chiamata ogni n msecondi
            setTimeout(requestCPU, 10000);
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
            setTimeout(requestJobs, 10000);
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
    $.ajax({
        type: "POST",
        url: 'IseriesWebServices.asmx/GetASPPercentage',
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (point) {
            var data = point.d;

            chartASP.series[0].name = "iSeries"; 
            chartASP.series[0].setData([data]);   
           
            // chiamata ogni n secondi
            setTimeout(requestASP, 1000);
        },
        error: function (response) {

            alert("Err: "+ response.responseText);

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
            setTimeout(requestUsers, 1000);
        },
        error: function (response) {

            alert("Err: "+response.responseText);

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

function Grid_FTPList() {
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
            pageSize: 300,   
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
            { field: "Name", title: "Nome", width: 60 },
            { field: "CreateTime", title: "CreateTime", width: 30 },
            { field: "CreateTimeFrom", title: "CreateTimeFrom", width: 30 },
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
        pageSize:10,
        scrollable: true,
        pageable: {
            refresh: true,
            pageSizes: true
        },
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
// grid dei jobs
function Grid_JOBSEDD()
{
    Data = new kendo.data.DataSource
    (
        {
            type: "json",
            transport: {
                read: {
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "IseriesWebServices.asmx/JOBLIST",
                    datatype: "json"
                }
                   
            },
            pageSize: 10,
            schema:
            {
                data: "d",
                model:
                {

                    fields:
                        {
                            Subsystem: { type: "string" },
                            Name: { type: "string" },
                            Number : { type: "string" },
                            Status: { type: "string" },
                            ActiveStatus: { type: "string" },
                            User: { type: "string" }
                        }
                }
            }
           
               
            

        }
    );
    jobgrid = $("#GridJobs").kendoGrid({
        dataSource: Data,

        columns: [
                        { field: "Subsystem", title: "Subsystem", width: 60 },
                        { field: "Name", title: "Name", width: 70 },
                        { field: "Number", title: "Number", width: 60 },
                        { field: "Status", title: "Status", width: 60 },
                        { field: "ActiveStatus", title: "ActiveStatus", width: 65 },
                        { field: "User", title: "User", width: 60 }

        ],
        height: 200,
        change: function (e) {
                rowSelect(this); //evento di click sulla riga

            
        },
        dataBound: function(e) {
            haveRows(e);
        },
        selectable: "row",
        pageSize: 10,
        scrollable: true,
        pageable: {
            refresh: true,
            pageSizes: true
        },
        sortable: {
            mode: "multiple"
        }
    });

    // funzione di selezione della riga e lettura joblog
    function rowSelect(e)
    {
        clearInterval(intervaljob);
        clearInterval(intervalftplist);
        var selectedRows = e.select();
        var selectedDataItems = [];
        
        for (var i = 0; i < selectedRows.length; i++) {
            var dataItem = e.dataItem(selectedRows[i]);
            selectedDataItems.push(dataItem);
        }
        if (selectedDataItems.length > 0) {
            var currentDataItem = selectedDataItems[0];
            kendoConsole.log("Selected: " + currentDataItem.Name);
            var par = currentDataItem.Name + "," + currentDataItem.User + "," + currentDataItem.Number;
            ReadJobLog(par);
        }
         
    }

    // funzione  controllo righe
    function haveRows(e) {
        try{
            var grid = e.sender;
            var data = grid.dataSource.data();

            if (data.length > 0)
                $("#MainContent_ImgStatJobs").attr('src', "Images/red_16.png");
            else
                $("#MainContent_ImgStatJobs").attr('src', "Images/green_16.png");
        }catch(error)
        {
            alert("Err: "+error.message);
        }
    } 
}

// lettura CODA MESSAGGI passando la data del datetimepicker
function Grid_MsgCoda(datax)
{
    if (datax == undefined)
        datax = "01/01/1970 00:00:00";// data old
     
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
                    data: { datax: "" + datax + "" }
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                },
                pageSize: 10
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
                alert("Err: "+e.responseText);
            }
          
        }
        );


    $("#GridMsgCoda").kendoGrid({
        dataSource: Data, 
                    
        columns: [
            { field: "datatime", title: "Data", width: 40 },
            { field: "severity", title: "severity", width: 17 },
            { field: "Msgid", title: "ID", width: 20},
            { field: "Testo", title: "Messaggio", width: 100 },
            { field: "Help", title: "Dettagli", width: 250 }
        ],
        height: 250,
        dataBound: function (e)
        {
            haveMessage(e);
        },
        scrollable: true,
        pageable: {
            refresh: true,
            pageSizes: true
        },
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
            $("#MainContent_ImgStatCoda").attr('src', "Images/red_16.png");
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
    intervaljob = setInterval(Grid_JOBSEDD, 30000); //richiama la funzione ogni tot msec
    intervalftplist = setInterval(Grid_FTPList, 30000); //richiama la funzione ogni tot msec

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


//le chiama quando ha finito di caricare la pagina

var intervaljob;// handle degli intervalli
var intervalftplist;

 
 



// PAGINA CARICATA
$(document).ready
(
    function () {

        
        GetIseriesProperties();// nome iseries e SN
        ChartCPU();//cpu
        ChartASP();//asp
        ChartJobs();// jobs
        ChartUsers();// users
        Grid_FTPList();//lista file
        Grid_JOBSEDD();// jobedd
        intervaljob = setInterval(Grid_JOBSEDD, 30000); //richiama la funzione ogni tot msec
        intervalftplist = setInterval(Grid_FTPList, 30000); //richiama la funzione ogni tot msec
      
        //GetIseriesUsers();
        //GetFtpFileList();
        //ReadFile();
        //Grid_properties();
        //ReadFile();

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

        var datatimeNow = $("#datetimepicker").data("kendoDateTimePicker").value();
        // recupero valore attuale e filtro il gg attuale
      
        var dataora = kendo.toString(datatimeNow, 'dd/MM/yyyy HH:mm:ss').toString();
        setInterval(Grid_MsgCoda(dataora), 30000); //richiama la funzione ogni tot msec
       

        $("#ChartUsers").click(function () { usersdetails(); });
        // provo a draggare
        $('.console').kendoDraggable({
            axis: "x",
            hint: Hint,
            dragstart: DragStart,
            drag: Drag,
            dragend: DragEnd
        });
        
    }
  

);



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


 


 

