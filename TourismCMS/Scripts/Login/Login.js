/**
* 登陆
*/
function Login() {
    var strUserName = $('#uname').val().trim();
    var strPassword = $('#pwd').val().trim();
    $.ajax({
        url: '/Login/Login',
        type: 'POST',
        data: {
            strUserName: strUserName,
            strPassword: strPassword
        },
        async: false,
        timeout: 1000,
        beforeSend: function () {
        },
        success: function (data) {

            if (data.Bresult) {
                window.location.href = data.Url;

            } else {
                alert(data.Notice);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (xhr) {
            alert('加载数据失败!');
        }
    });

}