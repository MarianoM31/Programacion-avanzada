$(function () {
    // ===== Fallbacks y caché de elementos =====
    var _lim = (typeof tiempoLimite !== "undefined" && tiempoLimite !== null) ? tiempoLimite : 120;

    const palabraSpan = $('#palabra');
    const tecladoDiv = $('#teclado');   // jQuery object
    const erroresSpan = $('#errores');
    const canvas = $('#ahorcado-canvas')[0];
    const ctx = canvas ? canvas.getContext('2d') : null;

    if (!tecladoDiv.length) {
        console.error("No existe #teclado en el DOM.");
        return;
    }

    const partidaIdRaw = $('#partida-id').val();
    const partidaId = parseInt(partidaIdRaw, 10);

    if (!partidaIdRaw || isNaN(partidaId)) {
        // No cortamos la ejecución; creamos el teclado para depurar visualmente
        console.error("Falta #partida-id o no es numérico. El teclado se mostrará, pero no podrá jugarse.");
    }

    // ===== Helpers para modal (fallback si no existen __setModalWin/lose) =====
    function mostrarModal(titulo, cuerpoHtml) {
        $("#resultadoTitulo").text(titulo || "Mensaje");
        $("#resultadoCuerpo").html(cuerpoHtml || "");
        try { $("#resultadoModal").modal("show"); } catch (e) {
            $("#resultadoModal").addClass('in').css('display', 'block');
        }
    }
    function modalWin() {
        if (window.__setModalWin) return window.__setModalWin();
        $("#resultadoModal").removeClass("lose").addClass("win");
        mostrarModal("¡Felicidades! 🎉", "Has ganado la partida.");
    }
    function modalLose(palabra) {
        if (window.__setModalLose) return window.__setModalLose(palabra);
        $("#resultadoModal").removeClass("win").addClass("lose");
        // Texto literal: "La palabra correcta era: XXXXX"
        mostrarModal("Partida perdida", "La palabra correcta era: " + (palabra || ""));
    }
    function mostrarError(msg) {
        mostrarModal("Error", $("<div/>").text(msg || "Ocurrió un error.").html());
    }

    // ===== Estado del juego =====
    let juegoTerminado = false; // (no lo usamos para bloquear, pero lo dejamos por si lo necesitas)

    // ===== 1) TEMPORIZADOR — iniciar/detener de forma segura =====
    let intervalo = null;
    let tiempo = parseInt(_lim, 10) || 120;

    function detenerTemporizador() {
        if (intervalo !== null) {
            clearInterval(intervalo);
            intervalo = null;
            console.log("[Timer] detenido");
        }
    }
    function iniciarTemporizador() {
        detenerTemporizador();               // evita duplicados
        $("#tiempo").text(tiempo);           // sincroniza la UI
        intervalo = setInterval(function () {
            tiempo--;
            $("#tiempo").text(tiempo);
            if (tiempo <= 0) {
                detenerTemporizador();       // ¡detén ya!
                finalizarJuego(false, "Tiempo agotado");
            }
        }, 1000);
    }

    // ✅ HOOK: detener el temporizador al abrir o cerrar el modal
    $("#resultadoModal").on("show.bs.modal hidden.bs.modal", function () {
        detenerTemporizador();
    });

    // Exponer por si quieres usarlo desde la vista
    window.__detenerTemporizador = detenerTemporizador;

    // Lanza una vez al iniciar la vista
    iniciarTemporizador();

    // ===== 2) Teclado =====
    function crearTeclado() {
        const alfabeto = 'ABCDEFGHIJKLMNÑOPQRSTUVWXYZ';
        tecladoDiv.empty();
        for (const letra of alfabeto) {
            const $boton = $('<button>')
                .attr('type', 'button')
                .addClass('tecla')
                .text(letra)
                .on('click', function () { manejarIntento(letra, $boton); });
            tecladoDiv.append($boton);
        }
        console.log("Teclado creado:", tecladoDiv.children().length, "botones");
    }

    // ===== 3) Intento con letra =====
    function manejarIntento(letra, boton) {
        // NO salimos por juegoTerminado
        if (boton) boton.prop('disabled', true); // solo bloquea la letra usada

        if (!partidaIdRaw || isNaN(partidaId)) {
            // mostrarError("ID de partida inválido.");
            return;
        }

        fetch("/Partida/JugarLetra", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ partidaId: partidaId, letra: letra })
        })
            .then(res => res.json())
            .then(data => {
                if (!data || data.success === false) {
                    // Backend reporta jugada inválida/partida cerrada; no interrumpimos
                    return;
                }

                // Actualiza UI
                palabraSpan.text(data.progreso);
                erroresSpan.text(data.errores);

                if (!data.acierto) {
                    dibujarAhorcado(data.errores);
                }

                if (data.terminado) {
                    const gano = (data.resultado === "Ganada");

                    // Si perdió, muestra la palabra con espacios en el tablero (opcional)
                    if (!gano && data.palabra) {
                        try {
                            const conEspacios = data.palabra.split('').join(' ');
                            $('#palabra').text(conEspacios);
                        } catch (e) { /* no-op */ }
                    }

                    finalizarJuego(gano, data.palabra || "");
                }
            })
            .catch(() => {
                // mostrarError("No se pudo conectar con el servidor.");
            });
    }

    // ===== 4) Finalizar juego =====
    function finalizarJuego(victoria, palabra) {
        // Detener el contador SIEMPRE al finalizar
        detenerTemporizador();

        // No marcamos fin ni deshabilitamos el teclado completo
        // juegoTerminado = true;                        // ← NO
        // tecladoDiv.children().prop('disabled', true); // ← NO

        // Estética del resultado
        palabraSpan.css({
            'background-color': 'white',
            'border-radius': '10px',
            'color': victoria ? 'green' : 'red'
        });

        setTimeout(function () {
            if (victoria) {
                modalWin();
            } else {
                modalLose(palabra || "");
            }
        }, 200);
    }

    // ===== 5) Dibujo del ahorcado =====
    let texturaCargada = null;

    const img = new Image();
    img.src = "/Resources/Img/textura-tiza.jpg";
    img.onload = function () {
        if (ctx) {
            try { texturaCargada = ctx.createPattern(img, 'repeat'); } catch (_) { }
        }
    };

    function dibujarAhorcado(errores) {
        if (!ctx || !texturaCargada) {
            console.warn("Contexto del canvas o textura no cargados.");
            return;
        }

        ctx.lineWidth = 6;
        ctx.strokeStyle = texturaCargada;

        switch (errores) {
            case 1:
                ctx.beginPath();
                ctx.moveTo(10, 230);
                ctx.lineTo(190, 230);
                ctx.stroke();
                break;
            case 2:
                ctx.moveTo(50, 230);
                ctx.lineTo(50, 20);
                ctx.stroke();
                break;
            case 3:
                ctx.moveTo(49, 20);
                ctx.lineTo(150, 20);
                ctx.stroke();
                break;
            case 4:
                ctx.moveTo(150, 19);
                ctx.lineTo(150, 60);
                ctx.stroke();
                break;
            case 5:
                ctx.beginPath();
                ctx.arc(150, 80, 20, 0, Math.PI * 2);
                ctx.moveTo(150, 100);
                ctx.lineTo(150, 170);
                ctx.moveTo(150, 120);
                ctx.lineTo(120, 150);
                ctx.moveTo(150, 120);
                ctx.lineTo(180, 150);
                ctx.moveTo(150, 169);
                ctx.lineTo(120, 200);
                ctx.moveTo(150, 169);
                ctx.lineTo(180, 200);
                ctx.stroke();
                break;
        }
    }

    // ===== 6) Inicialización =====
    crearTeclado();
});
