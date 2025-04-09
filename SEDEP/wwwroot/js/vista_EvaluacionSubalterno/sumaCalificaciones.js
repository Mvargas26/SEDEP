$(document).ready(function () {
    // Función para calcular el total
    function calcularTotal() {
        let total = 0;

        $('.input-calificacion').each(function () {
            total += parseFloat($(this).val()) || 0;
        });

        $('#resultado-total').val(total.toFixed(2));
    }

    // Escuchar cambios en los inputs
    $('.input-calificacion').on('input', calcularTotal);

    // Calcular inicialmente por si hay valores precargados
    calcularTotal();
});