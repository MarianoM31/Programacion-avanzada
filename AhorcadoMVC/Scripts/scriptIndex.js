$(function () {

    //--------------Acciones de botones registro y login--------------
    $("#btnNuevaPartida").on("click", function () {
        $("#display-newrun").css("display", "flex");
    });

    $("#btnCloseXNewRun").on("click", function () {
        $("#display-newrun").css("display", "none");
    });

    $("#btnCloseNewRun").on("click", function () {
        $("#display-newrun").css("display", "none");
    });

    $("#identificacion-newrun").on("blur", function () {
        var id = $(this).val();
        if (id) {
            $.ajax({
                url: '/Partida/ObtenerNombreJugador',
                type: 'GET',
                data: { id: id },
                success: function (data) {
                    if (data.success) {
                        $("#nombre-user-newrun").val(data.nombre);
                    } else {
                        $("#nombre-user-newrun").val(""); // Limpiar si no existe
                    }
                }
            });
        }
    });
})
