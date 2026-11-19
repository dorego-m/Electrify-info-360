
document.addEventListener("DOMContentLoaded", function () {

    const datos = document.getElementById("datosEstrategias");
    const grafico = document.getElementById("graficoEstrategias");

    if (!datos || !grafico) return;

    const sectores = JSON.parse(datos.dataset.sectores);

    if (sectores.length === 0) return;

    new Chart(grafico, {
        type: "bar",
        data: {
            labels: sectores.map(x => x.NombreSector),
            datasets: [{
                label: "Consumo estimado (kWh)",
                data: sectores.map(x => x.ConsumoKWh),
                borderRadius: 6
            }]
        },
        options: {
            indexAxis: "y",
            responsive: true,
            plugins: {
                legend: { display: false }
            },
            scales: {
                x: { beginAtZero: true }
            }
        }
    });

});
