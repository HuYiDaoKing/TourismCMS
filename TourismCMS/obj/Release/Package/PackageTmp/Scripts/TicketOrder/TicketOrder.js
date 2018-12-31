
/**
*绑定酒店列表
*/
function getPageTicketOrders(pageIndex) {

    var ticketId = $('#search_ticket_combo').val();
    var channelId = $('#search_channel_combo').val();
    var supplierId = $('#search_supplier_combo').val();
    var balacnestatusId = $('#search_balacnestatus_combo').val();
    var collectstatusId = $('#search_collectstatus_combo').val();
    var sellerId = $('#search_seller_combo').val();
    var settlement = $('#search_settlement_combo').val();
    var usedate = $('#search_usedate').val();
    var guestname = $('#search_guestname').val();
    var guestphone = $('#search_guestphone').val();

    var url = '/TicketOrder/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            start: start,
            limit: 50,
            ticketId: ticketId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            sellerId:sellerId,
            settlement:settlement,
            usedate: usedate,
            guestname: guestname,
            guestphone: guestphone,
            keyword: ''
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $('#ticket_order_table').html('');
            $('#ticket-order-table-tpl').tmpl(data.ticketOrders).appendTo('#ticket_order_table');

            //统计
            //$('#statistics').html('');
            //$('#statistics-tpl').tmpl(data.Statics).appendTo('#statistics');

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
            _getPageTicketOrders(pageIndex);
        }
    });
}

function _getPageTicketOrders(pageIndex) {

    var ticketId = $('#search_ticket_combo').val();
    var channelId = $('#search_channel_combo').val();
    var supplierId = $('#search_supplier_combo').val();
    var balacnestatusId = $('#search_balacnestatus_combo').val();
    var collectstatusId = $('#search_collectstatus_combo').val();
    var sellerId = $('#search_seller_combo').val();
    var settlement = $('#search_settlement_combo').val();
    var usedate = $('#search_usedate').val();
    var guestname = $('#search_guestname').val();
    var guestphone = $('#search_guestphone').val();

    var url = '/HotelOrder/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            start: start,
            limit: 50,
            ticketId: ticketId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            sellerId:sellerId,
            settlement:settlement,
            usedate: usedate,
            guestname: guestname,
            guestphone: guestphone,
            keyword: ''
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $('#ticket_order_table').html('');
            $('#ticket-order-table-tpl').tmpl(data.ticketOrders).appendTo('#ticket_order_table');

            //统计
            //$('#statistics').html('');
            //$('#statistics-tpl').tmpl(data.Statics).appendTo('#statistics');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}

function getStatics() {

    var ticketId = $('#search_ticket_combo').val();
    var channelId = $('#search_channel_combo').val();
    var supplierId = $('#search_supplier_combo').val();
    var balacnestatusId = $('#search_balacnestatus_combo').val();
    var collectstatusId = $('#search_collectstatus_combo').val();
    var sellerId = $('#search_seller_combo').val();
    var settlement = $('#search_settlement_combo').val();
    var usedate = $('#search_usedate').val();
    var guestname = $('#search_guestname').val();
    var guestphone = $('#search_guestphone').val();

    $.ajax({
        url: '/TicketOrder/GetStatics',
        type: 'GET',
        data: {
            ticketId: ticketId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            sellerId: sellerId,
            settlement: settlement,
            usedate: usedate,
            guestname: guestname,
            guestphone: guestphone,
            keyword: ''
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            //统计
            $('#statistics').html('');
            $('#statistics-tpl').tmpl(data).appendTo('#statistics');
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

    var id = tds.eq(1).text();

    $.ajax({
        url: '/TicketOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $("#Id").val(id);
            $("#suppliercombo").val(data.SupplierId);
            $("#ticketcombo").val(data.TicketId);
            $("#channelcombo").val(data.ChannelId);
            $("#BookTime").val(data.BookTime);
            $("#UseDate").val(data.UseDate);
            //$("#EndDate").val(data.EndDate);
            $("#GuestName").val(data.GuestName);
            $("#GuestPhone").val(data.GuestPhone);
            $("#TicketType").val(data.TicketType);
            $("#TicketNum").val(data.TicketNum);
            $("#OriginalPrice").val(data.OriginalPrice);
            $("#SellPrice").val(data.SellPrice);
            $("#ProxyIncome").val(data.ProxyIncome);
            $("#Description").val(data.Description);


            $('#editModal').modal('show');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}

//保存修改信息
function update_click() {

    var id = $('#Id').val();
    var ticketId = $('#ticketcombo').val();
    var channelId = $('#channelcombo').val();
    var supplierId = $('#suppliercombo').val();
    var bookTime = $('#BookTime').val();
    var useDate = $('#UseDate').val();
    //var endDate = $('#EndDate').val();
    var guestName = $('#GuestName').val();
    var guestPhone = $('#GuestPhone').val();
    var ticketType = $('#TicketType').val();
    var ticketNum = $('#TicketNum').val();
    var originalPrice = $('#OriginalPrice').val();
    var sellPrice = $('#SellPrice').val();
    var proxyIncome = $('#ProxyIncome').val();
    var description = $('#Description').val();


    if (Helpers.IsNullOrEmpty(ticketId) || Helpers.IsNullOrEmpty(channelId) || Helpers.IsNullOrEmpty(supplierId)) {
        showMsg('门票,渠道,供应商不能为空!');
        return;
    }

    if (Helpers.IsNullOrEmpty(bookTime)
        || Helpers.IsNullOrEmpty(useDate)
        || Helpers.IsNullOrEmpty(guestName)
        || Helpers.IsNullOrEmpty(guestPhone)
        || Helpers.IsNullOrEmpty(guestName)
        || Helpers.IsNullOrEmpty(ticketType)
        || Helpers.IsNullOrEmpty(ticketNum)
        || Helpers.IsNullOrEmpty(originalPrice)
        || Helpers.IsNullOrEmpty(sellPrice)
        || Helpers.IsNullOrEmpty(proxyIncome)

        ) {
        bootbox.alert("必填字段不能为空,请检查!");
        return;
    }

    if (!Helpers.isNum(ticketNum)) {
        bootbox.alert("票数必须是正整数,请检查!");
        return;
    }
    if (!Helpers.isDecimal(originalPrice)) {
        bootbox.alert("底价必须是正数或小数,请检查!");
        return;
    }
    if (!Helpers.isDecimal(sellPrice)) {
        bootbox.alert("售卖价必须是正整数,请检查!");
        return;
    }

    if (!Helpers.isRealNum(proxyIncome)) {
        bootbox.alert("代收必须是实数,请检查!");
        return;
    }


    $.ajax({
        url: '/TicketOrder/Update',
        type: 'POST',
        data: {
            id: id,
            ticketId: ticketId,
            channelId: channelId,
            supplierId: supplierId,
            bookTime:bookTime,
            useDate: useDate,
            //endDate: endDate,
            guestName: guestName,
            guestPhone: guestPhone,
            ticketType: ticketType,
            ticketNum: ticketNum,
            originalPrice: originalPrice,
            sellPrice: sellPrice,
            proxyIncome: proxyIncome,
            description: description
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            if (data.Bresult) {
                $('#editModal').modal('hide');
                window.location.href = '/TicketOrder/Index';
            }else
                bootbox.alert(data.Notice);
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
        url: '/TicketOrder/Delete',
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
                window.location.href = '/TicketOrder/Index';
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });

}

//审查
function review(obj) {
    var tds = $(obj).parent().parent().find('td');
    var id = tds.eq(1).text();
    $('#Id').val(tds.eq(1).text());
    //var reviewstatus = $('#reviewstatuscombo').val();

    $.ajax({
        url: '/TicketOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $("#reviewstatuscombo").val(data.ReviewStatusId);
            //$("#reviewstatuscombo").find("option[text='" + data.ReviewStatus + "']").attr("selected", true);
            //$("#reviewstatuscombo option[text=未审]").attr("selected", "true")
            $('#reviewModal').modal('show');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}

//结算状态信息
function updateBalanceStatus(obj) {

    var tds = $(obj).parent().parent().find('td');

    var id = tds.eq(1).text();
    $.ajax({
        url: '/TicketOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $("#Id").val(id);
            $("#BalanceRemark").val(data.BalanceRemark);
            $("#balacnestatuscombo").val(data.BalanceStatusId);

            $('#balanceModal').modal('show');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }

    });
}

//收款状态信息
function updateCollectStatus(obj) {

    var tds = $(obj).parent().parent().find('td');

    var id = tds.eq(1).text();
    $.ajax({
        url: '/TicketOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $("#Id").val(id);
            $("#CollectMoneyRemark").val(data.CollectMoneyRemark);
            $("#collectstatuscombo").val(data.CollectMoneyStatusId);

            $('#collectModal').modal('show');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }

    });
}

//保存修改信息
function review_click() {

    var id = $('#Id').val();
    var reviewStatusId = $('#reviewstatuscombo').val();

    $.ajax({
        url: '/TicketOrder/Review',
        type: 'POST',
        data: {
            id: id,
            reviewStatusId: reviewStatusId
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            if (data.Bresult) {
                $('#reviewModal').modal('hide');
                window.location.href = '/TicketOrder/Index';
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}

//balance_click
function balance_click() {

    var id = $('#Id').val();
    var balanceStatusId = $('#balacnestatuscombo').val();
    var balanceRemark = $('#BalanceRemark').val();

    $.ajax({
        url: '/TicketOrder/Balance',
        type: 'POST',
        data: {
            id: id,
            balanceStatusId: balanceStatusId,
            balanceRemark: balanceRemark
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            showMsg(data.Notice);
            if (data.Bresult) {
                $('#balanceModal').modal('hide');
                window.location.href = '/TicketOrder/Index';
            }
            //$('#balanceModal').modal('hide');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}

//collect_click
function collect_click() {

    var id = $('#Id').val();
    var collectStatusId = $('#collectstatuscombo').val();
    var collectmoneyRemark = $('#CollectMoneyRemark').val();

    $.ajax({
        url: '/TicketOrder/Collect',
        type: 'POST',
        data: {
            id: id,
            collectStatusId: collectStatusId,
            collectmoneyRemark: collectmoneyRemark
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            showMsg(data.Notice);
            if (data.Bresult) {
                $('#collectModal').modal('hide');
                window.location.href = '/TicketOrder/Index';
            }
            //$('#collectModal').modal('hide');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}

//search_click
function search_click() {

    getPageTicketOrders(0);
    getStatics();
}

function showMsg(msg) {
    $('#noticInfo').html('');
    $('#noticInfo').html(msg);
}