ids = Array();

idsLab = Array();

$(document).ready(function () {
    $("#BranchLatitude").inputmask('decimal', {
        rightAlignNumerics: false
    });

    $("#BranchLongitude").inputmask('decimal', {
        rightAlignNumerics: false
    });
});

$.validator.addMethod("usercheck", function (user) {
    return /^[a-záéóóúàèìòùäëïöüñ\s]+$/i.test(user);
});

$.validator.addMethod("lettersDigits", function (user) {
    return /^[a-záéóóúàèìòùäëïöüñ.:,;0-9\s]+$/i.test(user);
});

$.validator.addMethod("pwcheck", function (password) {

    //initial strength
    var strength = 0;

    //if length is 8 characters or more, increase strength value
    if (password.length > 7) strength += 1;

    //if password contains both lower and uppercase characters, increase strength value
    if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1;

    //if it has numbers and characters, increase strength value
    if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1;

    //if it has one special character, increase strength value
    if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

    //if it has two special characters, increase strength value
    if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,",%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

    if (strength <= 3) {
        return false;
    } else {
        return true;
    }
});

$.validator.addMethod("emailcheck", function (email) {
    //return email.match(/(^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$)/);
    return /^[a-zA-Z0-9\._-]+@[a-zA-Z0-9-]{2,}[.][a-zA-Z]{2,4}$/.test(email);
});

function emptyImg(prefijoImg, arrayIds) {
    for (var it = 0; it < arrayIds.length; it++) {
        var id = "#" + prefijoImg + arrayIds[it];
        console.log($(id).get(0));
        var img = $(id).get(0).files;
        if (!(img.length > 0)) {
            return true;
        }
    }
    return false;
}

function isLastPorlet(idElement, tipoId) {
    var idsServer = $(idElement).attr('idsServer');

    var arrayIdsServer;
    var lenSer = 0;
    var lenClien = 0;
    if (idsServer.length > 0) {
        arrayIdsServer = idsServer.split(',');
        lenSer = arrayIdsServer.length - 1;
    }
    if (tipoId == 1)
    {
        lenClien = ids.length;
    }
    else
    {
        if (tipoId == 2) {
            lenClien = idsLab.length;
        } 
    }
    var cantElement = lenClien + lenSer;
    if (cantElement > 1) {
        return false;
    }
    return true;
}

function GetUrlBase() {
    var w = window.location.pathname.split("/");
    return "/" + w[1];
}

function readURL(input) {
    //var _URL = window.URL || window.webkitURL;

    var fileName = document.getElementById("uploadfile").value;
    var Extension = fileName.split(".")[1].toUpperCase();
    if (input.files && input.files[0]) {

        if (Extension == "PNG" || Extension == "JPG")//check image type
        {
            var reader = new FileReader();
            reader.readAsDataURL(input.files[0]);

            // ======================= check image height and width=========================
            reader.onload = function (e) {
                var imagepath = e.target.result;
                var imgUpload = document.getElementById("image");
                var img = new Image();
                img.onload = function () {
                    if (this.width > 501 && this.height > 501) {
                        document.getElementById("uploadfile").value = "";
                        imgUpload.src = "../vendor/template/assets/images/noimage.png";
                        bootbox.dialog({
                            message: "El tamaño de la imagen es muy grande",
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue"
                                }
                            }
                        });
                        //alert("The image you attempted to upload is too large...");
                    }
                };
                img.src = imagepath;
                imgUpload.src = imagepath;

            };


        }//check file type end
        else {
            document.getElementById("uploadfile").value = "";
            bootbox.dialog({
                message: "Solo se admiten imágenes con formato png, jpg",
                title: "Información",
                buttons: {
                    main: {
                        label: "Aceptar",
                        className: "blue"
                    }
                }
            });
        }
    }
}

function deleteElemExistente(element) {
    if (isLastPorlet('#idsServidor',1) == false) {
    var id1 = $(element).attr('id');
    var ids1 = $('#idsServidor').attr('idsServer');
    if (ids1 != "" && ids1 != undefined) {
        var arrayIds = ids1.split(',');
        var value = "";
        for (var i = 0; i < arrayIds.length - 1; i++) {
            if (id1 != arrayIds[i]) {
                value += arrayIds[i] + ",";
            }
        }
        $('#idsServidor').attr('idsServer',value);
    }

    $(element).parent().parent().parent().remove();
    } else {
        bootbox.dialog({
            message: "No se puede eliminar el último recuadro.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

function deleteElemExistenteLab(element) {
    if (isLastPorlet('#idsServidorLab',2) == false) {
    var id1 = $(element).attr('id');
    var ids1 = $('#idsServidorLab').attr('idsServer');
    if (ids1 != "" && ids1 != undefined) {
        var arrayIds = ids1.split(',');
        var value = "";
        for (var i = 0; i < arrayIds.length - 1; i++) {
            if (id1 != arrayIds[i]) {
                value += arrayIds[i] + ",";
            }
        }
        $('#idsServidorLab').attr('idsServer',value);
    }

    $(element).parent().parent().parent().remove();
    } else {
        bootbox.dialog({
            message: "No se puede eliminar el último recuadro.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

function deleteElem(divAEliminar) {
    if (isLastPorlet('#idsServidor', 1) == false) {
        var id = document.getElementById(divAEliminar);
        id.remove();
        var idRem = divAEliminar.split('_')[2];
        var arrNew = Array();
        for (var i = 0; i < ids.length; i++) {
            var va = ids[i];
            if (ids[i] != idRem) {
                arrNew.push(va);
            }
        }
        ids = arrNew;
    }else
    {
        bootbox.dialog({
            message: "No se puede eliminar el último recuadro.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
};

function deleteElemLab(divAEliminar) {
    if (isLastPorlet('#idsServidorLab',2) == false) {
        var id = document.getElementById(divAEliminar);
        id.remove();
        var idRem = divAEliminar.split('_')[2];
        var arrNew = Array();
        for (var i = 0; i < idsLab.length; i++) {
            var va = idsLab[i];
            if (idsLab[i] != idRem) {
                arrNew.push(va);
            }
        }
        idsLab = arrNew;
    } else {
        bootbox.dialog({
            message: "No se puede eliminar el último recuadro.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
};

function deleteElemWhoWeAre(divAEliminar) {

    var item = null;
    if (divAEliminar.id)
    {
        item = document.getElementById(divAEliminar.id);
    }
    else
    {
        item = item = document.getElementById(divAEliminar);
    }

    item.remove();
};

$("#BranchConsult").on("change", function (e) {

    var che = document.getElementById("BranchConsult").checked;
    if (che) {
        $("#BranchHour1").attr("disabled", "disabled");
        $("#BranchHour2").attr("disabled", "disabled");

        $("#BranchHour1").val("");
        $("#BranchHour2").val("");
    }
    else
    {
        $("#BranchHour1").removeAttr("disabled");
        $("#BranchHour2").removeAttr("disabled");
    }
    
});

function addHeaderWithOptionalLinkPortlet(optionalLink)
{
    if (!emptyImg("client_imageEncabezado_", ids)) {
        var idAdd = 0;
        if (ids.length > 0) {
            idAdd = ids[ids.length - 1];
        }
        ids.push(++idAdd);

        var divEncabezadoId = "client_divEncabezado_" + idAdd;
        var btnEliminarEncabezado = "client_btnEliminarEncabezado_" + idAdd;
        var textoEncabezado = "client_TextEncabezado_" + idAdd;
        var colorEncabezado = "client_ColorTextEncabezado_" + idAdd;
        var imagenEncabezado = "client_imageEncabezado_" + idAdd;

        var id = "'" + divEncabezadoId + "'";

        var html = '<div class="col-md-6 portlet portlet-sortable box blue-hoki" id="' + divEncabezadoId + '">' +
            '<div class="portlet-title">' +
            '<div class="caption">' +
            '<i class="fa fa-gift"></i>Encabezado' +
            '</div>' +
            '<div class="actions">' +
            '<a class="btn btn-default btn-sm" id="' + btnEliminarEncabezado + '" onclick="javascript: deleteElem(' + id + ');">' +
            '<i class="fa fa-remove"></i> Eliminar</a>' +
            '<a class="btn btn-sm btn-icon-only btn-default fullscreen" href="javascript:;"></a>' +
            '</div>' +
            '</div>' +
            '<div class="portlet-body">' +
            '<div class="form-group">' +
            '<div class="col-md-6">' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Texto</label>' +
            '<div>' +
            '<input type="text" id="' + textoEncabezado + '" name="' + textoEncabezado + '" class="form-control" value="" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Color</label>' +
            '<div>' +
            '<input type="text" id="' + colorEncabezado + '" name="' + colorEncabezado + '" class="form-control demo" data-control="wheel" value="" />' +
            '</div>' +
            '</div>' +
            '</div>';

        if (optionalLink === true)
        {
            var headerLink = "client_HeaderLink_" + idAdd;


            html += '<div class="row">' +
                '<div class="col-md-12">' +
                '<label class="control-label">Vínculo</label>' +
                '<div>' +
                '<input type="text" id="' + headerLink + '" name="' + headerLink + '" class="form-control" />' +
                '</div>' +
                '</div>' +
                '</div>';
        }


            html += '<div class="row">' +
            '<div class="col-md-12">' +
            '<div style="margin-top: 20px"><span class="label label-sm label-warning"> Nuevo </span></div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-6">' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Imagen</label>' +
            '<div>' +
            '<div class="fileinput fileinput-new" data-provides="fileinput">' +
            '<div class="fileinput-preview thumbnail" data-trigger="fileinput" style="height: 100px; width:200px">' +
            '</div>' +
            '<div>' +
            '<span class="btn red btn-outline btn-file">' +
            '<span class="fileinput-new"> Seleccionar Imagen </span>' +
            '<span class="fileinput-exists"> Cambiar </span>' +
            '<input type="file" name="' + imagenEncabezado + '" id="' + imagenEncabezado + '">' +
            '</span>' +
            '<a href="javascript:;" class="btn red fileinput-exists" data-dismiss="fileinput"> Eliminar </a>' +
            '</div></div></div></div></div></div></div></div></div>';

        $("#sortablePortlets").prepend(html);
        ComponentsColorPickers.init();
    }
    else {
        bootbox.dialog({
            message: "Debe llenar el campo de imagen vacío.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

$("#btnAdicionarEncabezado").on("click", function (e) {
    addHeaderWithOptionalLinkPortlet();
});

$("#btnAdicionarEncabezadoConVincluo").on("click", function () {
    addHeaderWithOptionalLinkPortlet(true);
});

function isAnyImageInTitledSectionEmpty(variablePrefix)
{
    var currentImageControls = $('input[id^=' + variablePrefix + ']');

    var emptyControls = currentImageControls.filter(function (index) {
        var img = $(this).get(0).files;
        if (!(img.length > 0)) {
            return true;
        }
        return false;
    });

    if(emptyControls.length > 0)
    {
        return true;
    }

    return false;
}

function getAllImageContainerIdsSorted(variablePrefix)
{
    var currentImageControls = $('div[id^=' + variablePrefix + ']');

    if (currentImageControls.length > 0) {
        var elementIndexes = $.map(currentImageControls,function (item) {
            console.log(item);
            var id = String(item.id);
            var nameComponents = id.split("_");
            return nameComponents[1];
        });

        elementIndexes.sort(function (a, b) { return a - b });
        return elementIndexes;
    }
    return Array();
}

function getNextIndexForTitledSection(variablePrefix)
{
    var currentImageControlsIds = getAllImageContainerIdsSorted(variablePrefix);

    if (currentImageControlsIds.length > 0)
    {
        return currentImageControlsIds[currentImageControlsIds.length - 1] + 1;
    }

    return 0;   
}

function addDoctorsOfficePortlet(variableName)
{
    var prefix = "ClientDivImageContainerHead_";
    var imageInputPrefix = "clientImageContainerHead_";

    if (!isAnyImageInTitledSectionEmpty(imageInputPrefix)) {
        var nextID = getNextIndexForTitledSection(prefix);

        var textoEncabezado = variableName + "[" + nextID + "].Title";
        var divEncabezado = prefix + nextID;
        var colorEncabezado = variableName + "[" + nextID + "].TitleColor";
        var imagenEncabezado = imageInputPrefix + variableName + "_" + nextID;
        var btnEliminarEncabezado = "client_btnDoctorsOfficeEliminarEncabezado_" + nextID;

        var id = "'" + divEncabezado + "'";

        var html = '<div class="col-md-6 portlet portlet-sortable box blue-hoki" id="' + divEncabezado + '">' +
            '<div class="portlet-title">' +
            '<div class="caption">' +
            '<i class="fa fa-gift"></i>Encabezado' +
            '</div>' +
            '<div class="actions">' +
            '<a class="btn btn-default btn-sm" id="' + btnEliminarEncabezado + '" onclick="javascript: deleteElemWhoWeAre(' + id + ');">' +
            '<i class="fa fa-remove"></i> Eliminar</a>' +
            '<a class="btn btn-sm btn-icon-only btn-default fullscreen" href="javascript:;"></a>' +
            '</div>' +
            '</div>' +
            '<div class="portlet-body">' +
            '<div class="form-group">' +
            '<div class="col-md-6">';

        var str = '<input type="hidden" id ="' + (prefix + "hidden" + nextID) + '" name="' + variableName + '.Index" value="' + nextID + '" />';

        html += str;
        html += '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Color</label>' +
            '<div>' +
            '<input type="text" id="' + colorEncabezado + '" name="' + colorEncabezado + '" class="form-control demo" data-control="wheel" value="" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Texto</label>' +
            '<div>' +
            '<input type="text" id="' + textoEncabezado + '" name="' + textoEncabezado + '" class="form-control"/>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<div style="margin-top: 20px"><span class="label label-sm label-warning"> Nuevo </span></div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-6">' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Imagen</label>' +
            '<div>' +
            '<div class="fileinput fileinput-new" data-provides="fileinput">' +
            '<div class="fileinput-preview thumbnail" data-trigger="fileinput" style="height: 100px; width:200px">' +
            '</div>' +
            '<div>' +
            '<span class="btn red btn-outline btn-file">' +
            '<span class="fileinput-new"> Seleccionar Imagen </span>' +
            '<span class="fileinput-exists"> Cambiar </span>' +
            '<input type="file" name="' + imagenEncabezado + '" id="' + imagenEncabezado + '">' +
            '</span>' +
            '<a href="javascript:;" class="btn red fileinput-exists" data-dismiss="fileinput"> Eliminar </a>' +
            '</div></div></div></div></div></div></div></div></div>';

        $("#sortablePortlets").prepend(html);
        ComponentsColorPickers.init();
    }
    else {
        bootbox.dialog({
            message: "Debe llenar el campo de imagen vacío.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

$("#btnAddDoctorsOfficeHeadImage").on("click", function (e) {
    addDoctorsOfficePortlet("HeadImages");
});
$("#btnAddDoctorsOfficeServiceImage").on("click", function (e) {
    addDoctorsOfficePortlet("ServicesSection");
});

$("#btnAddWhoWeAreHeadImage").on("click", function (e) {
    var prefix = "ClientDivImageContainerHead_";
    var imageInputPrefix = "clientImageContainerHead_";

    if (!isAnyImageInTitledSectionEmpty(imageInputPrefix)) {
        var nextID = getNextIndexForTitledSection(prefix);

        var textoEncabezado = "HeadImages[" + nextID + "].Title";
        var divEncabezado = prefix + nextID;
        var colorEncabezado = "HeadImages[" + nextID + "].TileColor";
        var imagenEncabezado = imageInputPrefix +"HeadImages_"+ nextID;
        var btnEliminarEncabezado = "client_btnWhoWeAreEliminarEncabezado_" + nextID;

        var id = "'" + divEncabezado + "'";

        var html = '<div class="col-md-6 portlet portlet-sortable box blue-hoki" id="' + divEncabezado + '">' +
        '<div class="portlet-title">' +
        '<div class="caption">' +
        '<i class="fa fa-gift"></i>Encabezado' +
        '</div>' +
        '<div class="actions">' +
        '<a class="btn btn-default btn-sm" id="' + btnEliminarEncabezado + '" onclick="javascript: deleteElemWhoWeAre(' + id + ');">' +
        '<i class="fa fa-remove"></i> Eliminar</a>' +
        '<a class="btn btn-sm btn-icon-only btn-default fullscreen" href="javascript:;"></a>' +
        '</div>' +
        '</div>' +
        '<div class="portlet-body">' +
        '<div class="form-group">' +
        '<div class="col-md-6">' +
        '<input type="hidden" id ="' + (prefix + "hidden" + nextID) + '" name="HeadImages.Index" value="' + nextID + '" />' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<label class="control-label">Color</label>' +
        '<div>' +
        '<input type="text" id="' + colorEncabezado + '" name="' + colorEncabezado + '" class="form-control demo" data-control="wheel" value="" />' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<label class="control-label">Texto</label>' +
        '<div>' +
        '<input type="text" id="' + textoEncabezado + '" name="' + textoEncabezado + '" class="form-control"/>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<div style="margin-top: 20px"><span class="label label-sm label-warning"> Nuevo </span></div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="col-md-6">' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<label class="control-label">Imagen</label>' +
        '<div>' +
        '<div class="fileinput fileinput-new" data-provides="fileinput">' +
        '<div class="fileinput-preview thumbnail" data-trigger="fileinput" style="height: 100px; width:200px">' +
        '</div>' +
        '<div>' +
        '<span class="btn red btn-outline btn-file">' +
        '<span class="fileinput-new"> Seleccionar Imagen </span>' +
        '<span class="fileinput-exists"> Cambiar </span>' +
        '<input type="file" name="' + imagenEncabezado + '" id="' + imagenEncabezado + '">' +
        '</span>' +
        '<a href="javascript:;" class="btn red fileinput-exists" data-dismiss="fileinput"> Eliminar </a>' +
        '</div></div></div></div></div></div></div></div></div>';

        $("#sortablePortlets").prepend(html);
        ComponentsColorPickers.init();
    }
    else {
        bootbox.dialog({
            message: "Debe llenar el campo de imagen vacío.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
});

$("#btnAddWhoWeAreValueImage").on("click", function (e) {
    var prefix = "ClientDivImageContainerValues_";
    var imageInputPrefix = "clientImageContainerValues_";

        if (!isAnyImageInTitledSectionEmpty(imageInputPrefix)) {
            var nextID = getNextIndexForTitledSection(prefix);

            var textoEncabezado = "ValuesSection[" + nextID + "].Title";
            var divEncabezado = prefix + nextID;
            var colorEncabezado = "ValuesSection[" + nextID + "].TileColor";
            var imagenEncabezado = imageInputPrefix +"ValuesSection_" + nextID;
            var btnEliminarEncabezado = "client_btnWhoWeAreEliminarEncabezado_" + nextID;
            var messageColor = "ValuesSection[" + nextID + "].TextColor";
            var messageText = "ValuesSection[" + nextID + "].Text";

            var id = "'" + divEncabezado + "'";

            var html = '<div class="col-md-6 portlet portlet-sortable box blue-hoki" id="' + divEncabezado + '" style="height:305px; margin-top:5px; margin-bottom:5px">' +
            '<div class="portlet-title">' +
            '<div class="caption">' +
            '<i class="fa fa-gift"></i>Valor' +
            '</div>' +
            '<div class="actions">' +
            '<a class="btn btn-default btn-sm" id="' + btnEliminarEncabezado + '" onclick="javascript: deleteElemWhoWeAre(' + id + ');">' +
            '<i class="fa fa-remove"></i> Eliminar</a>' +
            '<a class="btn btn-sm btn-icon-only btn-default fullscreen" href="javascript:;"></a>' +
            '</div>' +
            '</div>' +
            '<div class="portlet-body">' +
            '<div class="form-group">' +
            '<div class="col-md-6">' +
            '<input type="hidden" id ="' + (prefix + "hidden" + nextID) + '" name="ValuesSection.Index" value="' + nextID + '" />' +
            '<div class="row  col-md-12">' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Color del título</label>' +
            '<div>' +
            '<input type="text" id="' + colorEncabezado + '" name="' + colorEncabezado + '" class="form-control demo" data-control="wheel" value="" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Color del Mensaje</label>' +
            '<div>' +
            '<input type="text" id="'+messageColor+'" name="'+messageColor+'" class="form-control demo"' +
            'data-control="wheel" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Texto</label>' +
            '<div>' +
            '<input type="text" id="' + textoEncabezado + '" name="' + textoEncabezado + '" class="form-control"/>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-6" style="padding:0px">' +
            '<div class="row">' +
            '<div class="col-md-12">' +
            '<label class="control-label">Imagen</label>' +
            '<div>' +
            '<div class="fileinput fileinput-new" data-provides="fileinput" style="height: 100px; width:180px">' +
            '<div class="fileinput-preview thumbnail" data-trigger="fileinput" style="height: 100px; width:180px">' +
            '</div>' +
            '<div>' +
            '<span class="btn red btn-outline btn-file">' +
            '<span class="fileinput-new"> Seleccionar Imagen </span>' +
            '<span class="fileinput-exists"> Cambiar </span>' +
            '<input type="file" name="' + imagenEncabezado + '" id="' + imagenEncabezado + '">' +
            '</span>' +
            '<a href="javascript:;" class="btn red fileinput-exists" data-dismiss="fileinput"> Eliminar </a>' +
            '</div></div></div></div></div>'+
            '<div class="row">' +
             '<div class="col-md-12">' +
             '<div style="margin-top: 10px"><span class="label label-sm label-warning"> Nuevo </span></div>' +
             '</div>' +
             '</div>' +
             '</div>' +

             '<div class="row col-md-12">'+
             '<div class="col-md-12" style="margin-top: -8px">'+
             '<label class="control-label">Mensaje</label>'+
             '<div>'+
             '<input type="text" id="'+messageText+'" name="'+messageText+'" class="form-control" />'+
             '</div>'+
             '</div>'+
             '</div> ' +
             '</div></div></div>';

            $("#sortablePortletsValues").prepend(html);
            ComponentsColorPickers.init();
        }
    else {
        bootbox.dialog({
            message: "Debe llenar el campo de imagen vacío.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
});

$("#btnAddWhoWeAreHistoryImage").on("click", function (e) {
    var prefix = "ClientDivImageContainerHistory_";
    var imageInputPrefix = "clientImageContainerHistory_";

    if (!isAnyImageInTitledSectionEmpty(imageInputPrefix)) {
        var nextID = getNextIndexForTitledSection(prefix);

        var textoEncabezado = "HistoryImages[" + nextID + "].Title";
        var divEncabezado = prefix + nextID;
        var colorEncabezado = "HistoryImages[" + nextID + "].TileColor";
        var imagenEncabezado = imageInputPrefix + "HistoryImages_" + nextID;
        var btnEliminarEncabezado = "client_btnWhoWeAreEliminarHistoria_" + nextID;
        var messageColor = "HistoryImages[" + nextID + "].TextColor";
        var messageText = "HistoryImages[" + nextID + "].Text";
        var subtitleColor = "HistoryImages[" + nextID + "].SubTitleColor";
        var subtitle = "HistoryImages[" + nextID + "].SubTitle";
        

        var id = "'" + divEncabezado + "'";

        var html = '<div class="col-md-6 portlet portlet-sortable box blue-hoki"  style="height:430px; margin-top:5px;margin-bottom:5px" id="' + divEncabezado + '">' +
             '<div class="portlet-title">' +
             '<div class="caption">' +
             '<i class="fa fa-gift"></i>Historia' +
             '</div>' +
             '<div class="actions">' +
             '<a class="btn btn-default btn-sm" id="' + btnEliminarEncabezado + '" onclick="javascript: deleteElemWhoWeAre($(' + id + ');">' +
             '<i class="fa fa-remove"></i> Eliminar</a>' +
             '<a class="btn btn-sm btn-icon-only btn-default fullscreen" href="javascript:;"></a>' +
             '</div>' +
             '</div>' +
             '<div class="portlet-body">' +
             '<div class="form-group">' +
             '<div class="col-md-6">' +
             '<input type="hidden" id ="' + (prefix + "hidden" + nextID) + '" name="HistoryImages.Index" value="' + nextID + '" />' +
             '<div class="row  col-md-12">' +
             '<div class="row">' +
             '<div class="col-md-12">' +
             '<label class="control-label">Color del título</label>' +
             '<div>' +
             '<input type="text" id="' + colorEncabezado + '" name="' + colorEncabezado + '" class="form-control demo" data-control="wheel" value="" />' +
             '</div>' +
             '</div>' +
             '</div>' +
             '<div class="row">' +
             '<div class="col-md-12">' +
             '<label class="control-label">Color del subtítulo</label>' +
             '<div>' +
             '<input type="text" id="' + subtitleColor + '" name="' + subtitleColor + '" class="form-control demo" data-control="wheel" value="" />' +
             '</div>' +
             '</div>' +
             '</div>' +
             '<div class="row">' +
             '<div class="col-md-12">' +
             '<label class="control-label">Color del Mensaje</label>' +
             '<div>' +
             '<input type="text" id="' + messageColor + '" name="' + messageColor + '" class="form-control demo"' +
             'data-control="wheel" />' +
             '</div>' +
             '</div>' +
             '</div>' +
             '</div>' +
             '</div>' +
             '<div class="col-md-6" style="padding:0">' +
             '<div class="row">' +
             '<div class="col-md-12">' +
             '<label class="control-label">Imagen</label>' +
             '<div>' +
             '<div class="fileinput fileinput-new" data-provides="fileinput" style="width: 180px; height: 100px;">' +
             '<div class="fileinput-preview thumbnail" data-trigger="fileinput" style="width: 180px; height: 100px;">' +
             '</div>' +
             '<div>' +
             '<span class="btn red btn-outline btn-file">' +
             '<span class="fileinput-new"> Seleccionar Imagen </span>' +
             '<span class="fileinput-exists"> Cambiar </span>' +
             '<input type="file" name="' + imagenEncabezado + '" id="' + imagenEncabezado + '">' +
             '</span>' +
             '<a href="javascript:;" class="btn red fileinput-exists" data-dismiss="fileinput"> Eliminar </a>' +
             '</div></div></div></div>' +
             '<div class="row">' +
              '<div class="col-md-12">' +
              '<div style="margin-top: 10px"><span class="label label-sm label-warning"> Nuevo </span></div>' +
              '</div>' +
              '</div>' +
              '</div></div>' +

             
             '<div class="col-md-12" style="margin-top: -12px">' +
             '<label class="control-label">Título</label>' +
             '<div>' +
             '<input type="text" id="' + textoEncabezado + '" name="' + textoEncabezado + '" class="form-control"/>' +
             '</div>' +
             '</div>' +
           

           
             '<div class="col-md-12">' +
             '<label class="control-label">Subtítulo</label>' +
             '<div>' +
             '<input type="text" id="' + subtitle + '" name="' + subtitle + '" class="form-control"/>' +
             '</div>' +
             '</div>' +
        

              
              '<div class="col-md-12">' +
              '<label class="control-label">Mensaje</label>' +
              '<div>' +
              '<input type="text" id="' + messageText + '" name="' + messageText + '" class="form-control" />' +
              '</div>' +
              '</div>' +
             

              

              '</div></div></div>';

        $("#sortablePortletsHistory").prepend(html);
        ComponentsColorPickers.init();
    }
    else {
        bootbox.dialog({
            message: "Debe llenar el campo de imagen vacío.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
});

$("#btnAdicionarEncabezadoLab").on("click", function (e) {
    if (!emptyImg("client_imageEncabezadoLab_", idsLab)) {
        var idAdd = 0;
        if (idsLab.length > 0) {
            idAdd = idsLab[ids.length - 1];
        }
        idsLab.push(++idAdd);

        var divEncabezadoId = "client_divEncabezadoLab_" + idAdd;
        var btnEliminarEncabezado = "client_btnEliminarEncabezadoLab_" + idAdd;
        var textoEncabezado = "client_TextEncabezadoLab_" + idAdd;
        var colorEncabezado = "client_ColorTextEncabezadoLab_" + idAdd;
        var imagenEncabezado = "client_imageEncabezadoLab_" + idAdd;

        var id = "'" + divEncabezadoId + "'";

        var html = '<div class="col-md-6 portlet portlet-sortable box blue-hoki" id="' + divEncabezadoId + '" >' +
        '<div class="portlet-title">' +
        '<div class="caption">' +
        '<i class="fa fa-gift"></i>Imagen de Carrusel' +
        '</div>' +
        '<div class="actions">' +
        '<a class="btn btn-default btn-sm" id="' + btnEliminarEncabezado + '" onclick="javascript: deleteElemLab(' + id + ');">' +
        '<i class="fa fa-remove"></i> Eliminar</a>' +
        '<a class="btn btn-sm btn-icon-only btn-default fullscreen" href="javascript:;"></a>' +
        '</div>' +
        '</div>' +
        '<div class="portlet-body">' +
        '<div class="form-group">' +
        '<div class="col-md-6">' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<label class="control-label">Texto</label>' +
        '<div>' +
        '<input type="text" id="' + textoEncabezado + '" name="' + textoEncabezado + '" class="form-control" value="" />' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<label class="control-label">Color</label>' +
        '<div>' +
        '<input type="text" id="' + colorEncabezado + '" name="' + colorEncabezado + '" class="form-control demo" data-control="wheel" value="" />' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<div style="margin-top: 20px"><span class="label label-sm label-warning"> Nuevo </span></div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="col-md-6">' +
        '<div class="row">' +
        '<div class="col-md-12">' +
        '<label class="control-label">Imagen</label>' +
        '<div>' +
        '<div class="fileinput fileinput-new" data-provides="fileinput">' +
        '<div class="fileinput-preview thumbnail" data-trigger="fileinput" style="height: 100px; width:200px">' +
        '</div>' +
        '<div>' +
        '<span class="btn red btn-outline btn-file">' +
        '<span class="fileinput-new"> Seleccionar Imagen </span>' +
        '<span class="fileinput-exists"> Cambiar </span>' +
        '<input type="file" name="' + imagenEncabezado + '" id="' + imagenEncabezado + '">' +
        '</span>' +
        '<a href="javascript:;" class="btn red fileinput-exists" data-dismiss="fileinput"> Eliminar </a>' +
        '</div></div></div></div></div></div></div></div></div>';

        $("#sortablePortletsLab").prepend(html);
        ComponentsColorPickers.init();
    }
    else {
        bootbox.dialog({
            message: "Debe llenar el campo de imagen vacío.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
});

function getOfferType() {
    run_waitMe("#submit_form", "Cargando...");
    var idOferta = $("#TipoOferta").val();
    var w = GetUrlBase();
    $.ajax({
        type: 'GET',
        url: w + '/Admin/OfferPage/EditAjax?id=' + idOferta,
        success: function (responseData) {
            if (responseData.Success) {
                ocultarWaitme("#submit_form");
                window.location.href = w + "/Admin/OfferPage/Edit?id=" + idOferta;
            } else {
                ocultarWaitme("#submit_form");
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
        },
        error:
            function (errormessage) {
                ocultarWaitme("#submit_form");
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

function getBlogType() {
    run_waitMe("#submit_form", "Cargando...");
    var idBlog = $("#TipoBlog").val();
    var w = getSiteRootUrl();
    $.ajax({
        type: 'GET',
        url: w + '/Admin/BlogPage/EditAjax?id=' + idBlog,
        success: function (responseData) {
            if (responseData.Success) {
                ocultarWaitme("#submit_form");
                window.location.href = w + "/Admin/BlogPage/Edit?id=" + idBlog;
            } else {
                ocultarWaitme("#submit_form");
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
        },
        error:
            function (errormessage) {
                ocultarWaitme("#submit_form");
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

function dataForm_Textos(data, formId) {
    var arra = $("#" + formId).serializeArray();
    for (var f = 0; f < arra.length; f++) {
        var name = arra[f].name;
        var type = document.getElementById(name).type;
        if (type === 'select-one') {
            var opt = $("#" + name + " option:selected");
            data.append(name, opt[0].value);
        } else {
            data.append(name, $("#" + name).val());
        }
    }
    return data;
}

function getDataForWhoWeAre(data, formId) {
    var arra = $("#" + formId).serializeArray();
    for (var f = 0; f < arra.length; f++) {
        var name = arra[f].name;

       /* if (name.includes("Index") == false) {
            var type = document.getElementById(name).type;
            if (type === 'select-one') {
                var opt = $("#" + name + " option:selected");
                data.append(name, opt[0].value);
            } else {
                var value = $("#" + name).val();
                data.append(name, value);
            }
        }
        else
        {*/
            data.append(name, arra[f].value);
        //}
    }
    return data;
}

function dataForm_Archivos(dataForm, formulario){

    $(formulario).find(':input').each(function () {
        var elemento = this;
        //Si recibe tipo archivo 'file'
        if (elemento.type === 'file') {
            if (elemento.value !== '') {
                console.log($('#' + elemento.id));
                for (var i = 0; i < $('#' + elemento.id).prop("files").length; i++) {
                    dataForm.append(elemento.name, $('#' + elemento.id).prop("files")[i]);
                }
            }
        }
    });
    return dataForm;
}

function getFilesForWhoWeAre(dataForm, form)
{
    $(form).find(':input').each(function () {
        var elemento = this;
        //Si recibe tipo archivo 'file'
        if (elemento.type === 'file') {
            if (elemento.value !== '' && elemento.files.length > 0) {
                dataForm.append(elemento.name, elemento.files[0]);
            }
        }
    });
    return dataForm;
}

function btnPreviewHomeClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if(valor == 0)
        valPrev = w + "/Admin/HomePage/PreviewEdit";
    else
        valPrev = w + "/Admin/HomePage/ApplyView";

    var data = new FormData();
    dataForm_Textos(data, "submit_form");
    dataForm_Archivos(data, "#submit_form");

    var idsCliente = "";
    for (var i = 0; i < ids.length; i++) {
        idsCliente += ids[i] + ",";
    }
    data.append('idsCliente', idsCliente);

    var ids2 = $('#idsServidor').attr('idsServer');
    data.append('idsServidor', ids2);

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=home",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/HomePage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/HomePage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

function addServerFilesReferenceToWhoWeAreModel(formData, serverIdsPrefix)
{
    var serverContainers = $("div[id^=" + serverIdsPrefix + "]");
    serverContainers.each(function () {
        var idComponents = this.id.split("_");
        var variableName = idComponents[1];
        var index = idComponents[2] + "_" + idComponents[3];
        var value = idComponents[3];
        var finalVariableName = variableName + '[' + index + '].ImageFileName';
        formData.append(finalVariableName, value);
    });
}

function addClientFilesReferenceToWhoWeAreModel(formData, clientIdsPrefix)
{
    
    var serverContainers = $("input[id^=" + clientIdsPrefix + "]");
    serverContainers.each(function (container) {

        var id = this.id;
        var idComponents = id.split("_");
        //imageInputPrefix +"ValuesSection_" + nextID;
        //imageInputPrefix + AdSection.ImageFileName
        var finalVariableName = "";
        var valueId = "";
     
        if (id.includes(']') == false)
        {
            var variableName = idComponents[1];
            var index = idComponents[2];
            valueId = index;
            finalVariableName = variableName + '[' + index + '].ImageFileName';
        }
        else
        {
            finalVariableName = idComponents[1] + '_' + idComponents[2];
            valueId = idComponents[2].split(']')[0];
        }
        
        if (this.files.length != 0)
        {
            formData.append(finalVariableName, this.name);
        }
        else
        {
            formData.append(finalVariableName, valueId);
        }

    });
}

function btnApplyWhoWeAreClick(valor, controllerName)
{
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/" + controllerName + "/PreviewPage";
    else
        valPrev = w + "/Admin/" + controllerName + "/AddOrEdit";

    var data = new FormData();
    getDataForWhoWeAre(data, "submit_form");
    getFilesForWhoWeAre(data, "#submit_form");

    /*var idsCliente = getAllImageContainerIdsSorted("ClientDivImages");
    var idsServidor = getAllImageContainerIdsSorted("serverImageContainer");

    data.append('clientIds', idsCliente);
    data.append('serverIds', idsServidor);*/
    addServerFilesReferenceToWhoWeAreModel(data, "serverImageContainer_");
    //debugger;
    addClientFilesReferenceToWhoWeAreModel(data, "clientImageContainer");

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev,
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor == 0) {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/" + controllerName + "/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/" + controllerName + "/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyWhoWeAre").on("click", function (e) {
    btnApplyWhoWeAreClick(1,"WhoWeAre");
});
$("#btnPreviewWhoWeAre").on("click", function (e) {
    btnApplyWhoWeAreClick(0, "WhoWeAre");
});

$("#btnApplyWhoWeAre").on("click", function (e) {
    btnApplyWhoWeAreClick(1, "WhoWeAre");
});
$("#btnPreviewDoctorsOffice").on("click", function (e) {
    btnApplyWhoWeAreClick(0, "DoctorsOffice");
});
$("#btnApplyDoctorsOffice").on("click", function (e) {
    btnApplyWhoWeAreClick(1, "DoctorsOffice");
});

$("#btnApplyHome").on("click", function (e) {
    btnPreviewHomeClick("1");
});

$("#btnPreviewHome").on("click", function (e) {
    btnPreviewHomeClick("0");
});

$("#btnApplyPillar").on("click", function (e) {
    btnPreviewPillarPageClick("1");
});

$("#btnPreviewPillar").on("click", function (e) {
    btnPreviewPillarPageClick("0");
});

function btnPreviewPillarPageClick(valor) {

    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/PillarPage/PreviewEdit";
    else
        valPrev = w + "/Admin/PillarPage/ApplyView";

    var data = new FormData();

    data.append("TextPillarPageText1", CKEDITOR.instances.TextPillarPageText1.getData());
    data.append("TextPillarPageText2", CKEDITOR.instances.TextPillarPageText2.getData());

    var ImagePillarPage = $("#ImagePillarPage").get(0).files;
    if (ImagePillarPage.length > 0) {
        data.append("ImagePillarPage", ImagePillarPage[0]);
    }
    else {
        data.append("ImagePillarPage", $("#imgImagePillarPage").attr('src'));
    }

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=pillar",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/PillarPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/PillarPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

function btnPreviewOfferClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/OfferPage/PreviewEdit";
    else
        valPrev = w + "/Admin/OfferPage/ApplyView";

    var data = new FormData();
    //dataForm_Textos(data, "submit_form");
    //dataForm_Archivos(data, "#submit_form");

    var ImagenProductosOfertas = $("#ImagenProductosOfertas").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ImagenProductosOfertas.length > 0) {
        data.append("ImagenProductosOfertas", ImagenProductosOfertas[0]);
    }

    data.append("TipoOferta", $("#TipoOferta").val());
    data.append("ColorFondoOfertas", $("#ColorFondoOfertas").val());
    data.append("TextoOfertas1", $("#TextoOfertas1").val());
    data.append("TextoOfertas2", $("#TextoOfertas2").val());
    data.append("TextoOfertas3", $("#TextoOfertas3").val());
    data.append("ColorTextoOfertas1", $("#ColorTextoOfertas1").val());
    data.append("ColorTextoOfertas2", $("#ColorTextoOfertas2").val());
    data.append("ColorTextoOfertas3", $("#ColorTextoOfertas3").val());
    data.append("TextoResaltadoOfertas1", $("#TextoResaltadoOfertas1").val());
    data.append("TextoResaltadoOfertas2", $("#TextoResaltadoOfertas2").val());
    data.append("TextoResaltadoOfertas3", $("#TextoResaltadoOfertas3").val());
    data.append("ColorTextoResaltadoOfertas1", $("#ColorTextoResaltadoOfertas1").val());
    data.append("ColorTextoResaltadoOfertas2", $("#ColorTextoResaltadoOfertas2").val());
    data.append("ColorTextoResaltadoOfertas3", $("#ColorTextoResaltadoOfertas3").val());
    data.append("TipoTexto1", $("#TipoTexto1").val());
    data.append("TipoTexto2", $("#TipoTexto2").val());
    data.append("TipoTexto3", $("#TipoTexto3").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=offer",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/OfferPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/OfferPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyOffer").on("click", function (e) {
    btnPreviewOfferClick("1");
});

$("#btnPreviewOffer").on("click", function (e) {
    btnPreviewOfferClick("0");
});

function btnPreviewCardClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/BillingPage/PreviewEdit";
    else
        valPrev = w + "/Admin/BillingPage/ApplyView";

    var data = new FormData();

    var inicio_imagenLab = "client_imageEncabezadoLab_";
    var inicio_textoLab = "client_TextEncabezadoLab_";
    var inicio_colorLab = "client_ColorTextEncabezadoLab_";
    var idsClienteLab= "";
    for (var i = 0; i < idsLab.length; i++) {
        idsClienteLab += idsLab[i] + ",";
        var clave_imgLab = inicio_imagenLab + idsLab[i];
        var id_imgLab = "#" + inicio_imagenLab + idsLab[i];
        var clave_textLab = inicio_textoLab + idsLab[i];
        var id_textLab = "#" + inicio_textoLab + idsLab[i];
        var clave_colorLab = inicio_colorLab + idsLab[i];
        var id_colorLab = "#" + inicio_colorLab + idsLab[i];
        data.append(clave_imgLab, $(id_imgLab).get(0).files[0]);
        data.append(clave_textLab, $(id_textLab).val());
        data.append(clave_colorLab, $(id_colorLab).val());
    }
    data.append('idsClienteLab', idsClienteLab);

    var inicio_texto_sLab = "server_TextEncabezadoLab_";
    var inicio_color_sLab = "server_ColorTextEncabezadoLab_";
    var ids2Lab = $('#idsServidorLab').attr('idsServer');
    if (ids2Lab != "" && ids2Lab != undefined) {
        var arrayIdsLab = ids2Lab.split(',');

        for (var i = 0; i < arrayIdsLab.length - 1; i++) {
            var clave_text_sLab = inicio_texto_sLab + arrayIdsLab[i];
            var id_text_sLab = "#" + inicio_texto_sLab + arrayIdsLab[i];
            var clave_color_sLab = inicio_color_sLab + arrayIdsLab[i];
            var id_color_sLab = "#" + inicio_color_sLab + arrayIdsLab[i];
            data.append(clave_text_sLab, $(id_text_sLab).val());
            data.append(clave_color_sLab, $(id_color_sLab).val());
        }
    }
    data.append('idsServidorLab', ids2Lab);

    var inicio_imagen = "client_imageEncabezado_";
    var inicio_texto = "client_TextEncabezado_";
    var inicio_color = "client_ColorTextEncabezado_";
    var idsCliente = "";
    for (var i = 0; i < ids.length; i++) {
        idsCliente += ids[i] + ",";
        var clave_img = inicio_imagen + ids[i];
        var id_img = "#" + inicio_imagen + ids[i];
        var clave_text = inicio_texto + ids[i];
        var id_text = "#" + inicio_texto + ids[i];
        var clave_color = inicio_color + ids[i];
        var id_color = "#" + inicio_color + ids[i];
        data.append(clave_img, $(id_img).get(0).files[0]);
        data.append(clave_text, $(id_text).val());
        data.append(clave_color, $(id_color).val());
    }
    data.append('idsCliente', idsCliente);

    var inicio_texto_s = "server_TextEncabezado_";
    var inicio_color_s = "server_ColorTextEncabezado_";
    var ids2 = $('#idsServidor').attr('idsServer');
    if (ids2 != "" && ids2 != undefined) {
        var arrayIds = ids2.split(',');
        for (var i = 0; i < arrayIds.length - 1; i++) {
            var clave_text_s = inicio_texto_s + arrayIds[i];
            var id_text_s = "#" + inicio_texto_s + arrayIds[i];
            var clave_color_s = inicio_color_s + arrayIds[i];
            var id_color_s = "#" + inicio_color_s + arrayIds[i];
            data.append(clave_text_s, $(id_text_s).val());
            data.append(clave_color_s, $(id_color_s).val());
        }
    }
    data.append('idsServidor', ids2);

    var BeneficiosTarjetaImagen = $("#BeneficiosTarjetaImagen").get(0).files;
    var BeneficiosTarjetaImagenXs = $("#BeneficiosTarjetaImagenXs").get(0).files;
    var AumentaBeneficiosImagen1 = $("#AumentaBeneficiosImagen1").get(0).files;
    var AumentaBeneficiosImagen2 = $("#AumentaBeneficiosImagen2").get(0).files;

    // Add the uploaded image content to the form data collection
    if (BeneficiosTarjetaImagen.length > 0) {
        data.append("BeneficiosTarjetaImagen", BeneficiosTarjetaImagen[0]);
    }
    if (BeneficiosTarjetaImagenXs.length > 0) {
        data.append("BeneficiosTarjetaImagenXs", BeneficiosTarjetaImagenXs[0]);
    }
    if (AumentaBeneficiosImagen1.length > 0) {
        data.append("AumentaBeneficiosImagen1", AumentaBeneficiosImagen1[0]);
    }
    if (AumentaBeneficiosImagen2.length > 0) {
        data.append("AumentaBeneficiosImagen2", AumentaBeneficiosImagen2[0]);
    }   
    
    data.append("BeneficiosTarjeta", CKEDITOR.instances.BeneficiosTarjeta.getData());
    data.append("BeneficiosTarjetaParrafo", CKEDITOR.instances.BeneficiosTarjetaParrafo.getData());
    data.append("AumentaBeneficiosTarjeta", CKEDITOR.instances.AumentaBeneficiosTarjeta.getData());
    data.append("TituloLaboratorios", $("#TituloLaboratorios").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=card",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/BillingPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/BillingPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyCard").on("click", function (e) {
    btnPreviewCardClick("1");
});

$("#btnPreviewCard").on("click", function (e) {
    btnPreviewCardClick("0");
});

function btnPreviewBranchClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/BranchPage/PreviewEdit";
    else
        valPrev = w + "/Admin/BranchPage/ApplyView";

    var data = new FormData();

    var inicio_imagen = "client_imageEncabezado_";
    var inicio_texto = "client_TextEncabezado_";
    var inicio_color = "client_ColorTextEncabezado_";
    var idsCliente = "";
    for (var i = 0; i < ids.length; i++) {
        idsCliente += ids[i] + ",";
        var clave_img = inicio_imagen + ids[i];
        var id_img = "#" + inicio_imagen + ids[i];
        var clave_text = inicio_texto + ids[i];
        var id_text = "#" + inicio_texto + ids[i];
        var clave_color = inicio_color + ids[i];
        var id_color = "#" + inicio_color + ids[i];
        data.append(clave_img, $(id_img).get(0).files[0]);
        data.append(clave_text, $(id_text).val());
        data.append(clave_color, $(id_color).val());
    }
    data.append('idsCliente', idsCliente);

    var inicio_texto_s = "server_TextEncabezado_";
    var inicio_color_s = "server_ColorTextEncabezado_";
    var ids2 = $('#idsServidor').attr('idsServer');
    if (ids2 != "" && ids2 != undefined) {
        var arrayIds = ids2.split(',');
        var value = "";
        for (var i = 0; i < arrayIds.length - 1; i++) {
            var clave_text_s = inicio_texto_s + arrayIds[i];
            var id_text_s = "#" + inicio_texto_s + arrayIds[i];
            var clave_color_s = inicio_color_s + arrayIds[i];
            var id_color_s = "#" + inicio_color_s + arrayIds[i];
            data.append(clave_text_s, $(id_text_s).val());
            data.append(clave_color_s, $(id_color_s).val());
        }
    }
    data.append('idsServidor', ids2);

    data.append("TextoSucursales1", $("#TextoSucursales1").val());
    data.append("TextoSucursales2", $("#TextoSucursales2").val());
    data.append("ColorTextoSucursales1", $("#ColorTextoSucursales1").val());
    data.append("ColorTextoSucursales2", $("#ColorTextoSucursales2").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=branch",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/BranchPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/BranchPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyBranch").on("click", function (e) {
    btnPreviewBranchClick("1");
});

$("#btnPreviewBranch").on("click", function (e) {
    btnPreviewBranchClick("0");
});

function btnPreviewFoseClick(valor) {

    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/FosePage/PreviewEdit";
    else
        valPrev = w + "/Admin/FosePage/ApplyView";

    var data = new FormData();

    var inicio_imagen = "client_imageEncabezado_";
    var inicio_texto = "client_TextEncabezado_";
    var inicio_color = "client_ColorTextEncabezado_";
    var idsCliente = "";
    for (var i = 0; i < ids.length; i++) {
        idsCliente += ids[i] + ",";
        var clave_img = inicio_imagen + ids[i];
        var id_img = "#" + inicio_imagen + ids[i];
        var clave_text = inicio_texto + ids[i];
        var id_text = "#" + inicio_texto + ids[i];
        var clave_color = inicio_color + ids[i];
        var id_color = "#" + inicio_color + ids[i];
        data.append(clave_img, $(id_img).get(0).files[0]);
        data.append(clave_text, $(id_text).val());
        data.append(clave_color, $(id_color).val());
    }
    data.append('idsCliente', idsCliente);

    var inicio_texto_s = "server_TextEncabezado_";
    var inicio_color_s = "server_ColorTextEncabezado_";
    var ids2 = $('#idsServidor').attr('idsServer');
    if (ids2 != "" && ids2 != undefined) {
        var arrayIds = ids2.split(',');
        for (var i = 0; i < arrayIds.length - 1; i++) {
            var clave_text_s = inicio_texto_s + arrayIds[i];
            var id_text_s = "#" + inicio_texto_s + arrayIds[i];
            var clave_color_s = inicio_color_s + arrayIds[i];
            var id_color_s = "#" + inicio_color_s + arrayIds[i];
            data.append(clave_text_s, $(id_text_s).val());
            data.append(clave_color_s, $(id_color_s).val());
        }
    }
    data.append('idsServidor', ids2);

    data.append("TextoSucursalesFose", CKEDITOR.instances.TextoSucursalesFose.getData());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=fose",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/FosePage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/FosePage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyFose").on("click", function (e) {
    btnPreviewFoseClick("1");
});

$("#btnPreviewFose").on("click", function (e) {
    btnPreviewFoseClick("0");
});

function btnPreviewBlogClick(valor) {

    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/BlogPage/PreviewEdit";
    else
        valPrev = w + "/Admin/BlogPage/ApplyView";

    var data = new FormData();

    data.append("ColorBgHeadBlogPage", $("#ColorBgHeadBlogPage").val());
    data.append("ColorTextHeadBlogPage", $("#ColorTextHeadBlogPage").val());
    data.append("TextHeadBlogPage", $("#TextHeadBlogPage").val());
    data.append("ColorTextDescHeadBlogPage", $("#ColorTextDescHeadBlogPage").val());
    data.append("TitleDescBlogPage", $("#TitleDescBlogPage").val());
    data.append("ColorTitleDescBlogPage", $("#ColorTitleDescBlogPage").val());
    data.append("ColorBgTitleDescBlogPage", $("#ColorBgTitleDescBlogPage").val());
    data.append("TextDescBlogPage", CKEDITOR.instances.TextDescBlogPage.getData());

    data.append("TipoBlog", $("#TipoBlog").val());

    var ImageBlogPage = $("#ImageBlogPage").get(0).files;
    if (ImageBlogPage.length > 0) {
        data.append("ImageBlogPage", ImageBlogPage[0]);
    }
    else {
        data.append("ImageBlogPage", $("#imgImageBlogPage").attr('src'));
    }
    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=blog",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/BlogPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/BlogPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyBlog").on("click", function (e) {
    btnPreviewBlogClick("1");
});

$("#btnPreviewBlog").on("click", function (e) {
    btnPreviewBlogClick("0");
});

function isValidNews() {
    var valido = true;

    var ImageHeadNewsPage = $("#ImageHeadNewsPage").get(0).files;
    var ImageDescriptionNewsPage = $("#ImageDescriptionNewsPage").get(0).files;
    if (!(ImageHeadNewsPage.length > 0)) {
        if ($("#imgImageHeadNewsPage").attr('src') == "") {
            return false;
        }
    }

    if (!(ImageDescriptionNewsPage.length > 0)) {
        if ($("#imgImageDescriptionNewsPage").attr('src') == "") {
            return false;
        }
    }    

    if ($("#ColorBgHeadNewsPage").val() == "" || $("#ColorBgHeadNewsPage").val().trim() == "") {
        return false;
    }
    if ($("#TextHeadNewsPage").val() == "" || $("#TextHeadNewsPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorTextHeadNewsPage").val() == "" || $("#ColorTextHeadNewsPage").val().trim() == "") {
        return false;
    }
    if ($("#SubTextHeadNewsPage").val() == "" || $("#SubTextHeadNewsPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorSubTextHeadNewsPage").val() == "" || $("#ColorSubTextHeadNewsPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorBgSubTextHeadNewsPage").val() == "" || $("#ColorBgSubTextHeadNewsPage").val().trim() == "") {
        return false;
    }
    if ($("#TitleDescription1NewsPage").val() == "" || $("#TitleDescription1NewsPage").val().trim() == "") {
        return false;
    }
    /*if ($("#TitleDescription2NewsPage").val() == "" || $("#TitleDescription2NewsPage").val().trim() == "") {
        return false;
    }*/
    if ($("#ColorTitleDescription1NewsPage").val() == "" || $("#ColorTitleDescription1NewsPage").val().trim() == "") {
        return false;
    }
    /*if ($("#ColorTitleDescription2NewsPage").val() == "" || $("#ColorTitleDescription2NewsPage").val().trim() == "") {
        return false;
    }*/

    var des1 = CKEDITOR.instances.TextDescription1NewsPage.getData();
    if (des1 == "" || des1.trim() == "") {
        return false;
    }
    /*var des2 = CKEDITOR.instances.TextDescription2NewsPage.getData();
    if (des2 == "" || des2.trim() == "") {
        return false;
    }*/

    //If a title is provided, then a title color must be provided as well
    if ($("#TitleDescription2NewsPage").val().trim() != "" && $("#ColorTitleDescription2NewsPage").val().trim() == "") {
        return false;
    }








    //if ($("#ColorTextDescription1NewsPage").val() == "" || $("#ColorTextDescription1NewsPage").val().trim() == "") {
    //    return false;
    //}
    //if ($("#ColorTextDescription2NewsPage").val() == "" || $("#ColorTextDescription2NewsPage").val().trim() == "") {
    //    return false;
    //}
    return true;
}

function btnPreviewNewsClick(valor) {
    if (isValidNews()) {
        run_waitMe("#submit_form", "Cargando...");
        var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/BlogPage/PreviewEditNews";
    else
        valPrev = w + "/Admin/BlogPage/ApplyViewNews";

    var data = new FormData();

    data.append("ColorBgHeadNewsPage", $("#ColorBgHeadNewsPage").val());
    data.append("TextHeadNewsPage", $("#TextHeadNewsPage").val());
    data.append("ColorTextHeadNewsPage", $("#ColorTextHeadNewsPage").val());
    data.append("SubTextHeadNewsPage", $("#SubTextHeadNewsPage").val());
    data.append("ColorSubTextHeadNewsPage", $("#ColorSubTextHeadNewsPage").val());
    data.append("ColorBgSubTextHeadNewsPage", $("#ColorBgSubTextHeadNewsPage").val()); 
    data.append("TitleDescription1NewsPage", $("#TitleDescription1NewsPage").val());
    data.append("TitleDescription2NewsPage", $("#TitleDescription2NewsPage").val());
    data.append("ColorTitleDescription1NewsPage", $("#ColorTitleDescription1NewsPage").val());
    data.append("ColorTitleDescription2NewsPage", $("#ColorTitleDescription2NewsPage").val());
    data.append("TextDescription1NewsPage", CKEDITOR.instances.TextDescription1NewsPage.getData());
    data.append("TextDescription2NewsPage", CKEDITOR.instances.TextDescription2NewsPage.getData());
    //data.append("ColorTextDescription1NewsPage", $("#ColorTextDescription1NewsPage").val());
    //data.append("ColorTextDescription2NewsPage", $("#ColorTextDescription2NewsPage").val());

    var ImageHeadNewsPage = $("#ImageHeadNewsPage").get(0).files;
    if (ImageHeadNewsPage.length > 0) {
        data.append("ImageHeadNewsPage", ImageHeadNewsPage[0]);
    }
    else {
        data.append("ImageHeadNewsPage", $("#imgImageHeadNewsPage").attr('src'));
    }

    var ImageDescriptionNewsPage = $("#ImageDescriptionNewsPage").get(0).files;
    if (ImageDescriptionNewsPage.length > 0) {
        data.append("ImageDescriptionNewsPage", ImageDescriptionNewsPage[0]);
    }
    else {
        data.append("ImageDescriptionNewsPage", $("#imgImageDescriptionNewsPage").attr('src'));
    }

    // Make Ajax request with the contentType = false, and procesDate = false
    var idNews = $("#idNews").attr("idNews");
    var idBlog = $("#idBlog").attr("idBlog");

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?idNews=" + idNews+"&idBlog="+idBlog,
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/BlogPage/PreviewEditNews", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/BlogPage/ListNews?idBlog=" + idBlog;
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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
    });
    } else {
        bootbox.dialog({
            message: "Debe llenar todos los campos del Wizard para Previsualizar o Aplicar Cambios.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

$("#btnApplyNews").on("click", function (e) {
    btnPreviewNewsClick("1");
});

$("#btnPreviewNews").on("click", function (e) {
    btnPreviewNewsClick("0");
});

function btnPreviewServiceClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/ServicePage/PreviewEdit";
    else
        valPrev = w + "/Admin/ServicePage/ApplyView";

    var data = new FormData();

    var ImagenEncabezadoLogoServicio = $("#ImagenEncabezadoLogoServicio").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ImagenEncabezadoLogoServicio.length > 0) {
        data.append("ImagenEncabezadoLogoServicio", ImagenEncabezadoLogoServicio[0]);
    }

    data.append("Texto1Servicio", $("#Texto1Servicio").val());
    data.append("Texto2Servicio", $("#Texto2Servicio").val());
    data.append("ColorTexto1Servicio", $("#ColorTexto1Servicio").val());
    data.append("ColorTexto2Servicio", $("#ColorTexto2Servicio").val());
    data.append("ColorFondoSerivicio", $("#ColorFondoSerivicio").val());
    data.append("TextoTituloServicio", $("#TextoTituloServicio").val());
    data.append("TextDescripcionServicio", $("#TextDescripcionServicio").val());
    data.append("ColorTextDescripcionServicio", $("#ColorTextDescripcionServicio").val());
    data.append("ColorTextoTituloServicio", $("#ColorTextoTituloServicio").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=service",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/ServicePage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/ServicePage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyService").on("click", function (e) {
    btnPreviewServiceClick("1");
});

$("#btnPreviewService").on("click", function (e) {
    btnPreviewServiceClick("0");
});

function btnPreviewSaDClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/SaDPage/PreviewEdit";
    else
        valPrev = w + "/Admin/SaDPage/ApplyView";

    var data = new FormData();

    var ImageBgSaD = $("#ImageBgSaD").get(0).files;

    var ImagenLogoSaD = $("#ImagenLogoSaD").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ImagenLogoSaD.length > 0) {
        data.append("ImagenLogoSaD", ImagenLogoSaD[0]);
    }

    if (ImageBgSaD.length > 0) {
        data.append("ImageBgSaD", ImageBgSaD[0]);
    }

    data.append("Texto1SaD", $("#Texto1SaD").val());
    data.append("Texto2SaD", $("#Texto2SaD").val());
    data.append("ColorTexto1SaD", $("#ColorTexto1SaD").val());
    data.append("ColorTexto2SaD", $("#ColorTexto2SaD").val());
    data.append("TextoTituloSaD", $("#TextoTituloSaD").val());
    data.append("ColorTextoTituloSaD", $("#ColorTextoTituloSaD").val());
    data.append("NumeroprincipalSaD", $("#NumeroprincipalSaD").val());
    data.append("ColorNumeroprincipalSaD", $("#ColorNumeroprincipalSaD").val());
    data.append("ColorBgNumeroprincipalSaD", $("#ColorBgNumeroprincipalSaD").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=sad",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/SaDPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/SaDPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplySaD").on("click", function (e) {
    btnPreviewSaDClick("1");
});

$("#btnPreviewSaD").on("click", function (e) {
    btnPreviewSaDClick("0");
});

function btnPreviewTeamClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/JoinTeamPage/PreviewEdit";
    else
        valPrev = w + "/Admin/JoinTeamPage/ApplyView";

    var data = new FormData();

    var inicio_imagen = "client_imageEncabezado_";
    var inicio_texto = "client_TextEncabezado_";
    var inicio_color = "client_ColorTextEncabezado_";
    var idsCliente = "";
    for (var i = 0; i < ids.length; i++) {
        idsCliente += ids[i] + ",";
        var clave_img = inicio_imagen + ids[i];
        var id_img = "#" + inicio_imagen + ids[i];
        var clave_text = inicio_texto + ids[i];
        var id_text = "#" + inicio_texto + ids[i];
        var clave_color = inicio_color + ids[i];
        var id_color = "#" + inicio_color + ids[i];
        data.append(clave_img, $(id_img).get(0).files[0]);
        data.append(clave_text, $(id_text).val());
        data.append(clave_color, $(id_color).val());
    }
    data.append('idsCliente', idsCliente);

    var inicio_texto_s = "server_TextEncabezado_";
    var inicio_color_s = "server_ColorTextEncabezado_";
    var ids2 = $('#idsServidor').attr('idsServer');
    if (ids2 != "" && ids2 != undefined) {
        var arrayIds = ids2.split(',');
        var value = "";
        for (var i = 0; i < arrayIds.length - 1; i++) {
            var clave_text_s = inicio_texto_s + arrayIds[i];
            var id_text_s = "#" + inicio_texto_s + arrayIds[i];
            var clave_color_s = inicio_color_s + arrayIds[i];
            var id_color_s = "#" + inicio_color_s + arrayIds[i];
            data.append(clave_text_s, $(id_text_s).val());
            data.append(clave_color_s, $(id_color_s).val());
        }
    }
    data.append('idsServidor', ids2);

    data.append("ColorText1JoinTeamPage", $("#ColorText1JoinTeamPage").val());
    data.append("ColorText2JoinTeamPage", $("#ColorText2JoinTeamPage").val());
    data.append("SubTextJoinTeamPage1", $("#SubTextJoinTeamPage1").val());
    data.append("SubTextJoinTeamPage2", $("#SubTextJoinTeamPage2").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=team",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/JoinTeamPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/JoinTeamPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyTeam").on("click", function (e) {
    btnPreviewTeamClick("1");
});

$("#btnPreviewTeam").on("click", function (e) {
    btnPreviewTeamClick("0");
});

function btnPreviewInvestorClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/InvestorPage/PreviewEdit";
    else
        valPrev = w + "/Admin/InvestorPage/ApplyView";

    var data = new FormData();

    data.append("HeadTextInvestorPage", $("#HeadTextInvestorPage").val());
    data.append("ColorHeadTextInvestorPage", $("#ColorHeadTextInvestorPage").val());
    data.append("ColorHeadBgInvestorPage", $("#ColorHeadBgInvestorPage").val());
    data.append("SubTextInvestorPage", $("#SubTextInvestorPage").val());
    data.append("ColorSubTextInvestorPage", $("#ColorSubTextInvestorPage").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=investor",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/InvestorPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/InvestorPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyInvestor").on("click", function (e) {
    if ($("#ColorHeadBgInvestorPage").val() === "" && $("#HeadTextInvestorPage").val() === "" && $("#ColorHeadTextInvestorPage").val() === "" && $("#SubTextInvestorPage").val() === "") {
        //alert("No hay cambios que aplicar");
        bootbox.dialog({
            message: "No hay cambios que aplicar",
            title: "Información",
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
        btnPreviewInvestorClick("1");
    }
});

$("#btnPreviewInvestor").on("click", function (e) {
    btnPreviewInvestorClick("0");
});

function isValidPromocion() {
    var valido = true;
    var ImageLogo1PromocionPage = $("#ImageLogo1PromocionPage").get(0).files;
    var ImageLogo2PromocionPage = $("#ImageLogo2PromocionPage").get(0).files;
    var HeadImagePromocionPage = $("#HeadImagePromocionPage").get(0).files;

    if (!(ImageLogo1PromocionPage.length > 0)) {
        if($("#ImageLogo1PromocionPageSrc").attr('src') == "")
        {
            return false;
        }
    }
    if (!(ImageLogo2PromocionPage.length > 0)) {
        if ($("#ImageLogo2PromocionPageSrc").attr('src') == "") {
            return false;
        }
    }
    if (!(HeadImagePromocionPage.length > 0)) {
        if ($("#HeadImagePromocionPageSrc").attr('src') == "") {
            return false;
        }
    }
    if ($("#HeadTextPromocionPage").val() == "" || $("#HeadTextPromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#SpanHeadTextPromocionPage").val() == "" || $("#SpanHeadTextPromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorHeadBgPromocionPage").val() == "" || $("#ColorHeadBgPromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#HeadtextColorPromocionPage").val() == "" || $("#HeadtextColorPromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#SpanHeadtextColorPromocionPage").val() == "" || $("#SpanHeadtextColorPromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#SubText1PromocionPage").val() == "" || $("#SubText1PromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#SubText2PromocionPage").val() == "" || $("#SubText2PromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#TextFosePromocionPage").val() == "" || $("#TextFosePromocionPage").val().trim() == "") {
        return false;
    }
    if ($("#TextColorFosePromocionPage").val() == "" || $("#TextColorFosePromocionPage").val().trim() == "") {
        return false;
    }
    return true;
}

function btnPreviewPromocionClick(valor) {
    if (isValidPromocion()) {
        run_waitMe("#submit_form", "Cargando...");
        var w = getSiteRootUrl();
        var valPrev = "";
        if (valor == 0)
            valPrev = w + "/Admin/FosePage/PreviewEditPromocion";
        else
            valPrev = w + "/Admin/FosePage/ApplyViewPromocion";

        var data = new FormData();

        var ImageLogo1PromocionPage = $("#ImageLogo1PromocionPage").get(0).files;
        var ImageLogo2PromocionPage = $("#ImageLogo2PromocionPage").get(0).files;
        var HeadImagePromocionPage = $("#HeadImagePromocionPage").get(0).files;

        // Add the uploaded image content to the form data collection
        if (ImageLogo1PromocionPage.length > 0) {
            data.append("ImageLogo1PromocionPage", ImageLogo1PromocionPage[0]);
        }
        else
        {
            data.append("ImageLogo1PromocionPage", $("#ImageLogo1PromocionPageSrc").attr('src'));
        }

        if (ImageLogo2PromocionPage.length > 0) {
            data.append("ImageLogo2PromocionPage", ImageLogo2PromocionPage[0]);
        }
        else
        {
            data.append("ImageLogo2PromocionPage", $("#ImageLogo2PromocionPageSrc").attr('src'));
        }

        if (HeadImagePromocionPage.length > 0) {
            data.append("HeadImagePromocionPage", HeadImagePromocionPage[0]);
        }
        else
        {
            data.append("HeadImagePromocionPage", $("#HeadImagePromocionPageSrc").attr('src'));
        }

        data.append("TextFosePromocionPage", $("#TextFosePromocionPage").val());
        data.append("TextColorFosePromocionPage", $("#TextColorFosePromocionPage").val());
        data.append("HeadTextPromocionPage", $("#HeadTextPromocionPage").val());
        data.append("SpanHeadTextPromocionPage", $("#SpanHeadTextPromocionPage").val());
        data.append("ColorHeadBgPromocionPage", $("#ColorHeadBgPromocionPage").val());
        data.append("HeadtextColorPromocionPage", $("#HeadtextColorPromocionPage").val());
        data.append("SpanHeadtextColorPromocionPage", $("#SpanHeadtextColorPromocionPage").val());
        data.append("SubText1PromocionPage", $("#SubText1PromocionPage").val());
        data.append("SubText2PromocionPage", $("#SubText2PromocionPage").val());

        var idPromo = $("#idPromo").attr("idPromo");
        var idFose = $("#idFose").attr("idFose");

        // Make Ajax request with the contentType = false, and procesDate = false
        var ajaxRequest = $.ajax({
            type: "POST",
            url: valPrev + "?idPromo=" + idPromo + "&idFose=" + idFose,
            contentType: false,
            processData: false,
            data: data
        });

        ajaxRequest.done(function(responseData, textStatus) {
            if (textStatus === "success") {
                if (responseData != null) {
                    if (responseData.Success) {
                        if (valor === "0") {
                            ocultarWaitme("#submit_form");
                            window.open(w + "/Admin/FosePage/PreviewEditPromocion", "_blank");
                        } else {
                            ocultarWaitme("#submit_form");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Información",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue",
                                        callback: function () {
                                            window.location.href = w + "/Admin/FosePage/ListPromocions?idPromo=" + idPromo + "&idFose=" + idFose;
                                        }
                                    }
                                }
                            });
                        }
                    } else {
                        ocultarWaitme("#submit_form");
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
                ocultarWaitme("#submit_form");
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
    } else {
        bootbox.dialog({
            message: "Debe llenar todos los campos del Wizard para Previsualizar o Aplicar Cambios.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

$("#btnApplyPromocion").on("click", function (e) {
    btnPreviewPromocionClick("1");
});

$("#btnPreviewPromocion").on("click", function (e) {
    btnPreviewPromocionClick("0");
});

function btnPreviewPrivacityClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/PrivacityPage/PreviewEdit";
    else
        valPrev = w + "/Admin/PrivacityPage/ApplyView";

    var data = new FormData();

    data.append("HeadTextPrivacityPage", $("#HeadTextPrivacityPage").val());
    data.append("ColorHeadTextPrivacityPage", $("#ColorHeadTextPrivacityPage").val());
    data.append("BgColorHeadPrivacityPage", $("#BgColorHeadPrivacityPage").val());
    data.append("TextDescriptionPrivacityPage", CKEDITOR.instances.TextDescriptionPrivacityPage.getData());
    data.append("TextColorPrivacityPage", $("#TextColorPrivacityPage").val());
    data.append("TextTitlePrivacityPage", $("#TextTitlePrivacityPage").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=privacity",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/PrivacityPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/PrivacityPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyPrivacity").on("click", function (e) {
    btnPreviewPrivacityClick("1");
});

$("#btnPreviewPrivacity").on("click", function (e) {
    btnPreviewPrivacityClick("0");
});

function btnPreviewProviderClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/ProviderPage/PreviewEdit";
    else
        valPrev = w + "/Admin/ProviderPage/ApplyView";

    var data = new FormData();

    data.append("HeadTextProviderPage", $("#HeadTextProviderPage").val());
    data.append("ColorHeadTextProviderPage", $("#ColorHeadTextProviderPage").val());
    data.append("BgColorHeadProviderPage", $("#BgColorHeadProviderPage").val());
    data.append("SubText1ProviderPage", $("#SubText1ProviderPage").val());
    data.append("ColorSubTextProviderPage", $("#ColorSubTextProviderPage").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=provider",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/ProviderPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {                                        
                                        window.location.href = w + "/Admin/ProviderPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyProvider").on("click", function (e) {
    btnPreviewProviderClick("1");
});

$("#btnPreviewProvider").on("click", function (e) {
    btnPreviewProviderClick("0");
});

function btnPreviewTermsClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/ConditionsTermsPage/PreviewEdit";
    else
        valPrev = w + "/Admin/ConditionsTermsPage/ApplyView";

    var data = new FormData();

    data.append("HeadTextConditionsTermsPage", $("#HeadTextConditionsTermsPage").val());
    data.append("ColorHeadTextConditionsTermsPage", $("#ColorHeadTextConditionsTermsPage").val());
    data.append("BgColorHeadConditionsTermsPage", $("#BgColorHeadConditionsTermsPage").val());
    data.append("TextDescriptionConditionsTermsPage", CKEDITOR.instances.TextDescriptionConditionsTermsPage.getData());
    data.append("TextColorConditionsTermsPage", $("#TextColorConditionsTermsPage").val());
    data.append("TextTitleConditionsTermsPage", $("#TextTitleConditionsTermsPage").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=privacity",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/ConditionsTermsPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/ConditionsTermsPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyTerms").on("click", function (e) {
    btnPreviewTermsClick("1");
});

$("#btnPreviewTerms").on("click", function (e) {
    btnPreviewTermsClick("0");
});

function btnPreviewContactClick(valor) {
    run_waitMe("#submit_form", "Cargando...");
    var w = getSiteRootUrl();
    var valPrev = "";
    if (valor == 0)
        valPrev = w + "/Admin/ContactPage/PreviewEdit";
    else
        valPrev = w + "/Admin/ContactPage/ApplyView";

    var data = new FormData();

    data.append("HeadTextContactPage", $("#HeadTextContactPage").val());
    data.append("ColorHeadTextContactPage", $("#ColorHeadTextContactPage").val());
    data.append("SubText1ContactPage", $("#SubText1ContactPage").val());
    data.append("SubText2ContactPage", $("#SubText2ContactPage").val());
    data.append("BgColorHeadContactPage", $("#BgColorHeadContactPage").val());
    data.append("ColorSubText1ContactPage", $("#ColorSubText1ContactPage").val());
    data.append("ColorSubText2ContactPage", $("#ColorSubText2ContactPage").val());
    data.append("ColorTextFooterContactPage", $("#ColorTextFooterContactPage").val());
    data.append("EmailSaDContactPage", $("#EmailSaDContactPage").val());
    data.append("TelSaDContactPage", $("#TelSaDContactPage").val());
    data.append("TelAaPContactPage", $("#TelAaPContactPage").val());
    data.append("TelAddressContactPage", $("#TelAddressContactPage").val());
    data.append("AddressContactPage", $("#AddressContactPage").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=" + valor + "&p=contact",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    if (valor === "0") {
                        ocultarWaitme("#submit_form");
                        window.open(w + "/Admin/ContactPage/PreviewEdit", "_blank");
                    } else {
                        ocultarWaitme("#submit_form");
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        window.location.href = w + "/Admin/ContactPage/Index";
                                    }
                                }
                            }
                        });
                    }
                } else {
                    ocultarWaitme("#submit_form");
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
            ocultarWaitme("#submit_form");
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

$("#btnApplyContact").on("click", function (e) {
    btnPreviewContactClick("1");
});

$("#btnPreviewContact").on("click", function (e) {
    btnPreviewContactClick("0");
});

function isValidProduct() {
    var ImageProductPage = $("#ImageProductPage").get(0).files;
    if (!(ImageProductPage.length > 0)) {
        if ($("#ImageProductPageSrc").attr('src') == "")
        {
            return false;
        }        
    }
    if ($("#BgColorProductPage").val() == "" && $("#BgColorProductPage").val().trim() == "") {
        return false;
    }
    if ($("#TextTitleProductPage").val() == "" && $("#TextTitleProductPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorTextTitleProductPage").val() == "" && $("#ColorTextTitleProductPage").val().trim() == "") {
        return false;
    }
    if ($("#TextDescription1ProductPage").val() == "" && $("#TextDescription1ProductPage").val().trim() == "") {
        return false;
    }
    if ($("#TextDescription2ProductPage").val() == "" && $("#TextDescription2ProductPage").val().trim() == "") {
        return false;
    }
    if ($("#TextCharacteristic1ProductPage").val() == "" && $("#TextCharacteristic1ProductPage").val().trim() == "") {
        return false;
    }
    if ($("#TextCharacteristic2ProductPage").val() == "" && $("#TextCharacteristic2ProductPage").val().trim() == "") {
        return false;
    }

    if ($("#ColorTextDescription1ProductPage").val() == "" && $("#ColorTextDescription1ProductPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorTextDescription2ProductPage").val() == "" && $("#ColorTextDescription2ProductPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorTextCharacteristic1ProductPage").val() == "" && $("#ColorTextCharacteristic1ProductPage").val().trim() == "") {
        return false;
    }
    if ($("#ColorTextCharacteristic2ProductPage").val() == "" && $("#ColorTextCharacteristic2ProductPage").val().trim() == "") {
        return false;
    }
    return true;
}

function btnPreviewProductClick(valor) {
    if (isValidProduct()) {
        run_waitMe("#submit_form", "Cargando...");
        var w = getSiteRootUrl();
        var valPrev = "";
        if (valor == 0)
            valPrev = w + "/Admin/FosePage/PreviewEditPromocionProduct";
        else
            valPrev = w + "/Admin/FosePage/ApplyViewPromocionProduct";

        var data = new FormData();

        var ImageProductPage = $("#ImageProductPage").get(0).files;

        // Add the uploaded image content to the form data collection
        if (ImageProductPage.length > 0) {
            data.append("ImageProductPage", ImageProductPage[0]);
        }
        else
        {
            data.append("ImageProductPage", $("#ImageProductPageSrc").attr('src'));
        }

        data.append("BgColorProductPage", $("#BgColorProductPage").val());
        data.append("TextTitleProductPage", $("#TextTitleProductPage").val());
        data.append("ColorTextTitleProductPage", $("#ColorTextTitleProductPage").val());
        data.append("TextDescription1ProductPage", $("#TextDescription1ProductPage").val());
        data.append("TextDescription2ProductPage", $("#TextDescription2ProductPage").val());
        data.append("TextCharacteristic1ProductPage", $("#TextCharacteristic1ProductPage").val());
        data.append("TextCharacteristic2ProductPage", $("#TextCharacteristic2ProductPage").val());

        data.append("ColorTextDescription1ProductPage", $("#ColorTextDescription1ProductPage").val());
        data.append("ColorTextDescription2ProductPage", $("#ColorTextDescription2ProductPage").val());
        data.append("ColorTextCharacteristic1ProductPage", $("#ColorTextCharacteristic1ProductPage").val());
        data.append("ColorTextCharacteristic2ProductPage", $("#ColorTextCharacteristic2ProductPage").val());

        var idProd = $("#idProd").attr("idProd");
        var idPromo = $("#idPromo").attr("idPromo");
        var idFose = $("#idFose").attr("idFose");

        // Make Ajax request with the contentType = false, and procesDate = false
        var ajaxRequest = $.ajax({
            type: "POST",
            url: valPrev + "?idProd=" + idProd + "&idPromo=" + idPromo,
            contentType: false,
            processData: false,
            data: data
        });

        ajaxRequest.done(function(responseData, textStatus) {
            if (textStatus === "success") {
                if (responseData != null) {
                    if (responseData.Success) {
                        if (valor === "0") {
                            ocultarWaitme("#submit_form");
                            window.open(w + "/Admin/FosePage/PreviewEditPromocionProduct?idProd=" + idProd + "&idPromo=" + idPromo, "_blank");
                        } else {
                            ocultarWaitme("#submit_form");
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Información",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue",
                                        callback: function () {
                                            window.location.href = w + "/Admin/FosePage/ListPromocionsProducts?idFose=" + idFose + "&idPromo=" + idPromo;
                                        }
                                    }
                                }
                            });
                        }
                    } else {
                        ocultarWaitme("#submit_form");
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
                ocultarWaitme("#submit_form");
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
        bootbox.dialog({
            message: "Debe llenar todos los campos del Wizard para Previsualizar o Aplicar Cambios.",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
    }
}

$("#btnApplyProductPage").on("click", function (e) {
    btnPreviewProductClick("1");
});

$("#btnPreviewProductPage").on("click", function (e) {
    btnPreviewProductClick("0");
});

function SubirArchivosPilares() {
    run_waitMe("#formPillar", "Subiendo...");
    var w = getSiteRootUrl();
    var valPrev = w + "/Admin/Pillar/UploadFile";

    var data = new FormData();

    var imagenPilar = $("#PillarImage").get(0).files;

    // Add the uploaded image content to the form data collection
    if (imagenPilar.length > 0) {
        data.append("PillarImage", imagenPilar[0]);
    }
    var pillarId = "0";
    if ($("#PillarId").val() != undefined) {
        pillarId = $("#PillarId").val()
        data.append("PillarId", pillarId );
    }
    data.append("PillarName", $("#PillarName").val());
    data.append("PillarDescription", $("#PillarDescription").val());
    data.append("PillarLink", $("#PillarLink").val());
    data.append("PillarActive", $("#PillarActive").val());


    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=0&p=pillarImage",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    ocultarWaitme("#formPillar");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Información",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue",
                                callback: function () {
                                    window.location.href = w + "/Admin/Pillar/Index";
                                }
                            }
                        }
                    });
                } else {
                    ocultarWaitme("#formPillar");
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
            ocultarWaitme("#formPillar");
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

$("#btnUploadPillarImage").on("click", function (e) {

    var imagen = $("#PillarImage").get(0).files;

    // Add the uploaded image content to the form data collection
    if (imagen.length === 0) {

        //alert("Debe seleccionar un archivo");
        bootbox.dialog({
            message: "Debe seleccionar una imagen",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "red"
                }
            }
        });
    } else {
        if ($("#PillarName").val().length === 0) {

            //alert("Debe seleccionar un archivo");
            bootbox.dialog({
                message: "Debe entrar un nombre.",
                title: "Información",
                buttons: {
                    main: {
                        label: "Aceptar",
                        className: "blue"
                    }
                }
            });
        } else {
            if ($("#PillarDescription").val().length === 0) {
                //alert("Debe seleccionar un archivo");
                bootbox.dialog({
                    message: "Debe entrar una descripción.",
                    title: "Información",
                    buttons: {
                        main: {
                            label: "Aceptar",
                            className: "blue"
                        }
                    }
                });
            } else {
                SubirArchivosPilares();
            }
        }
    }

});


function SubirArchivosCitas() {
    run_waitMe("#formQuote", "Subiendo...");
    var w = getSiteRootUrl();
    var valPrev = w + "/Admin/Quote/UploadFile";

    var data = new FormData();

    var imagenCita = $("#QuoteAuthorPhoto").get(0).files;

    // Add the uploaded image content to the form data collection
    if (imagenCita.length > 0) {
        data.append("QuoteAuthorPhoto", imagenCita[0]);
    }
    
    var quoteId = "0";
    if ($("#QuoteId").val() != undefined) {
        quoteId = $("#QuoteId").val()
        data.append("QuoteId", quoteId);
    }

    data.append("QuoteText", $("#QuoteText").val());
    data.append("QuoteAuthor", $("#QuoteAuthor").val());
    data.append("QuoteAuthorSign", $("#QuoteAuthorSign").val());


    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=0&p=QuoteAuthorPhoto",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    ocultarWaitme("#formQuote");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Información",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue",
                                callback: function () {
                                    window.location.href = w + "/Admin/Quote/Index";
                                }
                            }
                        }
                    });
                } else {
                    ocultarWaitme("#formQuote");
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
            ocultarWaitme("#formQuote");
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

$("#btnUploadQuoteImage").on("click", function (e) {

    var imagen = $("#QuoteAuthorPhoto").get(0).files;

    // Add the uploaded image content to the form data collection
    if (imagen.length === 0) {

        //alert("Debe seleccionar un archivo");
        bootbox.dialog({
            message: "Debe seleccionar una imagen",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "red"
                }
            }
        });
    } else {
        if ($("#QuoteText").val().length === 0) {

            //alert("Debe seleccionar un archivo");
            bootbox.dialog({
                message: "Debe introducir la cita.",
                title: "Información",
                buttons: {
                    main: {
                        label: "Aceptar",
                        className: "blue"
                    }
                }
            });
        } else {
            if ($("#QuoteAuthor").val().length === 0) {
                //alert("Debe seleccionar un archivo");
                bootbox.dialog({
                    message: "Debe introducir el nombre del autor.",
                    title: "Información",
                    buttons: {
                        main: {
                            label: "Aceptar",
                            className: "blue"
                        }
                    }
                });
            } else {
                SubirArchivosCitas();
            }
        }
    }

});



function SubirArchivosClick() {
    run_waitMe("#formReportType", "Subiendo...");
    var w = getSiteRootUrl();
    var valPrev = w + "/Admin/ReportType/UploadFile";

    var data = new FormData();

    var ArchivoReporte = $("#ArchivoReporte").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ArchivoReporte.length > 0) {
        data.append("ArchivoReporte", ArchivoReporte[0]);
    }

    data.append("TipoReporte", $("#TipoReporte").val());
    data.append("IdYearReporte", $("#touchspin_3").val());
    data.append("ReporteDescriptionFile", $("#ReporteDescriptionFile").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=0&p=investorfile",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    ocultarWaitme("#formReportType");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Información",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue",
                                callback: function () {
                                    window.location.href = w + "/Admin/ReportType/Index";
                                }
                            }
                        }
                    });
                } else {
                    ocultarWaitme("#formReportType");
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
            ocultarWaitme("#formReportType");
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

$("#btnUploadFile").on("click", function (e) {

    var ArchivoReporte = $("#ArchivoReporte").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ArchivoReporte.length === 0) {

        //alert("Debe seleccionar un archivo");
        bootbox.dialog({
            message: "Debe seleccionar un archivo, entrar un año y poner una descripción",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "red"
                }
            }
        });
    } else {
        if ($("#touchspin_3").val().length === 0) {

            //alert("Debe seleccionar un archivo");
            bootbox.dialog({
                message: "Debe entrar un valor para el Año.",
                title: "Información",
                buttons: {
                    main: {
                        label: "Aceptar",
                        className: "blue"
                    }
                }
            });
        } else {
            if ($("#ReporteDescriptionFile").val().length === 0) {
                //alert("Debe seleccionar un archivo");
                bootbox.dialog({
                    message: "Debe entrar una descripción para el archivo.",
                    title: "Información",
                    buttons: {
                        main: {
                            label: "Aceptar",
                            className: "blue"
                        }
                    }
                });
            } else {
                SubirArchivosClick();
            }
        }
    }
    
});

function SubirArchivosDocsClick() {
    run_waitMe("#formDocumentType", "Subiendo...");
    var w = getSiteRootUrl();
    var valPrev = w + "/Admin/DocumentType/UploadFile";

    var data = new FormData();

    var ArchivoDocumento = $("#ArchivoDocumento").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ArchivoDocumento.length > 0) {
        data.append("ArchivoDocumento", ArchivoDocumento[0]);
    }

    data.append("TipoDocumento", $("#TipoDocumento").val());
    data.append("textDescArchivo", $("#textDescArchivo").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=0&p=providerfile",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    ocultarWaitme("#formDocumentType");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Información",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue",
                                callback: function () {
                                    window.location.href = w + "/Admin/DocumentType/Index";
                                }
                            }
                        }
                    });
                } else {
                    ocultarWaitme("#formDocumentType");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Error",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue"
                            }
                        }
                    });
                }
            }
        } else {
            ocultarWaitme("#formDocumentType");
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

$("#btnUploadFileDocument").on("click", function (e) {

    var ArchivoDocumento = $("#ArchivoDocumento").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ArchivoDocumento.length === 0) {
        bootbox.dialog({
            message: "Debe seleccionar un archivo",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
        //alert("Debe seleccionar un archivo");
    }
    else {
        SubirArchivosDocsClick();
    }
});

function SubirArchivosImageServiceClick() {
    run_waitMe("#formServiceType", "Subiendo...");
    var w = getSiteRootUrl();
    var valPrev = w + "/Admin/ServicePage/UploadFile";

    var data = new FormData();

    var ArchivoImagen = $("#filer_input1").get(0).files;

    // Add the uploaded image content to the form data collection
    if (ArchivoImagen.length > 0) {
        for (var i = 0; i < ArchivoImagen.length; i++)
            data.append("ArchivoImagen_" + i, ArchivoImagen[i]);
    }

    data.append("ServiceTypeName", $("#ServiceTypeName").val());
    data.append("ServiceTypeNameDescription", $("#ServiceTypeNameDescription").val());
    data.append("ServiceTypeProdutcsDescription", $("#ServiceTypeProdutcsDescription").val());
    data.append("ServiceTypeActive", $("#ServiceTypeActive").val());

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        type: "POST",
        url: valPrev + "?v=0&p=servicefile",
        contentType: false,
        processData: false,
        data: data
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    ocultarWaitme("#formServiceType");
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Información",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue",
                                callback: function () {
                                    window.location.href = w + "/Admin/ServiceType/Index";
                                }
                            }
                        }
                    });
                } else {
                    ocultarWaitme("#formServiceType");
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
            ocultarWaitme("#formServiceType");
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

$("#btnUploadFileImageService").on("click", function (e) {

    var ArchivoDocumento = $("#filer_input1").get(0).files;

    // Add the uploaded image content to the form data collection
    if ($("#ServiceTypeName").val().length === 0 || $("#ServiceTypeName").val() === "") {
        bootbox.dialog({
            message: "Debe introducir un nombre de Tipo de Servicio",
            title: "Información",
            buttons: {
                main: {
                    label: "Aceptar",
                    className: "blue"
                }
            }
        });
        //alert("Debe introducir un nombre de Tipo de Servicio");
    }
    else {
        if (ArchivoDocumento.length === 0) {
            bootbox.dialog({
                message: "Debe seleccionar un archivo",
                title: "Información",
                buttons: {
                    main: {
                        label: "Aceptar",
                        className: "blue"
                    }
                }
            });
            //alert("Debe seleccionar un archivo");
        }
        else {

            SubirArchivosImageServiceClick();
        }
    }

});

function showPleaseWait() {
    var modalLoading = '<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false role="dialog">\
        <div class="modal-dialog">\
            <div class="modal-content">\
                <div class="modal-header">\
                    <h4 class="modal-title">Por favor, espere...</h4>\
                </div>\
                <div class="modal-body">\
                    <div class="progress">\
                      <div class="progress-bar progress-bar-success progress-bar-striped active" role="progressbar"\
                      aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width:100%; height: 40px">\
                      </div>\
                    </div>\
                </div>\
            </div>\
        </div>\
    </div>';
    $(document.body).append(modalLoading);
    $("#pleaseWaitDialog").modal("show");
}

/**
 * Hides "Please wait" overlay. See function showPleaseWait().
 */
function hidePleaseWait() {
    $("#pleaseWaitDialog").modal("hide");
}

function updatePromotions() {
    showPleaseWait();
    var w = getSiteRootUrl();
    var valPrev = w + "/Admin/Promotions/UpdatePromotions";

    // Make Ajax request with the contentType = false, and procesDate = false
    var ajaxRequest = $.ajax({
        url: valPrev,
        contentType: false,
        processData: false
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {
                    hidePleaseWait();
                    bootbox.dialog({
                        message: responseData.Message,
                        title: "Información",
                        buttons: {
                            main: {
                                label: "Aceptar",
                                className: "blue",
                            }
                        }
                    });
                } else {
                    hidePleaseWait();
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
            hidePleaseWait();
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


