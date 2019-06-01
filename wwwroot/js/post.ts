(function (w) {
    let postid = $('#PostId').val();
    let $postcontainer = $('#PostContent');
    let $getTagsOfPost = () => $.get('/PostApi/GetTagsOfPost', { postId: postid });
    let $deletePost = () => $.post('/PostApi/DeletePost', { "postId": postid });

    let $tags = $('#js-tags');

    let $simplemde = new SimpleMDE({
        element: $postcontainer[0]
    });
    // $simplemde.value($postcontainer.val().toString());
    // $simplemde.togglePreview();
    $('#Page').append($simplemde.markdown($postcontainer.val().toString()));

    $simplemde.toTextArea();
    $simplemde = null;

    $getTagsOfPost()
        .done((tags: Array<Tag>) => {
            appendBadge(tags);
            //_tags = tags;
            $tags.data('tags', tags);

        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        });
    $('#deletebtn').click(() => { 
        $deletePost().done((sucess)=>{
            alertSuccessMessage(sucess);
            setTimeout(function () {
                $(location).attr('href', '/')
            }, 1000);
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        });
    });
}(window));