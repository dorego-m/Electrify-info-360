const formulario = document.getElementById("formRegistrarEmpresa");

if (formulario) {
    formulario.addEventListener("submit", function (event) {
        let valido = true;

        const campos = [
            ["razonSocial", "errorRazonSocial"],
            ["nombreAdmin", "errorNombreAdmin"],
            ["apellidoAdmin", "errorApellidoAdmin"],
            ["emailAdmin", "errorEmailAdmin"],
            ["contrasenaAdmin", "errorContrasenaAdmin"],
            ["idPlan", "errorPlan"]
        ];

        campos.forEach(([campoId, errorId]) => {
            const campo = document.getElementById(campoId);
            const error = document.getElementById(errorId);

            error.textContent = "";

            if (!campo.value.trim()) {
                error.textContent = "Este campo es obligatorio.";
                valido = false;
            }
        });

        const email = document.getElementById("emailAdmin");
        const errorEmail = document.getElementById("errorEmailAdmin");

        if (email.value.trim() &&
            !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)) {
            errorEmail.textContent = "Ingresa un email válido.";
            valido = false;
        }

        const contrasena = document.getElementById("contrasenaAdmin");
        const errorContrasena = document.getElementById("errorContrasenaAdmin");

        if (contrasena.value.length < 8) {
            errorContrasena.textContent =
                "La contraseña debe tener al menos 8 caracteres.";
            valido = false;
        }

        if (!valido) {
            event.preventDefault();
        }
    });
}