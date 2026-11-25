
document.addEventListener("DOMContentLoaded", function () {

    const datos = document.getElementById("datosInforme");
    const formulario = document.querySelector('form[asp-action="InformeConsumo"]')
        || document.querySelector('form[method="get"]');

    if (!datos || !formulario) return;

    const fechaInicio = document.getElementById("fechaInicio");
    const fechaFin = document.getElementById("fechaFin");

    // Datos iniciales que llegan desde la View
    const etiquetas = JSON.parse(datos.dataset.etiquetas);
    const consumos = JSON.parse(datos.dataset.consumos);
    const costos = JSON.parse(datos.dataset.costos);
    const sectores = JSON.parse(datos.dataset.sectores);
    const dispositivos = JSON.parse(datos.dataset.dispositivos);

    // Crear los gráficos
    const graficoConsumo = new Chart(
        document.getElementById("graficoConsumo"),
        {
            type: "bar",
            data: {
                labels: etiquetas,
                datasets: [{
                    label: "Consumo estimado (kWh)",
                    data: consumos,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                scales: { y: { beginAtZero: true } }
            }
        }
    );

    const graficoCosto = new Chart(
        document.getElementById("graficoCosto"),
        {
            type: "line",
            data: {
                labels: etiquetas,
                datasets: [{
                    label: "Costo estimado ($)",
                    data: costos,
                    tension: 0.25,
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                scales: { y: { beginAtZero: true } }
            }
        }
    );

    
const graficoSectores = new Chart(
    document.getElementById("graficoSectores"),
    {
        type: "doughnut",
        data: {
            labels: sectores.map(x => x.nombreSector),
            datasets: [{
                label: "Consumo estimado (kWh)",
                data: sectores.map(x => x.consumoKWh),
                backgroundColor: [
                    "#36C5F0",
                    "#536DFE",
                    "#8B5CF6",
                    "#20C997",
                    "#FFC857",
                    "#F76C6C",
                    "#A3D977",
                    "#E879F9"
                ],
                borderColor: "#06142B",
                borderWidth: 2
            }]
        },
        options: {
            responsive: true
        }
    }
);


    const graficoDispositivos = new Chart(
        document.getElementById("graficoDispositivos"),
        {
            type: "bar",
            data: {
                labels: dispositivos.map(x => x.NombreDispositivo),
                datasets: [{
                    label: "Consumo estimado (kWh)",
                    data: dispositivos.map(x => x.ConsumoKWh)
                }]
            },
            options: {
                responsive: true,
                indexAxis: "y",
                scales: { x: { beginAtZero: true } }
            }
        }
    );

    // Actualizar los gráficos y los indicadores
    function actualizarInforme(data) {

        const nuevasEtiquetas = data.informe.map(x =>
            x.fecha.substring(8, 10) + "/" + x.fecha.substring(5, 7)
        );

        graficoConsumo.data.labels = nuevasEtiquetas;
        graficoConsumo.data.datasets[0].data =
            data.informe.map(x => x.consumoKWhEstimado);
        graficoConsumo.update();

        graficoCosto.data.labels = nuevasEtiquetas;
        graficoCosto.data.datasets[0].data =
            data.informe.map(x => x.costoEstimado);
        graficoCosto.update();

        graficoSectores.data.labels =
            data.sectores.map(x => x.nombreSector);
        graficoSectores.data.datasets[0].data =
            data.sectores.map(x => x.consumoKWh);
        graficoSectores.update();

        graficoDispositivos.data.labels =
            data.dispositivos.map(x => x.nombreDispositivo);
        graficoDispositivos.data.datasets[0].data =
            data.dispositivos.map(x => x.consumoKWh);
        graficoDispositivos.update();

        document.getElementById("consumoTotal").textContent =
            Number(data.consumoTotal).toLocaleString("es-AR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }) + " kWh";

        document.getElementById("costoTotal").textContent =
            "$ " + Number(data.costoTotal).toLocaleString("es-AR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });

        document.getElementById("promedioDiario").textContent =
            Number(data.promedioDiario).toLocaleString("es-AR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }) + " kWh";
    }

    // Solicitar los datos al Controller
    async function cargarInforme() {

        if (!fechaInicio.value || !fechaFin.value) return;

        if (fechaFin.value < fechaInicio.value) {
            alert("La fecha final no puede ser anterior a la inicial.");
            return;
        }

        const parametros = new URLSearchParams({
            fechaInicio: fechaInicio.value,
            fechaFin: fechaFin.value
        });

        try {
            const respuesta = await fetch(
                `/Home/ObtenerDatosInforme?${parametros}`,
                { method: "GET" }
            );

            if (!respuesta.ok) {
                throw new Error("No se pudo obtener el informe.");
            }

            const data = await respuesta.json();

            actualizarInforme(data);
        }
        catch (error) {
            console.error("Error al cargar el informe:", error);
            alert("Ocurrió un error al actualizar el informe.");
        }
    }

    // Evitar que el formulario recargue la página
    formulario.addEventListener("submit", function (e) {
        e.preventDefault();
        cargarInforme();
    });

    // Selección de períodos
    document.querySelectorAll("[data-periodo]").forEach(boton => {

        boton.addEventListener("click", function () {

            const periodo = this.dataset.periodo;
            const hoy = new Date();
            const inicio = new Date(hoy);

            if (periodo === "semana") {
                inicio.setDate(hoy.getDate() - 6);
            }
            else if (periodo === "mes") {
                inicio.setDate(1);
            }
            else if (periodo === "anio") {
                inicio.setMonth(0, 1);
            }

            const formatoFecha = fecha => {
                const anio = fecha.getFullYear();
                const mes = String(fecha.getMonth() + 1).padStart(2, "0");
                const dia = String(fecha.getDate()).padStart(2, "0");

                return `${anio}-${mes}-${dia}`;
            };

            fechaInicio.value = formatoFecha(inicio);
            fechaFin.value = formatoFecha(hoy);

            cargarInforme();
        });
    });

});
