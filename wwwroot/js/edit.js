"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tool_1 = require("./tool");
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
        tool_1.tool.appendBadge(tags, true);
        $tags.data('tags', tags);
        tool_1.tool.createTypeheadOfAllTags(alltags);
        $alltags.data('alltags', alltags);
    })
        .fail(function (error) {
        tool_1.tool.alertErrorMessage(error.responseText);
    });
    $alltags.on('click', '.btn', function () {
        tool_1.tool.addPostTags();
    });
    $tags.on('click', '.close', function (e) {
        tool_1.tool.removePostTags(e);
    });
    $('#submitbtn').click(function () { tool_1.tool.submitPost($simplemde); });
}(window));
//# sourceMappingURL=edit.js.map