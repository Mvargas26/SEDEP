document.getElementById("btnGuardarNotas").addEventListener("click", updateValorActual);

function updateValorActual() {
    // Recolectar datos de ambas tablas
    const objetivosAsignados = obtenerDatosTabla('#tbObjetivosAsignados tbody tr');
    const competenciasAsignadas = obtenerDatosTabla('#tbCompetenciasAsignadas tbody tr');

    //otros datos necesarios
    const cedFuncionario = document.getElementById('ceduFuncionario').innerText;
    const idEvaluacion = document.getElementById('idEvaluacion').innerText;
    const resultadoTotal = document.getElementById('resultado-total').value;
    const idConglo = document.getElementById('idConglo').innerText;



    //valida que el resultado total cumpla
    if (resultadoTotal == null || resultadoTotal=='') {
        alert('Aún  no se asigna sus calificaciones.');
        return;
    }
    if (resultadoTotal < 1 || resultadoTotal > 100) {
        alert('Revise los pesos asignados, no puede ser menor a 1% ni mayor a 100%');
        return;
    }

    // Crear objeto con todos los datos
    const evaluacionData = {
        objetivosAsignados: objetivosAsignados,
        competenciasAsignadas: competenciasAsignadas,
        idEvaluacion: idEvaluacion,
        cedFuncionario: cedFuncionario,
        idConglo: idConglo
    };

    // Enviar al servidor
    enviarPeticionEvaluacion(evaluacionData);

}//fn

function obtenerDatosTabla(selector) {
    const filas = document.querySelectorAll(selector);
    return Array.from(filas).map(fila => {
        const celdas = fila.querySelectorAll('td');
        return {
            tipo: celdas[0].innerText,
            nombre: celdas[1].innerText,
            peso: celdas[2].innerText.replace('%', ''),
            meta: celdas[3].innerText,
            actual: celdas[4].innerText.replace('%', ''),
            id: celdas[6].innerText //en la posicion 5 lo que esta es el boton x esoe ste es 6
        };
    });
}

//fucion de la peticion
async function enviarPeticionEvaluacion(evaluacionData) {
    try {
        const data = await (await fetch('/Evaluacion/RealizarEvaluacionComoFuncionario', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(evaluacionData)
        })).json();

        if (!data.success) throw new Error(data.error);

        alert('Notas Actualizadas Correctamente');
    } catch (error) {
        alert(`Error: ${error.message}`);
    }
}//fin enviarPeticionEvaluacion