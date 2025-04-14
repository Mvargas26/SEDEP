
$(document).ready(function () {
    // Variables para mantener el estado
    let campoActualEdicionCompetencia = null;
    let pesoCompetencia = 0;
    let datosOriginalesCompetencia = null;

    // Función para cerrar el modal y resetear
    const cerrarModalCompetencia = () => {
        $('#editarActualCompetenciaModal').modal('hide');
        resetModalCompetencia();
    };

    // Función para resetear el estado del modal
    const resetModalCompetencia = () => {
        $('#modalValorActualCompetencia').val('');
        $('#valorErrorCompetencia').addClass('d-none');
        campoActualEdicionCompetencia = null;
        pesoCompetencia = 0;
        datosOriginalesCompetencia = null;
    };

    // Función para validar el valor ingresado
    const validarValorCompetencia = (valor) => {
        const numValor = parseFloat(valor);
        return !isNaN(numValor) && numValor >= 1 && numValor <= pesoCompetencia;
    };

    // Configurar el modal para evitar cierre accidental
    $('#editarActualCompetenciaModal').modal({
        backdrop: 'static',
        keyboard: false,
        show: false
    });

    // Manejador para abrir el modal
    $(document).on('click', '.btn-editar-actual-competencia', function () {
        const button = $(this);
        datosOriginalesCompetencia = {
            tipo: button.data('tipo'),
            nombre: button.data('nombre'),
            peso: button.data('peso'),
            meta: button.data('meta'),
            actual: button.data('actual'),
            id: button.data('id')
        };

        // Llenar datos en el modal
        $('#modalNombreCompetencia').text(datosOriginalesCompetencia.nombre);
        $('#modalTipoCompetencia').text(datosOriginalesCompetencia.tipo);
        $('#modalPesoCompetencia').text(`${datosOriginalesCompetencia.peso}%`);
        $('#modalMetaCompetencia').text(datosOriginalesCompetencia.meta);
        $('#modalValorActualCompetencia').val(datosOriginalesCompetencia.actual);
        $('#maxPesoCompetencia').text(datosOriginalesCompetencia.peso);

        // Configurar validación inicial
        pesoCompetencia = parseFloat(datosOriginalesCompetencia.peso);
        campoActualEdicionCompetencia = button.closest('tr').find('td:nth-child(5)');

        // Mostrar modal
        $('#editarActualCompetenciaModal').modal('show');
    });

    // Validación en tiempo real
    $('#modalValorActualCompetencia').on('input', function () {
        const valor = $(this).val();
        const errorElement = $('#valorErrorCompetencia');

        if (validarValorCompetencia(valor)) {
            errorElement.addClass('d-none');
        } else {
            errorElement.removeClass('d-none');
            errorElement.text(`El valor debe estar entre 1 y ${pesoCompetencia}%`);
        }
    });

    // Manejador para confirmar cambios
    $('#modalBtnConfirmarCompetencia').click(function () {
        const nuevoValor = $('#modalValorActualCompetencia').val();
        const errorElement = $('#valorErrorCompetencia');

        if (!validarValorCompetencia(nuevoValor)) {
            errorElement.text(`El valor debe estar entre 1 y ${pesoCompetencia}%`).removeClass('d-none');
            return;
        }

        // Actualizar visualmente en la tabla
        if (campoActualEdicionCompetencia) {
            campoActualEdicionCompetencia.text(`${nuevoValor}%`);
            campoActualEdicionCompetencia.data('actual', nuevoValor);
        }

        cerrarModalCompetencia();
    });

    // Manejadores para cerrar el modal
    $('#editarActualCompetenciaModal .close, #editarActualCompetenciaModal .btn-secondary').on('click', cerrarModalCompetencia);

    // Prevenir cierre del modal al hacer clic fuera
    $('#editarActualCompetenciaModal').on('click.dismiss.bs.modal', function (e) {
        if (e.target === this) {
            e.preventDefault();
            e.stopPropagation();
        }
    });
});