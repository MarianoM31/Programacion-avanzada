$(function () {
    const palabraSpan = $('#palabra');
    const tecladoDiv = $('#teclado');
    const erroresSpan = $('#errores');
    const canvas = $('#ahorcado-canvas')[0];
    const ctx = canvas.getContext('2d');

    let letrasAdivinadas = Array(palabraSecreta.length).fill('_');
    let errores = 0;
    const maxErrores = 5;
    let juegoTerminado = false;

    // --- 2. TEMPORIZADOR ---
    let tiempo = parseInt(tiempoLimite);
    const intervalo = setInterval(() => {
        if (juegoTerminado) return;

        tiempo--;
        $('#tiempo').text(tiempo);

        if (tiempo <= 0) {
            finalizarJuego(false); // El jugador pierde si se acaba el tiempo
        }
    }, 1000);

    // --- 3. LÓGICA DEL JUEGO ---
    function normalizar(texto) {
        return texto.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    }

    //Creación del Teclado
    function crearTeclado() {
        const alfabeto = 'ABCDEFGHIJKLMNÑOPQRSTUVWXYZ';
        for (const letra of alfabeto) {
            const $boton = $('<button>').text(letra);
            $boton.on('click', () => manejarIntento(letra));
            tecladoDiv.append($boton);
        }
    }

    function manejarIntento(letra) {
        if (juegoTerminado) return;

        // Deshabilitar botón
        const botonPresionado = tecladoDiv.children().filter(function () {
            return $(this).text() === letra;
        });
        botonPresionado.prop('disabled', true);

        const palabraNormalizada = normalizar(palabraSecreta);
        const letraNormalizada = normalizar(letra);
        let acierto = false;

        //Arreglar lo de las tildes NO FUNCIONA BIEN
        for (let i = 0; i < palabraNormalizada.length; i++) {
            if (palabraNormalizada[i] === letraNormalizada) {
                letrasAdivinadas[i] = palabraSecreta[i]; // Conserva tildes
                acierto = true;
            }
        }

        actualizarPalabraVisible();

        if (acierto) {
            if (!letrasAdivinadas.includes('_')) {
                finalizarJuego(true);
            }
        } else {
            errores++;
            erroresSpan.text(errores);
            dibujarAhorcado();

            if (errores >= maxErrores) {
                finalizarJuego(false);
            }
        }
    }

    function actualizarPalabraVisible() {
        palabraSpan.text(letrasAdivinadas.join(''));
    }

    function finalizarJuego(victoria) {
        juegoTerminado = true;
        clearInterval(intervalo);

        tecladoDiv.children().prop('disabled', true);

        if (victoria) {
            palabraSpan.css('color', 'green');
            palabraSpan.css('background-color', 'white');
            palabraSpan.css('border-radius', '10px');
            setTimeout(() => alert('¡Felicidades, has ganado!'), 200);
        } else {
            palabraSpan.css('color', 'red');
            palabraSpan.css('background-color', 'white');
            palabraSpan.css('border-radius', '10px');
            letrasAdivinadas = palabraSecreta.split('');
            actualizarPalabraVisible();
            setTimeout(() => alert(`¡Has perdido! La palabra era: ${palabraSecreta}`), 200);
        }
    }

    // --- 4. DIBUJO DEL AHORCADO ---
    let texturaCargada = null;

    //Cargar imagen como textura
    const img = new Image();
    img.src = "/Resources/Img/textura-tiza.jpg";
    img.onload = function () {
        texturaCargada = ctx.createPattern(img, 'repeat');
    };

    //Dubujo del ahorcado
    function dibujarAhorcado() {

        if (!texturaCargada) {
            //Aún no ha cargado la imagen, salta o espera
            console.warn("La textura aún no se ha cargado.");
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

    // --- 5. INICIALIZACIÓN ---
    crearTeclado();
});