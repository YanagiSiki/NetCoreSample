declare interface JQuery {
    typeahead(option: any, value: any): JQuery;
    serializeJSON(): JQuery;
}
declare class SimpleMDE {
    constructor(option: any)
}
declare interface Tag {
    TagId: number;
    TagName: string;
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

function appendBadge(tags: Array<Tag>) {
    var $container = $('#js-tags');
    tags.forEach(t => {
        $container.append(`<span class='badge badge-pill badge-primary'>
                    <a href='#' >${t.TagName}</a>
                    <i class='close fa fa-times' data-tagname='${t.TagName}'></i>
                    </span> `);
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
    appendBadge(tag);
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

function submit() {
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
    
    $.post(submitUrl, { "post": formdata })
        .done(() => {
            alertSuccessMessage('Update Success !');
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        });
}

(function (w) {
    let postid = $('#PostId').val();
    let $getTagsOfPost = $.get('/PostApi/GetTagsOfPost', {postId: postid});
    let $getAllTags = $.get('/PostApi/GetAllTags');
    let $alltags = $('#js-alltags');
    let $tags = $('#js-tags');

    waitForEl(".editor-preview, editor-preview-side", function () {
        $(".editor-preview").attr("class", "editor-preview markdown-body");
        $(".editor-preview-side").attr("class", "editor-preview-side markdown-body");
    });

    let simplemde = new SimpleMDE({
        element: document.getElementById("PostContent"),
        spellChecker: false,
        renderingConfig: {
            codeSyntaxHighlighting: true
        }
    });

    //let _tags: Array<Tag>;
    //let _allTags: Array<Tag>;

    $getTagsOfPost
        .done((tags: Array<Tag>) => {
            appendBadge(tags);
            //_tags = tags;
            $tags.data('tags', tags)
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        })

    $getAllTags
        .done((alltags: Array<Tag>) => {
            crateTypeheadOfAllTags(alltags);
            //_allTags = tags;         
            $alltags.data('alltags', alltags);
        })
        .fail((error) => {
            alertErrorMessage(error.responseText)
        })

    $alltags.on('click', '.btn', function () {
        addPostTags();
    });

    $tags.on('click', '.close', function (e: JQueryEventObject) {
        removePostTags(e);
    });

    $('#submitbtn').click(() => { submit() });
}(window));