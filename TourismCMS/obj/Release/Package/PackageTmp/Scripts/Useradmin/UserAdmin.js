/**
*绑用户列表
*/
function getPageUsers(pageIndex) {
    var url = '/UserAdmin/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        //dataType: 'jsonp',
        data: { start: start, limit: 50, keyword: '' },
        //jsonp: 'jsoncallback',
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            //$('.cur-cat-list').html('');
            //$('#software-list-tpl').tmpl(data.softwares).appendTo('.cur-cat-list');
            //createPage(5, data.pageNum, data.total);
            //console.log(data);
            $('#user_table').html('');
            $('#user-table-tpl').tmpl(data.users).appendTo('#user_table');
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
            _getPageUsers(pageIndex);
        }
    });
}

function _getPageUsers(pageIndex) {
    var url = '/UserAdmin/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        //dataType: 'jsonp',
        data: { start: start, limit: 50, keyword: '' },
        //jsonp: 'jsoncallback',
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            //$('.cur-cat-list').html('');
            //$('#software-list-tpl').tmpl(data.softwares).appendTo('.cur-cat-list');
            $('#user_table').html('');
            $('#user-table-tpl').tmpl(data.users).appendTo('#user_table');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}

function createUserCombo() {

    $.ajax({
        url: '/UserAdmin/GetComboData',
        type: 'GET',
        async: false,
        timeout: 1000,
        beforeSend: function () {

        },
        success: function (data) {
            $('#usercombo').html('');
            $('#user-combo-tpl').tmpl(data).appendTo('#usercombo');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });

}

//获取角色数据
function createRoleCombo() {

    $.ajax({
        url: '/Role/GetRoleComboData',
        type: 'GET',
        async: false,
        timeout: 1000,
        beforeSend: function () {

        },
        success: function (data) {
            $('#rolecombo').html('');
            $('#role-combo-tpl').tmpl(data).appendTo('#rolecombo');
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
    //$('#name').val(tds.eq(0).text());

    //This is new code for the radio.
    //if (tds.eq(1).text() == '男') { $('#p_man').attr("checked", "checked"); } else { $('#p_woman').attr("checked", "checked"); }

    $('#Id').val(tds.eq(1).text());
    $('#Name').val(tds.eq(2).text());
    $('#IDCard').val(tds.eq(3).text());
    $('#Email').val(tds.eq(4).text());
    $('#AccountId').val(tds.eq(5).text());
    $('#Phone').val(tds.eq(6).text());
    $('#Position').val(tds.eq(7).text());
    $('#Description').val(tds.eq(8).text());

    $('#editModal').modal('show');
}


//删除
function delete_click(strIds){

    $.ajax({
        url: '/UserAdmin/Delete',
        type: 'POST',
        data: {
            strIds:strIds
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            if (data.Bresult) {
                window.location.href = '/UserAdmin/Index';
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });

}

//修改用户角色

function updateUserRole_click(obj) {
    var tds = $(obj).parent().parent().find('td');
    var userId = tds.eq(1).text();
    $('#UserId').val(userId);

    $("#rolecombo").val(data.ChannelId);
}

function updateUserRole() {

    
    var roleId = $('#rolecombo').val();
    var userId = $('#UserId').val();

    $.ajax({
        url: '/UserAdmin/UpdateUserRole',
        type: 'POST',
        data: {
            userId: userId,
            roleId:roleId
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            if (data.Bresult) {
                $('#editRoleModal').modal('hide');
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}