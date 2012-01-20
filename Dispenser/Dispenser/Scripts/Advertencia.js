//-----Solicitudes de dispensador------
function alerta(costo) {
    //alert(document.getElementById("hdnValue").value);
    var confirmacion = confirm("Solictud enviada el costo total fue de: $" + costo + " desea enviar" + '\n' + "una nueva solicitud?");

    if (confirmacion == true) {

        window.location.href = 'Solicitud_Dispensadores.aspx';
        return;

    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}

function alerta2(costo) {
    //alert(document.getElementById("hdnValue").value);
    var confirmacion = confirm("Solictud enviada el costo total fue de: $" + costo + " pero sera corrida al siguiente mes" + '\n' + "ya que excede su presupuesto actual.");

    if (confirmacion == true) {
        
        window.location.href = 'Solicitud_Dispensadores.aspx';
        return;

    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}

function alerta3(costo) {
    //alert(document.getElementById("hdnValue").value);
    var confirmacion = confirm("Solictud enviada el costo total fue de: $" + costo + " desea enviar" + '\n' + "una nueva solicitud?");

    if (confirmacion == true) {

        window.location.href = 'Solicitud_Clientes_Nuevos.aspx';
        return;

    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}

function alerta4(costo) {
    //alert(document.getElementById("hdnValue").value);
    var confirmacion = confirm("Solictud enviada el costo total fue de: $" + costo + " desea enviar" + '\n' + "una nueva solicitud?");

    if (confirmacion == true) {

        window.location.href = 'Solicitud_Clientes_Nuevos.aspx';
        return;

    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}
//------------------------------------

//----- Usuarios -------

function exitoNuevoUsuario(exito) {

    if (exito == 1) {
        var confirmacion = confirm("Usuario creado con exito, se envio correo de notificacion." + '\n' + "¿Desea ingresar un nuevo usuario?");
    } else {
        var confirmacion = confirm("Usuario creado con exito, sin notificacion por error en servidor de correos." + '\n' + "¿Desea ingresar un nuevo usuario?");
    }

    if (confirmacion == true) {
        window.location.href = 'Agregar_Usuario.aspx';
        return;
    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}

function errorEdicion(error) {
    alert('No se pudo actualizar el usuario error:' + '\n' + error);
}

//-----------------------

//------- General -------
function errorEnvio(mensaje) {
    alert(mensaje);
    window.location.href = '../Default.aspx';
}
//-----------------------

//------- Aprobacion de solicitudes -------
function aprobadoRechazado(mensaje) {
    alert(mensaje);
    window.location.href = 'AutorizacionSolicitudes.aspx';
}
//-----------------------------------------

//--------------- Editar Kam ---------------
function editarKam() {
    var confirmacion = confirm("KAM editado con exito." + '\n' + "¿Desea seguir editando?");

    if (confirmacion == true) {
        window.location.href = 'Editar_KAM.aspx';
        return;
    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}
//------------------------------------------

//------------- Editar Socio ---------------
function esditarSocio() {
    var confirmacion = confirm("Socio editado con exito." + '\n' + "¿Desea seguir editando?");

    if (confirmacion == true) {
        window.location.href = 'Editar_Socio.aspx';
        return;
    } else {
        window.location.href = '../Default.aspx';
        return;
    }
}
//------------------------------------------

//Custom validator solicitud dispensadores.
function validarSimbolos(oSrc, args) 
{
    var direccion = args.Value.toString();

    for (i = 0; i < direccion.length; i++) {
        if (direccion.charAt(i) == "," || direccion.charAt(i) == "'" || direccion.charAt(i) == "-" || direccion.charAt(i) == ";") {
            args.IsValid = false;
        }
    }

    args.IsValid = true;
}

function longitudTelefono(oSrc, args) {
    args.IsValid = (args.Value.length >= 7);
}

function inversion(oSrc, args) {
    args.IsValid = (args.Value > 100);
}
//------------------------------------------

//-------------- Editar Vendedor --------------
function vendedorEditado() {
    alert('Vendedor editado con exito.');
    window.location.href = '../Default.aspx';
}
//---------------------------------------------