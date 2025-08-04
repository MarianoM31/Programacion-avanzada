$(function () {
    const palabraSpan = $('#palabra');
    const tecladoDiv = $('#teclado');
    const erroresSpan = $('#errores');
    const canvas = $('#ahorcado-canvas')[0];
    const ctx = canvas?.getContext('2d'); // Protege si canvas no existe

    const partidaIdRaw = $('#partida-id').val();
    const partidaId = parseInt(partidaIdRaw);

    if (!partidaIdRaw || isNaN(partidaId)) {
        alert("Error: No se pudo obtener el ID de la partida. Verifica si estás en la vista correcta.");
        return;
    }

    console.log("Partida ID detectado:", partidaId);
    let juegoTerminado = false;

    // 1. TEMPORIZADOR
    let tiempo = parseInt(tiempoLimite);
    const intervalo = setInterval(() => {
        if (juegoTerminado) return;

        tiempo--;
        $('#tiempo').text(tiempo);

        if (tiempo <= 0) {
            finalizarJuego(false, "Tiempo agotado");
        }
    }, 1000);

    // 2. Crear teclado
    function crearTeclado() {
        const alfabeto = 'ABCDEFGHIJKLMNÑOPQRSTUVWXYZ';
        for (const letra of alfabeto) {
            const $boton = $('<button>').text(letra);
            $boton.on('click', () => manejarIntento(letra, $boton));
            tecladoDiv.append($boton);
        }
    }

    // 3. Manejar intento con letra
    function manejarIntento(letra, boton) {
        if (juegoTerminado) return;

        boton.prop('disabled', true);

        fetch("/Partida/JugarLetra", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ partidaId: partidaId, letra: letra })
        })
            .then(res => res.json())
            .then(data => {
                if (!data.success) {
                    if (data.message) alert(data.message);
                    return;
                }

                palabraSpan.text(data.progreso);
                erroresSpan.text(data.errores);

                if (!data.acierto) {
                    dibujarAhorcado(data.errores);
                }

                if (data.terminado) {
                    finalizarJuego(data.resultado === "Ganada", data.resultado);
                }
            })
            .catch(() => {
                alert("Error al conectar con el servidor.");
            });
    }

    // 4. Finalizar juego
    function finalizarJuego(victoria, mensaje) {
        juegoTerminado = true;
        clearInterval(intervalo);
        tecladoDiv.children().prop('disabled', true);

        palabraSpan.css({
            'background-color': 'white',
            'border-radius': '10px',
            'color': victoria ? 'green' : 'red'
        });

        setTimeout(() => {
            if (victoria) {
                alert("¡Felicidades! Has ganado 🎉");
            } else {
                alert(`Has perdido. ${mensaje || ''}`);
            }
        }, 200);
    }

    // 5. Dibujo del ahorcado
    let texturaCargada = null;

    const img = new Image();
    img.src = "/Resources/Img/textura-tiza.jpg";
    img.onload = function () {
        if (ctx) {
            texturaCargada = ctx.createPattern(img, 'repeat');
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

    // 6. Inicialización
    crearTeclado();
});
