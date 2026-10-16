document.addEventListener("DOMContentLoaded", function () {
    const formularioRegistro = document.getElementById("formCrearCuenta");
    const formularioLogin = document.getElementById("formLogin");

    function mostrarError(id, mensaje) {
        const elemento = document.getElementById(id);
        if (elemento) elemento.textContent = mensaje;
    }

    function emailValido(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    if (formularioLogin) {
        formularioLogin.addEventListener("submit", function (evento) {
            const email = document.getElementById("email").value.trim();
            const contraseña = document.getElementById("contraseña").value;
            let valido = true;

            mostrarError("errorEmail", "");
            mostrarError("errorContraseña", "");

            if (!emailValido(email)) {
                mostrarError("errorEmail", "Ingresa un email valido.");
                valido = false;
            }
            if (contraseña.trim() === "") {
                mostrarError("errorContraseña", "Ingresa tu contraseña.");
                valido = false;
            }
            if (!valido) evento.preventDefault();
        });
    }

    if (formularioRegistro) {
        formularioRegistro.addEventListener("submit", function (evento) {
            const nombre = document.getElementById("nombre").value.trim();
            const apellido = document.getElementById("apellido").value.trim();
            const email = document.getElementById("email").value.trim();
            const contraseña = document.getElementById("contraseña").value;
            const empresa = document.getElementById("idEmpresa").value;
            let valido = true;

            ["errorNombre", "errorApellido", "errorEmail", "errorContraseña", "errorEmpresa"]
                .forEach(id => mostrarError(id, ""));

            if (nombre.length < 2) {
                mostrarError("errorNombre", "El nombre debe tener al menos 2 caracteres.");
                valido = false;
            }
            if (apellido.length < 2) {
                mostrarError("errorApellido", "El apellido debe tener al menos 2 caracteres.");
                valido = false;
            }
            if (!emailValido(email)) {
                mostrarError("errorEmail", "Ingresa un email valido.");
                valido = false;
            }
            if (contraseña.length < 6) {
                mostrarError("errorContraseña", "La contraseña debe tener al menos 6 caracteres.");
                valido = false;
            }
            if (empresa === "") {
                mostrarError("errorEmpresa", "Selecciona una empresa.");
                valido = false;
            }
            if (!valido) evento.preventDefault();
        });
    }
});
