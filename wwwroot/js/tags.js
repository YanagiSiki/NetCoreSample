(function (w) {
    var $getTags = $.get('/TagApi/GetTags');
    $getTags
        .done(function (PostCountOfTag) {
        appendBadgeAndCount(PostCountOfTag);
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
}(window));
//# sourceMappingURL=tags.js.map