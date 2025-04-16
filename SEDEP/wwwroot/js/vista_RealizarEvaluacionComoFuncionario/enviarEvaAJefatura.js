document.getElementById('btnEnviarEvaluacionJefatura').addEventListener('click', function () {
    const confirmar = confirm("¿Está seguro que desea enviar la evaluación a la Jefatura?");

    if (confirmar) {
        const idEvaluacion = document.getElementById('idEvaluacion').innerText;

        fetch('/Evaluacion/EnviarEvaluacionAlaJefatura', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ idEvaluacion: idEvaluacion })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert("Evaluación enviada correctamente a jefatura");
                    window.location.href = data.redirectUrl || '/Evaluacion/Index';
                } else {
                    alert("Error: " + (data.message || "No se pudo enviar la evaluación"));
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert("Ocurrió un error al procesar la solicitud");
            });
    }
});