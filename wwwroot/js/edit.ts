import { tool } from "./tool";

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

            tool.appendBadge(tags, true);
            $tags.data('tags', tags);

            tool.createTypeheadOfAllTags(alltags);
            $alltags.data('alltags', alltags);
        })
        .fail((error) => {
            tool.alertErrorMessage(error.responseText)
        });


    $alltags.on('click', '.btn', function () {
        tool.addPostTags();
    });

    $tags.on('click', '.close', function (e: JQueryEventObject) {
        tool.removePostTags(e);
    });

    $('#submitbtn').click(() => { tool.submitPost($simplemde) });
}(window));