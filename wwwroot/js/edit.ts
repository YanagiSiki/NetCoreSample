(function (w) {
    let postid = $('#PostId').val();
    let $getTagsOfPost = () => $.get('/PostApi/GetTagsOfPost', { postId: postid });
    let $getAllTags = () => $.get('/PostApi/GetAllTags');
    let $alltags = $('#js-alltags');
    let $tags = $('#js-tags');

    // waitForEl(".editor-preview, editor-preview-side", function () {
    //     $(".editor-preview").attr("class", "editor-preview markdown-body");
    //     $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
    // });

    let $simplemde = new SimpleMDE({
        element: document.getElementById("PostContent"),
        renderingConfig: {
            codeSyntaxHighlighting: true
        },
        spellChecker: false,
    });

    $.when($getTagsOfPost(), $getAllTags())
        .done((result1, result2) => {
            let tags: Array<Tag> = result1[0];
            let alltags: Array<Tag> = result2[0];

            appendBadge(tags, true);
            $tags.data('tags', tags);

            createTypeheadOfAllTags(alltags);
            $alltags.data('alltags', alltags);
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        });


    $alltags.on('click', '.btn', function () {
        addPostTags();
    });

    $tags.on('click', '.close', function (e: JQueryEventObject) {
        removePostTags(e);
    });

    $('#submitbtn').click(() => { submitPost($simplemde) });
}(window));