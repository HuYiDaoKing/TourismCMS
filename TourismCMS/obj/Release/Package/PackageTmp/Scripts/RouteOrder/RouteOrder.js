
/**
*绑定酒店列表
*/
function getPageRouteOrders(pageIndex) {

    var routeId = $('#search_route_combo').val();
    var channelId = $('#search_channel_combo').val();
    var supplierId = $('#search_supplier_combo').val();
    var balacnestatusId = $('#search_balacnestatus_combo').val();
    var collectstatusId = $('#search_collectstatus_combo').val();
    //var startdate = $('#search_startdate').val();
    var sellerId = $('#search_seller_combo').val();
    var flag = $('#search_flag_combo').val();
    var settlement = $('#search_settlement_combo').val();
    var startdate = $('#search_startdate').val();
    var enddate = $('#search_enddate').val();

    var guestname = $('#search_guestname').val();
    var guestphone = $('#search_guestphone').val();

    var url = '/RouteOrder/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            start: start,
            limit: 50,
            routeId: routeId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            //startdate: startdate,
            sellerId:sellerId,
            flag: flag,
            settlement:settlement,
            startdate: startdate,
            enddate: enddate,

            guestname: guestname,
            guestphone: guestphone,
            keyword: ''
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $('#route_order_table').html('');
            $('#route-order-table-tpl').tmpl(data.routeOrders).appendTo('#route_order_table');

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
            _getPageRouteOrders(pageIndex);
        }
    });
}

function _getPageRouteOrders(pageIndex) {

    var routeId = $('#search_route_combo').val();
    var channelId = $('#search_channel_combo').val();
    var supplierId = $('#search_supplier_combo').val();
    var balacnestatusId = $('#search_balacnestatus_combo').val();
    var collectstatusId = $('#search_collectstatus_combo').val();
    //var startdate = $('#search_startdate').val();
    var sellerId = $('#search_seller_combo').val();
    var flag = $('#search_flag_combo').val();
    var settlement = $('#search_settlement_combo').val();
    var startdate = $('#search_startdate').val();
    var enddate = $('#search_enddate').val();

    var guestname = $('#search_guestname').val();
    var guestphone = $('#search_guestphone').val();

    var url = '/RouteOrder/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            start: start,
            limit: 50,
            routeId: routeId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            //startdate: startdate,
            sellerId:sellerId,
            flag: flag,
            settlement:settlement,
            startdate: startdate,
            enddate: enddate,
            guestname: guestname,
            guestphone: guestphone,
            keyword: ''
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $('#route_order_table').html('');
            $('#route-order-table-tpl').tmpl(data.routeOrders).appendTo('#route_order_table');

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

    var routeId = $('#search_route_combo').val();
    var channelId = $('#search_channel_combo').val();
    var supplierId = $('#search_supplier_combo').val();
    var balacnestatusId = $('#search_balacnestatus_combo').val();
    var collectstatusId = $('#search_collectstatus_combo').val();
    var sellerId = $('#search_seller_combo').val();
    var flag = $('#search_flag_combo').val();
    var settlement = $('#search_settlement_combo').val();
    var startdate = $('#search_startdate').val();
    var enddate = $('#search_enddate').val();

    var guestname = $('#search_guestname').val();
    var guestphone = $('#search_guestphone').val();

    $.ajax({
        url: '/RouteOrder/GetStatics',
        type: 'GET',
        data: {
            routeId: routeId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            //startdate: startdate,
            sellerId:sellerId,
            flag: flag,
            settlement:settlement,
            startdate: startdate,
            enddate: enddate,
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
        url: '/RouteOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $("#Id").val(id);
            $("#suppliercombo").val(data.SupplierId);
            $("#routecombo").val(data.RouteId);
            $("#channelcombo").val(data.ChannelId);
            $("#BookTime").val(data.BookTime);
            $("#StartDate").val(data.StartDate);
            $("#EndDate").val(data.EndDate);
            $("#GuestName").val(data.GuestName);
            $("#GuestPhone").val(data.GuestPhone);
            $('#AdultNum').val(data.AdultNum);
            $('#AdultOriginalPrice').val(data.AdultOriginalPrice);
            $('#AdultSellPrice').val(data.AdultSellPrice);
            $('#ChildNum').val(data.ChildNum);
            $('#ChildOriginalPrice').val(data.ChildOriginalPrice);
            $('#ChildSellPrice').val(data.ChildSellPrice);
            $('#ExtrasOriginPrice').val(data.ExtrasOriginPrice);
            $('#ExtrasSellPrice').val(data.ExtrasSellPrice);
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
    var routeId = $('#routecombo').val();
    var channelId = $('#channelcombo').val();
    var supplierId = $('#suppliercombo').val();
    var bookTime = $('#BookTime').val();
    var startDate = $('#StartDate').val();
    var endDate = $('#EndDate').val();
    var guestName = $('#GuestName').val();
    var guestPhone = $('#GuestPhone').val();
    var adultNum = $('#AdultNum').val();
    var adultOriginalPrice = $('#AdultOriginalPrice').val();
    var adultSellPrice = $('#AdultSellPrice').val();
    var childNum = $('#ChildNum').val();
    var childOriginalPrice = $('#ChildOriginalPrice').val();
    var childSellPrice = $('#ChildSellPrice').val();

    var extrasOriginPrice = $('#ExtrasOriginPrice').val();
    var extrasSellPrice = $('#ExtrasSellPrice').val();

    var proxyIncome = $('#ProxyIncome').val();
    var description = $('#Description').val();


    if (Helpers.IsNullOrEmpty(routeId)
            || Helpers.IsNullOrEmpty(channelId)
            || Helpers.IsNullOrEmpty(supplierId)
            || Helpers.IsNullOrEmpty(bookTime)
            || Helpers.IsNullOrEmpty(startDate)
            || Helpers.IsNullOrEmpty(endDate)
            || Helpers.IsNullOrEmpty(guestName)
            || Helpers.IsNullOrEmpty(guestPhone)
            || Helpers.IsNullOrEmpty(adultNum)
            || Helpers.IsNullOrEmpty(adultOriginalPrice)
            || Helpers.IsNullOrEmpty(adultSellPrice)
            || Helpers.IsNullOrEmpty(childNum)
            || Helpers.IsNullOrEmpty(childOriginalPrice)
            || Helpers.IsNullOrEmpty(childSellPrice)
            || Helpers.IsNullOrEmpty(proxyIncome)
            || Helpers.IsNullOrEmpty(extrasOriginPrice)
            || Helpers.IsNullOrEmpty(extrasSellPrice)

            ) {
        bootbox.alert("必填字段不能为空,请检查!");
        return;
    }

    //新增不能是全部(-1)
    if (routeId == -1 || channelId == -1 || supplierId == -1) {
        bootbox.alert("请检查,酒店,渠道,供应商不能为空!");
        return;
    }

    if (!Helpers.isNum(adultNum)) {
        bootbox.alert("成人数必须是正整数,请检查!");
        return;
    }
    if (!Helpers.isNum(childNum)) {
        bootbox.alert("儿童数必须是正整数,请检查!");
        return;
    }
    if (!Helpers.isDecimal(adultOriginalPrice)) {
        bootbox.alert("成人底价必须是正数或小数,请检查!");
        return;
    }
    if (!Helpers.isDecimal(adultSellPrice)) {
        bootbox.alert("成人售价必须是正数或小数,请检查!");
        return;
    }
    if (!Helpers.isDecimal(childOriginalPrice)) {
        bootbox.alert("小孩底价必须是正整数,请检查!");
        return;
    }
    if (!Helpers.isDecimal(childSellPrice)) {
        bootbox.alert("小孩售价必须是正整数,请检查!");
        return;
    }

    if (!Helpers.isDecimal(extrasOriginPrice)) {
        bootbox.alert("杂费底价必须是正整数,请检查!");
        return;
    }

    if (!Helpers.isDecimal(extrasSellPrice)) {
        bootbox.alert("杂费售卖价必须是正整数,请检查!");
        return;
    }

    if (!Helpers.isRealNum(proxyIncome)) {
        bootbox.alert("代收必须是实数,请检查!");
        return;
    }

    $.ajax({
        url: '/RouteOrder/Update',
        type: 'POST',
        data: {
            id: id,
            routeId: routeId,
            channelId: channelId,
            supplierId: supplierId,
            bookTime: bookTime,
            startDate: startDate,
            endDate: endDate,
            guestName: guestName,
            guestPhone: guestPhone,
            adultNum: adultNum,
            adultOriginalPrice: adultOriginalPrice,
            adultSellPrice: adultSellPrice,
            childNum: childNum,
            childOriginalPrice: childOriginalPrice,
            childSellPrice: childSellPrice,
            extrasOriginPrice: extrasOriginPrice,
            extrasSellPrice:extrasSellPrice,
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
                window.location.href = '/RouteOrder/Index';
            } else
                bootbox.alert(data.Notice);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('修改数据失败!');
        }
    });

}

//删除
function delete_click(strIds) {

    $.ajax({
        url: '/RouteOrder/Delete',
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
                window.location.href = '/RouteOrder/Index';
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
        url: '/RouteOrder/GetById',
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
        url: '/RouteOrder/GetById',
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
        url: '/RouteOrder/GetById',
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
        url: '/RouteOrder/Review',
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
                window.location.href = '/RouteOrder/Index';
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
        url: '/RouteOrder/Balance',
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
            //showMsg(data.Notice);
            //$('#balanceModal').modal('hide');
            if (data.Bresult) {
                $('#balanceModal').modal('hide');
                window.location.href = '/RouteOrder/Index';
            }
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
        url: '/RouteOrder/Collect',
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
            //showMsg(data.Notice);
            //$('#collectModal').modal('hide');
            if (data.Bresult) {
                $('#collectModal').modal('hide');
                window.location.href = '/RouteOrder/Index';
            }
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

    getPageRouteOrders(0);
    getStatics();
}

function showMsg(msg) {
    $('#noticInfo').html('');
    $('#noticInfo').html(msg);
}