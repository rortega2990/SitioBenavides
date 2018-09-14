/**
 * Created by Dayarel on 09/03/2017.
 */
$(document).ready(function(){
    $('.branch-offices-name').click(function (evento) {
        var arrId = $(this).attr('id').split('-');
        var id = $(this).attr('id');
        $(this).removeClass('inactive');
        $(this).addClass('active');
        $('.branch-offices-address').each(function () {
            if ($(this).attr('id') == 'address-' + arrId[1]) {
                $(this).removeClass('hidden');
            } else {
                $(this).addClass('hidden');
            }
        });
        $('.branch-offices-name').each(function () {
            if ($(this).attr('id') != id) {
                $(this).removeClass('active');
                $(this).addClass('inactive');
            }
        });
    });







    // validate form on keyup and submit
    var validator = $("#sampleform").validate({
        rules: {
            titulo: "required",
            codtbol: "required",
            fecha: "required",
            subtitulo: "required",
            email: {
                email: true
            },
            relboletinnovedades: "required"
        },
        messages: {
            titulo: "Por favor, inserte el título.",
            codtbol: "Por favor, seleccione el tipo de boletín.",
            fecha: "Por favor, inserte la fecha.",
            subtitulo: "Por favor, inserte la subject.",
            email: {
                email: "Por favor, introduzca una dirección válida de correo"
            },
            relboletinnovedades: "Por favor, seleccione al menos una novedad."
        },
        // the errorPlacement has to take the layout into account
        errorPlacement: function(error, element) {
            error.insertAfter(element.parent().find('label:first'));
        },
        // specifying a submitHandler prevents the default submit, good for the demo
        submitHandler: function() {
            $('#cargador-total').slideDown();
            $('#error').slideUp();
            $('#msg-error-unique').slideUp();
            $('#msg-error-validate-dates').slideUp();
            $('#success').slideUp();
            if(($('#relboletinnovedades').val() == null) && ($('#codtbol').val() != 8)){
                $('#msg-error').slideDown();
                $('#cargador-total').slideUp();
            }else{
                $('#msg-error').slideUp();
                var isUnique = $.ajax({
                    type: 'POST',
                    url: baseurl + 'boletines-trade-unique',
                    async: false,
                    data: $("#sampleform").serialize()
                }).responseText;
                if(isUnique){
                    $('#msg-error-validate-dates').slideUp();
                    $('#msg-error-unique').slideUp();
                    var isValidDates = $.ajax({
                        type: 'POST',
                        url: baseurl + 'boletines-trade-validate-dates',
                        async: false,
                        data: $("#sampleform").serialize()
                    }).responseText;
                    if(!isValidDates){
                        $('#msg-error-validate-dates').slideUp();
                        submit();
                    }else{
                        $('#cargador-total').slideUp();
                        $('#msg-error-validate-dates').slideDown();
                    }
                } else {
                    $('#cargador-total').slideUp();
                    $('#msg-error-validate-dates').slideUp();
                    $('#msg-error-unique').slideDown();
                }
            }
        },
        // set new class to error-labels to indicate valid fields
        success: function(label) {
            // set &nbsp; as text for IE
            label.html("&nbsp;").addClass("ok");
        }
    });

    $("#fecha" ).datepicker({
        changeYear: true,
        changeMonth: true,
        disable: true,
        dateFormat:'yy-mm-dd'
    });

    $('#codtbol').change(function() {
        var id = $('#codboletin').val();
        var codseg = $('#codtbol').val();
        if(codseg == '3'){
            $('#segmento').slideDown();
        }else{
            $('#segmento').slideUp();
        }
        if(codseg == '5'){
            $('#content-evento').slideDown();
            $('#content-main-offer').slideUp();
            $('#offer').slideUp();
            $('#content-sugerencias').slideUp();
            $('#content-noticias').slideUp();
        }else{
            $('#content-evento').slideUp();
            $('#content-main-offer').slideDown();
            $('#offer').slideDown();
            $('#content-sugerencias').slideDown();
            $('#content-noticias').slideDown();
        }
        if((codseg == '6') || (codseg == '7') || (codseg == 8)){
            if(codseg == 8){
                $('#activo').prop('checked', false);
                $('#group_activo').slideUp();
                //$('#activo').slideUp();
                //$('#label_activo').slideUp();
            } else {
                $('#group_activo').slideDown();
                //$('#activo').slideDown();
                //$('#label_activo').slideDown();
            }
            var etiqueta = $('#etiqueta').val();
            $('#content-form').slideDown();
            $('#cargador-content').slideDown();
            $('#content-form').load(baseurl + 'boletines-trade-charge-content',{
                codseg: codseg,
                id: id,
                etiqueta: etiqueta
            },function(){
                $('#cargador-content').slideUp();
            });
        } else {
            $('#group_activo').slideDown();
        }

        if ((codseg != 8) && (id != '')){
            $('#viewport').html('');
        }
    });

    $('#type').change(function() {
        var type = $(this).val();
        var url = $('#btn_preview').attr('href').split('/');
        switch (type) {
            case 'generate':
                $('#btn_preview').attr('href','/'+url[1]+'/'+url[2]+'/'+url[3]+'/generate');
                break;
            default:
                $('#btn_preview').attr('href','/'+url[1]+'/'+url[2]+'/'+url[3]+'/preview');
        }
        return false;
    });

    $('#idioma').change(function() {
        var idioma = $(this).val();
        $('#codidio').val($(this).val());
        var url = $('#btn_preview').attr('href').split('/');
        $('.idioma').each(function() {
            if($(this).attr('id') === idioma){
                $(this).addClass('color_gray');
            } else {
                $(this).removeClass('color_gray');
            }
        });
        var isPublic = $.ajax({
            type: 'POST',
            url: baseurl + 'boletines-trade-is-public',
            async: false,
            data: $("#sampleform").serialize()
        }).responseText;
        if(isPublic){
            $('#content-btn-code').addClass('hidden');
        } else {
            $('#content-btn-code').removeClass('hidden');
        }
        $('#btn_code').attr('href','/'+url[1]+'/'+url[2]+'/'+idioma+'/'+url[4]);
        $('#btn_preview').attr('href','/'+url[1]+'/'+url[2]+'/'+idioma+'/'+url[4]);
        return false;
    });

    var _URL = window.URL || window.webkitURL;
    $('.input-image').bind('change', function() {
        var id = $(this).attr('id');
        var file, img;
        if ((file = this.files[0])) {
            img = new Image();
            img.onload = function() {
                if((this.width != 850)){
                    alert('El ancho de la imagen deben ser igual que 850 píxeles');
                    $('#'+id).val('');
                }
            };
            img.onerror = function() {
                alert( "not a valid file: " + file.type);
            };
            img.src = _URL.createObjectURL(file);
        }
    });
});

$('.delete-image').click(function(evento){
    var type = this.id;
    var id = $('#codboletin').val();
    switch (type) {
        case 'delete-image':
            $('#cargador-image').slideDown();
            $('#content-image').load(baseurl + 'boletines-trade-delete-image',{
                id: id
            },function(){
                $('#content-image').slideUp();
                $('#cargador-image').slideUp();
            });
            break
        case 'delete-banner':
            $('#cargador-banner').slideDown();
            $('#content-banner').load(baseurl + 'boletines-trade-delete-banner',{
                id: id
            },function(){
                $('#content-banner').slideUp();
                $('#cargador-banner').slideUp();
            });
            break
        default:
            break
    }
});


