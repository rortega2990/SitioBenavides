
var app = function () {

    var zoom_env = function () {
        var magnify = 1.2;
        var zoom = function (magnify) {
            $('.product').trigger('zoom.destroy');
            $('.product').zoom({
                on: 'grab',
                magnify: magnify,
                onZoomIn: function () {
                    $('.product span, .zoom, .products-img').css('opacity', 0);
                },
                onZoomOut: function () {
                    $('.product span, .zoom, .products-img').css('opacity', 1);
                }
            });
            // $('.product span.nx').html(magnify.toFixed(1)+'x');
            $(".zoom-slider").slider("value", magnify);
        };
        $('a.zoom-in').on('click', function (e) {
            e.preventDefault();
            if (magnify < 3) {
                magnify += .1;
                zoom(magnify);
            }
        });
        $('a.zoom-out').on('click', function (e) {
            e.preventDefault();
            if (magnify > 1) {
                magnify -= .1;
                zoom(magnify);
            }
        });
        $(".zoom-slider").slider({
            min: 1,
            max: 3,
            step: .1,
            value: magnify,
            orientation: "horizontal",
            animate: true,
            slide: function (e, ui) {
                new_magnify = parseFloat(ui.value.toFixed(1));
                magnify = new_magnify;
                zoom(magnify);
            }
        });

        //always at the end !important
        zoom(magnify);
    };
    //binding zoom functions
    if ($('.zoom').length) {
        zoom_env();
    }

    //-moz support
    if ($('.animated-component').length) {
        $('.animated-component').hover(function () {
            $('img', this).css('-moz-transform', 'scale(1.1)');
            $('.hover-bgcolor', this).css('opacity', .08);
            $('.hover-color', this).css('color', $('.hover-color', this).attr('data-hcolor'));
            // if ($(this).parents('.top').length) {
                
            // } else {
            //     $('.hover-color', this).css('color', 'rgba(42, 52, 121, 0.5)');
            // }
        }, function () {
            $('img', this).css('-moz-transform', 'scale(1)');
            $('.hover-bgcolor', this).css('opacity', 0);
            $('.hover-color', this).css('color', '#ffffff');
        });
    }
};

$(document).ready(app);