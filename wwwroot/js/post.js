(function (w) {
    var postid = $('#PostId').val();
    var $postcontainer = $('#PostContent');
    var $getTagsOfPost = $.get('/PostApi/GetTagsOfPost', { postId: postid });
    var $tags = $('#js-tags');
    var $simplemde = new SimpleMDE({
        element: $postcontainer[0]
    });
    // $simplemde.value($postcontainer.val().toString());
    // $simplemde.togglePreview();
    $('#Page').append($simplemde.markdown($postcontainer.val().toString()));
    $simplemde.toTextArea();
    $simplemde = null;
    // $(".editor-preview").attr("class", "editor-preview markdown-body");
    // $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
    //let _tags: Array<Tag>;
    //let _allTags: Array<Tag>;
    $getTagsOfPost
        .done(function (tags) {
        appendBadge(tags);
        //_tags = tags;
        $tags.data('tags', tags);
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
}(window));
//# sourceMappingURL=post.js.map