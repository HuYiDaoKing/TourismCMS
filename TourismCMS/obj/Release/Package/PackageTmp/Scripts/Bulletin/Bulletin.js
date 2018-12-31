/**
*绑定酒店列表
*/
function getPageBulletins(pageIndex) {
    var url = '/Bulletin/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: { start: start, limit: 50, keyword: '' },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $('#bulletin_table').html('');
            $('#bulletin-table-tpl').tmpl(data.bulletins).appendTo('#bulletin_table');
            createPage(50, data.pageNum, data.total);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}

function createPage(pageSize, buttons, total) {
    $(".pagination").jBootstrapPage({
        pageSize: pageSize,
        total: total,
        maxPageButton: buttons,
        onPageClicked: function (obj, pageIndex) {
            _getPageBulletins(pageIndex);
        }
    });
}

function _getPageBulletins(pageIndex) {
    var url = '/Bulletin/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: { start: start, limit: 50, keyword: '' },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $('#bulletin_table').html('');
            $('#bulletin-table-tpl').tmpl(data.bulletins).appendTo('#bulletin_table');
            createPage(50, data.pageNum, data.total);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}