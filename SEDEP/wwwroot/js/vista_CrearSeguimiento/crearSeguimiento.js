document.getElementById('btnCrearSeguimiento').addEventListener('click', function () {
    const confirmar = confirm("¿Está seguro que desea crear el seguimiento para este Sub alterno?");

    if (confirmar) {
        const idEvaluacion = document.getElementById('idEvaluacion').innerText;

        fetch('/Evaluacion/CrearSeguimiento', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ idEvaluacion: idEvaluacion })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert("Seguimiento creado Correctamente.");
                    window.location.href = data.redirectUrl || '/Evaluacion/Index';
                } else {
                    alert("Error: " + (data.message || "No se pudo crear el seguimiento"));
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert("Ocurrió un error al procesar la solicitud");
            });
    }
});