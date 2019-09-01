(function (w) {
    var $getTags = function () { return $.get('/TagApi/GetTags'); };
    $getTags()
        .done(function (PostCountOfTag) {
        appendBadgeAndCount(PostCountOfTag);
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
}(window));
//# sourceMappingURL=tags.js.map