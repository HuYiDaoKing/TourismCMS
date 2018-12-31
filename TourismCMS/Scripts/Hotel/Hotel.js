
/**
*绑定酒店列表
*/
function getPageHotels(pageIndex) {
    var url = '/Hotel/GetTableRecords';
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
            $('#hotel_table').html('');
            $('#hotel-table-tpl').tmpl(data.hotels).appendTo('#hotel_table');
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
            _getPageHoltels(pageIndex);
        }
    });
}

function _getPageHoltels(pageIndex) {
    var url = '/Hotel/GetTableRecords';
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
            $('#role-table-tpl').tmpl(data.hotels).appendTo('#role_table');
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
    $('#Phone').val(tds.eq(3).text());
    $('#Address').val(tds.eq(4).text());
    $('#Description').val(tds.eq(5).text());

    $('#editModal').modal('show');
}

//保存修改信息
function update_click() {

    var id = $('#Id').val();
    var strName = $('#Name').val();
    var strPhone = $('#Phone').val();
    var strAddress = $('#Address').val();
    var strDescription = $('#Description').val();

    if (Helpers.IsNullOrEmpty(strName) || Helpers.IsNullOrEmpty(strPhone) || Helpers.IsNullOrEmpty(strAddress)) {
        showMsg('必填字段不能为空!');
        return;
    }

    //if (!Helpers.CheckMobile(strPhone)) {
    //    showMsg('请输入正确手机号码!');
    //    return;
    //}

    $.ajax({
        url: '/Hotel/Update',
        type: 'POST',
        data: {
            id: id,
            strName: strName,
            strPhone: strPhone,
            strAddress:strAddress,
            strDescription: strDescription
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            if (data.Bresult) {
                $('#editModal').modal('hide');
                window.location.href = '/Hotel/Index';
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
        url: '/Hotel/Delete',
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
                window.location.href = '/Hotel/Index';
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