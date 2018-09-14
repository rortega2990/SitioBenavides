function run_waitMe(objet, texto) {
    var w = window.location.pathname.split('/');
    var base = "/" + w[1];

    $(objet).waitMe({
        effect: 'img',
        text: texto,
        bg: 'rgba(255,255,255,0.7)',
        color: '#000',
        sizeW: '',
        sizeH: '',
        source: base + '/Content/svg/img.svg',
        onClose: function () { }
    });
}
function ocultarWaitme(objet) {
    $(objet).waitMe('hide');
}