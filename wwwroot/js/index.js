(function (w) {
    var $postcontents = $('.PostContent');
    debugger;
    $.each($postcontents, function (idx, $postcontainer) {
        var simplemde = new SimpleMDE({
            element: $postcontainer,
            renderingConfig: {
                codeSyntaxHighlighting: true
            },
            toolbar: false,
            // toolbarTips: false,
            status: false,
        });
        simplemde.togglePreview();
    });
    $(".editor-preview").attr("class", "editor-preview markdown-body");
    $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
}(window));
//# sourceMappingURL=index.js.map