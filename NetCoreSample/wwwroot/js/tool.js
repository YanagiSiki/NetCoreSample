export var tool;
(function (tool) {
    function alertErrorMessage(errorMessage) {
        $('#_ErrorFlashMessage').append(`<div class="alert alert-danger">
                                        <a href="#" class="close" data-dismiss="alert">&times;</a>
                                        <strong>Error!</strong> ${errorMessage}
                                    </div>`);
    }
    tool.alertErrorMessage = alertErrorMessage;
    function alertSuccessMessage(sucessMessage) {
        $('#_ErrorFlashMessage').append(` <div class="alert alert-success">
                                        <a href="#" class="close" data-dismiss="alert">&times;</a>
                                        <strong>Sucess!</strong> ${sucessMessage}
                                    </div>`);
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
    tool.appendBadge = appendBadge;
    function appendBadgeAndCount(PostCountOfTag) {
        var $container = $('#js-postCountofTag');
        PostCountOfTag.forEach(t => {
            $container.append(`<a href='/Home/Tag/${t.Tag.TagId}' class='badge badge-success'>${t.Tag.TagName}(${t.Count})</a>`);
        });
    }
    tool.appendBadgeAndCount = appendBadgeAndCount;
    function addPostTags() {
        let $alltags = $('#js-alltags');
        let $tags = $('#js-tags');
        let tagName = $('#js-alltags select').val().toString();
        let _allTags = $alltags.data('alltags');
        let _tags = $tags.data('tags');
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
    tool.addPostTags = addPostTags;
    function removePostTags(e) {
        let $tags = $('#js-tags');
        let _tags = $tags.data('tags');
        let tagName = $(e.target).data('tagname');
        $(e.target).parent('span').remove();
        _tags = _tags.filter((t, i, arr) => { return t.TagName != tagName; });
        $tags.data('tags', _tags);
    }
    tool.removePostTags = removePostTags;
    function createTypeheadOfAllTags(tags) {
        let dropdownValue = tags.map((value, index) => {
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
        let $form = $('#form');
        let $tags = $('#js-tags');
        let formdata = $form.serializeJSON();
        let postid = parseInt($('#PostId').val().toString());
        let posttags = $.map($tags.data('tags'), (t, i) => {
            return {
                PostId: postid,
                TagId: t.TagId,
                Tag: t
            };
        });
        let submitUrl = !!postid ? '/PostApi/UpdatePost' : '/PostApi/InsertPost';
        formdata.PostTags = posttags;
        formdata.PostContent = $simplemde.value();
        $.post(submitUrl, { "post": formdata })
            .done((postId) => {
            alertSuccessMessage('Update Success !');
            window.location.href = `/Home/Post/${postId}`;
        })
            .fail((error) => {
            alertErrorMessage(error.responseText);
        });
    }
    tool.submitPost = submitPost;
})(tool || (tool = {}));
//# sourceMappingURL=tool.js.map