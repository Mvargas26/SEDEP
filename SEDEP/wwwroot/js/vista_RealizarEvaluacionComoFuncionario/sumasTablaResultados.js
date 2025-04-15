// Objeto global para almacenar los valores sumados
window.sumasGlobales = {
    objetivos: {},
    competencias: {},
    total: 0
};


function actualizarTablaResultados() {
    let sumaTotal = 0;

    // Combinar ambos tipos de sumas
    const sumasCombinadas = { ...window.sumasGlobales.objetivos, ...window.sumasGlobales.competencias };

    // Actualizar tabla de resultados
    document.querySelectorAll('#tablaResultados tbody tr').forEach(row => {
        const nombreTipoCell = row.querySelector('td:nth-child(1) strong');
        const inputValor = row.querySelector('td:nth-child(3) input');
        const tipoIdCell = row.querySelector('td[data-tipo-id]');

        if (nombreTipoCell && inputValor && tipoIdCell) {
            const nombreTipo = nombreTipoCell.textContent.trim();
            const suma = sumasCombinadas[nombreTipo] || 0;

            inputValor.value = suma.toFixed(2);
            sumaTotal += suma;
        }
    });

    // Actualizar total
    const resultadoTotal = document.getElementById('resultado-total');
    if (resultadoTotal) {
        resultadoTotal.value = sumaTotal.toFixed(2);
        resultadoTotal.style.backgroundColor = Math.abs(sumaTotal - 100) > 1 ? '#ffecec' : '';
    }

    window.sumasGlobales.total = sumaTotal;
}