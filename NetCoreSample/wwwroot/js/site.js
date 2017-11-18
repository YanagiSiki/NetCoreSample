(function (w) {
    //side bar toggle
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').removeClass('noAction');
        $('#sidebar').toggleClass('active');
        localStorage.setItem("isToggle", String($('#sidebar').is('.active')));
    });
    //record side bar toggle state
    if (!!localStorage.getItem('isToggle') && localStorage.getItem('isToggle') !== String($('#sidebar').is('.active'))) {
        $('#sidebar').addClass('noAction');
        $('#sidebar').toggleClass('active');
    }
}(window));
//# sourceMappingURL=site.js.map