import * as $ from 'jquery';
import {tool} from './tool.js';

(function (w) {
    let $getTags = () => $.get('/TagApi/GetTags');

    $getTags()
        .done((PostCountOfTag: Array<PostCountOfTag>) => {
            // TODO: 處理標籤資料
        })
        .fail((error) => {
            tool.alertErrorMessage(error.responseText);
        });
})(window);