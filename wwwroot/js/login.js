(function (w) {
    $('#submitbtn').click(function () {
        var submitUrl = '/UserApi/Login';
        var returnUrl = $('#returnUrl').val().toString();
        var $form = $('#form');
        var formdata = $form.serializeJSON();
        $.post(submitUrl, { "user": formdata })
            .done(function () {
            alertSuccessMessage('Login Success !');
            if (!!returnUrl)
                window.location.href = returnUrl;
            else
                window.location.href = '/';
        })
            .fail(function (error) {
            alertErrorMessage(error.responseText);
        });
    });
}(window));
//# sourceMappingURL=login.js.map