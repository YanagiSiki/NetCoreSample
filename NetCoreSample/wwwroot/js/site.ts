(function (w) {
    //side bar toggle
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        $('#content').toggleClass('active');
        localStorage.setItem("isToggle", String($('#sidebar').is('.active')));
    });
    //record side bar toggle state
    if (!!localStorage.getItem('isToggle') && localStorage.getItem('isToggle') !== String($('#sidebar').is('.active'))) {
        $('#content').toggleClass('active');
        $('#sidebar').toggleClass('active');
    }
    hljs.initHighlightingOnLoad();
    hljs.initLineNumbersOnLoad();
}(window));