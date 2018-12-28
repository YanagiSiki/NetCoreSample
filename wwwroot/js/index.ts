(function (w) {
    let $postcontents = $('.PostContent');
    debugger
    $.each($postcontents, (idx, $postcontainer) => {
        let simplemde = new SimpleMDE({
            element: $postcontainer,
            spellChecker: false,
            renderingConfig: {
                codeSyntaxHighlighting: true
            },
            toolbar: false,
            toolbarTips: false,
        });
        simplemde.togglePreview();
    })
}(window));