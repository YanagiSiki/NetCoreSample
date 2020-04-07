(function (w) {
    var postid = $('#PostId').val();
    var $getTagsOfPost = function () { return $.get('/PostApi/GetTagsOfPost', { postId: postid }); };
    var $getAllTags = function () { return $.get('/PostApi/GetAllTags'); };
    var $alltags = $('#js-alltags');
    var $tags = $('#js-tags');
    // waitForEl(".editor-preview, editor-preview-side", function () {
    //     $(".editor-preview").attr("class", "editor-preview markdown-body");
    //     $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
    // });
    var $simplemde = new SimpleMDE({
        element: document.getElementById("PostContent"),
        renderingConfig: {
            codeSyntaxHighlighting: true
        },
        spellChecker: false,
    });
    $.when($getTagsOfPost(), $getAllTags())
        .done(function (result1, result2) {
        var tags = result1[0];
        var alltags = result2[0];
        appendBadge(tags, true);
        $tags.data('tags', tags);
        createTypeheadOfAllTags(alltags);
        $alltags.data('alltags', alltags);
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
    $alltags.on('click', '.btn', function () {
        addPostTags();
    });
    $tags.on('click', '.close', function (e) {
        removePostTags(e);
    });
    $('#submitbtn').click(function () { submitPost($simplemde); });
}(window));
//# sourceMappingURL=edit.js.map