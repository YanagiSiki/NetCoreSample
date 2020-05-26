import { tool } from "./tool.js";
(function (w) {
    // https://stackoverflow.com/questions/27807853/html5-how-to-make-a-form-submit-after-pressing-enter-at-any-of-the-text-inputs
    // 這段自定義submit，無法按enter自動執行form submit，所以加上這段keypress
    $('#form :text, :password').keypress(function (event) {
        if (event.keyCode == 13 || event.which == 13) {
            $('#submitbtn').click();
            event.preventDefault();
        }
    });
    $('#submitbtn').click(() => {
        let submitUrl = '/UserApi/Login';
        let returnUrl = $('#returnUrl').val().toString();
        let $form = $('#form');
        let formdata = $form.serializeJSON();
        $.post(submitUrl, { "user": formdata })
            .done(() => {
            tool.alertSuccessMessage('Login Success !');
            if (!!returnUrl)
                window.location.href = returnUrl;
            else
                window.location.href = '/';
        })
            .fail((error) => {
            tool.alertErrorMessage(error.responseText);
        });
    });
}(window));
//# sourceMappingURL=login.js.map