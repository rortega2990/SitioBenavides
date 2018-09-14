﻿// add :focus selector
jQuery.expr[':'].focus = function (elem) {
    return elem === document.activeElement && (elem.type || elem.href);
};


//$.editable.addInputType('datepicker', {
//    element: function (settings, original) {
//        var input = $('<input>');
//        if (settings.width != 'none') { input.width(settings.width); }
//        if (settings.height != 'none') { input.height(settings.height); }
//        input.attr('autocomplete', 'off');
//        $(this).append(input);
//        return (input);
//    },
//    plugin: function (settings, original) {
//        /* Workaround for missing parentNode in IE */
//        var form = this;
//        settings.onblur = 'ignore';
//        $(this).find('input').datepicker().bind('click', function () {
//            $(this).datepicker('show');
//            return false;
//        }).bind('dateSelected', function (e, selectedDate, $td) {
//            $(form).submit();
//        });
//    }
//});


$.editable.addInputType('datepicker', {

    /* create input element */
    element: function (settings, original) {
        var form = $(this),
            input = $('<input />');
        input.attr('autocomplete', 'off');
        form.append(input);
        return input;
    },

    /* attach jquery.ui.datepicker to the input element */
    plugin: function (settings, original) {
        var form = this,
            input = form.find("input");

        // Don't cancel inline editing onblur to allow clicking datepicker
        settings.onblur = 'nothing';

        datepicker = {
            onSelect: function () {
                // clicking specific day in the calendar should
                // submit the form and close the input field
                form.submit();
            },

            onClose: function () {
                setTimeout(function () {
                    if (!input.is(':focus')) {
                        // input has NO focus after 150ms which means
                        // calendar was closed due to click outside of it
                        // so let's close the input field without saving
                        original.reset(form);
                    } else {
                        // input still HAS focus after 150ms which means
                        // calendar was closed due to Enter in the input field
                        // so lets submit the form and close the input field
                        form.submit();
                    }

                    // the delay is necessary; calendar must be already
                    // closed for the above :focus checking to work properly;
                    // without a delay the form is submitted in all scenarios, which is wrong
                }, 150);
            }
        };

        if (settings.datepicker) {
            jQuery.extend(datepicker, settings.datepicker);
        }

        input.datepicker(datepicker);
    }
});