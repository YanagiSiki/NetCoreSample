(function (w) {
    let $postcontents = $('.PostContent');
    $.each($postcontents, (idx, postcontainer) => {
        let $postcontainer = $(postcontainer);
        let $simplemde = new SimpleMDE({
            element: $postcontainer[0]
        });
        

        $simplemde.toTextArea();
        $simplemde = null;
    })
    // $(".editor-preview").attr("class", "editor-preview markdown-body");
    // $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
}(window));