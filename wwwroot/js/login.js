"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tool_1 = require("./tool");
(function (w) {
    $('#submitbtn').click(function () {
        var submitUrl = '/UserApi/Login';
        var returnUrl = $('#returnUrl').val().toString();
        var $form = $('#form');
        var formdata = $form.serializeJSON();
        $.post(submitUrl, { "user": formdata })
            .done(function () {
            tool_1.tool.alertSuccessMessage('Login Success !');
            if (!!returnUrl)
                window.location.href = returnUrl;
            else
                window.location.href = '/';
        })
            .fail(function (error) {
            tool_1.tool.alertErrorMessage(error.responseText);
        });
    });
}(window));
//# sourceMappingURL=login.js.map