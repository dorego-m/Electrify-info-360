
document.addEventListener("DOMContentLoaded", function () {

    const datos = document.getElementById("datosInforme");

    if (!datos) return;

    const etiquetas = JSON.parse(datos.dataset.etiquetas);
    const consumos = JSON.parse(datos.dataset.consumos);
    const costos = JSON.parse(datos.dataset.costos);
    const sectores = JSON.parse(datos.dataset.sectores);
    const dispositivos = JSON.parse(datos.dataset.dispositivos);

    // Gráfico de consumo diario
    new Chart(document.getElementById("graficoConsumo"), {
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
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Gráfico de costo diario
    new Chart(document.getElementById("graficoCosto"), {
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
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Gráfico de consumo por sector
    new Chart(document.getElementById("graficoSectores"), {
        type: "doughnut",
        data: {
            labels: sectores.map(x => x.NombreSector),
            datasets: [{
                label: "Consumo estimado (kWh)",
                data: sectores.map(x => x.ConsumoKWh)
            }]
        },
        options: {
            responsive: true
        }
    });

    // Gráfico de consumo por dispositivo
    new Chart(document.getElementById("graficoDispositivos"), {
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
            scales: {
                x: { beginAtZero: true }
            }
        }
    });

    // Selección de período
    document.querySelectorAll("[data-periodo]").forEach(boton => {
        boton.addEventListener("click", function () {

            const periodo = this.dataset.periodo;
            const hoy = new Date();
            const inicio = new Date(hoy);

            if (periodo === "semana") {
                inicio.setDate(hoy.getDate() - 6);
            } else if (periodo === "mes") {
                inicio.setDate(1);
            } else if (periodo === "anio") {
                inicio.setMonth(0, 1);
            }

            const formatoFecha = fecha => {
                const anio = fecha.getFullYear();
                const mes = String(fecha.getMonth() + 1).padStart(2, "0");
                const dia = String(fecha.getDate()).padStart(2, "0");

                return `${anio}-${mes}-${dia}`;
            };

            document.getElementById("fechaInicio").value = formatoFecha(inicio);
            document.getElementById("fechaFin").value = formatoFecha(hoy);

            document.getElementById("fechaInicio").form.requestSubmit();
        });
    });

});
