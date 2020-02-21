(function (w) {

    $('#submitbtn').click(() => {
        let submitUrl = '/UserApi/Login';
        let returnUrl = $('#returnUrl').val().toString();
        let $form = $('#form');
        let formdata: any = $form.serializeJSON();
        $.post(submitUrl, { "user": formdata })
            .done(() => {
                alertSuccessMessage('Login Success !');
                if (!!returnUrl) window.location.href = returnUrl;
                else window.location.href = '/';
            })
            .fail((error) => {
                alertErrorMessage(error.responseText)
            });
    });
}(window));