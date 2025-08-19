// Scripts/scriptIndex.js

(function () {
    // Utilidad: obtén elemento seguro
    function el(id) { return document.getElementById(id); }

    // Esperar a que el DOM esté listo (sin jQuery)
    function ready(fn) {
        if (document.readyState !== 'loading') fn();
        else document.addEventListener('DOMContentLoaded', fn);
    }

    ready(function () {
        var btnNueva = el('btnNuevaPartida');       // <a id="btnNuevaPartida">
        var panel = el('display-newrun');        // <section id="display-newrun">
        var btnClose = el('btnCloseNewRun');        // <a id="btnCloseNewRun">
        var btnX = el('btnCloseXNewRun');       // <i id="btnCloseXNewRun">
        var formNew = document.querySelector('#display-newrun form'); // el form dentro del panel

        // Asegura que el panel arranque oculto si tu CSS no lo hace
        if (panel && (panel.style.display === '' || panel.style.display === 'block')) {
            panel.style.display = 'none';
        }

        function abrirPanel(e) {
            if (e) e.preventDefault();
            if (panel) panel.style.display = 'flex';    // usar flex para que se apliquen justify/align
        }

        function cerrarPanel(e) {
            if (e) e.preventDefault();
            if (panel) panel.style.display = 'none';
        }


        if (btnNueva) btnNueva.addEventListener('click', abrirPanel);
        if (btnClose) btnClose.addEventListener('click', cerrarPanel);
        if (btnX) btnX.addEventListener('click', cerrarPanel);

        // Evita que cualquier enlace con href="#" haga scroll arriba si falla preventDefault
        document.querySelectorAll('a[href="#"]').forEach(function (a) {
            a.addEventListener('click', function (e) { e.preventDefault(); });
        });

        // Validación mínima antes de enviar (opcional)
        if (formNew) {
            formNew.addEventListener('submit', function () {
                // Si quieres, aquí puedes validar campos requeridos extra
                // y cerrar el panel para dar feedback visual:
                // panel.style.display = 'none';
            });
        }

        // Sugerencia: si el botón de enviar cambia de id/nombre, ajusta aquí
        var btnStart = el('btn-start-run'); // <button type="submit" id="btn-start-run">
        if (btnStart) {
            // No es necesario manejar el click: al ser type="submit", envía el form con BeginForm("Crear","Partida")
            // Solo nos aseguramos de que el botón exista para depurar fácilmente
            // console.log('Botón Iniciar Partida listo');
        }
    });
    document.getElementById('identificacion').addEventListener('blur', function () {
        var id = this.value.trim();
        if (id !== "") {
            fetch('/Home/GetNombrePorId?id=' + id)
                .then(response => response.json())
                .then(data => {
                    if (data.existe) {
                        document.getElementById('nombre').value = data.nombre;
                    } else {
                        document.getElementById('nombre').value = "";
                    }
                })
                .catch(error => console.error('Error:', error));
        }
    });

})();
