
// FUNCIONES DE CRITERIOS DE BÚSQUEDA

function LimpiarCriterios() {
    window.location.reload();
}

// FUNCIONES DE GENERACIÓN DE REPORTES

function GenerarReporte(urlBase, formatoReporte) {

    // Se hace la llamada a la función de validación que comprobará cada reporte si existieran comprobaciones
    if (!validacionGeneracionReporte()) 
        return false;
    

    var filtros = $("#_frmBusqueda").serialize(); // Serializa los datos del formulario
    var url = urlBase + "?id=" + tipoReporte + "&" + filtros; // Construye la URL con los filtros
    console.log(url)
    $.ajax({
        url: url, // URL para obtener los datos
        type: 'GET',
        success: function (data) {

            var myObject = data.reporte;

            if (formatoReporte == 'excel')
                descargarExcel(myObject);

            if (formatoReporte == 'pdf')
                descargarPDF(myObject);

        },
        error: function (jqXHR, textStatus, errorThrown) {
            var mensaje = "Ocurrió un error en la llamada AJAX: " + textStatus + " - " + errorThrown;
            Mensaje(mensaje, 1);
            return;
        }
    });

}

function validacionGeneracionReporte() {

    var resp = true;
    
    switch (tipoReporte) {
        // case 4: // Tarjeta de Existencia de Producto (Kardex)

        //     if (parseInt($("#cbIdBodega").val()) == 0 || parseInt($("#cbIdProductoAlimenticio").val()) == 0 || parseInt($("#cbIdUnidadMedida").val()) == 0 || parseInt($("#cbIdConvenio").val()) == 0) {

        //         Swal.fire({
        //             title: "Error!",
        //             text: "Debe de completar todos los filtros de búsqueda para descargar el reporte.",
        //             icon: "warning"
        //         });

        //         resp = false;
        //     }

        //     break;
        case 9: // Constancia de Salida de Alimentos

            if ($("#txtNoConstancia").val() == '') {

                Swal.fire({
                    title: "Error!",
                    text: "Debe de completar todos los filtros de búsqueda para descargar el reporte.",
                    icon: "warning"
                });

                resp = false;
            }

            break;
    }

    return resp;
}


// Descarga de Excel en formato xlsx (abre perfecto pero no muestra estilos)
function descargarExcel(tblHtml) {
    // Crear un contenedor temporal
    var tempDiv = document.createElement("div");
    tempDiv.innerHTML = tblHtml;

    // Obtener la tabla como un elemento real del DOM
    var tabla = tempDiv.querySelector("table");

    if (!tabla) {
        console.error("No se encontró una tabla válida en el HTML proporcionado.");
        return;
    }

    // Convertir la tabla a una hoja de cálculo
    var wb = XLSX.utils.table_to_book(tabla, { sheet: nombreReporte.substring(0, 30) });

    // Escribir el archivo en formato binario
    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });

    // Función para convertir a Blob
    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    // Descargar el archivo
    saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), nombreReporte + ".xlsx");
}

function descargarPDF(tblHtml) {
    // Limpiar el HTML de la tabla
    tblHtml = limpiarHTML(tblHtml);

    // Crear un contenedor temporal
    var tempDiv = document.createElement("div");
    tempDiv.innerHTML = tblHtml;

    // Obtener la tabla como un elemento real del DOM
    var tabla = tempDiv.querySelector("table");

    if (!tabla) {
        console.error("No se encontró una tabla válida en el HTML proporcionado.");
        return;
    }

    // Agregar la tabla al cuerpo del documento temporalmente
    document.body.appendChild(tabla);

    // Usar html2canvas para convertir la tabla en una imagen
    html2canvas(tabla, {
        scale: 2, // Aumentar la escala para mejorar la calidad
        logging: true // Habilitar logging para depuración
    }).then(function (canvas) {
        // Eliminar la tabla del DOM después de capturarla
        document.body.removeChild(tabla);

        var imgData = canvas.toDataURL('image/jpeg');
        var pdf = new jspdf.jsPDF({
            orientation: 'landscape', // Orientación horizontal
            unit: 'mm',
            format: 'a4'
        });

        var imgWidth = 280; // Ancho de la imagen en el PDF
        var imgHeight = (canvas.height * imgWidth) / canvas.width; // Altura proporcional
        pdf.addImage(imgData, 'JPEG', 10, 10, imgWidth, imgHeight);

        // Descargar el PDF
        pdf.save(nombreReporte + ".pdf");
    }).catch(function (error) {
        console.error("Error al generar el PDF:", error);
        // Asegúrate de eliminar la tabla del DOM en caso de error
        if (tabla.parentNode) {
            document.body.removeChild(tabla);
        }
    });
}

function limpiarHTML(html) {
    return html.replace(/\r?\n|\r/g, "").replace(/\s+/g, " ").trim();
}





// Descarga de Excel en formato xls (muestra mensaje de advertencia antes de abrir documento)
function descargarExcelBKP(tbl) {

    var blob = new Blob(["\ufeff" + tbl], {
        type: "application/xls"
    });
    saveAs(blob, nombreReporte + ".xls");
}

// Descarga en Excel en formato xls (muestra mensaje de advertencia antes de abrir documento)
function descargarExcelBKP2(tblHtml, nombreReporte) {
    // Agregar encabezado para indicar que es un archivo Excel
    var htmlExcel =
        '<html xmlns:x="urn:schemas-microsoft-com:office:excel">' +
        '<head><meta charset="UTF-8">' +
        '<!--[if gte mso 9]>' +
        '<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>' +
        '<x:Name>Hoja1</x:Name>' +
        '<x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions>' +
        '</x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml>' +
        '<![endif]-->' +
        '</head><body>' +
        tblHtml +
        '</body></html>';

    // Crear el archivo Blob
    var blob = new Blob(["\ufeff" + htmlExcel], {
        type: "application/vnd.ms-excel;charset=utf-8"
    });

    // Descargar el archivo
    saveAs(blob, nombreReporte + ".xls");
}



