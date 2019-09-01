(function (w) {
    var $postcontainer = $('#PostContent');
    var $simplemde = new SimpleMDE({
        element: $postcontainer[0]
    });
    $('#Page').append($simplemde.markdown($postcontainer.val().toString()));
    $simplemde.toTextArea();
}(window));
//# sourceMappingURL=about.js.map