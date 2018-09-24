

function GetUrlBase() {
    var w = window.location.pathname.split('/');
    return "/" + w[1];
}


function getFilesNames(elemento) {
    var w = getSiteRootUrl();
    var valPrev = w + "/Investor/FilesByIdYear";

    var idYear = $(elemento).attr('id');
    var n = "#" + idYear + " option:selected";
    var year = $(n).attr('value');
    var idreport = idYear.split('_')[1];

    var ajaxRequest = $.ajax({
        type: "GET",
        url: valPrev + "?idReport=" + idreport + "&year=" + year
    });

    ajaxRequest.done(function (responseData, textStatus) {
        if (textStatus === "success") {
            if (responseData != null) {
                if (responseData.Success) {

                    var report = "#filesReport_" + idreport;
                    $(report).remove();
                    var raya = "#divRaya_" + idreport;
                    $(raya).remove();

                    var divReport = '<div id="filesReport_' + idreport + '">';

                    for (var i = 0; i < responseData.Data.length; i++) {
                        var addressFile = w + responseData.Data[i].AddressFile;
                        divReport += '<div class="row">' + 
                                        '<div class="col-md-2 col-md-offset-4 col-sm-2 col-sm-offset-3" style="text-align: left; margin-top: 10px">' +
                                            '<span style="color: #4169e1; font-size: 16px; font-weight: 800">' + responseData.Data[i].DescriptionFile + '</span>' +
                                        '</div>' +
                                        '<div style="text-align: left; margin-top: 10px">' +
                                            '<a class="btn btn-success btn-descargar" target="_blank" href="' + addressFile + '">Descargar</a>' +
                                        '</div></div>';
                    }
                    divReport += '</div>';

                    var htmlraya = '<div class="row no-margin-right margin-bottom-20" id="divRaya_' + idreport + '">' +
                        '<div class="col-md-1 col-md-offset-5 col-sm-3 col-sm-offset-4 hidden-xs linea-roja"></div></div>';
                    $("#report_" + idreport).append(divReport);
                    $("#report_" + idreport).append(htmlraya);
                } else {
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

function Loading(divElement) {
    App.blockUI({
        target: divElement,
        overlayColor: 'none',
        cenrerY: true,
        animate: true
    });
    //App.blockUI({
    //    target: divElement,
    //    boxed: true
    //});
    //App.blockUI({
    //    target: divElement,
    //    overlayColor: 'none',
    //    cenrerY: true,
    //    animate: true
    //        });
}

function UnLoading(divElement) {
    App.unblockUI(divElement);
}

function showPleaseWait() {
    var modalLoading = '<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false role="dialog">\
        <div class="modal-dialog">\
            <div class="modal-content">\
                <div class="modal-header">\
                    <h4 class="modal-title">Por favor, espere Estamos iniciando...</h4>\
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

function Logout() {
    var w = getSiteRootUrl();
    $.ajax({
        type: 'POST',
        url: w + '/Account/LogOff',
        success: function (responseData) {
            if (responseData.Success) {
                window.location.href = w + '/Home';
            } else {
                if (!responseData.Success) {
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
            }
        },
        error:
            function (errormessage) {
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

$.validator.addMethod("usercheck", function (user) {
    return /^[a-záéóóúàèìòùäëïöüñ\s]+$/i.test(user);
});

$.validator.addMethod("lettersDigits", function (user) {
    return /^[a-záéóóúàèìòùäëïöüñ.:,;0-9\s]+$/i.test(user);
});

$.validator.addMethod("onlyDigits", function (card) {
    return /^1[0-9]{10}$/.test(card);
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
    //return /^[a-zA-Z0-9\._-]+@[a-zA-Z0-9-]{2,}[.][a-zA-Z]{2,4}$/.test(email);
    return /^[a-zA-Z0-9\._-]+@[a-zA-Z]\w+(?:\.[a-zA-Z]\w+){1,3}$/.test(email);
});

function autocompleteData() {
    showPleaseWait();

    var base = getSiteRootUrl();
    var cardValue = $("#Card").val();
    $.ajax({
        url: base + '/Account/GetDataFromCard/' + cardValue,
        data: { card: cardValue },
        type: "GET",
        success: function (data) {
            hidePleaseWait();
            if (data.Success) {
                $("#Names").val(data.Data.Nombre);
                $("#LastName1").val(data.Data.ApPaterno);
                $("#LastName2").val(data.Data.ApMaterno);
                $("#Email").val(data.Data.Email);
                $("#PhoneNumber").val(data.Data.Celular);
                $("#BirthDate").val(data.Data.FechaNacimiento);
                if (data.Data.Genero == 1) {
                    $("#generoM").attr("checked", true);
                    $("#generoF").attr("checked", false);
                }
                if (data.Data.Genero == 2) {
                    $("#generoF").attr("checked", true);
                    $("#generoM").attr("checked", false);

                }
                $("#imagenUsuario").attr("src", data.Data.Imagen);
            } else {
                hidePleaseWait();
                bootbox.dialog({
                    message: data.Message,
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
        error: function (response) {
            hidePleaseWait();
            bootbox.dialog({
                message: response.Message,
                title: "Error",
                buttons: {
                    main: {
                        label: "Aceptar",
                        className: "red"
                    }
                }
            });
            //alert("error : " + response.Message);
        }
    });
}

$(document).ready(function () {
    $("#PhoneNumber").mask("(999) 999-9999");
    $("#TeamPhoneNumber").mask("(999) 999-9999");
    $("#ContactPhoneNumber").mask("(999) 999-9999");
    $("#Card").mask("99999999999");
    $("#CodePostal").mask("99999");
    //$("#BirthDate").mask("mm/dd/yyyy");
    

    $("#registerForm #Card").blur(function () {
        var validator = $("#registerForm").validate();
        var cardValidator = validator.element("#Card");
        if ($("#Card").valid() == true) {
            autocompleteData();
        }
    });
    
    
    //$("#perfilContactForm").validate({
    //    rules: {
    //        Email: {
    //            required: true,
    //            emailcheck: true,
    //            maxlength: 150
    //        }
    //    },
    //    messages: {
            
    //        Email: {
    //            required: "El campo Email es requerido",
    //            emailcheck: "Entre una dirección de correo válida"
    //        }
    //    },
    //    submitHandler: function () {
    //        Loading('#form_info_01');
    //        var w = GetUrlBase();
    //        var data = new FormData();
    //        data.append("Subject", $("#Subject").val());
    //        data.append("Comments", $("#Comments").val());
    //        data.append("Email", $("#Email").val());
    //        data.append("PerfilContactForm", $('#perfilContactForm').serialize());
    //        $.ajax({
    //            type: 'POST',
    //            url: w + '/Contact',
    //            data: $("#perfilContactForm").serialize(),
    //            success: function (responseData) {
    //                if (responseData.Success) {
    //                    UnLoading('#form_info_01');
    //                    bootbox.dialog({
    //                        message: responseData.Message,
    //                        title: "Información",
    //                        buttons: {
    //                            main: {
    //                                label: "Aceptar",
    //                                className: "blue",
    //                                callback: function () {
    //                                    window.location.href = w;
    //                                }
    //                            }
    //                        }
    //                    });

    //                } else {
    //                    if (!responseData.Success) {
    //                        UnLoading('#form_info_01');
    //                        bootbox.dialog({
    //                            message: responseData.Message,
    //                            title: "Error",
    //                            buttons: {
    //                                main: {
    //                                    label: "Aceptar",
    //                                    className: "red"
    //                                }
    //                            }
    //                        });
    //                    }
    //                }
    //            },
    //            error:
    //                function (errormessage) {
    //                    UnLoading('#form_info_01');
    //                    bootbox.dialog({
    //                        message: "Ha ocurrido un error interno en el servidor.",
    //                        title: "Error",
    //                        buttons: {
    //                            main: {
    //                                label: "Aceptar",
    //                                className: "red"
    //                            }
    //                        }
    //                    });
    //                }
    //        });
    //    }
    //});



     


 

    $("#perfilForm").validate({
        rules: {
            Card: { required: true },
            Names: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            LastName1: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            LastName2: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            BirthDate: { required: true },
            Email: {
                required: true,
                emailcheck: true,
                maxlength: 150
            },
            CGenre: { required: true }
        },
        messages: {
            Card: {
                required: "El campo Tarjeta es requerido"
            },
            Names: {
                required: "El campo Nombre es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            LastName1: {
                required: "El campo Apellido Paterno es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            LastName2: {
                required: "El campo Apellido Materno es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            
            Email: {
                required: "El campo Email es requerido",
                emailcheck: "Entre una dirección de correo válida"
            },
            
            CGenre: { required: "El campo Género es requerido" },
            BirthDate: {
                required: "El campo Fecha de nacimiento es requerido"
            }
        },
        submitHandler: function () {
            Loading('#form_info_02');
            var w = GetUrlBase();
            var data = new FormData();
            var ImagenUsuarioRegistro = $("#ImagenUsuarioRegistro").get(0).files;
            // Add the uploaded image content to the form data collection
            if (ImagenUsuarioRegistro.length > 0) {
                data.append("ImagenUsuarioRegistro", ImagenUsuarioRegistro[0]);
            }

            data.append("Card", $("#Card").val());
            data.append("Names", $("#Names").val());
            data.append("LastName1", $("#LastName1").val());
            data.append("LastName2", $("#LastName2").val());
            data.append("BirthDate", $("#BirthDate").val());
            data.append("CGenre", $("input[name='CGenre']:checked").val());
            data.append("Email", $("#Email").val());
            data.append("Password", $("#Password").val());

            //data.append("PerfilForm", $('#perfilForm').serialize());

            $.ajax({
                type: 'POST',
                url: w + '/Update',
                datatype: "jsonp",
                //data: $("#perfilForm").serialize(),
                contentType: false,
                processData: false,
                data:data,
                success: function (responseData) {
                    if (responseData.Success) {
                        UnLoading('#form_info_02');
                        bootbox.dialog({
                            message: "Se ha actualizado satisfactoriamente",
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    //callback: function () {
                                    //    window.location.href = w + '/Perfil';
                                    //}
                                }
                            }
                        });
                        
                    } else {
                        if (!responseData.Success) {
                            UnLoading('#form_info_02');
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
                },
                error:
                    function (errormessage) {
                        UnLoading('#form_info_02');
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
    });



    $("#perfilSuscForm").validate({
        submitHandler: function () {
            Loading('#form_info_12');
            var w = GetUrlBase();
            var data = new FormData();
       
            //data.append("AceptaContactoCorreo", $("#AceptaContactoCorreo").val());
            data.append("AceptaContactoCorreo", $("input[name='AceptaContactoCorreo']:checked").val());
            data.append("AceptaContactoSMS", $("input[name='AceptaContactoSMS']:checked").val());
            //data.append("PerfilForm", $('#perfilForm').serialize());

            $.ajax({
                type: 'POST',
                url: w + '/UpdateSusc',
                datatype: "jsonp",
                //data: $("#perfilForm").serialize(),
                contentType: false,
                processData: false,
                data: data,
                success: function (responseData) {
                    if (responseData.Success) {
                        UnLoading('#form_info_12');
                        $('#myModal').modal('toggle');
                        bootbox.dialog({
                            message: "Se ha actualizado satisfactoriamente",
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    //callback: function () {
                                    //    window.location.href = w + '/Perfil';
                                    //}
                                }
                            }
                        });

                    } else {
                        if (!responseData.Success) {
                            UnLoading('#form_info_12');
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
                },
                error:
                    function (errormessage) {
                        UnLoading('#form_info_12');
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
    });




    



    $("#registerForm").validate({ 
        rules: {
            Card: { required: true, minlength: 11, maxlength: 11, onlyDigits: false },
            Names: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            LastName1: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            LastName2: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            BirthDate: { required: true },
            City: { required: false, minlength: 2, maxlength: 100, lettersDigits: true },
            Email: {
                required: true,
                emailcheck: true,
                maxlength: 150
            },
            HasChildren: {
                required: true
            },
            Password: {
                required: true,
                pwcheck: false,
                minlength: 6,
                maxlength: 50
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            },
            CGenre: { required: true }
        },
        messages: {
            Card: {
                required: "El campo Tarjeta es requerido",
                onlyDigits: "Tarjeta no válida"
            },
            Names: {
                required: "El campo Nombre es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            LastName1: {
                required: "El campo Apellido Paterno es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            LastName2: {
                required: "El campo Apellido Materno es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            City: { 
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                lettersDigits: "Solo se permiten letras, números y signos de puntuación."
            },
            Email: {
                required: "El campo Email es requerido",
                emailcheck: "Entre una dirección de correo válida"
            },
            HasChildren: {
                required: "El campo ¿Tienes hijos? es requerido"
            },
            CGenre: { required: "El campo Género es requerido" },
            BirthDate: {
                required: "El campo Fecha de nacimiento es requerido"
            },
            Password: {
                required: "El campo Contraseña es requerido",
                pwcheck: "La contraseña es débil. Entre letras minúsculas y mayúsculas, números y caracteres especiales: !,%,&,@,#,$,^,*,?,_,~",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres")
            },
            ConfirmPassword: {
                required: "El campo Confirmar contraseña es requerido",
                equalTo: "La contraseña no coincide"
            }
        },
        submitHandler: function () {
            Loading('#modal-registro');
            var w = getSiteRootUrl();
            var data = new FormData();
            var ImagenUsuarioRegistro = $("#ImagenUsuarioRegistro").get(0).files;
            // Add the uploaded image content to the form data collection
            if (ImagenUsuarioRegistro.length > 0) {
                data.append("ImagenUsuarioRegistro", ImagenUsuarioRegistro[0]);
            }
            data.append("Card", $("#Card").val());
            data.append("Names", $("#Names").val());
            data.append("LastName1", $("#LastName1").val());
            data.append("LastName2", $("#LastName2").val());
            data.append("BirthDate", $("#BirthDate").val());
            data.append("CGenre", $("input[name='CGenre']:checked").val());
            data.append("HasChildren", $("input[name='HasChildren']:checked").val());
            data.append("ClubPeques", $("input[name='ClubPeques']:checked").val());
            data.append("Email", $("#Email").val());
            data.append("Password", $("#Password").val());
            data.append("CodePostal", $("#CodePostal").val());
            data.append("PhoneNumber", $("#PhoneNumber").val());
            data.append("City", $("#City").val());
            $.ajax({
                type: 'POST',
                url: w + '/Account/Register?id=1',
                datatype: "jsonp",
                contentType: false,
                processData: false,
                data: data,
                success: function (responseData) {
                    if (responseData.Success) {
                        UnLoading('#modal-registro');
                        $("#modal-registro").modal('hide');
                        window.location.href = w + '/Perfil';
                    } else {
                        if (!responseData.Success) {
                            UnLoading('#modal-registro');
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
                },
                error:
                    function (errormessage) {
                        UnLoading('#modal-registro');
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
    });
    
    $("#recuperarPassForm").validate({
        rules: {
            Email: { required: true }
        },
        messages: {
            Email: {
                required: "El campo Correo Electrónico es requerido"
            }
        },
        submitHandler: function () {
            var w = getSiteRootUrl();
            //Loading('#modal-tu-cuenta');
            $.ajax({
                type: 'POST',
                datatype: "jsonp",
                url: w + '/Account/ForgotPassword',
                data: $('#recuperarPassForm').serialize(),
                success: function (responseData) {

                    if (responseData.Success) {
                        UnLoading('#modal-recuperar-password');
                        
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        $("#CardForgotPass").val("");
                                        $("#Email").val("");
                                        
                                    }
                                }
                            }
                        });
                        $("#modal-recuperar-password").modal('hide');
                    } else {
                        if (!responseData.Success) {
                            //UnLoading('#modal-tu-cuenta');
                            document.getElementById('mensajeForgotPassword').innerHTML = responseData.Message;
                        }
                    }
                },
                error:
                    function (errormessage) {
                        //UnLoading('#modal-tu-cuenta');
                        document.getElementById('mensajeForgotPassword').innerHTML = "Ha ocurrido un error interno en el servidor.";
                    }
            });
        }
    });
    
    $("#changePassForm").validate({
        rules: {
            OldPassword: { required: true },
            Password: { required: true },
            ConfirmPassword: { required: true,equalTo: "#Password" }
        },
        messages: {
            OldPassword: {
                required: "El campo Contraseña Anterior es requerido"
            },
            Password: {
                required: "El campo Contraseña es requerido"
            },
            ConfirmPassword: {
                required: "El campo Confirmar Contraseña es requerido",
                equalTo: "La contraseña no coincide"
            }
        },
        submitHandler: function () {
            var w = getSiteRootUrl();
            Loading('#modal-change-password');
            $.ajax({
                type: 'POST',
                datatype: "jsonp",
                url: w + '/Account/ChangePassword',
                data: $('#changePassForm').serialize(),
                success: function (responseData) {
                    if (responseData.Success) {
                        UnLoading('#modal-change-password');

                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        $("#OldPassword").val("");
                                        $("#Password").val("");
                                        $("#ConfirmPassword").val("");
                                    }
                                }
                            }
                        });
                        $("#modal-change-password").modal('hide');
                    } else {
                        if (!responseData.Success) {
                            //UnLoading('#modal-tu-cuenta');
                            UnLoading('#modal-change-password');
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
                },
                error:
                    function (errormessage) {
                        UnLoading('#modal-change-password');
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
    });

    $("#loginForm").validate({
        rules: {
            CardLogin: { required: true },
            PasswordLogin: { required: true }
        },
        messages: {
            CardLogin: {
                required: "El campo Tarjeta es requerido"
            },
            PasswordLogin: {
                required: "El campo Contraseña es requerido"
            }
        },
        submitHandler: function () {
            var w = getSiteRootUrl();
            Loading('#modal-tu-cuenta');
            $.ajax({
                type: 'POST',
                url: w + '/Account/Login',
                datatype:"jsonp",
                data: $('#loginForm').serialize(),
                success: function (responseData) {
                    if (responseData.Success) {
                        UnLoading('#modal-tu-cuenta');
                        document.getElementById('nombreLoggeado').innerHTML = responseData.Data.Names;
                        document.getElementById('saldoLoggeado').innerHTML = responseData.Data.Mount;
                        document.getElementById('tarjetaLoggeado').innerHTML = responseData.Data.Card;
                        //document.getElementById('clubLoggeado').innerHTML = responseData.Data.CreationDateClubPeques;
                        var perfil = $("<li><a href='/Perfil'>Mi Perfil</a></li>");
                        var liCerrarSesion = $('<li><a id="linCerrarSesion" href="javascript:Logout();">Cerrar sesión</a></li>');
                        var soloParaTi = $('<li><a href="#">Solo para ti</a></li>');
                        $("#menuOverlay").append(perfil);
                        $("#menuOverlay").append(liCerrarSesion);
                        $("#menuOverlay").append(soloParaTi);
                        $("#menuTuCuenta").remove();
                        $("#menuRegistrate").remove();
                        $("#modal-tu-cuenta").modal('hide');
                        $("#modal-tu-cuenta-logged").modal('show');
                    } else {
                        if (!responseData.Success) {
                            UnLoading('#modal-tu-cuenta');
                            document.getElementById('mensajeLogin').innerHTML = responseData.Message;
                        }
                    }
                },
                error:
                    function (errormessage) {
                        UnLoading('#modal-tu-cuenta');
                        document.getElementById('mensajeLogin').innerHTML = "Ha ocurrido un error interno en el servidor.";
                    }
            });
        }
    });

    $("#equipoForm").validate({
        rules: {
            TeamNames: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            TeamLastnames: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            TeamEmail: {
                required: true,
                email: true,
                maxlength: 150
            },
            TeamPhoneNumber: { required: true }
        },
        messages: {
            TeamNames: {
                required: "El campo Nombre (s) es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            TeamLastnames: {
                required: "El campo Apellidos",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            TeamEmail: {
                required: "El campo Email es requerido",
                emailcheck: "Entre una dirección de correo válida"
            },
            TeamPhoneNumber: {
                required: "El campo Teléfono es requerido"
            }
        },
        submitHandler: function () {
            Loading('#equipoForm');
            //document.getElementById('mensajeUneteEquipo').innerHTML = "";
            var w = getSiteRootUrl();

            var data = new FormData();

            var area = $("#TeamInterestArea option:selected").map(function () { return this.value }).get().join(", ");
            data.append("TeamInterestArea", area);
            data.append("TeamEmail", $("#TeamEmail").val());
            data.append("TeamPhoneNumber", $("#TeamPhoneNumber").val());
            data.append("TeamLastnames", $("#TeamLastnames").val());
            data.append("TeamNames", $("#TeamNames").val());
            data.append("TeamAddress", $("#TeamAddress").val());
            data.append("InterestRegion", $("#InterestRegion").val());

            var ajaxRequest = $.ajax({
                type: "POST",
                url: w + '/JoinTeam/Join?id=1',
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (responseData, textStatus) {
                if (textStatus === "success") {
                    if (responseData != null) {
                        if (responseData.Success) {
                            UnLoading('#equipoForm');
                            bootbox.dialog({
                                message: responseData.Message,
                                title: "Información",
                                buttons: {
                                    main: {
                                        label: "Aceptar",
                                        className: "blue",
                                        callback: function () {
                                            $("#TeamNames").val("");
                                            $("#TeamLastnames").val("");
                                            $("#TeamEmail").val("");
                                            $("#TeamPhoneNumber").val("");
                                            $("#TeamInterestArea").val("");
                                            $("#TeamAddress").val("");
                                            $("#TeamInterestArea").multiselect('refresh');
                                        }
                                    }
                                }
                            });                            
                        } else {
                            UnLoading('#equipoForm');
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
                    UnLoading('#equipoForm');
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
    });

    $("#contactForm").validate({
        rules: {
            ContactNames: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            ContactSuggests: { required: true, minlength: 2, maxlength: 100, usercheck: true },
            ContactEmail: {
                required: true,
                email: true,
                maxlength: 150
            },
            ContactPhoneNumber: { required: true }
        },
        messages: {
            ContactNames: {
                required: "El campo Nombre (s) es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres"),
                usercheck: "Solo se permiten letras."
            },
            ContactSuggests: {
                required: "El campo Sugerencias es requerido",
                minlength: jQuery.validator.format("Entre al menos {0} caracteres")
            },
            ContactEmail: {
                required: "El campo Email es requerido",
                emailcheck: "Entre una dirección de correo válida"
            },
            ContactPhoneNumber: {
                required: "El campo Teléfono es requerido"
            }
        },
        submitHandler: function () {
            Loading('#contactForm');
            //document.getElementById('mensajeContacto').innerHTML = "";
            var w = getSiteRootUrl();
            $.ajax({
                type: 'POST',
                url: w + '/Contact/SendSug',
                data: $('#contactForm').serialize(),
                success: function (responseData) {
                    if (responseData.Success) {
                        UnLoading('#contactForm');
                        bootbox.dialog({
                            message: responseData.Message,
                            title: "Información",
                            buttons: {
                                main: {
                                    label: "Aceptar",
                                    className: "blue",
                                    callback: function () {
                                        $("#ContactNames").val("");
                                        $("#ContactPhoneNumber").val("");
                                        $("#ContactEmail").val("");
                                        $("#ContactSuggests").val("");
                                    }
                                }
                            }
                        });                        
                    } else {
                        if (!responseData.Success) {
                            UnLoading('#contactForm');
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
                },
                error:
                    function (errormessage) {
                        UnLoading('#contactForm');
                        bootbox.dialog({
                            message: "Los datos no se pudieron enviar. Ha ocurrido un error en el servidor.",
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
    });
    
    var v = getSiteRootUrl();
    var table = $('#promotions');

    // begin first table
    table.dataTable({
        "responsive": true,
        "info": false,
        "ajax": {
            "url": v + '/Content/promociones.txt',
            "dataSrc": ""
        },
        "columns": [
            { "data": "CodigoGratis" },
            { "data": "NombreProdAcumula" },
            { "data": "Laboratorio" },
            {
                "data": "Regla",
                "render": function (data, type, full, meta) {
                    return full.Regla + " obtén " + full.CodigoGratis;
                }
            },
            { "data": "LimitePeriodicidad" }
        ],

        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "search": "",
            "searchPlaceholder": "Encuentra tu producto...",
            "emptyTable": "No existen datos a mostrar",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ elementos",
            "infoEmpty": "No se encuentran elementos",
            "infoFiltered": "(Total de elementos: _MAX_)",
            "lengthMenu": "Mostrar _MENU_",
            //"search": "Buscar:",
            "zeroRecords": "No existen coincidencias",
            "paginate": {
                "previous": "<",
                "next": ">",
                "last": "Último",
                "first": "Primero"
            }
        },
        
        "dom": '<"top"f>rt<"bottom"lp><"clear">',

        "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.

        "bLengthChange": false,
        // set the initial value
        "pageLength": 10,
        "pagingType": "simple_numbers",
        "order": [
            [0, "asc"]
        ] // set first column as a default sort by asc
    });
});

function recuperarPass() {
    var card = $("#CardLogin").val();
  
    $("#modal-tu-cuenta").modal('hide');
    $("#modal-recuperar-password").modal('show');
    $("#CardForgotPass").val(card);

}