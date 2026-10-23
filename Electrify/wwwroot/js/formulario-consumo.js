//DE NUEVO, la IA ayudo mucho con todo lo del formulario..

(function () {
    const form = document.getElementById("formConsumo");
    if (!form) return;

    const sedeSelect = document.getElementById("IDSede");
    const container = document.getElementById("detallesConsumo");
    const addButton = document.getElementById("agregarDetalle");
    const tarifa = document.getElementById("TarifaKWh");
    const fechaInicio = document.getElementById("FechaInicio");
    const fechaFin = document.getElementById("FechaFin");
    const data = window.electrifyInfraestructura || { sectores: [], dispositivos: [] };

    function convertirDecimal(valor) {
    return Number(String(valor).replace(",", "."));
}

    function escapeHtml(value) {
        return String(value ?? "").replace(/[&<>"']/g, c => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c]));
    }

    function actualizarIndices() {
        [...container.querySelectorAll(".detalle-consumo")].forEach((row, index) => {
            row.querySelector(".numero-detalle").textContent = index + 1;
            row.querySelector(".dispositivo-detalle").name = `Detalles[${index}].IDDispositivo`;
            row.querySelector(".cantidad-detalle").name = `Detalles[${index}].Cantidad`;
            row.querySelector(".horas-detalle").name = `Detalles[${index}].HorasPorDia`;
            row.querySelector(".dias-detalle").name = `Detalles[${index}].DiasUso`;
        });
    }

    function actualizarSectores(row) {
        const sectorSelect = row.querySelector(".sector-detalle");
        const selectedSede = Number(sedeSelect.value);
        const old = sectorSelect.value;
        sectorSelect.innerHTML = '<option value="">Seleccioná sector</option>';
        data.sectores.filter(s => s.sede === selectedSede).forEach(s => {
            const option = document.createElement("option");
            option.value = s.id;
            option.textContent = s.nombre;
            sectorSelect.appendChild(option);
        });
        if ([...sectorSelect.options].some(o => o.value === old)) sectorSelect.value = old;
        actualizarDispositivos(row);
    }

    function actualizarDispositivos(row) {
        const sectorId = Number(row.querySelector(".sector-detalle").value);
        const select = row.querySelector(".dispositivo-detalle");
        const old = select.value;
        select.innerHTML = '<option value="">Seleccioná dispositivo</option>';
        data.dispositivos.filter(d => d.sector === sectorId && d.sede === Number(sedeSelect.value)).forEach(d => {
            const option = document.createElement("option");
            option.value = d.id;
            option.dataset.potencia = d.potencia;
            option.textContent = `${d.nombre} (${d.potencia} W)`;
            select.appendChild(option);
        });
        if ([...select.options].some(o => o.value === old)) select.value = old;
        calcular();
    }

    function crearFila() {
        const row = document.createElement("div");
        row.className = "detalle-consumo border rounded p-3 mb-3";
        row.innerHTML = `
            <div class="d-flex justify-content-between"><h6>Dispositivo <span class="numero-detalle"></span></h6><button type="button" class="btn btn-sm btn-outline-danger quitar-detalle">Quitar</button></div>
            <div class="mb-3"><label class="form-label">Sector</label><select class="form-select sector-detalle" required><option value="">Seleccioná sector</option></select></div>
            <div class="mb-3"><label class="form-label">Dispositivo</label><select class="form-select dispositivo-detalle" required><option value="">Seleccioná dispositivo</option></select></div>
            <div class="row"><div class="col-md-4 mb-3"><label class="form-label">Cantidad</label><input class="form-control cantidad-detalle" type="number" min="1" value="1" required></div><div class="col-md-4 mb-3"> <label class="form-label">Horas por día</label> <input class="form-control horas-detalle" type="number" min="0.25" max="24" step="0.25" value="8" required> </div><div class="col-md-4 mb-3"><label class="form-label">Días de uso</label><input class="form-control dias-detalle" type="number" min="1" value="1" required></div></div>
            <div class="small text-muted resumen-detalle">Elegí un dispositivo para calcular el consumo estimado.</div>`;
        container.appendChild(row);
        actualizarIndices();
        actualizarSectores(row);
        return row;
    }

    function calcular() {
        let total = 0;
        container.querySelectorAll(".detalle-consumo").forEach(row => {
            const selected = row.querySelector(".dispositivo-detalle").selectedOptions[0];
            const potencia = Number(selected?.dataset.potencia || 0);
            const cantidad = Number(row.querySelector(".cantidad-detalle").value || 0);
            const horas = convertirDecimal(row.querySelector(".horas-detalle").value);
            const dias = Number(row.querySelector(".dias-detalle").value || 0);
            const kwh = potencia * cantidad * horas * dias / 1000;
            total += kwh;
            row.querySelector(".resumen-detalle").textContent = potencia > 0 ? `Consumo estimado del dispositivo: ${kwh.toFixed(2)} kWh` : "Elegí un dispositivo con potencia registrada.";
        });
        const costo = total * Number(tarifa.value || 0);
        document.getElementById("totalKWh").textContent = total.toLocaleString("es-AR", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        document.getElementById("totalCosto").textContent = costo.toLocaleString("es-AR", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    addButton.addEventListener("click", crearFila);
    sedeSelect.addEventListener("change", () => container.querySelectorAll(".detalle-consumo").forEach(actualizarSectores));
    tarifa.addEventListener("input", calcular);
    fechaInicio.addEventListener("change", () => {
        if (fechaFin.value && fechaInicio.value > fechaFin.value) fechaFin.value = fechaInicio.value;
    });
    fechaFin.addEventListener("change", () => {
        if (fechaInicio.value && fechaFin.value < fechaInicio.value) fechaInicio.value = fechaFin.value;
        const days = Math.max(1, Math.floor((new Date(fechaFin.value) - new Date(fechaInicio.value)) / 86400000) + 1);
        container.querySelectorAll(".dias-detalle").forEach(input => input.max = days);
    });

    container.addEventListener("change", event => {
        const row = event.target.closest(".detalle-consumo");
        if (!row) return;
        if (event.target.classList.contains("sector-detalle")) actualizarDispositivos(row);
        else if (event.target.classList.contains("dispositivo-detalle")) calcular();
    });
    container.addEventListener("input", event => { if (event.target.matches("input")) calcular(); });
    container.addEventListener("click", event => {
        const button = event.target.closest(".quitar-detalle");
        if (!button) return;
        if (container.querySelectorAll(".detalle-consumo").length === 1) {
            const row = button.closest(".detalle-consumo");
            row.querySelector(".sector-detalle").value = "";
            row.querySelector(".dispositivo-detalle").value = "";
            calcular();
            return;
        }
        button.closest(".detalle-consumo").remove();
        actualizarIndices();
        calcular();
    });

    form.addEventListener("submit", event => {
        let valid = true;
        if (!sedeSelect.value || !fechaInicio.value || !fechaFin.value || fechaInicio.value > fechaFin.value || Number(tarifa.value) < 0) valid = false;
        const rows = [...container.querySelectorAll(".detalle-consumo")];
        if (!rows.length) valid = false;
        rows.forEach(row => {
            if (!row.querySelector(".dispositivo-detalle").value || Number(row.querySelector(".cantidad-detalle").value) < 1 || Number(row.querySelector(".horas-detalle").value) <= 0 || Number(row.querySelector(".horas-detalle").value) > 24 || Number(row.querySelector(".dias-detalle").value) < 1) valid = false;
        });
        if (!valid) {
            event.preventDefault();
            alert("Revisá la sede, las fechas y los datos de cada dispositivo antes de guardar.");
        }
    });

    container.querySelectorAll(".detalle-consumo").forEach(row => {
        actualizarSectores(row);
        row.querySelector(".sector-detalle").addEventListener("change", () => actualizarDispositivos(row));
    });
    calcular();
})();
