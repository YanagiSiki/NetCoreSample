import { tool } from './tool.js';
(function (w) {
    let $getTags = () => $.get('/TagApi/GetTags');
    $getTags()
        .done((PostCountOfTag) => {
        tool.appendBadgeAndCount(PostCountOfTag);
    })
        .fail((error) => {
        tool.alertErrorMessage(error.responseText);
    });
}(window));
//# sourceMappingURL=tags.js.map