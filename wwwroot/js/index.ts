(function (w) {
    let $postcontents: any = $('#PostContent');
    $.each($postcontents, (idx, $postcontainer) => {
        let simplemde = new SimpleMDE({
            element: $postcontainer,
            renderingConfig: {
                codeSyntaxHighlighting: true
            },
            toolbar: false,
            // toolbarTips: false,
            status: false,
        });
        simplemde.togglePreview();
        simplemde.value($postcontainer.data('contant'));
    })
    $(".editor-preview").attr("class", "editor-preview markdown-body");
    $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
}(window));