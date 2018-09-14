var TableDataEditableOfferType = function () {

    var handleTable = function () {

        function restoreRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);

            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                oTable.fnUpdate(aData[i], nRow, i, false);
            }

            oTable.fnDraw();
        }

        function editRow(oTable, nRow,id) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);
            jqTds[0].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[0] + '">';
            var che = aData[1] == '<span class="label label-sm label-success">Activa</span>' ? 'checked="checked"' : "";
            jqTds[1].innerHTML = '<input type="checkbox" class="check-box" ' + che + ' >';
            jqTds[2].innerHTML = '<a class="edit" href="" id="'+id+'">Guardar</a>';
            jqTds[3].innerHTML = '<a class="cancel" href="">Cancelar</a>';
        }

        function saveRow(oTable, nRow,id) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
            var activo = jqInputs[1].checked;
            var categoria = jqInputs[0].value;
            var actv = jqInputs[1].checked ? '<span class="label label-sm label-success">Activa</span>' : '<span class="label label-sm label-warning">Desactivada</span>';
            oTable.fnUpdate(actv, nRow, 1, false);
            oTable.fnUpdate('<a class="edit" href="" id="' + id + '">Editar</a>', nRow, 2, false);
            oTable.fnUpdate('<a class="delete" href="" id="' + id + '">Eliminar</a>', nRow, 3, false);
            oTable.fnDraw();
          
        }

        function cancelEditRow(oTable, nRow,id) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
            var actv = jqInputs[1].checked ? '<span class="label label-sm label-success"> Activa </span>' : '<span class="label label-sm label-warning"> Desactivada </span>';
            oTable.fnUpdate(actv, nRow, 1, false);
            oTable.fnUpdate('<a class="edit" href=""id="' + id + '">Editar</a>', nRow, 2, false);
            oTable.fnDraw();
        }

        var table = $('#tableOfferType');

        var oTable = table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No existen datos a mostrar",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ elementos",
                "infoEmpty": "No se encuentran elementos",
                "infoFiltered": "(Total de elementos: _MAX_)",
                "lengthMenu": "Mostrar _MENU_",
                "search": "Buscar:",
                "zeroRecords": "No existen coincidencias",
                "paginate": {
                    "previous": "Anterior",
                    "next": "Siguiente",
                    "last": "&Uacute;ltimo",
                    "first": "Primero"
                }
            },

            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "Todos"] // change per page values here
            ],

            //// Or you can use remote translation file
            ////"language": {
            ////   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            ////},

            // set the initial value
            "pageLength": 5,

            //"language": {
            //    "lengthMenu": " _MENU_ elementos"
            //},
            "columnDefs": [{ // set default column settings
                'orderable': true,
                'targets': [0]
            },
            {
                "searchable": true,
                "targets": [0]
            }],
            "order": [
                [0, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = $("#tableOfferType_wrapper");

        function crearDatos1(categoria, activo, oTable, nEditing) {
            var data = new FormData();
            data.append("NombreTipoOferta", categoria);
            data.append("EstadoTipoOferta", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/OfferType/Create",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableOfferType");
                            var newId = responseData.Data;
                            saveRow(oTable, nEditing, newId);
                           // $(nEditing).find("td:first").html("Untitled");
                            nEditing = null;
                            nNew = false;
                            $('#btnNewOfferType').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableOfferType");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableOfferType");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        function crearDatos(categoria, activo, oTable, nEditing)
        {
            var data = new FormData();
            data.append("NombreTipoOferta", categoria);
            data.append("EstadoTipoOferta", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/OfferType/Create",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableOfferType");
                            var newId = responseData.Data;
                            saveRow(oTable, nEditing, newId);
                            nEditing = null;
                            nNew = false;
                            $('#btnNewOfferType').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableOfferType");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableOfferType");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });            
        }

        function salvarDatos(categoria, activo, id, oTable, nEditing)
        {
            var data = new FormData();
            data.append("IdTipoOferta", id);
            data.append("NombreTipoOferta", categoria);
            data.append("EstadoTipoOferta", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/OfferType/Edit",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableOfferType");
                            saveRow(oTable, nEditing, id);
                            nEditing = null;
                            $('#btnNewOfferType').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableOfferType");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableOfferType");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        var nEditing = null;
        var nNew = false;
        var ifNewSave = true;

        $('#btnNewOfferType').click(function (e) {
            e.preventDefault();

            if (nNew && nEditing) {
                bootbox.confirm("La fila anterior no se ha guardado. Desea guardarla ahora?", function (result) {
                    if(result)
                    {
                        var jqInputs = $('input', nEditing);
                        var activo = jqInputs[1].checked;
                        var categoria = jqInputs[0].value;
                        if (categoria == "" || categoria.trim().length == 0) {
                            bootbox.dialog({
                                message: "El campo Categor&iacute;a es requerido",
                                title: "Informaci&oacute;n",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue"
                                    }
                                }
                            });
                        }
                        else {
                            crearDatos1(categoria, activo, oTable, nEditing);
                        }
                    }
                    else {
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;
                        return;
                    }
                });
            }
            else
            {
                var aiNew = oTable.fnAddData(['', '', '', '']);
                var nRow = oTable.fnGetNodes(aiNew[0]);
                editRow(oTable, nRow, 0);
                nEditing = nRow;
                nNew = true;
            }            
        });

        table.on('click', '.delete', function (e) {
            e.preventDefault();

            var nRow = $(this).parents('tr')[0];
            var id = $(this).attr('id');
            bootbox.confirm("Usted est&aacute; seguro que desea eliminar este elemento?", function (result) {
                if (result) {

                    var w = window.location.pathname.split("/");
                    var base = "/" + w[1];

                    var ajaxRequest = $.ajax({
                        type: "POST",
                        url: base + "/Admin/OfferType/Delete?id=" + id,
                    });

                    ajaxRequest.done(function (responseData, textStatus) {
                        if (textStatus === "success") {
                            if (responseData != null) {
                                if (responseData.Success) {
                                    ocultarWaitme("#tableOfferType");
                                    oTable.fnDeleteRow(nRow);
                                } else {
                                    ocultarWaitme("#tableOfferType");
                                    bootbox.dialog({
                                        message: responseData.Message,
                                        title: "Error",
                                        buttons: {
                                            main: {
                                                label: "Aceptar",
                                                className: "red"
                                            }
                                        }
                                    });
                                }
                            }
                        } else {
                            ocultarWaitme("#tableOfferType");
                            bootbox.dialog({
                                message: "Ha ocurrido un error interno en el servidor.",
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    });
                }
                else {
                    
                    return;
                }
            });            
        });

        table.on('click', '.cancel', function (e) {
            e.preventDefault();
            $('#btnNewOfferType').removeClass('disabled');
            if (nNew) {
                oTable.fnDeleteRow(nEditing);
                nEditing = null;
                nNew = false;
            } else {
                restoreRow(oTable, nEditing);
                nEditing = null;
            }
        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();

            if (this.innerHTML == "Editar") {
                $('#btnNewOfferType').addClass("disabled");
            }            

            if (nNew && nEditing && this.innerHTML == "Editar") {
                bootbox.confirm("La fila anterior no se ha guardado. Desea guardarla ahora?", function (result) {
                    if (result) {
                        var jqInputs = $('input', nEditing);
                        var activo = jqInputs[1].checked;
                        var categoria = jqInputs[0].value;
                        if (categoria == "" || categoria.trim().length == 0) {
                            bootbox.dialog({
                                message: "El campo Categor&iacute;a es requerido",
                                title: "Informaci&oacute;n",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue"
                                    }
                                }
                            });
                        }
                        else {
                            crearDatos1(categoria, activo, oTable, nEditing);
                        }
                    }
                    else {
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;
                        return;
                    }
                });
            }
            else {
                
                /* Get the row as a parent of the link that was clicked on */
                var nRow = $(this).parents('tr')[0];
                var id = $(this).attr('id');

                if (nEditing !== null && nEditing != nRow) {
                    /* Currently editing - but not this row - restore the old before continuing to edit mode */
                    nNew = false;
                    restoreRow(oTable, nEditing);
                    editRow(oTable, nRow, id);
                    nEditing = nRow;
                } else if (nEditing == nRow && this.innerHTML == "Guardar") {
                    /* Editing this row and want to save it */

                    var jqInputs = $('input', nEditing);
                    var activo = jqInputs[1].checked;
                    var categoria = jqInputs[0].value;

                    if (categoria == "" || categoria.trim().length == 0) {
                        bootbox.dialog({
                            message: "El campo Categor&iacute;a es requerido",
                            title: "Informaci&oacute;n",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue"
                                }
                            }
                        });
                    }
                    else
                    {                        
                        if (id == 0) {
                            crearDatos(categoria, activo, oTable, nEditing);
                        }
                        else {
                            salvarDatos(categoria, activo, id, oTable, nEditing);
                        }
                    }  

                } else {
                    /* No edit in progress - let's start one */
                    nNew = false;
                    editRow(oTable, nRow, id);
                    nEditing = nRow;
                }
            }
           
        });
    }

    return {

        //main function to initiate the module
        init: function () {
            handleTable();
        }

    };

}();

var TableDataEditableBlogType = function () {

    var handleTable = function () {

        function restoreRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);

            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                oTable.fnUpdate(aData[i], nRow, i, false);
            }

            oTable.fnDraw();
        }

        function editRow(oTable, nRow, id) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);
            jqTds[0].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[0] + '">';
            var che = aData[1] == '<span class="label label-sm label-success">Activa</span>' ? 'checked="checked"' : "";
            jqTds[1].innerHTML = '<input type="checkbox" class="check-box" ' + che + ' >';
            jqTds[2].innerHTML = '<a class="edit" href="" id="' + id + '">Guardar</a>';
            jqTds[3].innerHTML = '<a class="cancel" href="">Cancelar</a>';
        }

        function saveRow(oTable, nRow, id) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
            var activo = jqInputs[1].checked;
            var categoria = jqInputs[0].value;
            var actv = jqInputs[1].checked ? '<span class="label label-sm label-success">Activa</span>' : '<span class="label label-sm label-warning">Desactivada</span>';
            oTable.fnUpdate(actv, nRow, 1, false);
            oTable.fnUpdate('<a class="edit" href="" id="' + id + '">Editar</a>', nRow, 2, false);
            oTable.fnUpdate('<a class="delete" href="" id="' + id + '">Eliminar</a>', nRow, 3, false);
            oTable.fnDraw();

        }

        function cancelEditRow(oTable, nRow, id) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
            var actv = jqInputs[1].checked ? '<span class="label label-sm label-success"> Activa </span>' : '<span class="label label-sm label-warning"> Desactivada </span>';
            oTable.fnUpdate(actv, nRow, 1, false);
            oTable.fnUpdate('<a class="edit" href=""id="' + id + '">Editar</a>', nRow, 2, false);
            oTable.fnDraw();
        }

        var table = $('#tableBlogType');

        var oTable = table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No existen datos a mostrar",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ elementos",
                "infoEmpty": "No se encuentran elementos",
                "infoFiltered": "(Total de elementos: _MAX_)",
                "lengthMenu": "Mostrar _MENU_",
                "search": "Buscar:",
                "zeroRecords": "No existen coincidencias",
                "paginate": {
                    "previous": "Anterior",
                    "next": "Siguiente",
                    "last": "&Uacute;ltimo",
                    "first": "Primero"
                }
            },

            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "Todos"] // change per page values here
            ],

            //// Or you can use remote translation file
            ////"language": {
            ////   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            ////},

            // set the initial value
            "pageLength": 5,

            //"language": {
            //    "lengthMenu": " _MENU_ elementos"
            //},
            "columnDefs": [{ // set default column settings
                'orderable': true,
                'targets': [0]
            },
            {
                "searchable": true,
                "targets": [0]
            }],
            "order": [
                [0, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = $("#tableBlogType_wrapper");

        function crearDatos1(categoria, activo, oTable, nEditing) {
            var data = new FormData();
            data.append("NombreTipoBlog", categoria);
            data.append("EstadoTipoBlog", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/BlogType/Create",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableBlogType");
                            var newId = responseData.Data;
                            saveRow(oTable, nEditing, newId);
                            // $(nEditing).find("td:first").html("Untitled");
                            nEditing = null;
                            nNew = false;
                            $('#btnNewBlogType').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableBlogType");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableBlogType");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        function crearDatos(categoria, activo, oTable, nEditing) {
            var data = new FormData();
            data.append("NombreTipoBlog", categoria);
            data.append("EstadoTipoBlog", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/BlogType/Create",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableBlogType");
                            var newId = responseData.Data;
                            saveRow(oTable, nEditing, newId);
                            nEditing = null;
                            nNew = false;
                            $('#btnNewBlogType').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableBlogType");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableBlogType");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        function salvarDatos(categoria, activo, id, oTable, nEditing) {
            var data = new FormData();
            data.append("IdTipoBlog", id);
            data.append("NombreTipoBlog", categoria);
            data.append("EstadoTipoBlog", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/BlogType/Edit",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableBlogType");
                            saveRow(oTable, nEditing, id);
                            nEditing = null;
                            $('#btnNewBlogType').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableBlogType");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableBlogType");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        var nEditing = null;
        var nNew = false;
        var ifNewSave = true;

        $('#btnNewBlogType').click(function (e) {
            e.preventDefault();

            if (nNew && nEditing) {
                bootbox.confirm("La fila anterior no se ha guardado. Desea guardarla ahora?", function (result) {
                    if (result) {
                        var jqInputs = $('input', nEditing);
                        var activo = jqInputs[1].checked;
                        var categoria = jqInputs[0].value;
                        if (categoria == "" || categoria.trim().length == 0) {
                            ocultarWaitme("#tableBlogType");
                            bootbox.dialog({
                                message: "El campo Categor&iacute;a es requerido",
                                title: "Informaci&oacute;n",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue"
                                    }
                                }
                            });
                        }
                        else {
                            crearDatos1(categoria, activo, oTable, nEditing);
                        }
                    }
                    else {
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;
                        return;
                    }
                });
            }
            else {
                var aiNew = oTable.fnAddData(['', '', '', '']);
                var nRow = oTable.fnGetNodes(aiNew[0]);
                editRow(oTable, nRow, 0);
                nEditing = nRow;
                nNew = true;
            }
        });

        table.on('click', '.delete', function (e) {
            e.preventDefault();

            var nRow = $(this).parents('tr')[0];
            var id = $(this).attr('id');
            bootbox.confirm("Usted est&aacute; seguro que desea eliminar este elemento?", function (result) {
                if (result) {

                    var w = window.location.pathname.split("/");
                    var base = "/" + w[1];

                    var ajaxRequest = $.ajax({
                        type: "POST",
                        url: base + "/Admin/BlogType/Delete?id=" + id,
                    });

                    ajaxRequest.done(function (responseData, textStatus) {
                        if (textStatus === "success") {
                            if (responseData != null) {
                                if (responseData.Success) {
                                    ocultarWaitme("#tableBlogType");
                                    oTable.fnDeleteRow(nRow);
                                } else {
                                    ocultarWaitme("#tableBlogType");
                                    ocultarWaitme("#tableBlogType");
                                    bootbox.dialog({
                                        message: responseData.Message,
                                        title: "Error",
                                        buttons: {
                                            main: {
                                                label: "Aceptar",
                                                className: "red"
                                            }
                                        }
                                    });
                                }
                            }
                        } else {
                            ocultarWaitme("#tableBlogType");
                            bootbox.dialog({
                                message: "Ha ocurrido un error interno en el servidor.",
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    });
                }
                else {

                    return;
                }
            });
        });

        table.on('click', '.cancel', function (e) {
            e.preventDefault();
            $('#btnNewBlogType').removeClass('disabled');
            if (nNew) {
                oTable.fnDeleteRow(nEditing);
                nEditing = null;
                nNew = false;
            } else {
                restoreRow(oTable, nEditing);
                nEditing = null;
            }
        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();

            if (this.innerHTML == "Editar") {
                $('#btnNewBlogType').addClass("disabled");
            }

            if (nNew && nEditing && this.innerHTML == "Editar") {
                bootbox.confirm("La fila anterior no se ha guardado. Desea guardarla ahora?", function (result) {
                    if (result) {
                        var jqInputs = $('input', nEditing);
                        var activo = jqInputs[1].checked;
                        var categoria = jqInputs[0].value;
                        if (categoria == "" || categoria.trim().length == 0) {
                            bootbox.dialog({
                                message: "El campo Categor&iacute;a es requerido",
                                title: "Informaci&oacute;n",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue"
                                    }
                                }
                            });
                        }
                        else {
                            crearDatos1(categoria, activo, oTable, nEditing);
                        }
                    }
                    else {
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;
                        return;
                    }
                });
            }
            else {

                /* Get the row as a parent of the link that was clicked on */
                var nRow = $(this).parents('tr')[0];
                var id = $(this).attr('id');

                if (nEditing !== null && nEditing != nRow) {
                    /* Currently editing - but not this row - restore the old before continuing to edit mode */
                    nNew = false;
                    restoreRow(oTable, nEditing);
                    editRow(oTable, nRow, id);
                    nEditing = nRow;
                } else if (nEditing == nRow && this.innerHTML == "Guardar") {
                    /* Editing this row and want to save it */

                    var jqInputs = $('input', nEditing);
                    var activo = jqInputs[1].checked;
                    var categoria = jqInputs[0].value;

                    if (categoria == "" || categoria.trim().length == 0) {
                        bootbox.dialog({
                            message: "El campo Categor&iacute;a es requerido",
                            title: "Informaci&oacute;n",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue"
                                }
                            }
                        });
                    }
                    else {
                        if (id == 0) {
                            crearDatos(categoria, activo, oTable, nEditing);
                        }
                        else {
                            salvarDatos(categoria, activo, id, oTable, nEditing);
                        }
                    }

                } else {
                    /* No edit in progress - let's start one */
                    nNew = false;
                    editRow(oTable, nRow, id);
                    nEditing = nRow;
                }
            }
        });
    }

    return {

        //main function to initiate the module
        init: function () {
            handleTable();
        }

    };

}();

var TableDataEditableInterestArea = function () {

    var handleTable = function () {

        function restoreRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);

            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                oTable.fnUpdate(aData[i], nRow, i, false);
            }

            oTable.fnDraw();
        }

        function editRow(oTable, nRow, id) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);
            jqTds[0].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[0] + '">';
            var che = aData[1] == '<span class="label label-sm label-success">Activa</span>' ? 'checked="checked"' : "";
            jqTds[1].innerHTML = '<input type="checkbox" class="check-box" ' + che + ' >';
            jqTds[2].innerHTML = '<a class="edit" href="" id="' + id + '">Guardar</a>';
            jqTds[3].innerHTML = '<a class="cancel" href="">Cancelar</a>';
        }

        function saveRow(oTable, nRow, id) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
            var activo = jqInputs[1].checked;
            var categoria = jqInputs[0].value;
            var actv = jqInputs[1].checked ? '<span class="label label-sm label-success">Activa</span>' : '<span class="label label-sm label-warning">Desactivada</span>';
            oTable.fnUpdate(actv, nRow, 1, false);
            oTable.fnUpdate('<a class="edit" href="" id="' + id + '">Editar</a>', nRow, 2, false);
            oTable.fnUpdate('<a class="delete" href="" id="' + id + '">Eliminar</a>', nRow, 3, false);
            oTable.fnDraw();

        }

        function cancelEditRow(oTable, nRow, id) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 0, false);
            var actv = jqInputs[1].checked ? '<span class="label label-sm label-success"> Activa </span>' : '<span class="label label-sm label-warning"> Desactivada </span>';
            oTable.fnUpdate(actv, nRow, 1, false);
            oTable.fnUpdate('<a class="edit" href=""id="' + id + '">Editar</a>', nRow, 2, false);
            oTable.fnDraw();
        }

        var table = $('#tableInterestArea');

        var oTable = table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No existen datos a mostrar",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ elementos",
                "infoEmpty": "No se encuentran elementos",
                "infoFiltered": "(Total de elementos: _MAX_)",
                "lengthMenu": "Mostrar _MENU_",
                "search": "Buscar:",
                "zeroRecords": "No existen coincidencias",
                "paginate": {
                    "previous": "Anterior",
                    "next": "Siguiente",
                    "last": "&Uacute;ltimo",
                    "first": "Primero"
                }
            },

            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "Todos"] // change per page values here
            ],

            //// Or you can use remote translation file
            ////"language": {
            ////   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            ////},

            // set the initial value
            "pageLength": 5,

            //"language": {
            //    "lengthMenu": " _MENU_ elementos"
            //},
            "columnDefs": [{ // set default column settings
                'orderable': true,
                'targets': [0]
            },
            {
                "searchable": true,
                "targets": [0]
            }],
            "order": [
                [0, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = $("#tableInterestArea_wrapper");

        function crearDatos1(categoria, activo, oTable, nEditing) {
            var data = new FormData();
            data.append("NombreTipoArea", categoria);
            data.append("EstadoTipoArea", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/InterestArea/Create",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableInterestArea");
                            var newId = responseData.Data;
                            saveRow(oTable, nEditing, newId);
                            // $(nEditing).find("td:first").html("Untitled");
                            nEditing = null;
                            nNew = false;
                            $('#btnNewInterestArea').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableInterestArea");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableInterestArea");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        function crearDatos(categoria, activo, oTable, nEditing) {
            var data = new FormData();
            data.append("NombreTipoArea", categoria);
            data.append("EstadoTipoArea", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/InterestArea/Create",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableInterestArea");
                            var newId = responseData.Data;
                            saveRow(oTable, nEditing, newId);
                            nEditing = null;
                            nNew = false;
                            $('#btnNewInterestArea').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableInterestArea");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableInterestArea");
                    bootbox.dialog({
                        message: "Ha ocurrido un error interno en el servidor.",
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        function salvarDatos(categoria, activo, id, oTable, nEditing) {
            var data = new FormData();
            data.append("IdTipoArea", id);
            data.append("NombreTipoArea", categoria);
            data.append("EstadoTipoArea", activo);

            var w = window.location.pathname.split("/");
            var base = "/" + w[1];

            var ajaxRequest = $.ajax({
                type: "POST",
                url: base + "/Admin/InterestArea/Edit",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            ocultarWaitme("#tableInterestArea");
                            saveRow(oTable, nEditing, id);
                            nEditing = null;
                            $('#btnNewInterestArea').removeClass('disabled');
                        } else {
                            ocultarWaitme("#tableInterestArea");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    }
                } else {
                    ocultarWaitme("#tableInterestArea");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Ha ocurrido un error interno en el servidor.",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "red"
                            }
                        }
                    });
                }
            });
        }

        var nEditing = null;
        var nNew = false;
        var ifNewSave = true;

        $('#btnNewInterestArea').click(function (e) {
            e.preventDefault();

            if (nNew && nEditing) {
                bootbox.confirm("La fila anterior no se ha guardado. Desea guardarla ahora?", function (result) {
                    if (result) {
                        var jqInputs = $('input', nEditing);
                        var activo = jqInputs[1].checked;
                        var categoria = jqInputs[0].value;
                        if (categoria == "" || categoria.trim().length == 0) {
                            bootbox.dialog({
                                message: "El campo Categor&iacute;a es requerido",
                                title: "Informaci&oacute;n",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue"
                                    }
                                }
                            });
                        }
                        else {
                            crearDatos1(categoria, activo, oTable, nEditing);
                        }
                    }
                    else {
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;
                        return;
                    }
                });
            }
            else {
                var aiNew = oTable.fnAddData(['', '', '', '']);
                var nRow = oTable.fnGetNodes(aiNew[0]);
                editRow(oTable, nRow, 0);
                nEditing = nRow;
                nNew = true;
            }
        });

        table.on('click', '.delete', function (e) {
            e.preventDefault();

            var nRow = $(this).parents('tr')[0];
            var id = $(this).attr('id');
            bootbox.confirm("Usted est&aacute; seguro que desea eliminar este elemento?", function (result) {
                if (result) {

                    var w = window.location.pathname.split("/");
                    var base = "/" + w[1];

                    var ajaxRequest = $.ajax({
                        type: "POST",
                        url: base + "/Admin/InterestArea/Delete?id=" + id,
                    });

                    ajaxRequest.done(function (responseData, textStatus) {
                        if (textStatus === "success") {
                            if (responseData != null) {
                                if (responseData.Success) {
                                    ocultarWaitme("#tableInterestArea");
                                    oTable.fnDeleteRow(nRow);
                                } else {
                                    ocultarWaitme("#tableInterestArea");
                                    bootbox.dialog({
                                        message: responseData.Message,
                                        title: "Error",
                                        buttons: {
                                            main: {
                                                label: "Aceptar",
                                                className: "red"
                                            }
                                        }
                                    });
                                }
                            }
                        } else {
                            ocultarWaitme("#tableInterestArea");
                            bootbox.dialog({
                                message: "Ha ocurrido un error interno en el servidor.",
                                title: "Error",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "red"
                                    }
                                }
                            });
                        }
                    });
                }
                else {

                    return;
                }
            });
        });

        table.on('click', '.cancel', function (e) {
            e.preventDefault();
            $('#btnNewInterestArea').removeClass('disabled');
            if (nNew) {
                oTable.fnDeleteRow(nEditing);
                nEditing = null;
                nNew = false;
            } else {
                restoreRow(oTable, nEditing);
                nEditing = null;
            }
        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();

            if (this.innerHTML == "Editar") {
                $('#btnNewInterestArea').addClass("disabled");
            }

            if (nNew && nEditing && this.innerHTML == "Editar") {
                bootbox.confirm("La fila anterior no se ha guardado. Desea guardarla ahora?", function (result) {
                    if (result) {
                        var jqInputs = $('input', nEditing);
                        var activo = jqInputs[1].checked;
                        var categoria = jqInputs[0].value;
                        if (categoria == "" || categoria.trim().length == 0) {
                            bootbox.dialog({
                                message: "El campo Categor&iacute;a es requerido",
                                title: "Informaci&oacute;n",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue"
                                    }
                                }
                            });
                        }
                        else {
                            crearDatos1(categoria, activo, oTable, nEditing);
                        }
                    }
                    else {
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;
                        return;
                    }
                });
            }
            else {

                /* Get the row as a parent of the link that was clicked on */
                var nRow = $(this).parents('tr')[0];
                var id = $(this).attr('id');

                if (nEditing !== null && nEditing != nRow) {
                    /* Currently editing - but not this row - restore the old before continuing to edit mode */
                    nNew = false;
                    restoreRow(oTable, nEditing);
                    editRow(oTable, nRow, id);
                    nEditing = nRow;
                } else if (nEditing == nRow && this.innerHTML == "Guardar") {
                    /* Editing this row and want to save it */

                    var jqInputs = $('input', nEditing);
                    var activo = jqInputs[1].checked;
                    var categoria = jqInputs[0].value;

                    if (categoria == "" || categoria.trim().length == 0) {
                        bootbox.dialog({
                            message: "El campo Categor&iacute;a es requerido",
                            title: "Informaci&oacute;n",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue"
                                }
                            }
                        });
                    }
                    else {
                        if (id == 0) {
                            crearDatos(categoria, activo, oTable, nEditing);
                        }
                        else {
                            salvarDatos(categoria, activo, id, oTable, nEditing);
                        }
                    }

                } else {
                    /* No edit in progress - let's start one */
                    nNew = false;
                    editRow(oTable, nRow, id);
                    nEditing = nRow;
                }
            }
        });
    }

    return {

        //main function to initiate the module
        init: function () {
            handleTable();
        }

    };

}();

jQuery(document).ready(function() {
    TableDataEditableOfferType.init();
    TableDataEditableBlogType.init();
    TableDataEditableInterestArea.init();
});