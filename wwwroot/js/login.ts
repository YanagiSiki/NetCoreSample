import { tool } from "./tool.js";

(function (w) {

    $('#submitbtn').click(() => {
        let submitUrl = '/UserApi/Login';
        let returnUrl = $('#returnUrl').val().toString();
        let $form = $('#form');
        let formdata: any = $form.serializeJSON();
        $.post(submitUrl, { "user": formdata })
            .done(() => {
                tool.alertSuccessMessage('Login Success !');
                if (!!returnUrl) window.location.href = returnUrl;
                else window.location.href = '/';
            })
            .fail((error) => {
                tool.alertErrorMessage(error.responseText)
            });
    });
}(window));