(function (w) {
    let $getTags = () => $.get('/TagApi/GetTags');

    $getTags()
        .done((PostCountOfTag: Array<PostCountOfTag>) => {
            appendBadgeAndCount(PostCountOfTag);
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        })
}(window));