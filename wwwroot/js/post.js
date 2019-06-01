(function (w) {
    var postid = $('#PostId').val();
    var $postcontainer = $('#PostContent');
    var $getTagsOfPost = function () { return $.get('/PostApi/GetTagsOfPost', { postId: postid }); };
    var $deletePost = function () { return $.post('/PostApi/DeletePost', { "postId": postid }); };
    var $tags = $('#js-tags');
    var $simplemde = new SimpleMDE({
        element: $postcontainer[0]
    });
    // $simplemde.value($postcontainer.val().toString());
    // $simplemde.togglePreview();
    $('#Page').append($simplemde.markdown($postcontainer.val().toString()));
    $simplemde.toTextArea();
    $simplemde = null;
    $getTagsOfPost()
        .done(function (tags) {
        appendBadge(tags);
        //_tags = tags;
        $tags.data('tags', tags);
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
    $('#deletebtn').click(function () {
        $deletePost().done(function (sucess) {
            alertSuccessMessage(sucess);
            setTimeout(function () {
                $(location).attr('href', '/');
            }, 1000);
        })
            .fail(function (error) {
            alertErrorMessage(error.responseText);
        });
    });
}(window));
//# sourceMappingURL=post.js.map