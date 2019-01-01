declare interface JQuery {
    twbsPagination(option: any): JQuery;
}

(function (w) {
    
    $('#pagination').twbsPagination({
        items: 20,
        itemOnPage: 8,
        currentPage: 1,
        cssStyle: '',
        prevText: '<span aria-hidden="true">&laquo;</span>',
        nextText: '<span aria-hidden="true">&raquo;</span>',
        href: true
    })
}(window));