"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tool_1 = require("./tool");
(function (w) {
    var $getTags = function () { return $.get('/TagApi/GetTags'); };
    $getTags()
        .done(function (PostCountOfTag) {
        tool_1.tool.appendBadgeAndCount(PostCountOfTag);
    })
        .fail(function (error) {
        tool_1.tool.alertErrorMessage(error.responseText);
    });
}(window));
//# sourceMappingURL=tags.js.map