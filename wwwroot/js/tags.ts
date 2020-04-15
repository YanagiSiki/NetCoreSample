import {tool} from './tool';

(function (w) {
    let $getTags = () => $.get('/TagApi/GetTags');

    $getTags()
        .done((PostCountOfTag: Array<PostCountOfTag>) => {
            tool.appendBadgeAndCount(PostCountOfTag);
        })
        .fail((error) => {
            tool.alertErrorMessage(error.responseText)
        })
}(window));