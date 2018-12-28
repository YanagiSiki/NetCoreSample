(function (w) {
    var $postcontents = $('.PostContent');
    debugger;
    $.each($postcontents, function (idx, $postcontainer) {
        var simplemde = new SimpleMDE({
            element: $postcontainer,
            spellChecker: false,
            renderingConfig: {
                codeSyntaxHighlighting: true
            },
            toolbar: false,
            toolbarTips: false,
        });
        simplemde.togglePreview();
    });
}(window));
//# sourceMappingURL=index.js.map