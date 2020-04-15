$(() => { 
    GetComboDataBase(); 
})

function GetComboDataBase() {
    var Select = document.getElementById("BD");
    Select.innerHTML = "";
     
    $.ajax({
        url: "/Home/ListarBDs",
        type: "GET",
        dataType: "json",
        async: false,
        success: function (data) { 
            if (data != null) {
                CargarComboDataBase(data, "BD");
            } else {
                respuestaAjax = JSON.parse("Error Consultando Datos");
            }
        },
    });
}


function GetComboJobs() {
    var Select = document.getElementById("NombreJob");
    Select.innerHTML = "";

     var datos = {
        DataBaseName: $("#BD").val()
    }
   
    $.ajax({
        url: "/Home/ListarJobs",
        type: "POST",
        data: datos,
        dataType: "json",
        async: false,
        success: function (data) {
            console.log(data)
            if (data != null) {
                CargarComboDataBase(data, "NombreJob");
            } else {
                respuestaAjax = JSON.parse("Error Consultando Datos");
            }
        },
    });
}

 


function CargarComboDataBase(data, idcombo) {
    var Select = document.getElementById(idcombo);
    let option = document.createElement("option");
    option.text = "Seleccione una opcion";
    option.value = 0;  
    Select.add(option);


    let options = [] 
    for (var i = 0; i < data.length; i++) {
        let option = $("<option>")
        option.attr("value", data[i]);
        option.append(data[i])
        options.push(option)
    }
    $("#" + idcombo).append(options);
}




function GetJobsByDataBase() {
}



