
/**
*绑定角色列表
*/
function getPageChannels(pageIndex) {
    var url = '/Channel/GetTableRecords';
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
            $('#channel_table').html('');
            $('#channel-table-tpl').tmpl(data.channels).appendTo('#channel_table');
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
            _getPageChannels(pageIndex);
        }
    });
}

function _getPageChannels(pageIndex) {
    var url = '/Channel/GetTableRecords';
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
            $('#channel_table').html('');
            $('#channel-table-tpl').tmpl(data.channels).appendTo('#channel_table');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}

//修改
function update(obj) {
    var tds = $(obj).parent().parent().find('td');

    $('#Id').val(tds.eq(1).text());
    $('#typecombo').val(tds.eq(2).text());
    $('#Name').val(tds.eq(3).text());
    $('#Description').val(tds.eq(4).text());

    $('#editModal').modal('show');
}

//保存修改信息
function update_click() {

    var id = $('#Id').val();
    var strType=$('#typecombo').val();
    var strName = $('#Name').val();
    var strDescription = $('#Description').val();

    if (Helpers.IsNullOrEmpty(strName)) {
        showMsg('必填字段不能空!');
        return;
    }

    $.ajax({
        url: '/Channel/Update',
        type: 'POST',
        data: {
            id: id,
            strType:strType,
            strName: strName,
            strDescription: strDescription
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            if (data.Bresult) {
                $('#editModal').modal('hide');
                Helpers.Refresh();
            } else {
                bootbox.alert(data.Notice);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}

//删除
function delete_click(strIds) {

    $.ajax({
        url: '/Channel/Delete',
        type: 'POST',
        data: {
            strIds: strIds
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            if (data.Bresult) {
                window.location.href = '/Channel/Index';
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });

}

function showMsg(message) {
    $('#message_info').html('');
    $('#message_info').html(message);
}