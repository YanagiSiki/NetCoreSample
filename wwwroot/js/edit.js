(function (w) {
    var postid = $('#PostId').val();
    var $getTagsOfPost = function () { return $.get('/PostApi/GetTagsOfPost', { postId: postid }); };
    var $getAllTags = function () { return $.get('/PostApi/GetAllTags'); };
    var $alltags = $('#js-alltags');
    var $tags = $('#js-tags');
    waitForEl(".editor-preview, editor-preview-side", function () {
        $(".editor-preview").attr("class", "editor-preview markdown-body");
        $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
    });
    var $simplemde = new SimpleMDE({
        element: document.getElementById("PostContent"),
        renderingConfig: {
            codeSyntaxHighlighting: true
        }
    });
    //let _tags: Array<Tag>;
    //let _allTags: Array<Tag>;
    $getTagsOfPost()
        .done(function (tags) {
        appendBadge(tags, true);
        //_tags = tags;
        $tags.data('tags', tags);
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
    $getAllTags()
        .done(function (alltags) {
        createTypeheadOfAllTags(alltags);
        //_allTags = tags;         
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
    $('#submitbtn').click(function () { submit($simplemde); });
}(window));
//# sourceMappingURL=edit.js.map