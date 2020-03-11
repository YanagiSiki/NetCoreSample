function sortNumber(a, b) {
    return a - b;
}
(function (w) {
    var $page = $('#page');
    var currentPage = $page.data('currentpage') || 1;
    var totalPage = $page.data('totalpage') || 1;
    var pageRange = $page.data('pagerange') || 2;
    var url = $page.data('url') || "./";
    $page.append('<nav aria-label="Page navigation example"><ul class="pagination d-flex justify-content-center"></ul></nav>');
    var $pagination = $page.find('.pagination');
    var pages = [];
    pages.push(currentPage);
    for (var i = 1; i <= pageRange; i++) {
        pages.push(currentPage + i);
        pages.push(currentPage - i);
    }
    pages.sort(sortNumber);
    /* Go Head */
    $pagination.append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + url + "/1\" aria-label=\"Head\"><span aria-hidden=\"true\">&laquo;</span></a></li>");
    /* Previous */
    if (currentPage == 1) {
        $pagination.append('<li class="page-item disabled"><a class="page-link" aria-label="Previous"><span aria-hidden="true">&lsaquo;</span></a></li>');
    }
    else
        $pagination.append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + url + "/" + (currentPage - 1) + "\" aria-label=\"Previous\"><span aria-hidden=\"true\">&lsaquo;</span></a></li>");
    /* Pages */
    for (var i = 0; i < pages.length; i++) {
        var page = pages[i];
        if (page < 1 || page > totalPage)
            continue;
        if (page == currentPage) {
            $pagination.append("<li class=\"page-item disabled\"><a class=\"page-link\">" + page + "</a></li>");
            continue;
        }
        $pagination.append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + url + "/" + page + "\">" + page + "</a></li>");
    }
    /* Next */
    if (currentPage == totalPage) {
        $pagination.append('<li class="page-item disabled"><a class="page-link" aria-label="Next"><span aria-hidden="true">&rsaquo;</span></a></li>');
    }
    else
        $pagination.append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + url + "/" + (currentPage + 1) + "\" aria-label=\"Next\"><span aria-hidden=\"true\">&rsaquo;</span></a></li>");
    /* Go End */
    $pagination.append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + url + "/" + totalPage + "\" aria-label=\"End\"><span aria-hidden=\"true\">&raquo;</span></a></li>");
}(window));
//# sourceMappingURL=pagination.js.map