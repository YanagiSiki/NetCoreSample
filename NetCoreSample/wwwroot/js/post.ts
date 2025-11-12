import * as $ from 'jquery';
import {tool} from './tool.js';
import {sortNumber} from './pagination.js';

(function (w) {
    let postid = $('#PostId').val();
    let $postcontainer = $('#PostContent');
    let $getTagsOfPost = () => $.get('/PostApi/GetTagsOfPost', { postId: postid });

    let $tags = $('#js-tags');

    let $simplemde = new SimpleMDE({
        element: $postcontainer[0]
    });
    // $simplemde.value($postcontainer.val().toString());
    // $simplemde.togglePreview();
    $('#Page').append($simplemde.markdown($postcontainer.val().toString()));

    $simplemde.toTextArea();
    $simplemde = null;

    anchors.add();

    $getTagsOfPost()
        .done((tags: Array<Tag>) => {
            tool.appendBadge(tags);
            //_tags = tags;
            $tags.data('tags', tags);

        })
        .fail((error) => {
            tool.alertErrorMessage(error.responseText)
        });
        
    // 宣告 $deletePost
    const $deletePost = () => $.post('/PostApi/DeletePost', { postId: postid });
    $('#deletebtn').click(() => { 
        $deletePost().done((sucess)=>{
            tool.alertSuccessMessage(sucess);
            setTimeout(function () {
                $(location).attr('href', '/')
            }, 1000);
        })
        .fail((error) => {
            tool.alertErrorMessage(error.responseText)
        });
    });
}(window));