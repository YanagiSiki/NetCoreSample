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

    //let _tags: Array<Tag>;
    //let _allTags: Array<Tag>;

    $getTagsOfPost()
        .done((tags: Array<Tag>) => {
            appendBadge(tags, true);
            //_tags = tags;
            $tags.data('tags', tags)
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        })

    $getAllTags()
        .done((alltags: Array<Tag>) => {
            createTypeheadOfAllTags(alltags);
            //_allTags = tags;         
            $alltags.data('alltags', alltags);
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        })

    $alltags.on('click', '.btn', function () {
        addPostTags();
    });

    $tags.on('click', '.close', function (e: JQueryEventObject) {
        removePostTags(e);
    });

    $('#submitbtn').click(() => { submitPost($simplemde) });
}(window));