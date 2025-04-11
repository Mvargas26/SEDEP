document.getElementById("btnEnviarEvaluacion").addEventListener("click", enviarEvaluacion);

function enviarEvaluacion() {
    // Recolectar datos de ambas tablas
    const objetivos = obtenerDatosTabla('#tablaObjetivos tbody tr');
    const competencias = obtenerDatosTabla('#tablaCompetencias tbody tr');

    // Crear objeto con todos los datos
    const evaluacionData = {
        objetivos: objetivos,
        competencias: competencias
        // Puedes agregar más datos aquí si necesitas
    };

    // Enviar al servidor
    fetch('/Evaluacion/EvaluacionSubalterno', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(evaluacionData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error en la respuesta del servidor');
            }
            return response.json();
        })
        .then(data => {
            // Manejar respuesta exitosa
            alert('Evaluación enviada correctamente');
            // Redirigir o hacer algo más
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error al enviar la evaluación');
        });
}

// Función auxiliar para extraer datos de las filas de la tabla
function obtenerDatosTabla(selector) {
    const filas = document.querySelectorAll(selector);
    return Array.from(filas).map(fila => {
        const celdas = fila.querySelectorAll('td');
        return {
            id: celdas[0].innerText,
            nombre: celdas[1].innerText,
            tipo: celdas[2].innerText,
            peso: celdas[3].innerText.replace('%', ''),
            meta: celdas[4].innerText,
            actual: celdas[5].innerText.replace('%', '')
        };
    });
}