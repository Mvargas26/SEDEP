﻿// Función para agregar filas a la tabla de Objetivos
function agregarFilaObjetivos() {
    const selectObj = document.getElementById("selectObjetivo");
    const inputPesoObj = document.getElementById("inputPesoObj");
    const inputMetaObj = document.getElementById("inputMetaObj");

    const valorSeleccionado = selectObj.value;
    if (!valorSeleccionado) {
        alert("Seleccione un objetivo.");
        return;
    }

    // Extraer los valores
    const [idObj, nombreObj, tipoObj, idTipoObj] = valorSeleccionado.split("|");

    const pesoIngresado = inputPesoObj.value;
    if (!pesoIngresado) {
        alert("Ingrese un peso para el objetivo.");
        return;
    }

    const metaIngresada = inputMetaObj.value || "Sin meta";

    const tbody = document.querySelector("#tablaObjetivos tbody");
    const nuevaFila = document.createElement("tr");

    // Celda de id
    const tdID = document.createElement("td");
    tdID.innerText = idObj;
    // Celda de Objetivo
    const tdObjetivo = document.createElement("td");
    tdObjetivo.innerText = nombreObj;

    // Celda de Tipo (Ahora mostrará el IdTipoObjetivo en lugar de "tipoObj")
    const tdTipo = document.createElement("td");
    tdTipo.innerText = idTipoObj;

    // Celda de Peso
    const tdPeso = document.createElement("td");
    tdPeso.innerText = pesoIngresado + "%";

    // Celda de Meta
    const tdMeta = document.createElement("td");
    tdMeta.innerText = metaIngresada;

    // Celda Actual fijo a "0%"
    const tdActual = document.createElement("td");
    tdActual.innerText = "0%";

    // Celda de Acciones
    const tdAcciones = document.createElement("td");
    const btnEliminar = document.createElement("button");
    btnEliminar.classList.add("btn", "btn-sm", "btn-danger");
    btnEliminar.innerText = "Eliminar";
    btnEliminar.onclick = function () {
        tbody.removeChild(nuevaFila);
    };
    tdAcciones.appendChild(btnEliminar);

    // Agregar celdas a la fila
    nuevaFila.appendChild(tdID);
    nuevaFila.appendChild(tdObjetivo);
    nuevaFila.appendChild(tdTipo); // Se mostrará el IdTipoObjetivo
    nuevaFila.appendChild(tdPeso);
    nuevaFila.appendChild(tdMeta);
    nuevaFila.appendChild(tdActual);
    nuevaFila.appendChild(tdAcciones);

    // Insertar la fila en la tabla
    tbody.appendChild(nuevaFila);

    // Actualizar sumas por tipo
    actualizarSumasPorTipo();

    // Limpiar los inputs
    inputPesoObj.value = "";
    inputMetaObj.value = "";
}//fin agregarFilaObjetivos

// Función para agregar filas a la tabla de Competencias
function agregarFilaCompetencias() {
    const selectComp = document.getElementById("selectCompetencia");
    const inputPesoComp = document.getElementById("inputPesoComp");
    const inputMetaComp = document.getElementById("inputMetaComp");

    const valorSeleccionado = selectComp.value;
    if (!valorSeleccionado) {
        alert("Seleccione una competencia.");
        return;
    }

    // Extraer los valores
    const [idComp, nombreComp, calificacion, idTipoComp] = valorSeleccionado.split("|");

    const pesoIngresado = inputPesoComp.value;
    if (!pesoIngresado) {
        alert("Ingrese un peso para la competencia.");
        return;
    }

    const metaIngresada = inputMetaComp.value || "Sin meta";

    const tbody = document.querySelector("#tablaCompetencias tbody");
    const nuevaFila = document.createElement("tr");

    // Celda de id
    const tdID = document.createElement("td");
    tdID.innerText = idComp;

    // Celda de Competencia
    const tdCompetencia = document.createElement("td");
    tdCompetencia.innerText = nombreComp;

    // Celda de Tipo (Mostrará el IdTipoCompetencia)
    const tdTipo = document.createElement("td");
    tdTipo.innerText = idTipoComp;

    // Celda de Peso
    const tdPeso = document.createElement("td");
    tdPeso.innerText = pesoIngresado + "%";

    // Celda de Meta
    const tdMeta = document.createElement("td");
    tdMeta.innerText = metaIngresada;

    // Celda Actual fijo a "0%"
    const tdActual = document.createElement("td");
    tdActual.innerText = "0%";

    // Celda de Acciones
    const tdAcciones = document.createElement("td");
    const btnEliminar = document.createElement("button");
    btnEliminar.classList.add("btn", "btn-sm", "btn-danger");
    btnEliminar.innerText = "Eliminar";
    btnEliminar.onclick = function () {
        tbody.removeChild(nuevaFila);
    };
    tdAcciones.appendChild(btnEliminar);

    // Agregar celdas a la fila
    nuevaFila.appendChild(tdID);
    nuevaFila.appendChild(tdCompetencia);
    nuevaFila.appendChild(tdTipo); // Se mostrará el IdTipoCompetencia
    nuevaFila.appendChild(tdPeso);
    nuevaFila.appendChild(tdMeta);
    nuevaFila.appendChild(tdActual);
    nuevaFila.appendChild(tdAcciones);

    // Insertar la fila en la tabla
    tbody.appendChild(nuevaFila);

    // Actualizar sumas por tipo
    actualizarSumasPorTipo();

    // Limpiar los inputs
    inputPesoComp.value = "";
    inputMetaComp.value = "";
}//fin fila comp

// Función para actualizar las sumas por tipo
function actualizarSumasPorTipo() {
    // Obtener todos los tipos definidos en tablaResultados
    const tiposDefinidos = {};
    document.querySelectorAll('#tablaResultados tbody tr').forEach(row => {
        const idTipoCell = row.querySelector('td:nth-child(4)');
        if (idTipoCell && idTipoCell.textContent !== '-') {
            const idTipo = idTipoCell.textContent.trim();
            tiposDefinidos[idTipo] = {
                input: row.querySelector('td:nth-child(3) input'),
                porcentajeDeseado: parseFloat(
                    row.querySelector('td:nth-child(2) strong')?.textContent
                        .replace('Deseado:', '')
                        .replace('%', '')
                        .trim() || '0'
                )
            };
        }
    });

    // Calcular sumas actuales por tipo
    const sumasPorTipo = {};

    // Sumar pesos de objetivos
    document.querySelectorAll('#tablaObjetivos tbody tr').forEach(row => {
        const idTipo = row.querySelector('td:nth-child(3)')?.textContent.trim();
        const peso = parseFloat(row.querySelector('td:nth-child(4)')?.textContent.replace('%', '') || '0');

        if (idTipo) {
            sumasPorTipo[idTipo] = (sumasPorTipo[idTipo] || 0) + peso;
        }
    });

    // Sumar pesos de competencias 
    document.querySelectorAll('#tablaCompetencias tbody tr').forEach(row => {
        const idTipo = row.querySelector('td:nth-child(3)')?.textContent.trim();
        const peso = parseFloat(row.querySelector('td:nth-child(4)')?.textContent.replace('%', '') || '0');

        if (idTipo) {
            sumasPorTipo[idTipo] = (sumasPorTipo[idTipo] || 0) + peso;
        }
    });

    // Actualizar inputs en tablaResultados
    let sumaTotal = 0;
    for (const [idTipo, sumaActual] of Object.entries(sumasPorTipo)) {
        sumaTotal += sumaActual;

        if (tiposDefinidos[idTipo]) {
            tiposDefinidos[idTipo].input.value = sumaActual.toFixed(2);

            // Cambiar color si no coincide con el deseado
            const diferencia = Math.abs(sumaActual - tiposDefinidos[idTipo].porcentajeDeseado);
            tiposDefinidos[idTipo].input.style.backgroundColor = diferencia > 1 ? '#e62929' : '';
        }
    }

    // Actualizar suma total
    const resultadoTotal = document.getElementById('resultado-total');
    if (resultadoTotal) {
        resultadoTotal.value = sumaTotal.toFixed(2);
        resultadoTotal.style.backgroundColor = Math.abs(sumaTotal - 100) > 1 ? '#e62929' : '';
    }
}//fin 

