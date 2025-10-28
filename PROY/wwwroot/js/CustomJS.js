window.setTimeout(function () {
    $(".alert").fadeTo(500, 0).slideUp(500, function () {
        $(this).remove();
    });
}, 4000);


$(document).ready(function () {
    $('.table').DataTable({
        dom: 'Bfrtip',
        buttons: [
            "excel",
            "pdf",
            { extent: "print", text: "<span class='glyphicon glyphicon-print'></span> Print" },
            { extent: "excelHtml5", text: "<span class='glyphicon glyphicon-th-list'></span> Excel HTML5 Export" },
            { extent: "pdfHtml5", text: "<span class='glyphicon glyphicon-save'></span> PDF HTML5 Export", title: "Filename" }
        ], 
        "destroy": true,        
        "paging": true,          // Habilitar paginación
        "searching": true,       // Habilitar búsqueda
        "ordering": true,        // Habilitar ordenación de columnas
        "lengthMenu": [5, 10, 25, 50], // Opciones de número de registros por página     
        "info": true,  
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "No se encontraron resultados",
            "info": "Mostrando página _PAGE_ de _PAGES_ (Total de registros: _TOTAL_)",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(filtrado de _MAX_ registros totales)",
            "search": "Buscar:",
            "paginate": {
                "previous": "Anterior",
                "next": "Siguiente"
            }            
        }        
    });
});
