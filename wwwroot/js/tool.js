"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var tool;
(function (tool) {
    function alertErrorMessage(errorMessage) {
        $('#_ErrorFlashMessage').append("<div class=\"alert alert-danger\">\n                                        <a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>\n                                        <strong>Error!</strong> " + errorMessage + "\n                                    </div>");
    }
    tool.alertErrorMessage = alertErrorMessage;
    function alertSuccessMessage(sucessMessage) {
        $('#_ErrorFlashMessage').append(" <div class=\"alert alert-success\">\n                                        <a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>\n                                        <strong>Sucess!</strong> " + sucessMessage + "\n                                    </div>");
    }
    tool.alertSuccessMessage = alertSuccessMessage;
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
    tool.waitForEl = waitForEl;
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
    tool.substringMatcher = substringMatcher;
    ;
    function appendBadge(tags, isUseCloseBtn) {
        var $container = $('#js-tags');
        tags.forEach(function (t) {
            if (!!isUseCloseBtn)
                $container.append("<span class='badge badge-success'>\n                        <a href='/Home/Tag/" + t.TagId + "'>" + t.TagName + "</a>\n                        <i class='close fa fa-times' data-tagname='" + t.TagName + "'></i>\n                        </span> ");
            else
                // $container.append(`<span class='badge badge-pill badge-primary'>
                //             <a href='#' >${t.TagName}</a>
                //             </span> `);
                $container.append("<a href='/Home/Tag/" + t.TagId + "' class='badge badge-success'>" + t.TagName + "</a>");
        });
    }
    tool.appendBadge = appendBadge;
    function appendBadgeAndCount(PostCountOfTag) {
        var $container = $('#js-postCountofTag');
        PostCountOfTag.forEach(function (t) {
            $container.append("<a href='/Home/Tag/" + t.Tag.TagId + "' class='badge badge-success'>" + t.Tag.TagName + "(" + t.Count + ")</a>");
        });
    }
    tool.appendBadgeAndCount = appendBadgeAndCount;
    function addPostTags() {
        var $alltags = $('#js-alltags');
        var $tags = $('#js-tags');
        var tagName = $('#js-alltags select').val().toString();
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
    tool.addPostTags = addPostTags;
    function removePostTags(e) {
        var $tags = $('#js-tags');
        var _tags = $tags.data('tags');
        var tagName = $(e.target).data('tagname');
        $(e.target).parent('span').remove();
        _tags = _tags.filter(function (t, i, arr) { return t.TagName != tagName; });
        $tags.data('tags', _tags);
    }
    tool.removePostTags = removePostTags;
    function createTypeheadOfAllTags(tags) {
        var dropdownValue = tags.map(function (value, index) {
            return {
                name: value.TagName,
                value: value.TagName
            };
        });
        $('.ui.dropdown')
            .dropdown({
            allowAdditions: true,
            values: dropdownValue
        });
    }
    tool.createTypeheadOfAllTags = createTypeheadOfAllTags;
    function submitPost($simplemde) {
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
            .done(function (postId) {
            alertSuccessMessage('Update Success !');
            window.location.href = "/Home/Post/" + postId;
        })
            .fail(function (error) {
            alertErrorMessage(error.responseText);
        });
    }
    tool.submitPost = submitPost;
})(tool = exports.tool || (exports.tool = {}));
//# sourceMappingURL=tool.js.map