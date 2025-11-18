import type { Tags } from "./declaration.d.ts";
import { tool } from './tool.js';

(function (w) {
    let $getTags = () => $.get('/TagApi/GetTags');

    $getTags()
        .done((PostCountOfTag: Array<Tags>) => {
            tool.appendBadgeAndCount(PostCountOfTag);
        })
        .fail((error) => {
            tool.alertErrorMessage(error.responseText);
        });
})(window);