// Función para agregar filas a la tabla de Objetivos
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
    nuevaFila.appendChild(tdObjetivo);
    nuevaFila.appendChild(tdTipo); // Se mostrará el IdTipoObjetivo
    nuevaFila.appendChild(tdPeso);
    nuevaFila.appendChild(tdMeta);
    nuevaFila.appendChild(tdActual);
    nuevaFila.appendChild(tdAcciones);

    // Insertar la fila en la tabla
    tbody.appendChild(nuevaFila);

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
    nuevaFila.appendChild(tdCompetencia);
    nuevaFila.appendChild(tdTipo); // Se mostrará el IdTipoCompetencia
    nuevaFila.appendChild(tdPeso);
    nuevaFila.appendChild(tdMeta);
    nuevaFila.appendChild(tdActual);
    nuevaFila.appendChild(tdAcciones);

    // Insertar la fila en la tabla
    tbody.appendChild(nuevaFila);

    // Limpiar los inputs
    inputPesoComp.value = "";
    inputMetaComp.value = "";
}//fin fila comp

