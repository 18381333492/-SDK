

//获取账户信息
function hw_getInfo() {
    try {
        var result = window.lives.getAccount("huawei"); //获取账户信息
        if (result != 3) {
            result = JSON.parse(result);
            return result.account; //返回登录的账号
        }
        else {
            return null;
        }
    } catch (e) {
        alert(e.message);
    }
}

//判断是否存在微信APP
function hw_IsExitApp() {
    try {
        var res = window.lives.getAppExists("com.tencent.mm");
        return res;
    } catch (e) {
        alert(e.message);
    }
}

function setAccount(response) {
    var result = JSON.parse(response);
    if (result.code == 200) {
        $.toast("登录成功");
    }
    else {
        $.toast(result.message, "cancel");
    }
}