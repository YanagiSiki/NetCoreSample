function alertErrorMessage(errorMessage: string) {
    $('#_ErrorFlashMessage').append(`<div class="alert alert-danger">
                                    <a href="#" class="close" data-dismiss="alert">&times;</a>
                                    <strong>Error!</strong> ${errorMessage}
                                </div>`)

}
function alertSuccessMessage(sucessMessage: string) {
    $('#_ErrorFlashMessage').append(` <div class="alert alert-success">
                                    <a href="#" class="close" data-dismiss="alert">&times;</a>
                                    <strong>Sucess!</strong> ${sucessMessage}
                                </div>`)

}

declare interface JQuery {
    typeahead(option: any, value: any): JQuery;
    serializeJSON(): JQuery;
}
declare class SimpleMDE {
    constructor(option: any);
    value(): string;
    toTextArea(): void;
    value(value: string): void;
    togglePreview(): void;
    markdown(value: string): string;
}
declare interface Tag {
    TagId: number;
    TagName: string;
}
declare interface PostCountOfTag {
    Tag: Tag;
    Count: number;
}
declare namespace hljs {
    function initHighlightingOnLoad(): void;
    function initLineNumbersOnLoad(): void;
}

function waitForEl(selector, callback) {
    if ($(selector).length) {
        callback();
    } else {
        setTimeout(function () {
            waitForEl(selector, callback);
        }, 100);
    }
};

function substringMatcher(strs) {
    return function findMatches(q, cb) {
        var matches: any[];

        // an array that will be populated with substring matches
        matches = [];

        // regex used to determine if a string contains the substring `q`
        var substrRegex = new RegExp(q, 'i');

        // iterate through the pool of strings and for any string that
        // contains the substring `q`, add it to the `matches` array
        $.each(strs, function (i, str) {
            if (substrRegex.test(str)) {
                matches.push(str);
            }
        });

        cb(matches);
    };
};

function appendBadge(tags: Array<Tag>, isUseCloseBtn?: boolean) {
    var $container = $('#js-tags');
    tags.forEach(t => {
        if (!!isUseCloseBtn)
            $container.append(`<span class='badge badge-success'>
                    <a href='/Home/Tag/${t.TagId}'>${t.TagName}</a>
                    <i class='close fa fa-times' data-tagname='${t.TagName}'></i>
                    </span> `);
        else
            // $container.append(`<span class='badge badge-pill badge-primary'>
            //             <a href='#' >${t.TagName}</a>
            //             </span> `);
            $container.append(`<a href='/Home/Tag/${t.TagId}' class='badge badge-success'>${t.TagName}</a>`);
    });
}

function appendBadgeAndCount(PostCountOfTag: Array<PostCountOfTag>) {
    var $container = $('#js-postCountofTag');
    PostCountOfTag.forEach(t => {
        $container.append(`<a href='/Home/Tag/${t.Tag.TagId}' class='badge badge-success'>${t.Tag.TagName}(${t.Count})</a>`);
    });
}

function addPostTags() {
    let $alltags = $('#js-alltags');
    let $tags = $('#js-tags');
    let tagName = $('input.typeahead.tt-input').val().toString();
    let _allTags: Array<Tag> = $alltags.data('alltags');
    let _tags: Array<Tag> = $tags.data('tags');

    if (!!_tags.find((t, i, arr) => { return t.TagName == tagName; })) {
        alertErrorMessage('已經加入此Tag!');
        return;
    }

    let tag = _allTags.filter((t, i, arr) => { return t.TagName == tagName; });

    if (!tag.length) {
        tag.push({ TagId: 0, TagName: tagName });
    }
    appendBadge(tag, true);
    _tags.push(tag[0]);
    $tags.data('tags', _tags);
}


function removePostTags(e: JQueryEventObject) {
    let $tags = $('#js-tags');
    let _tags: Array<Tag> = $tags.data('tags');
    let tagName = $(e.target).data('tagname');
    $(e.target).parent('span').remove();
    _tags = _tags.filter((t, i, arr) => { return t.TagName != tagName });
    $tags.data('tags', _tags);
}

function crateTypeheadOfAllTags(tags: Array<Tag>) {
    var tagforautocomplete = $.map(tags, function (item, index) {
        return item.TagName;
    })

    $('#bloodhound .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
            name: 'tagforautocomplete',
            source: substringMatcher(tagforautocomplete)
        });
}

function submit($simplemde: SimpleMDE) {
    let $form = $('#form');
    let $tags = $('#js-tags');
    let formdata: any = $form.serializeJSON();
    let postid: number = parseInt($('#PostId').val().toString());
    let posttags = $.map($tags.data('tags'), (t, i) => {
        return {
            PostId: postid,
            TagId: t.TagId,
            Tag: t
        }
    })
    let submitUrl = !!postid ? '/PostApi/UpdatePost' : '/PostApi/InsertPost';
    formdata.PostTags = posttags;
    formdata.PostContent = $simplemde.value();
    $.post(submitUrl, { "post": formdata })
        .done(() => {
            alertSuccessMessage('Update Success !');
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        });
}