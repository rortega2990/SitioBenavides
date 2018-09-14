function getSiteRootUrl() {
    return window.location.origin ? window.location.origin : window.location.protocol + '/' + window.location.host;
}