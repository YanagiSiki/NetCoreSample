function alertErrorMessage(errorMessage) {
    $('#_ErrorFlashMessage').append("<div class=\"alert alert-danger\">\n                                    <a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>\n                                    <strong>Error!</strong> " + errorMessage + "\n                                </div>");
}
function alertSuccessMessage(sucessMessage) {
    $('#_ErrorFlashMessage').append(" <div class=\"alert alert-success\">\n                                    <a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>\n                                    <strong>Sucess!</strong> " + sucessMessage + "\n                                </div>");
}
function waitForEl(selector, callback) {
    if ($(selector).length) {
        callback();
    }
    else {
        setTimeout(function () {
            waitForEl(selector, callback);
        }, 100);
    }
}
;
function substringMatcher(strs) {
    return function findMatches(q, cb) {
        var matches;
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
}
;
function appendBadge(tags, isUseCloseBtn) {
    var $container = $('#js-tags');
    tags.forEach(function (t) {
        if (!!isUseCloseBtn)
            $container.append("<span class='badge badge-success'>\n                    <a href='#'>" + t.TagName + "</a>\n                    <i class='close fa fa-times' data-tagname='" + t.TagName + "'></i>\n                    </span> ");
        else
            // $container.append(`<span class='badge badge-pill badge-primary'>
            //             <a href='#' >${t.TagName}</a>
            //             </span> `);
            $container.append("<a href='#' class='badge badge-success'>" + t.TagName + "</a>");
    });
}
function addPostTags() {
    var $alltags = $('#js-alltags');
    var $tags = $('#js-tags');
    var tagName = $('input.typeahead.tt-input').val().toString();
    var _allTags = $alltags.data('alltags');
    var _tags = $tags.data('tags');
    if (!!_tags.find(function (t, i, arr) { return t.TagName == tagName; })) {
        alertErrorMessage('已經加入此Tag!');
        return;
    }
    var tag = _allTags.filter(function (t, i, arr) { return t.TagName == tagName; });
    if (!tag.length) {
        tag.push({ TagId: 0, TagName: tagName });
    }
    appendBadge(tag, true);
    _tags.push(tag[0]);
    $tags.data('tags', _tags);
}
function removePostTags(e) {
    var $tags = $('#js-tags');
    var _tags = $tags.data('tags');
    var tagName = $(e.target).data('tagname');
    $(e.target).parent('span').remove();
    _tags = _tags.filter(function (t, i, arr) { return t.TagName != tagName; });
    $tags.data('tags', _tags);
}
function crateTypeheadOfAllTags(tags) {
    var tagforautocomplete = $.map(tags, function (item, index) {
        return item.TagName;
    });
    $('#bloodhound .typeahead').typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
        name: 'tagforautocomplete',
        source: substringMatcher(tagforautocomplete)
    });
}
function submit($simplemde) {
    var $form = $('#form');
    var $tags = $('#js-tags');
    var formdata = $form.serializeJSON();
    var postid = parseInt($('#PostId').val().toString());
    var posttags = $.map($tags.data('tags'), function (t, i) {
        return {
            PostId: postid,
            TagId: t.TagId,
            Tag: t
        };
    });
    var submitUrl = !!postid ? '/PostApi/UpdatePost' : '/PostApi/InsertPost';
    formdata.PostTags = posttags;
    formdata.PostContent = $simplemde.value();
    $.post(submitUrl, { "post": formdata })
        .done(function () {
        alertSuccessMessage('Update Success !');
    })
        .fail(function (error) {
        alertErrorMessage(error.responseText);
    });
}
//# sourceMappingURL=tool.js.map