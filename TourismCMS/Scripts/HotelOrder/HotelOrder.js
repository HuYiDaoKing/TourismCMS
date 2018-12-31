/*
*查询并统计，参数为空时，页码使用当前页码或为0.
*/
var pagination = {
    pageIndex: 0,
    query: function () {
        if (arguments[0] || arguments[0] == 0) {
            this.pageIndex = arguments[0] 
        }
        getPageHotelOrders(this.pageIndex);
        //统计
        getStatics();
    }
};


/**
*绑定酒店列表
*/
function getPageHotelOrders(pageIndex) {

    var hotelId = $('#search_hotel_combo').val();
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

    var url = '/HotelOrder/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            start: start,
            limit: 50,
            hotelId: hotelId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            sellerId: sellerId,
            flag: flag,
            settlement: settlement,
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
            $('#hotel_order_table').html('');
            $('#hotel-order-table-tpl').tmpl(data.hotelOrders).appendTo('#hotel_order_table');
            createPage(pageIndex, 50, data.pageNum, data.total);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });
}

function createPage(pageIndex, pageSize, maxPage, total) {
    $(".pagination").jBootstrapPage({
        pageSize: pageSize,
        total: total,
        maxPageButton: maxPage,
        selectedIndex: pageIndex,
        onPageClicked: function (obj, pageIndex) {
            //_getPageHoltelOrders(pageIndex);
            setTimeout(function () {
                pagination.query(pageIndex);                
            }, 10);
        }
    });
}

//[Obsolete], 这个酒店拼写有误
function _getPageHoltelOrders(pageIndex) {

    var hotelId = $('#search_hotel_combo').val();
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

    var url = '/HotelOrder/GetTableRecords';
    start = pageIndex * 50;
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            start: start,
            limit: 50,
            hotelId: hotelId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            sellerId: sellerId,
            flag: flag,
            settlement: settlement,
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
            $('#hotel_order_table').html('');
            $('#hotel-order-table-tpl').tmpl(data.hotelOrders).appendTo('#hotel_order_table');

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

    var hotelId = $('#search_hotel_combo').val();
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
        url: '/HotelOrder/GetStatics',
        type: 'GET',
        data: {
            hotelId: hotelId,
            channelId: channelId,
            supplierId: supplierId,
            balacnestatusId: balacnestatusId,
            collectstatusId: collectstatusId,
            sellerId: sellerId,
            flag: flag,
            settlement: settlement,
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
    /*$('#Id').val(tds.eq(1).text());
    //$('#Name').val(tds.eq(2).text());
    //$('#Phone').val(tds.eq(3).text());
    //$('#Address').val(tds.eq(4).text());
    //$('#Description').val(tds.eq(5).text());
    $("#suppliercombo").val(tds.eq(2).text());
    $("#hotelcombo").val(tds.eq(3).text());
    $("#channelcombo").val(tds.eq(4).text());
    $("#StartDate").val(tds.eq(4).text());*/

    $.ajax({
        url: '/HotelOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {
            $("#Id").val(id);
            $("#suppliercombo").val(data.SupplierId);
            $("#hotelcombo").val(data.HotelId);
            $("#channelcombo").val(data.ChannelId);
            $("#BookTime").val(data.BookTime);
            $("#StartDate").val(data.StartDate);
            $("#EndDate").val(data.EndDate);
            $("#GuestName").val(data.GuestName);
            $("#GuestPhone").val(data.GuestPhone);
            $("#RoomType").val(data.RoomType);
            $("#RoomNum").val(data.RoomNum);
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

    //$('#editModal').modal('show');
}

//保存修改信息
function update_click() {

    var id = $('#Id').val();
    var hotelId = $('#hotelcombo').val();
    var channelId = $('#channelcombo').val();
    var supplierId = $('#suppliercombo').val();
    var bookTime = $('#BookTime').val();
    var startDate = $('#StartDate').val();
    var endDate = $('#EndDate').val();
    var guestName = $('#GuestName').val();
    var guestPhone = $('#GuestPhone').val();
    var roomType = $('#RoomType').val();
    var roomNum = $('#RoomNum').val();
    var originalPrice = $('#OriginalPrice').val();
    var sellPrice = $('#SellPrice').val();
    var proxyIncome = $('#ProxyIncome').val();
    var description = $('#Description').val();

    if (Helpers.IsNullOrEmpty(hotelId)
            || Helpers.IsNullOrEmpty(channelId)
            || Helpers.IsNullOrEmpty(supplierId)
            || Helpers.IsNullOrEmpty(bookTime)
            || Helpers.IsNullOrEmpty(startDate)
            || Helpers.IsNullOrEmpty(endDate)
            || Helpers.IsNullOrEmpty(guestName)
            || Helpers.IsNullOrEmpty(guestPhone)
            || Helpers.IsNullOrEmpty(guestName)
            || Helpers.IsNullOrEmpty(roomType)
            || Helpers.IsNullOrEmpty(roomNum)
            || Helpers.IsNullOrEmpty(originalPrice)
            || Helpers.IsNullOrEmpty(sellPrice)
            || Helpers.IsNullOrEmpty(proxyIncome)

            ) {
        bootbox.alert("必填字段不能为空,请检查!");
        return;
    }

    if (!Helpers.isNum(roomNum)) {
        bootbox.alert("房间数必须是正整数,请检查!");
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

    //新增不能是全部(-1)
    if (hotelId == -1 || channelId == -1 || supplierId == -1) {
        bootbox.alert("请检查,酒店,渠道,供应商不能为空!");
        return;
    }

    if (Helpers.IsNullOrEmpty(bookTime) || Helpers.IsNullOrEmpty(startDate) || Helpers.IsNullOrEmpty(endDate)) {
        bootbox.alert("预定时间,入住日期,离开日期不能为空!");
        return;
    }

    $.ajax({
        url: '/HotelOrder/Update',
        type: 'POST',
        data: {
            id: id,
            hotelId: hotelId,
            channelId: channelId,
            supplierId: supplierId,
            bookTime: bookTime,
            startDate: startDate,
            endDate: endDate,
            guestName: guestName,
            guestPhone: guestPhone,
            roomType: roomType,
            roomNum: roomNum,
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
                //window.location.href = '/HotelOrder/Index';
                pagination.query();
            } else
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
        url: '/HotelOrder/Delete',
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
                //window.location.href = '/HotelOrder/Index';
                pagination.query();
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
        url: '/HotelOrder/GetById',
        type: 'GET',
        data: { id: id },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            $("#reviewstatuscombo").val(data.ReviewStatusId);
            $('#reviewModal').modal('show');
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
        }
    });
}

//批量
function batchReview() {

    //$("#reviewstatuscombo").val(data.ReviewStatusId);
    $('#reviewModal').modal('show');
}

//结算状态信息
function updateBalanceStatus(obj) {

    var tds = $(obj).parent().parent().find('td');

    var id = tds.eq(1).text();
    $.ajax({
        url: '/HotelOrder/GetById',
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
        url: '/HotelOrder/GetById',
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
    var reviewStatusId = $('#reviewstatuscombo').val();

    //flag 0=单个 1=批量
    if (s == '') {
        var id = $('#Id').val();

        $.ajax({
            url: '/HotelOrder/Review',
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
                    //window.location.href = '/HotelOrder/Index';
                    pagination.query();
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function (xhr) {
                alert('加载数据失败!');
            }
        });
    } else {
        //批量
        $.ajax({
            url: '/HotelOrder/BatchReview',
            type: 'POST',
            data: {
                strIds: s,
                reviewStatusId: reviewStatusId
            },
            async: false,
            timeout: 1000,
            beforeSend: function () {
            },
            success: function (data) {

                if (data.Bresult) {
                    $('#reviewModal').modal('hide');
                    //window.location.href = '/HotelOrder/Index';
                    pagination.query();
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function (xhr) {
                alert('加载数据失败!');
            }
        });
    }

}

//balance_click
function balance_click() {

    var id = $('#Id').val();
    var balanceStatusId = $('#balacnestatuscombo').val();
    var balanceRemark = $('#BalanceRemark').val();

    $.ajax({
        url: '/HotelOrder/Balance',
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

            if (data.Bresult) {
                $('#balanceModal').modal('hide');
                //window.location.href = '/HotelOrder/Index';
                pagination.query();
            }
            //showMsg(data.Notice);
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
        url: '/HotelOrder/Collect',
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

            if (data.Bresult) {
                $('#collectModal').modal('hide');
                //window.location.href = '/HotelOrder/Index';
                pagination.query();
            }
            //showMsg(data.Notice);
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
    //getPageHotelOrders(0);
    //getStatics();//统计也会更新

    pagination.query(0);
}

function showMsg(message) {
    $('#message_info').html('');
    $('#message_info').html(message);
}

//输入框cookie HOTEL_ORDER前缀
function txtOnChange(fieldId) {
    return;


    //---------------屏蔽cookie缓存--------------
    var key = '';
    switch (fieldId) {
        case 'search_startdate':
            key += 'hotel_order_search_startdate';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_startdate').val());
            break;
        case 'search_enddate':
            key += 'hotel_order_search_enddate';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_enddate').val());
            break;
        case 'search_guestname':
            key += 'hotel_order_search_guestname';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_guestname').val());
            break;
        case 'search_guestphone':
            key += 'hotel_order_search_guestphone';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_guestphone').val());
            break;
            //combo
        case 'search_supplier_combo':
            key += 'hotel_order_search_supplier_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_supplier_combo').val());
            break;
        case 'search_seller_combo':
            key += 'hotel_order_search_seller_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_seller_combo').val());
            break;
        case 'search_balacnestatus_combo':
            key += 'hotel_order_search_balacnestatus_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_balacnestatus_combo').val());
            break;
        case 'search_collectstatus_combo':
            key += 'hotel_order_search_collectstatus_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_collectstatus_combo').val());
            break;
        case 'search_settlement_combo':
            key += 'hotel_order_search_settlement_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_settlement_combo').val());
            break;
        case 'search_hotel_combo':
            key += 'hotel_order_search_hotel_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_hotel_combo').val());
            break;
        case 'search_flag_combo':
            key += 'hotel_order_search_flag_combo';
            Helpers.delCookie(key);
            Helpers.SetCookie(key, $('#search_flag_combo').val());
            break;
    }
}

function txtSetValues() {
    $('#search_startdate').val(Helpers.getCookie('hotel_order_search_startdate'));
    $('#search_enddate').val(Helpers.getCookie('hotel_order_search_enddate'));
    $('#search_guestname').val(Helpers.getCookie('hotel_order_search_guestname'));
    $('#search_guestphone').val(Helpers.getCookie('hotel_order_search_guestphone'));
}

//seComboValue
function comboSetValue() {
    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_supplier_combo')))
        $('#search_supplier_combo').val(Helpers.getCookie('hotel_order_search_supplier_combo'));

    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_seller_combo')))
        $('#search_seller_combo').val(Helpers.getCookie('hotel_order_search_seller_combo'));

    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_balacnestatus_combo')))
        $('#search_balacnestatus_combo').val(Helpers.getCookie('hotel_order_search_balacnestatus_combo'));

    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_collectstatus_combo')))
        $('#search_collectstatus_combo').val(Helpers.getCookie('hotel_order_search_collectstatus_combo'));

    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_settlement_combo')))
        $('#search_settlement_combo').val(Helpers.getCookie('hotel_order_search_settlement_combo'));

    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_hotel_combo')))
        $('#search_hotel_combo').val(Helpers.getCookie('hotel_order_search_hotel_combo'));

    if (!Helpers.IsNullOrEmpty(Helpers.getCookie('hotel_order_search_flag_combo')))
        $('#search_flag_combo').val(Helpers.getCookie('hotel_order_search_flag_combo'));

}