
/**
*绑定角色列表
*/
function getPageSupplies(pageIndex) {
    var url = '/Supplier/GetTableRecords';
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
            $('#supplier_table').html('');
            $('#supplier-table-tpl').tmpl(data.suppliers).appendTo('#supplier_table');
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
            _getPageSuppliers(pageIndex);
        }
    });
}

function _getPageSuppliers(pageIndex) {
    var url = '/Supplier/GetTableRecords';
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
            $('#supplier_table').html('');
            $('#supplier-table-tpl').tmpl(data.suppliers).appendTo('#supplier_table');
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
    //$("#hotelcombo").val(data.HotelId);
    $('#typecombo').val(tds.eq(2).text());
    $('#Name').val(tds.eq(3).text());
    $('#Manager').val(tds.eq(4).text());
    $('#Phone').val(tds.eq(5).text());
    $('#Fax').val(tds.eq(6).text());
    $('#QQ').val(tds.eq(7).text());
    $('#WeChat').val(tds.eq(8).text());
    $('#AccountId').val(tds.eq(9).text());
    $('#Address').val(tds.eq(10).text());
    $('#ProxyType').val(tds.eq(11).text());
    $('#Description').val(tds.eq(12).text());

    $('#editModal').modal('show');
}

//保存修改信息
function update_click() {

    var id = $('#Id').val();
    var strType = $('#typecombo').val();
    var strName = $('#Name').val();
    var strManager = $('#Manager').val();
    var strPhone = $('#Phone').val();
    var strFax = $('#Fax').val();
    var strQQ = $('#QQ').val();
    var strWeChat = $('#WeChat').val();
    var strAccountId = $('#AccountId').val();
    var strAddress = $('#Address').val();
    var strProxyType = $('#ProxyType').val();
    var strDescription = $('#Description').val();

    if (Helpers.IsNullOrEmpty(strType))
    {
        showMsg('类型不能为空!');
        return;
    }

    if (Helpers.IsNullOrEmpty(strName) || Helpers.IsNullOrEmpty(strManager) || Helpers.IsNullOrEmpty(strPhone))
    {
        showMsg('必填字段不能为空!');
        return;
    }

    //if (!Helpers.CheckMobile(strPhone))
    //{
    //    showMsg('请填写正确的手机格式!');
    //    return;
    //}

    $.ajax({
        url: '/Supplier/Update',
        type: 'POST',
        data: {
            id: id,
            strType:strType,
            strName:strName,
            strManager: strManager,
            strPhone:strPhone,
            strFax: strFax,
            strQQ: strQQ,
            strWeChat: strWeChat,
            strAccountId: strAccountId,
            strAddress: strAddress,
            strProxyType:strProxyType,
            strDescription: strDescription
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            if (data.Bresult) {
                $('#editModal').modal('hide');
                window.location.href = '/Supplier/Index';
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
        url: '/Supplier/Delete',
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
                window.location.href = '/Supplier/Index';
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

function showMsg(message) {
    $('#message_info').html('');
    $('#message_info').html(message);
}