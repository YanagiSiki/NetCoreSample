// import SimpleMDE from 'simplemde';
(function (w) {
    let $postcontainer = $('#PostContent');
    let $simplemde = new SimpleMDE({
        element: $postcontainer[0]
    });
    $('#Page').append($simplemde.markdown($postcontainer.val().toString()));
    $simplemde.toTextArea();
}(window));