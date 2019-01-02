(function (w) {
    let $postcontents = $('.PostContent');
    $.each($postcontents, (idx, postcontainer) => {
        let $postcontainer = $(postcontainer);
        let simplemde = new SimpleMDE({
            element: postcontainer,
            renderingConfig: {
                codeSyntaxHighlighting: true
            },
            toolbar: false,
            // toolbarTips: false,
            status: false,
        });
        simplemde.value($postcontainer.data('contant'));
        simplemde.togglePreview();
        
    })
    $(".editor-preview").attr("class", "editor-preview markdown-body");
    $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
}(window));