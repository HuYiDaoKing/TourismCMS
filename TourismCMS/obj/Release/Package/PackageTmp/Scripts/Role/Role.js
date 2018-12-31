
/**
*绑定角色列表
*/
function getPageRoles(pageIndex) {
    var url = '/Role/GetTableRecords';
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
            $('#role_table').html('');
            $('#role-table-tpl').tmpl(data.roles).appendTo('#role_table');
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
            _getPageRoles(pageIndex);
        }
    });
}

function _getPageRoles(pageIndex) {
    var url = '/Role/GetTableRecords';
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
            $('#role_table').html('');
            $('#role-table-tpl').tmpl(data.users).appendTo('#role_table');
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
    $('#Name').val(tds.eq(2).text());
    $('#Description').val(tds.eq(3).text());

    $('#editModal').modal('show');
}

//保存修改信息
function update_click() {

    var id = $('#Id').val();
    var strName = $('#Name').val();
    var strDescription = $('#Description').val();

    if (Helpers.IsNullOrEmpty(strName)) {
        showMsg('必填字段不能为空!');
        return;
    }

    $.ajax({
        url: '/Role/Update',
        type: 'POST',
        data: {
            id: id,
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
        url: '/Role/Delete',
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
                window.location.href = '/Role/Index';
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });

}

function showMsg(msg) {
    $('#noticInfo').html('');
    $('#noticInfo').html(msg);
}