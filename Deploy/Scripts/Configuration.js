$J = jQuery.noConflict(true);

var libgrid;
var destgrid;
// genero l'accordion con jquery
$J(function () {
    $J("#accordion").accordion();// questo per usare jquery ui altrimenti va in conflitto con KENDO
    $J('#BtnSaveLib').button();
    $J('#BtnSaveLib').click(function () { readLibrariesToSave(); });
    $J('#BtnReload').button();
    $J('#BtnReload').click(function () { Grid_Destination(); });
    $J('#BtnSchedx').button();
    $J('#BtnSchedx').click(
        function () {
            window.location = "/scheduled.aspx";
        }
        ); 
    $J('#BtnSaveFtp').button(); 
    $J('#BtnSaveFtp').click(function () { Save_FTP_data(); });
    $J('#BtnTestFtp').button();
    $J('#BtnTestFtp').click(function () { Test_FTP(); });
    $J('#wait').hide(0);
    $J('#waitftp').hide(0);
});



// salva i dati su database dell'ftp
function Save_FTP_data()
{
    $('#waitftp').fadeIn("fast");// mostra il wait
    var address = document.getElementById("MainContent_TxtIndirizzo").value;
    var port = document.getElementById("MainContent_TxtPort").value;
    var path = document.getElementById("MainContent_TxtPath").value;
    var user = document.getElementById("MainContent_TxtUser").value;
    var pwd = document.getElementById("MainContent_TxtPwd").value;
    var ccsid = "";//document.getElementById("MainContent_TxtCCSID").value;
    var tipo = document.getElementById("MainContent_DDLSisOp").value;
    var sftp = document.getElementById("MainContent_checkSftp");

    var key = "";
    if (sftp.checked) {
        sftp = "S";
        var key = document.getElementById("MainContent_TxtKey").value;
    }
    else sftp = "F";
     


    
   
    ftp = { Address: address, Port: port, User: user, Pwd: pwd, Path: path, Tipo: tipo, CCSID: ccsid, Secure: sftp, Key: key };
    var jsonftp = JSON.stringify(ftp);// converto in JSON e invio
     
    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/Save_FTP_Parameters",
        data: "{'JsonFtp':'" + jsonftp + "'}" ,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#waitftp').fadeOut("fast");// nascondi il wait
            // messaggio di fine salvataggio
            $("#messageFtp").html('<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>' +
                                       '<strong>INFO:</strong> Parametri FTP salvati correttamente');
            $("#MsgFTP").fadeIn('slow');
            setTimeout(hideMessage, 2000);
            $('#BtnSaveFtp').fadeOut('slow');// faccio apparire il salva
        },
        error: function (msg) {
            alert(msg.statusText);
        }
    });
}


function Test_FTP() {

    $('#waitftp').fadeIn("fast");// mostra il wait
    var address = document.getElementById("MainContent_TxtIndirizzo").value;
    var port = document.getElementById("MainContent_TxtPort").value;
    var path = document.getElementById("MainContent_TxtPath").value;
    var user = document.getElementById("MainContent_TxtUser").value;
    var pwd = document.getElementById("MainContent_TxtPwd").value;
    var ccsid = "";//document.getElementById("MainContent_TxtCCSID").value;
    var tipo = document.getElementById("MainContent_DDLSisOp").value;
    var sftp = document.getElementById("MainContent_checkSftp");

    var key = "";
    if (sftp.checked) {
        sftp = "S";
        var key = document.getElementById("MainContent_TxtKey").value;
    }
    else sftp = "F";
 


    ftp = { Address: address, Port: port, User: user, Pwd: pwd, Path: path, Tipo: tipo, CCSID: ccsid, Secure: sftp, Key: key };
    var jsonftp = JSON.stringify(ftp);// converto in JSON e invio
    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/FTP_CHECK",
        data: "{'JsonFtp':'" + jsonftp + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == "OK") {
                $('#waitftp').fadeOut("fast");// nascondi il wait
                // messaggio di fine salvataggio
                $("#messageFtp").html('<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>' +
                                           '<strong>INFO:</strong> Connessione FTP effettuata correttamente');
                $("#MsgFTP").fadeIn('slow');
                setTimeout(hideMessage, 2000);
                $('#BtnSaveFtp').fadeIn('slow');// faccio apparire il salva
                
                ////////////////////
            }
            else
                alert(msg.d);
        },
        error: function (msg) {
            alert("Error");
        }
    });
};

// grid con le librerie da salvare 
function Grid_Destination()
{
    dataDest = new kendo.data.DataSource
        (
            {
                type: "json",
                transport: {
                    read: {
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "IseriesWebServices.asmx/Read_Saved_libraries",
                        datatype: "json"
                    }
                },
                schema:
                {
                    data: "d",
                    model:
                    {
                        fields:
                            {
                                
                                nome: { type: "string" },
                                Descrizione: { type: "string" },
                                attributo: { type: "string" },
                                compressione:{ type:"string" },
                                zip:{ type:"string" }
                            }
                    }
                } 
            }
        );
    // oggetto grid
    destgrid = $("#gridSelection").kendoGrid(
        {
            dataSource: dataDest,

            columns: [
                        { command: { text: "Cancella", click: Cancella }, title: " ", width: 40 },// cancella la lib da salvare
                        { field: "nome", title: "Nome", width: 50 },
                        { field: "Descrizione", title: "Testo", width: 100 },
                        { field: "attributo", title: "Attrib", width: 30 },
                        {
                            field: "compressione",
                            title: "Comp.Dati",
                            template: "<input type='checkbox' class='chkbxcomp' style='border:0px' id='SelectedComp' #= compressione ? checked=\"checked\" : \"\" # />",//#= compressione ? checked=\"checked\" : \"\" #
                            width: 40
                        },
                        {
                            field: "zip",
                            title: "Zip Savf",
                            template: "<input type='checkbox' class='chkbxzip' style='border:0px' id='SelectedZip' #= zip ? checked=\"checked\" : \"\" # />",//  
                            width: 30
                        }
                    ],
            editable: false,
            height: 400,
            change: function (e) {
            
            },
            dataBound: function (e) {
          
            },
           // selectable: "row",
            scrollable: true,
            sortable: {
                mode: "multiple"
            }

        }


    );
 
    function Cancella(e) {// cancello e ripristino del command
        e.preventDefault();
        var currentDataItem = this.dataItem($(e.currentTarget).closest("tr")); //cerca il <tr> più vicino
        Data.add({ nome: currentDataItem.nome, Descrizione: currentDataItem.Descrizione, attributo: currentDataItem.attributo });// ripristino la lib nell elenco
        dataDest.remove(currentDataItem); // rimuovo dal datasource destinazione        
    }
   
}



// funzioni che gestiscono il click nei checkbox
$(function () {

    $('#gridSelection').on('click', '.chkbxzip', function (e) {
        var checked = $(this).is(':checked');// valuta la prop checked come true o false
        var grid = $('#gridSelection').data().kendoGrid;// oggetto grid
        var Riga = grid.dataItem($(this).closest('tr'));// oggetto riga della grid derivata dal click sulla colonna interna
   
        Riga.zip = checked;// setto lo zip
        var selectedTd = $(e.target).closest("td");// seleziono l'oggetto nel td + vicino ovvero il check
        var grdChkBox = selectedTd.parents('tr').find("td:first").next("td").find('input:checkbox');// trovo il primo check box partendo da li
        grdChkBox.prop('checked', !grdChkBox.prop('checked'));// alla proprietà del checkbox metto l'opposto di quello che c'era
    })
  
    // stessa cosa si fa qui
    $('#gridSelection').on('click', '.chkbxcomp', function (e) {
        var checked = $(this).is(':checked');      
        var grid = $('#gridSelection').data().kendoGrid;
        var Riga = grid.dataItem($(this).closest('tr'));
        
        Riga.compressione = checked;// setto il compressione
        var selectedTd = $(e.target).closest("td");
        var grdChkBox = selectedTd.parents('tr').find("td:first").next("td").find('input:checkbox');
        grdChkBox.prop('checked', !grdChkBox.prop('checked'));
 
        
    });
  
 

})

 


function Grid_Libraries() {
    Data = new kendo.data.DataSource
    (
        {
            type: "json",
            transport: {
                read: {
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "IseriesWebServices.asmx/Get_libraries_list",
                    datatype: "json"
                } 
            },
            schema:
            {
                data: "d",
                model:
                {

                    fields:
                        {
                            nome: { type: "string" },
                            Descrizione: { type: "string" },
                            attributo: { type: "string" }
                        }
                }
            }

        }
    );

    libgrid = $("#GridLibraries").kendoGrid({
        dataSource: Data,

        columns: [
                    { command: { text: "Seleziona", click: Seleziona }, title: " ", width: 40 },
                    { field: "nome", title: "Nome", width: 60 },
                    { field: "Descrizione", title: "Testo", width: 100 },
                    { field: "attributo", title: "Attrib", width: 30 }
        ],
        height: 400,
        change: function (e) {
             

        },
        dataBound: function (e) {
            
        },
        //selectable: "row",
        scrollable: true,      
        sortable: {
            mode: "multiple"
        }
    });
  
    // funzione di selezione della riga e lettura librerie dal command select
    function Seleziona(e) {// cancello dalla sorgente e inserisco nella destinazione
        e.preventDefault();

        var currentDataItem = this.dataItem($(e.currentTarget).closest("tr"));


        var raw = dataDest.data();
        var length = raw.length;

        // iterate and remove "done" items
        var item, i;
        var found = false;
        // cerco se c'è già la riga grezza
        for (i = length - 1; i >= 0; i--) {

            item = raw[i];
            if (item.nome.trim() == currentDataItem.nome)// confronto il nome
            {
                found = true;
                break;
            }
        }

        if (!found)// se non c'è nel datasource di destinazione allora lo inserisco
            if (currentDataItem.Descrizione.trim() == "")
                currentDataItem.Descrizione = "NA";
            dataDest.add({ nome: currentDataItem.nome, Descrizione: currentDataItem.Descrizione, attributo: currentDataItem.attributo, compressione: true, zip: false });
        Data.remove(currentDataItem);// remove
  
    }



    // funzione di selezione della riga e lettura librerie
    function rowSelect(e) {
        
        var selectedRows = e.select();
        var selectedDataItems = [];

        for (var i = 0; i < selectedRows.length; i++) {
            var dataItem = e.dataItem(selectedRows[i]);
            selectedDataItems.push(dataItem);
        }
        if (selectedDataItems.length > 0)
        {

            var currentDataItem = selectedDataItems[0];

            kendoConsole.log(currentDataItem.nome);

            var raw = dataDest.data();
            var length = raw.length;

            // iterate and remove "done" items
            var item, i;
            var found = false;
            // cerco se c'è già la riga grezza
            for (i = length - 1; i >= 0; i--) {

                item = raw[i];
                if (item.nome == currentDataItem.nome)// confronto il nome
                {
                    found = true;
                    break;
                }
            }

            if (!found)// se non è stato ancora inserito lo metto nella grid destinazione
            {
                if (currentDataItem.Descrizione.trim() == "")
                    currentDataItem.Descrizione = "NA";
                dataDest.add({ nome: currentDataItem.nome, Descrizione: currentDataItem.Descrizione, attributo:currentDataItem.attributo, compressione: "false", zip: "true" });
                destgrid.dataSource = dataDest;// metto in grid
                var $tr = destgrid;
              
                e.dataSource.remove(currentDataItem); //removes dalla griglia sorgente così impedisce la riselezione
 
            }
  
        }

    }
 
    
}







//////////////////////////////////////////////////////////////////////SALVATAGGIO////////////////////////////
dataSave = new kendo.data.DataSource
    (
        {
            type: "json", 
            schema:
            {
                data: "d",
                model:
                {
                    fields:
                        {

                            nome: { type: "string" },
                            Descrizione: { type: "string" },
                            attributo: { type: "string" },
                            compressione: { type: "string" },
                            zip: { type: "string" }
                        }
                }
            }
        }
    );

function readLibrariesToSave()
{
    $('#wait').fadeIn("fast");
    var librerieDaSalvare = [];

    var datiGrezzi = dataDest.data();// tutti i dati contenuti nel datasource della Grid destinazione
    var length = datiGrezzi.length;

    // iterate and remove "done" items
    var item, i;
    var zip, comp;
    for (i = length - 1; i >= 0; i--)
    {

        item = datiGrezzi[i];

        // nel caso fosse vuoto gli do falso nel caso checked true altrimenti è true o false
        if (item.compressione == "") comp = "false"; else if (item.compressione == "checked") comp = "true"; else comp=item.compressione;
        if (item.zip == "")  zip = "false"; else if (item.zip == "checked")  zip = "true";  else zip=item.zip;
        

        // per un problema di quando leggo che si crea in automatico un campo type che non esiste devo ripulirlo
        // passando da un altro datasource
        dataSave.add({ nome: item.nome, Descrizione:  escapeStr(item.Descrizione.trim()), attributo:  item.attributo.trim(), compressione: comp, zip: zip });
        librerieDaSalvare.push(dataSave.data()[0]);
        dataSave.remove(dataSave.data()[0]);
    }
    saveLibraries(librerieDaSalvare);
}

function hideMessage()
{
    $("#EventMessage").fadeOut('slow');
    $("#MsgFTP").fadeOut('slow');
}


function saveLibraries(libs)
{

    var jsonLibs = JSON.stringify(libs);
     
    $.ajax({
        type: "POST",
        url: "IseriesWebServices.asmx/Save_Libraries_list",
        data: "{'JsonLibs':'" + jsonLibs + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            // messaggio di fine salvataggio
            $("#message").html('<span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>' +
                                       '<strong>INFO:</strong> Librerie Salvate correttamente');
            $("#EventMessage").fadeIn('slow');
            setTimeout(hideMessage, 2000);
            $('#wait').hide(0);
        },
        error: function (e) {
            $('#wait').hide(0);
            alert("Errore " + e.statusText);
        }
    });

}

function escapeStr(str) {
    if (str)
        return str.replace(/([ #;?%&,.+*~\':"!^$[\]()=>|\/@])/g, ' ')
    else
        return str;
}
 

$(document).ready(
    function () {
        Grid_Libraries();// grid librerie
        Grid_Destination();// grid destinazione lib da salvare
    }
);

//////////////////////////////////////////////////////////// FUNZIONI DI TEST ///////////////////////////////////////
//$(".k-grid-content tbody tr").each(function () {
//    var $tr = $(this);
//    var uid = $tr.attr("data-uid");
//    var dataRow = $('#grid').data("kendoGrid").dataSource.getByUid(uid);
//    $('#grid').data("kendoGrid").dataSource.remove(dataRow);
//});


